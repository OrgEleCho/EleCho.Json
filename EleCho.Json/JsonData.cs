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

        public static object? LoadToModel(Type modelType, IJsonData jsonData)
        {
            object? model;

            if (jsonData is JsonObject jsonObject)
            {
                if (typeof(IDictionary).IsAssignableFrom(modelType))
                {
                    Type[] genTypes = modelType.GetGenericArguments();
                    if (genTypes[0] != typeof(string))
                        throw new ArgumentException("Model type must be Dictionary<string, T> when it's a IDictionary");

                    Type valueType = genTypes[1];
                    IDictionary dict = (Activator.CreateInstance(modelType) as IDictionary)!;
                    foreach (var item in jsonObject)
                    {
                        dict.Add(item.Key, LoadToModel(valueType, item.Value));
                    }

                    model = dict;
                }
                else
                {
                    model = Activator.CreateInstance(modelType);
                    PropertyInfo[] props = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    FieldInfo[] fields = modelType.GetFields(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var prop in props.Where(p => p.CanWrite))
                    {
                        if (jsonObject.ContainsKey(prop.Name))
                        {
                            prop.SetValue(model, LoadToModel(prop.PropertyType, jsonObject[prop.Name]));
                        }
                    }
                    foreach (var field in fields)
                    {
                        if (jsonObject.ContainsKey(field.Name))
                        {
                            field.SetValue(model, LoadToModel(field.FieldType, jsonObject[field.Name]));
                        }
                    }
                }
            }
            else if (jsonData is JsonArray jsonArray)
            {
                if (modelType.IsArray)
                {
                    Type[] genTypes = modelType.GetGenericArguments();
                    Type elementType = genTypes[0];
                    Array array = Array.CreateInstance(elementType, jsonArray.Count);
                    for (int i = 0; i < jsonArray.Count; i++)
                    {
                        array.SetValue(LoadToModel(elementType, jsonArray[i]), i);
                    }
                    model = array;
                }
                else if (typeof(IList).IsAssignableFrom(modelType))
                {
                    Type[] genTypes = modelType.GetGenericArguments();
                    Type elementType = genTypes[0];
                    IList list = (Activator.CreateInstance(modelType) as IList)!;
                    for (int i = 0; i < jsonArray.Count; i++)
                    {
                        list.Add(LoadToModel(elementType, jsonArray[i]));
                    }
                    model = list;
                }
                else
                {
                    throw new ArgumentException("Model type must be Array or IList");
                }
            }
            else if (jsonData is JsonString jsonString)
            {
                if (typeof(string).IsAssignableFrom(modelType))
                {
                    model = jsonString.Value;
                }
                else if (typeof(Guid).IsAssignableFrom(modelType))
                {
                    model = Guid.Parse(jsonString.Value);
                }
                else if (typeof(DateTime).IsAssignableFrom(modelType))
                {
                    model = DateTime.Parse(jsonString.Value);
                }
                else if (typeof(DateTimeOffset).IsAssignableFrom(modelType))
                {
                    model = DateTimeOffset.Parse(jsonString.Value);
                }
                else if (typeof(TimeSpan).IsAssignableFrom(modelType))
                {
                    model = TimeSpan.Parse(jsonString.Value);
                }
                else if (typeof(Uri).IsAssignableFrom(modelType))
                {
                    model = new Uri(jsonString.Value);
                }
                else
                {
                    throw new ArgumentException("Model type must be string, Guid, DateTime, DateTimeOffset, TimeSpan or Uri");
                }
            }
            else if (jsonData is JsonNumber jsonNumber)
            {
                if (typeof(double).IsAssignableFrom(modelType))
                {
                    model = jsonNumber.Value;
                }
                else if (typeof(float).IsAssignableFrom(modelType))
                {
                    model = (float)jsonNumber.Value;
                }
                else if (typeof(decimal).IsAssignableFrom(modelType))
                {
                    model = (decimal)jsonNumber.Value;
                }
                else if (typeof(long).IsAssignableFrom(modelType))
                {
                    model = (long)jsonNumber.Value;
                }
                else if (typeof(int).IsAssignableFrom(modelType))
                {
                    model = (int)jsonNumber.Value;
                }
                else if (typeof(short).IsAssignableFrom(modelType))
                {
                    model = (short)jsonNumber.Value;
                }
                else if (typeof(byte).IsAssignableFrom(modelType))
                {
                    model = (byte)jsonNumber.Value;
                }
                else
                {
                    throw new ArgumentException("Model type must be decimal, double, float, long, int, short, byte");
                }
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