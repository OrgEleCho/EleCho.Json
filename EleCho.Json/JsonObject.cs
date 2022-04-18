using System.Collections.Generic;
using System.Linq;

namespace EleCho.Json
{
    /// <summary>
    /// Represents a JSON object. can be used to store JSON key value pairs.
    /// </summary>
    public class JsonObject : Dictionary<string, IJsonData>, IJsonData
    {
        /// <summary>
        /// Value is <see cref="JsonDataKind.Object"/>
        /// </summary>
        public JsonDataKind DataKind => JsonDataKind.Object;

        /// <summary>
        /// Creates a new instance of the <see cref="JsonObject"/> class.
        /// </summary>
        public JsonObject() { }
        /// <summary>
        /// Creates a new instance of the <see cref="JsonObject"/> class.
        /// </summary>
        /// <param name="data"></param>
        public JsonObject(IEnumerable<KeyValuePair<string, IJsonData>> data)
        {
            foreach (var item in data)
                Add(item.Key, item.Value);
        }

        /// <summary>
        /// Add a new key value pair to the object.
        /// </summary>
        /// <param name="key">String key</param>
        /// <param name="data">JSON data</param>
        public void Add(string key, JsonObject data) => Add(key, data as IJsonData);
        /// <summary>
        /// Add a new key value pair to the object.
        /// </summary>
        /// <param name="key">String key</param>
        /// <param name="data">JSON data</param>
        public void Add(string key, JsonArray data) => Add(key, data as IJsonData);
        /// <summary>
        /// Add a new key value pair to the object.
        /// </summary>
        /// <param name="key">String key</param>
        /// <param name="data">JSON data</param>
        public void Add(string key, JsonString data) => Add(key, data as IJsonData);
        /// <summary>
        /// Add a new key value pair to the object.
        /// </summary>
        /// <param name="key">String key</param>
        /// <param name="data">JSON data</param>
        public void Add(string key, JsonNumber data) => Add(key, data as IJsonData);
        /// <summary>
        /// Add a new key value pair to the object.
        /// </summary>
        /// <param name="key">String key</param>
        /// <param name="data">JSON data</param>
        public void Add(string key, JsonBoolean data) => Add(key, data as IJsonData);
        /// <summary>
        /// Add a new key value pair to the object.
        /// </summary>
        /// <param name="key">String key</param>
        /// <param name="data">JSON data</param>
        public void Add(string key, JsonNull data) => Add(key, data as IJsonData);


        /// <summary>
        /// Get the corresponding value of the JSON data.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object?> GetValue()
        {
            return this.ToDictionary(pair => pair.Key, pair => pair.Value.GetValue());
        }

        object? IJsonData.GetValue() => GetValue();
    }
}