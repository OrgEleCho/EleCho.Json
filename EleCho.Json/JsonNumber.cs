namespace EleCho.Json
{
    public class JsonNumber : IJsonData
    {
        public JsonDataKind DataKind => JsonDataKind.Number;
        public JsonNumber(double data) => Value = data;

        public double Value { get; }

        public double GetValue() => Value;
        object? IJsonData.GetValue() => Value;

        public static implicit operator JsonNumber(double value) => new JsonNumber(value);
        public static implicit operator double(JsonNumber value) => value.Value;
    }
}