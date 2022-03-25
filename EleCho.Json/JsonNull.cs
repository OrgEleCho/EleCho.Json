namespace EleCho.Json
{
    public class JsonNull : IJsonData
    {
        public JsonDataKind DataKind => JsonDataKind.Null;
        public JsonNull()
        {
        }

        public object? GetValue() => null;
    }
}