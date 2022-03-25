using System;

namespace EleCho.Json
{
    public struct JsonToken
    {
        public readonly JsonTokenKind Kind;
        public readonly string Value;

        public JsonToken(JsonTokenKind kind, string value)
        {
            Kind = kind;
            Value = value;
        }

        public static JsonToken None = new JsonToken(JsonTokenKind.None, string.Empty);
        public static JsonToken ObjectStart = new JsonToken(JsonTokenKind.ObjectStart, "{");
        public static JsonToken ObjectEnd = new JsonToken(JsonTokenKind.ObjectEnd, "}");
        public static JsonToken ArrayStart = new JsonToken(JsonTokenKind.ArrayStart, "[");
        public static JsonToken ArrayEnd = new JsonToken(JsonTokenKind.ArrayEnd, "]");
        public static JsonToken Colon = new JsonToken(JsonTokenKind.Colon, ":");
        public static JsonToken Comma = new JsonToken(JsonTokenKind.Comma, ",");
        public static JsonToken True = new JsonToken(JsonTokenKind.True, "true");
        public static JsonToken False = new JsonToken(JsonTokenKind.False, "false");
        public static JsonToken Null = new JsonToken(JsonTokenKind.Null, "null");

        public bool IsNone()
        {
            return Kind == JsonTokenKind.None;
        }
    }
}