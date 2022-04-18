using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EleCho.Json
{
    /// <summary>
    /// Convert between  JSON data and .NET object
    /// </summary>
    public static class JsonSerializer
    {
        /// <summary>
        /// Serialize any object to JSON document string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Serialize(IJsonData value)
        {
            StringWriter sw = new StringWriter();
            JsonWriter jw = new JsonWriter(sw);
            jw.Write(value);

            return sw.ToString();
        }

        /// <summary>
        /// Serialize any object to JSON document string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Serialize(object value)
        {
            StringWriter sw = new StringWriter();
            JsonWriter jw = new JsonWriter(sw);
            jw.Write(JsonData.FromValue(value));

            return sw.ToString();
        }
        
        /// <summary>
        /// Deserialize any object from JSON document string
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static IJsonData Deserialize(string json)
        {
            StringReader sr = new StringReader(json);
            JsonReader jr = new JsonReader(sr);
            return jr.Read();
        }

        /// <summary>
        /// Deserialize any object from JSON document string
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object? Deserialize(string json, Type type)
        {
            StringReader sr = new StringReader(json);
            JsonReader jr = new JsonReader(sr);
            IJsonData jsonData = jr.Read();

            return JsonData.ToValue(type, jsonData);
        }

        /// <summary>
        /// Deserialize any object from JSON document string
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="json">JSON document string</param>
        /// <returns></returns>
        public static T? Deserialize<T>(string json) where T : class
        {
            StringReader sr = new StringReader(json);
            JsonReader jr = new JsonReader(sr);
            IJsonData jsonData = jr.Read();

            return JsonData.ToValue(typeof(T), jsonData) as T;
        }

        /// <summary>
        /// Populate object from JSON document string
        /// </summary>
        /// <param name="json">JSON object data or array data</param>
        /// <param name="target">Target instance</param>
        /// <exception cref="ArgumentException"></exception>
        public static void Populate(string json, object target)
        {
            StringReader sr = new StringReader(json);
            JsonReader jr = new JsonReader(sr);
            IJsonData jsonData = jr.Read();

            if (jsonData is JsonObject jsonObject)
            {
                JsonData.PopulateObject(target, jsonObject);
            }
            else if (jsonData is JsonArray jsonArray)
            {
                JsonData.PopulateArray(target, jsonArray);
            }
            else
            {
                throw new ArgumentException("JSON data is not an object or array");
            }
        }
    }
}
