namespace EleCho.Json
{
    /// <summary>
    /// Represents a JSON null.
    /// </summary>
    public class JsonNull : IJsonData
    {
        /// <summary>
        /// Value is <see cref="JsonDataKind.Null"/>
        /// </summary>
        public JsonDataKind DataKind => JsonDataKind.Null;
        private JsonNull()
        {
        }

        /// <summary>
        /// Get an null object
        /// </summary>
        /// <returns></returns>
        public object? GetValue() => null;

        private static readonly JsonNull @null = new JsonNull();

        /// <summary>
        /// JSON null value
        /// </summary>
        public static JsonNull Null => @null;
    }
}