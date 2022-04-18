using System;

namespace EleCho.Json
{
    /// <summary>
    /// Represents a JSON boolean. true or false.
    /// </summary>
    public class JsonBoolean : IJsonData
    {
        /// <summary>
        /// Boolean value of current JSON boolean data.
        /// </summary>
        public bool Value { get; }
        /// <summary>
        /// Value is <see cref="JsonDataKind.Boolean"/>.
        /// 值为 <see cref="JsonDataKind.Boolean"/>.
        /// </summary>
        public JsonDataKind DataKind => JsonDataKind.Boolean;
        
        private JsonBoolean(bool data) => Value = data;

        /// <summary>
        /// Get the corresponding value of the JSON data.
        /// </summary>
        /// <returns></returns>
        public bool GetValue() => Value;
        object? IJsonData.GetValue() => Value;

        private static JsonBoolean trueInstance = new JsonBoolean(true);
        private static JsonBoolean falseInstance = new JsonBoolean(false);

        /// <summary>
        /// True
        /// </summary>
        public static JsonBoolean True => trueInstance;
        /// <summary>
        /// False
        /// </summary>
        public static JsonBoolean False => falseInstance;

        /// <summary>
        /// Cast to Boolean
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator bool(JsonBoolean data) => data.Value;

        /// <summary>
        /// Cast from Boolean
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator JsonBoolean(bool data) => data ? trueInstance : falseInstance;

        /// <inheritdoc/>
        public override int GetHashCode() => Tuple.Create(nameof(JsonBoolean), Value).GetHashCode();

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is JsonBoolean data && data.Value == Value;
    }
}