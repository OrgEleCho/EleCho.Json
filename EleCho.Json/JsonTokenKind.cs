namespace EleCho.Json
{
    /// <summary>
    /// JSON token kind
    /// </summary>
    public enum JsonTokenKind
    {
        /// <summary>
        /// No token.
        /// </summary>
        None,        // no token
        /// <summary>
        /// Start of JSON object
        /// </summary>
        ObjectStart, // {
        /// <summary>
        /// End of JSON object
        /// </summary>
        ObjectEnd,   // }
        /// <summary>
        /// Start of JSON array
        /// </summary>
        ArrayStart,  // [
        /// <summary>
        /// End of JSON array
        /// </summary>
        ArrayEnd,    // ]
        /// <summary>
        /// :
        /// </summary>
        Colon,       // :
        /// <summary>
        /// ,
        /// </summary>
        Comma,       // ,
        /// <summary>
        /// JSON string
        /// </summary>
        String,      // "..."
        /// <summary>
        /// JSON number
        /// </summary>
        Number,      // 123.456
        /// <summary>
        /// JSON boolean, true
        /// </summary>
        True,        // true
        /// <summary>
        /// JSON boolean, false
        /// </summary>
        False,       // false
        /// <summary>
        /// JSON null
        /// </summary>
        Null         // null
    }
}