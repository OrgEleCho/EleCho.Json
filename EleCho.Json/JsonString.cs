namespace EleCho.Json
{
    public class JsonString : IJsonData
    {
        public string Value { get; }
        public JsonDataKind DataKind => JsonDataKind.String;
        public JsonString(string data) => Value = data;


        public string GetValue() => Value;
        object? IJsonData.GetValue() => Value;

        public static implicit operator string(JsonString data) => data.Value;
        public static implicit operator JsonString(string data) => new JsonString(data);
    }
}