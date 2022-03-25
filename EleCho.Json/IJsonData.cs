namespace EleCho.Json
{
    public interface IJsonData
    {
        JsonDataKind DataKind { get; }

        object? GetValue();
    }
}