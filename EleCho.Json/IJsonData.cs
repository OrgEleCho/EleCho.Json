namespace EleCho.Json
{
    /// <summary>
    /// Represents the base interface of JSON data
    /// </summary>
    public interface IJsonData
    {
        /// <summary>
        /// Data kind of the JSON data
        /// 当前 JSON 数据的数据类型
        /// </summary>
        JsonDataKind DataKind { get; }

        /// <summary>
        /// Get the corresponding value of the JSON data.
        /// </summary>
        /// <returns>Any .NET Object. <br/>任意 .NET 类型.</returns>
        object? GetValue();
    }
}