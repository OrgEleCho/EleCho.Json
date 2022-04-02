namespace EleCho.Json
{
    /// <summary>
    /// JSON string
    /// </summary>
    public class JsonString : IJsonData
    {
        /// <summary>
        /// Value of the JSON data
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// Value is <see cref="JsonDataKind.String"/>
        /// </summary>
        public JsonDataKind DataKind => JsonDataKind.String;
        /// <summary>
        /// Create a new instance of the <see cref="JsonString"/> class.
        /// </summary>
        /// <param name="data"></param>
        public JsonString(string data) => Value = data;

        /// <summary>
        /// Get value of the JSON data
        /// </summary>
        /// <returns></returns>
        public string GetValue() => Value;
        object? IJsonData.GetValue() => Value;

        /// <summary>
        /// Cast to String
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator string(JsonString data) => data.Value;
        /// <summary>
        /// Cast from String
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator JsonString(string data) => new JsonString(data);
    }
}