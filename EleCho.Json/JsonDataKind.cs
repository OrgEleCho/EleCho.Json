namespace EleCho.Json
{
    /// <summary>
    /// JSON data kind
    /// </summary>
    public enum JsonDataKind
    {
        /// <summary>
        /// JSON null (null)
        /// </summary>
        Null,
        /// <summary>
        /// JSON object (JSON data dictionary)
        /// </summary>
        Object,
        /// <summary>
        /// JSON array (JSON data list)
        /// </summary>
        Array,
        /// <summary>
        /// JSON string (string)
        /// </summary>
        String,
        /// <summary>
        /// JSON number (a double number)
        /// </summary>
        Number,
        /// <summary>
        /// JSON boolean (true or false)
        /// </summary>
        Boolean,
    }
}