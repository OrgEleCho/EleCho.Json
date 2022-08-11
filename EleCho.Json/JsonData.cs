using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace EleCho.Json
{
    /// <summary>
    /// Provide static methods to convert between special .NET types and JSON types.
    /// 提供静态方法，将特定的 .NET 类型转换为 JSON 类型。
    /// </summary>
    public static class JsonData
    {
        private static readonly Type TypeJsonObject = typeof(JsonObject);
        private static readonly Type TypeJsonArray = typeof(JsonArray);
        private static readonly Type TypeJsonString = typeof(JsonString);
        private static readonly Type TypeJsonNumber = typeof(JsonNumber);
        private static readonly Type TypeJsonBoolean = typeof(JsonBoolean);
        private static readonly Type TypeJsonNull = typeof(JsonNull);

        private static readonly Type TypeByte = typeof(byte);
        private static readonly Type TypeSByte = typeof(sbyte);
        private static readonly Type TypeShort = typeof(short);
        private static readonly Type TypeUShort = typeof(ushort);
        private static readonly Type TypeInt = typeof(int);
        private static readonly Type TypeUInt = typeof(uint);
        private static readonly Type TypeLong = typeof(long);
        private static readonly Type TypeULong = typeof(ulong);
        private static readonly Type TypeFloat = typeof(float);
        private static readonly Type TypeDouble = typeof(double);
        private static readonly Type TypeDecimal = typeof(decimal);
        private static readonly Type TypeString = typeof(string);
        private static readonly Type TypeBoolean = typeof(bool);

        private static readonly Type TypeGuid = typeof(Guid);
        private static readonly Type TypeDateTime = typeof(DateTime);
        private static readonly Type TypeDateTimeOffset = typeof(DateTimeOffset);
        private static readonly Type TypeTimeSpan = typeof(TimeSpan);
        private static readonly Type TypeUri = typeof(Uri);

        private static readonly Type TypeIDictionary = typeof(IDictionary);
        private static readonly Type TypeIList = typeof(IList);

        private static bool strictMode = true;

        private static Dictionary<Type, IJsonDataHandler> handlers = new();

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<Type, IJsonDataHandler> Handlers
        {
            get => handlers;
        }

        /// <summary>
        /// Get or set if exception will be thrown when error
        /// 获取或设置当错误时是否抛出异常.
        /// </summary>
        public static bool StrictMode { get => strictMode; set => strictMode = value; }


        //public abstract JsonDataKind DataKind { get; }
        //public abstract object? GetValue();


        /// <summary>
        /// Convert a special .NET type to a JSON type.
        /// 将特定的 .NET 类型转换为 JSON 类型.
        /// </summary>
        /// <param name="value">Any basic number, boolean, string, dictionary, list, or other model object. <br/>任意基础数字, 布尔值, 字符串, 字典, 列表, 或者其他实体对象.</param>
        /// <returns></returns>
        public static IJsonData FromValue(object? value)
        {
            return value switch
            {
                null => Null,
                bool b => (JsonBoolean)b,

                char c => new JsonString(c.ToString()),
                byte n => (JsonNumber)n,
                sbyte n => (JsonNumber)n,
                short n => (JsonNumber)n,
                ushort n => (JsonNumber)n,
                int n => (JsonNumber)n,
                uint n => (JsonNumber)n,
                long n => (JsonNumber)n,
                ulong n => (JsonNumber)n,
                float n => (JsonNumber)n,
                double n => (JsonNumber)n,
                decimal n => (JsonNumber)(double)n,
                string s => new JsonString(s),

                IJsonData json => json,

                IList list => FromArrayValue(list),
                IDictionary dict => FromObjectValue(dict),
                IConvertible v => new JsonString(v.ToString()!),

                _ => FromModelValue(value),
            };
        }

        /// <summary>
        /// Convert dictionary to JSON object.
        /// 将字典转换为 JSON 对象
        /// </summary>
        /// <param name="dict">Data to convert. <br/>要转换的数据.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Key is not string</exception>
        public static JsonObject FromObjectValue(IDictionary dict)
        {
            JsonObject dictData = new JsonObject();
            IDictionaryEnumerator enumerator = dict.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key is string key)
                    dictData.Add(key, FromValue(enumerator.Value));
                else if (strictMode)
                    throw new ArgumentException("Key is not string");
            }

            return dictData;
        }
        /// <summary>
        /// Convert list to JSON array.
        /// 将列表转换为 JSON 数组
        /// </summary>
        /// <param name="list">Data to convert. <br/>要转换的数据.</param>
        /// <returns></returns>
        public static JsonArray FromArrayValue(IList list)
        {
            JsonArray listData = new JsonArray();
            IEnumerator enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                listData.Add(FromValue(enumerator.Current));
            }

            return listData;
        }
        /// <summary>
        /// Create JsonData from any model object
        /// </summary>
        /// <typeparam name="TObj">Any class type</typeparam>
        /// <param name="obj">Model object. <br/>实体对象.</param>
        /// <returns>JsonObject or JsonNull</returns>
        public static IJsonData FromModelValue<TObj>(TObj obj) where TObj : class
        {
            if (obj is null)
                return Null;

            if (handlers.TryGetValue(typeof(TObj), out IJsonDataHandler? handler))
                return handler.FromValue(obj);

            Type type = obj.GetType();
            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            JsonObject dictData = new JsonObject();
            foreach (var prop in props.Where(p => p.CanRead))
            {
                dictData.Add(prop.Name, FromValue(prop.GetValue(obj)));
            }
            foreach (var field in fields)
            {
                dictData.Add(field.Name, FromValue(field.GetValue(obj)));
            }

            return dictData;
        }

        /// <summary>
        /// Create Dictionary or model object from JsonObject
        /// 从 JsonObject 创建字典或者实体对象
        /// </summary>
        /// <param name="objType"><see cref="IDictionary"/> or any class type</param>
        /// <param name="jsonObject">Source data</param>
        /// <returns>Instance with specified type</returns>
        /// <exception cref="ArgumentException"></exception>
        public static object ToObjectValue(Type objType, JsonObject jsonObject)
        {
            object model;


            if (TypeJsonObject.IsAssignableFrom(objType))
            {
                model = jsonObject;
            }
            else if (TypeIDictionary.IsAssignableFrom(objType))
            {
                Type[] genTypes = objType.GetGenericArguments();
                if (genTypes[0] != TypeString)
                {
                    if (strictMode)
                        throw new ArgumentException("Model type must be Dictionary<string, T> when it's a IDictionary", nameof(objType));

                    return null!;  // make compiler happy
                }

                Type valueType = genTypes[1];
                IDictionary dict = (Activator.CreateInstance(objType) as IDictionary)!;
                foreach (var item in jsonObject)
                {
                    dict.Add(item.Key, ToValue(valueType, item.Value));
                }

                model = dict;
            }
            else
            {
                model = ToModelValue(objType, jsonObject);
            }

            return model;
        }

        /// <summary>
        /// Create model object from JsonObject
        /// 从 JsonObject 创建实体对象
        /// </summary>
        /// <param name="modelType">Any class type</param>
        /// <param name="jsonObject">Source data</param>
        /// <returns></returns>
        public static object ToModelValue(Type modelType, JsonObject jsonObject)
        {
            object model;

            if (handlers.TryGetValue(modelType, out IJsonDataHandler? handler))
                return handler.ToValue(jsonObject);

            model = Activator.CreateInstance(modelType)!;
            PropertyInfo[] props = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] fields = modelType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props.Where(p => p.CanWrite))
            {
                if (jsonObject.ContainsKey(prop.Name))
                {
                    prop.SetValue(model, ToValue(prop.PropertyType, jsonObject[prop.Name]));
                }
            }
            foreach (var field in fields)
            {
                if (jsonObject.ContainsKey(field.Name))
                {
                    field.SetValue(model, ToValue(field.FieldType, jsonObject[field.Name]));
                }
            }

            return model;
        }

        /// <summary>
        /// Populate Dictionary or model object from JsonObject
        /// 从 JsonObject 填充字典或者实体对象
        /// </summary>
        /// <param name="model">IDictionary or any object</param>
        /// <param name="jsonObject">Source data</param>
        /// <exception cref="ArgumentException"></exception>
        public static void PopulateObject(object model, JsonObject jsonObject)
        {
            if (model == null)
                return;

            Type objType = model.GetType();

            if (model is JsonObject modelObj)
            {
                foreach (var item in jsonObject)
                {
                    modelObj[item.Key] = item.Value;
                }
            }
            else if (model is IDictionary)
            {
                Type[] genTypes = objType.GetGenericArguments();
                if (genTypes[0] != TypeString)
                {
                    if (strictMode)
                        throw new ArgumentException("Model type must be Dictionary<string, T> when it's a IDictionary", nameof(objType));

                    return;
                }

                Type valueType = genTypes[1];
                IDictionary dict = (model as IDictionary)!;
                foreach (var item in jsonObject)
                {
                    dict.Add(item.Key, ToValue(valueType, item.Value));
                }
            }
            else
            {
                PropertyInfo[] props = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                FieldInfo[] fields = objType.GetFields(BindingFlags.Public | BindingFlags.Instance);

                foreach (var prop in props.Where(p => p.CanWrite))
                {
                    if (jsonObject.ContainsKey(prop.Name))
                    {
                        prop.SetValue(model, ToValue(prop.PropertyType, jsonObject[prop.Name]));
                    }
                }
                foreach (var field in fields)
                {
                    if (jsonObject.ContainsKey(field.Name))
                    {
                        field.SetValue(model, ToValue(field.FieldType, jsonObject[field.Name]));
                    }
                }
            }
        }
        /// <summary>
        /// Create Array or List object from JsonArray
        /// </summary>
        /// <param name="arrType">Array Type or IList Type</param>
        /// <param name="jsonArray">Source data</param>
        /// <returns>Instance with specified type</returns>
        /// <exception cref="ArgumentException"></exception>
        public static object ToArrayValue(Type arrType, JsonArray jsonArray)
        {
            object arr;

            if (TypeJsonArray.IsAssignableFrom(arrType))
            {
                arr = jsonArray;
            }
            else if (arrType.IsArray)
            {
                Type[] genTypes = arrType.GetGenericArguments();
                Type elementType = genTypes[0];
                Array array = Array.CreateInstance(elementType, jsonArray.Count);
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    array.SetValue(ToValue(elementType, jsonArray[i]), i);
                }
                arr = array;
            }
            else if (TypeIList.IsAssignableFrom(arrType))
            {
                Type[] genTypes = arrType.GetGenericArguments();
                Type elementType = genTypes[0];
                IList list = (Activator.CreateInstance(arrType) as IList)!;
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    list.Add(ToValue(elementType, jsonArray[i]));
                }
                arr = list;
            }
            else
            {
                if (strictMode)
                    throw new ArgumentException("Model type must be Array or IList");
                arr = null!;        // make compiler happy
            }

            return arr;
        }

        /// <summary>
        /// Populate Array or List object from JsonArray
        /// 从 JsonArray 填充数组或者列表对象
        /// </summary>
        /// <param name="arr">Array or IList</param>
        /// <param name="jsonArray">Source data</param>
        /// <exception cref="ArgumentException"></exception>
        public static void PopulateArray(object arr, JsonArray jsonArray)
        {
            if (arr == null)
                return;

            Type arrType = arr.GetType();

            if (arr is JsonArray arrArr)
            {
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    if (i < arrArr.Count)
                    {
                        arrArr[i] = jsonArray[i];
                    }
                    else
                    {
                        arrArr.Add(jsonArray[i]);
                    }
                }
            }
            else if (arrType.IsArray)
            {
                Type[] genTypes = arrType.GetGenericArguments();
                Type elementType = genTypes[0];
                Array array = (arr as Array)!;
                for (int i = 0; i < jsonArray.Count && i < array.Length; i++)
                {
                    array.SetValue(ToValue(elementType, jsonArray[i]), i);
                }
            }
            else if (TypeIList.IsAssignableFrom(arrType))
            {
                Type[] genTypes = arrType.GetGenericArguments();
                Type elementType = genTypes[0];
                IList list = (arr as IList)!;
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    if (i < list.Count)
                    {
                        list[i] = ToValue(elementType, jsonArray[i]);
                    }
                    else
                    {
                        list.Add(ToValue(elementType, jsonArray[i]));
                    }
                }
            }
            else
            {
                if (strictMode)
                    throw new ArgumentException("Model type must be Array or IList");
            }
        }

        /// <summary>
        /// Create number type from JsonNumber
        /// </summary>
        /// <param name="numType">Type of double, float, decimal, long, int, short or byte</param>
        /// <param name="jsonNumber">Source data</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static object ToNumberValue(Type numType, JsonNumber jsonNumber)
        {
            object num;

            if (TypeByte == numType)
            {
                num = jsonNumber.GetByteValue();
            }
            else if (TypeSByte == numType)
            {
                num = jsonNumber.GetSByteValue();
            }
            else if (TypeShort == numType)
            {
                num = jsonNumber.GetShortValue();
            }
            else if (TypeUShort == numType)
            {
                num = jsonNumber.GetUShortValue();
            }
            else if (TypeInt == numType)
            {
                num = jsonNumber.GetIntValue();
            }
            else if (TypeUInt == numType)
            {
                num = jsonNumber.GetUIntValue();
            }
            else if (TypeLong == numType)
            {
                num = jsonNumber.GetLongValue();
            }
            else if (TypeULong == numType)
            {
                num = jsonNumber.GetULongValue();
            }
            else if (TypeDouble == numType)
            {
                num = jsonNumber.GetDoubleValue();
            }
            else if (TypeFloat == numType)
            {
                num = jsonNumber.GetFloatValue();
            }
            else if (TypeDecimal == numType)
            {
                num = jsonNumber.GetDecimalValue();
            }

            else if (TypeJsonNumber == numType)
            {
                num = jsonNumber;
            }
            else
            {
                if (strictMode)
                    throw new ArgumentException("Model type must be decimal, double, float, long, int, short, byte");
                num = (numType.IsValueType ? Activator.CreateInstance(numType) : null)!;    // make compiler happy
            }

            return num;
        }
        /// <summary>
        /// Create formatable type from JsonString
        /// </summary>
        /// <param name="targetType">Type of String, Guid, DateTime, DateTimeOffset, TimeSpan and Ori</param>
        /// <param name="jsonString">Source data</param>
        /// <returns>Instance with specified type</returns>
        /// <exception cref="ArgumentException"></exception>
        public static object ToStringValue(Type targetType, JsonString jsonString)
        {
            object target;

            if (TypeString == targetType)
            {
                target = jsonString.Value;
            }
            else if (TypeJsonString == targetType)
            {
                target = jsonString;
            }
            else if (targetType.IsEnum)
            {
                target = Enum.Parse(targetType, jsonString.Value);
            }
            else if (TypeGuid == targetType)
            {
                target = Guid.Parse(jsonString.Value);
            }
            else if (TypeDateTime == targetType)
            {
                target = DateTime.Parse(jsonString.Value);
            }
            else if (TypeDateTimeOffset == targetType)
            {
                target = DateTimeOffset.Parse(jsonString.Value);
            }
            else if (TypeTimeSpan == targetType)
            {
                target = TimeSpan.Parse(jsonString.Value);
            }
            else if (TypeUri == targetType)
            {
                target = new Uri(jsonString.Value);
            }
            else
            {
                if (strictMode)
                    throw new ArgumentException("Model type must be string, Guid, DateTime, DateTimeOffset, TimeSpan or Uri");
                target = (targetType.IsValueType ? Activator.CreateInstance(targetType) : null)!;
            }

            return target;
        }

        /// <summary>
        /// Create bool or number from JsonBoolean
        /// </summary>
        /// <param name="targetType">Boolean type or any number type</param>
        /// <param name="jsonBoolean">Source data</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static object ToBooleanValue(Type targetType, JsonBoolean jsonBoolean)
        {
            object target;

            if (TypeBoolean == targetType)
            {
                target = jsonBoolean.Value;
            }
            else if (TypeJsonBoolean == targetType)
            {
                target = jsonBoolean;
            }
            else
            {
                try
                {
                    target = ToNumberValue(targetType, new JsonNumber(jsonBoolean.Value ? "1" : "0"));
                }
                catch (ArgumentException)
                {
                    if (strictMode)
                        throw new ArgumentException("Model type must be bool or number");
                    target = (targetType.IsValueType ? Activator.CreateInstance(targetType) : null)!;
                }
            }

            return target;
        }

        /// <summary>
        /// Create value for any type with JsonNull
        /// </summary>
        /// <param name="targetType">Any type (class, value, enum)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static object? ToNullValue(Type targetType)
        {
            object? target;

            if (TypeJsonNull == targetType)
            {
                target = JsonNull.Null;
            }
            else if (targetType.IsClass)
            {
                target = null;
            }
            else if (targetType.IsEnum)
            {
                target = Enum.ToObject(targetType, 0);
            }
            else if (targetType.IsValueType)
            {
                target = Activator.CreateInstance(targetType);
            }
            else
            {
                throw new ArgumentException("This should never happen");
            }

            return target;
        }

        /// <summary>
        /// Create model instance from specified JsonData
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static object? ToValue(Type modelType, IJsonData jsonData)
        {
            object? model;

            if (jsonData is JsonObject jsonObject)
            {
                model = ToObjectValue(modelType, jsonObject);
            }
            else if (jsonData is JsonArray jsonArray)
            {
                model = ToArrayValue(modelType, jsonArray);
            }
            else if (jsonData is JsonString jsonString)
            {
                model = ToStringValue(modelType, jsonString);
            }
            else if (jsonData is JsonNumber jsonNumber)
            {
                model = ToNumberValue(modelType, jsonNumber);
            }
            else if (jsonData is JsonBoolean jsonBoolean)
            {
                model = ToBooleanValue(modelType, jsonBoolean);
            }
            else if (jsonData is JsonNull)
            {
                model = ToNullValue(modelType);
            }
            else
            {
                if (strictMode)
                    throw new ArgumentException("JsonData type is not supported");
                model = modelType.IsValueType ? Activator.CreateInstance(modelType) : null!;
            }

            return model;
        }

        /// <summary>
        /// JSON null
        /// </summary>
        public static JsonNull Null => JsonNull.Null;
        /// <summary>
        /// JSON boolean, true
        /// </summary>
        public static JsonBoolean True => JsonBoolean.True;
        /// <summary>
        /// JSON boolean, false
        /// </summary>
        public static JsonBoolean False => JsonBoolean.False;
    }
}