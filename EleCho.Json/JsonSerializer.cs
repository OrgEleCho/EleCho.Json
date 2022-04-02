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
    }
}
