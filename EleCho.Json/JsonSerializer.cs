using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EleCho.Json
{
    public static class JsonSerializer
    {
        public static string Serialize(object value)
        {
            StringWriter sw = new StringWriter();
            JsonWriter jw = new JsonWriter(sw);
            jw.WriteData(JsonData.FromValue(value));

            return sw.ToString();
        }
        
        public static IJsonData Deserialize(string json)
        {
            StringReader sr = new StringReader(json);
            JsonReader jr = new JsonReader(sr);
            return jr.Read();
        }

        public static object? Deserialize(string json, Type type)
        {
            StringReader sr = new StringReader(json);
            JsonReader jr = new JsonReader(sr);
            IJsonData jsonData = jr.Read();

            return JsonData.ToValue(type, jsonData);
        }

        public static T? Deserialize<T>(string json) where T : class
        {
            StringReader sr = new StringReader(json);
            JsonReader jr = new JsonReader(sr);
            IJsonData jsonData = jr.Read();

            return JsonData.ToValue(typeof(T), jsonData) as T;
        }
    }
}
