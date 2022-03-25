using System.Collections.Generic;
using System.Linq;

namespace EleCho.Json
{
    public class JsonObject : Dictionary<string, IJsonData>, IJsonData
    {
        public JsonDataKind DataKind => JsonDataKind.Object;

        public JsonObject() { }
        public JsonObject(Dictionary<string, IJsonData> data)
        {
            foreach (var item in data)
                Add(item.Key, item.Value);
        }

        public void Add(string key, JsonObject data) => Add(key, data as IJsonData);
        public void Add(string key, JsonArray data) => Add(key, data as IJsonData);
        public void Add(string key, JsonString data) => Add(key, data as IJsonData);
        public void Add(string key, JsonNumber data) => Add(key, data as IJsonData);
        public void Add(string key, JsonBoolean data) => Add(key, data as IJsonData);
        public void Add(string key, JsonNull data) => Add(key, data as IJsonData);


        public Dictionary<string, object?> GetValue()
        {
            return this.ToDictionary(pair => pair.Key, pair => pair.Value.GetValue());
        }

        object? IJsonData.GetValue() => GetValue();
    }
}