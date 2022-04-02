using System;
using System.Collections;
using System.Collections.Generic;
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
        /// <summary>
        /// Convert a special .NET type to a JSON type.
        /// 将特定的 .NET 类型转换为 JSON 类型。
        /// </summary>
        /// <param name="value">Any basic number, boolean, dictionary, list, or other model object. <br/>任意基础数字, 布尔值, 字典, 列表, 或者其他实体对象.</param>
        /// <returns></returns>
        public static IJsonData FromValue(object? value)
        {
            return value switch
            {
                null => Null,
                bool b => (JsonBoolean)b,

                IConvertible n => new JsonNumber(n.ToDouble(null)),
                IList list => FromArrayValue(list),
                IDictionary dict => FromObjectValue(dict),

                IJsonData json => json,

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
                else
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


            if (typeof(IDictionary).IsAssignableFrom(objType))
            {
                Type[] genTypes = objType.GetGenericArguments();
                if (genTypes[0] != typeof(string))
                    throw new ArgumentException("Model type must be Dictionary<string, T> when it's a IDictionary", nameof(objType));

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
                model = Activator.CreateInstance(objType)!;
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

            return model;
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

            if (arrType.IsArray)
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
            else if (typeof(IList).IsAssignableFrom(arrType))
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
                throw new ArgumentException("Model type must be Array or IList");
            }

            return arr;
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
            
            if (typeof(double).IsAssignableFrom(numType))
            {
                num = jsonNumber.Value;
            }
            else if (typeof(float).IsAssignableFrom(numType))
            {
                num = (float)jsonNumber.Value;
            }
            else if (typeof(decimal).IsAssignableFrom(numType))
            {
                num = (decimal)jsonNumber.Value;
            }
            else if (typeof(long).IsAssignableFrom(numType))
            {
                num = (long)jsonNumber.Value;
            }
            else if (typeof(int).IsAssignableFrom(numType))
            {
                num = (int)jsonNumber.Value;
            }
            else if (typeof(short).IsAssignableFrom(numType))
            {
                num = (short)jsonNumber.Value;
            }
            else if (typeof(byte).IsAssignableFrom(numType))
            {
                num = (byte)jsonNumber.Value;
            }
            else
            {
                throw new ArgumentException("Model type must be decimal, double, float, long, int, short, byte");
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

            if (typeof(string).IsAssignableFrom(targetType))
            {
                target = jsonString.Value;
            }
            else if (typeof(Guid).IsAssignableFrom(targetType))
            {
                target = Guid.Parse(jsonString.Value);
            }
            else if (typeof(DateTime).IsAssignableFrom(targetType))
            {
                target = DateTime.Parse(jsonString.Value);
            }
            else if (typeof(DateTimeOffset).IsAssignableFrom(targetType))
            {
                target = DateTimeOffset.Parse(jsonString.Value);
            }
            else if (typeof(TimeSpan).IsAssignableFrom(targetType))
            {
                target = TimeSpan.Parse(jsonString.Value);
            }
            else if (typeof(Uri).IsAssignableFrom(targetType))
            {
                target = new Uri(jsonString.Value);
            }
            else
            {
                throw new ArgumentException("Model type must be string, Guid, DateTime, DateTimeOffset, TimeSpan or Uri");
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
                if (typeof(bool).IsAssignableFrom(modelType))
                {
                    model = jsonBoolean.Value;
                }
                else
                {
                    throw new ArgumentException("Model type must be bool");
                }
            }
            else if (jsonData is JsonNull)
            {
                if (modelType.IsClass)
                {
                    model = null;
                }
                else
                {
                    throw new ArgumentException("Model type must be class");
                }
            }
            else
            {
                throw new ArgumentException("JsonData type is not supported");
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