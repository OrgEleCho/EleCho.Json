using System;

namespace EleCho.Json
{
    /// <summary>
    /// JSON token
    /// </summary>
    public struct JsonToken
    {
        /// <summary>
        /// JSON token kind
        /// </summary>
        public readonly JsonTokenKind Kind;
        /// <summary>
        /// JSON token string value
        /// </summary>
        public readonly string? Value;

        /// <summary>
        /// Create a new token
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="value"></param>
        public JsonToken(JsonTokenKind kind, string? value)
        {
            Kind = kind;
            Value = value;
        }

        /// <summary>
        /// No token
        /// </summary>
        public static JsonToken None = new JsonToken(JsonTokenKind.None, null);
        /// <summary>
        /// Object start ('{')
        /// </summary>
        public static JsonToken ObjectStart = new JsonToken(JsonTokenKind.ObjectStart, null);
        /// <summary>
        /// Object end ('}')
        /// </summary>电话
        public static JsonToken ObjectEnd = new JsonToken(JsonTokenKind.ObjectEnd, null);
        /// <summary>
        /// Array start ('[')
        /// </summary>
        public static JsonToken ArrayStart = new JsonToken(JsonTokenKind.ArrayStart, null);
        /// <summary>
        /// Array end (']')
        /// </summary>
        public static JsonToken ArrayEnd = new JsonToken(JsonTokenKind.ArrayEnd, null);
        /// <summary>
        /// Colon (':')
        /// </summary>
        public static JsonToken Colon = new JsonToken(JsonTokenKind.Colon, null);
        /// <summary>
        /// Colon (',')
        /// </summary>
        public static JsonToken Comma = new JsonToken(JsonTokenKind.Comma, null);
        /// <summary>
        /// Keyword (true)
        /// </summary>
        public static JsonToken True = new JsonToken(JsonTokenKind.True, null);
        /// <summary>
        /// Keyword (false)
        /// </summary>
        public static JsonToken False = new JsonToken(JsonTokenKind.False, null);
        /// <summary>
        /// Keyword (null)
        /// </summary>
        public static JsonToken Null = new JsonToken(JsonTokenKind.Null, null);

        /// <summary>
        /// Is JSON none token
        /// </summary>
        /// <returns></returns>
        public bool IsNone()
        {
            return Kind == JsonTokenKind.None;
        }
    }
}