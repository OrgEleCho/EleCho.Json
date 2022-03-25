namespace EleCho.Json
{
    public class JsonBoolean : IJsonData
    {
        public bool Value { get; }
        public JsonDataKind DataKind => JsonDataKind.Boolean;
        public JsonBoolean(bool data) => Value = data;

        public bool GetValue() => Value;
        object? IJsonData.GetValue() => Value;

        public static implicit operator bool(JsonBoolean data) => data.Value;
        public static implicit operator JsonBoolean(bool data) => new JsonBoolean(data);
    }
}