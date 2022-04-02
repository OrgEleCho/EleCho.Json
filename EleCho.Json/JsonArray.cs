using System.Collections.Generic;

namespace EleCho.Json
{
    /// <summary>
    /// Represents a JSON array. can be used to store JSON objects. <br/>
    /// 表示一个JSON数组。可以用来存储JSON对象。
    /// </summary>
    public class JsonArray : List<IJsonData>, IJsonData
    {
        /// <summary>
        /// Value is <see cref="JsonDataKind.Array"/>. <br/>
        /// 值为 <see cref="JsonDataKind.Array"/>.
        /// </summary>
        public JsonDataKind DataKind => JsonDataKind.Array;


        /// <summary>
        /// Creates a new instance of the <see cref="JsonArray"/> class. <br/>
        /// 创建一个新的 <see cref="JsonArray"/> 实例。
        /// </summary>
        /// <param name="data">JSON data to add. <br/>要添加的 JSON 数据</param>
        public JsonArray(IEnumerable<IJsonData> data) => AddRange(data);

        /// <summary>
        /// Creates a new instance of the <see cref="JsonArray"/> class. <br/>
        /// 创建一个新的 <see cref="JsonArray"/> 实例。
        /// </summary>
        public JsonArray()
        {
        }

        /// <summary>
        /// Add a JSON data to the array. <br/>
        /// 将 JSON 数据添加到数组中。
        /// </summary>
        /// <param name="data">Data to add. <br/>要添加的数据.</param>
        public void Add(JsonObject data) => Add(data as IJsonData);

        /// <summary>
        /// Add a JSON data to the array. <br/>
        /// 将 JSON 数据添加到数组中。
        /// </summary>
        /// <param name="data">Data to add. <br/>要添加的数据.</param>
        public void Add(JsonArray data) => Add(data as IJsonData);

        /// <summary>
        /// Add a JSON data to the array. <br/>
        /// 将 JSON 数据添加到数组中。
        /// </summary>
        /// <param name="data">Data to add. <br/>要添加的数据.</param>        
        public void Add(JsonString data) => Add(data as IJsonData);
        
        /// <summary>
        /// Add a JSON data to the array. <br/>
        /// 将 JSON 数据添加到数组中。
        /// </summary>
        /// <param name="data">Data to add. <br/>要添加的数据.</param>        
        public void Add(JsonNumber data) => Add(data as IJsonData);

        /// <summary>
        /// Add a JSON data to the array. <br/>
        /// 将 JSON 数据添加到数组中。
        /// </summary>
        /// <param name="data">Data to add. <br/>要添加的数据.</param>
        public void Add(JsonBoolean data) => Add(data as IJsonData);

        /// <summary>
        /// Add a JSON data to the array. <br/>
        /// 将 JSON 数据添加到数组中。
        /// </summary>
        /// <param name="data">Data to add. <br/>要添加的数据.</param>
        public void Add(JsonNull data) => Add(data as IJsonData);

        /// <summary>
        /// Get the corresponding value of the JSON data.
        /// </summary>
        /// <returns>List of item values</returns>
        public List<object?> GetValue()
        {
            return new List<object?>(ConvertAll(x => x.GetValue()));
        }

        object? IJsonData.GetValue() => GetValue();
    }
}