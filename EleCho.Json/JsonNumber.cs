namespace EleCho.Json
{
    /// <summary>
    /// Represents a JSON number. storage double number
    /// </summary>
    public class JsonNumber : IJsonData
    {
        /// <summary>
        /// Value is <see cref="JsonDataKind.Number"/>
        /// </summary>
        public JsonDataKind DataKind => JsonDataKind.Number;
        /// <summary>
        /// Creates a new instance of the <see cref="JsonNumber"/> class.
        /// </summary>
        /// <param name="data"></param>
        public JsonNumber(double data) => Value = data;

        /// <summary>
        /// Get
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Get the double number value of this JSON number.
        /// </summary>
        /// <returns></returns>
        public double GetValue() => Value;
        object? IJsonData.GetValue() => Value;

        /// <summary>
        /// Cast from double number
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonNumber(double value) => new JsonNumber(value);

        /// <summary>
        /// Cast to double number
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator double(JsonNumber value) => value.Value;
    }
}