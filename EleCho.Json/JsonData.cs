using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EleCho.Json
{
    public static class JsonData
    {

        public static IJsonData FromValue(object value)
        {
            return value switch
            {
                null => Null,
                bool b => new JsonBoolean(b),
                byte b => new JsonNumber(b),
                int i => new JsonNumber(i),
                short s => new JsonNumber(s),
                long l => new JsonNumber(l),
                float f => new JsonNumber(f),
                double d => new JsonNumber(d),
                string str => new JsonString(str),

                IList list => FromArrayValue(list),
                IDictionary dict => FromObjectValue(dict),

                IJsonData json => json,

                _ => FromModelValue(value),
            };
        }
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
        /// Get JsonData from any model object
        /// </summary>
        /// <typeparam name="TObj">Any class type</typeparam>
        /// <param name="obj">Model object</param>
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
        /// </summary>
        /// <param name="objType">Dictionary<string, T> or any class type</param>
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
                    throw new ArgumentException("Model type must be Dictionary<string, T> when it's a IDictionary");

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
                model = Activator.CreateInstance(objType);
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

        public static JsonNull Null => new JsonNull();
        public static JsonBoolean True => new JsonBoolean(true);
        public static JsonBoolean False => new JsonBoolean(false);
    }
}