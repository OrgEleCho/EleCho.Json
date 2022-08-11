using System;

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
        public JsonNumber(string data) => Value = data;

        /// <summary>
        /// Get the double number value of this JSON number.
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Get the double number value of this JSON number.
        /// </summary>
        /// <returns></returns>
        public double GetValue() => Convert.ToDouble(Value);
        object? IJsonData.GetValue() => GetValue();


        /// <summary>
        /// Cast to byte number
        /// </summary>
        public byte GetByteValue() => Convert.ToByte(Value);
        /// <summary>
        /// Cast to sbyte number
        /// </summary>
        public sbyte GetSByteValue() => Convert.ToSByte(Value);
        /// <summary>
        /// Cast to short number
        /// </summary>
        public short GetShortValue() => Convert.ToInt16(Value);
        /// <summary>
        /// Cast to ushort number
        /// </summary>
        public ushort GetUShortValue() => Convert.ToUInt16(Value);
        /// <summary>
        /// Cast to int number
        /// </summary>
        public int GetIntValue() => Convert.ToInt32(Value);
        /// <summary>
        /// Cast to uint number
        /// </summary>
        public uint GetUIntValue() => Convert.ToUInt32(Value);
        /// <summary>
        /// Cast to long number
        /// </summary>
        public long GetLongValue() => Convert.ToInt64(Value);
        /// <summary>
        /// Cast to ulong number
        /// </summary>
        public ulong GetULongValue() => Convert.ToUInt64(Value);
        /// <summary>
        /// Cast to double number
        /// </summary>
        public double GetDoubleValue() => Convert.ToDouble(Value);
        /// <summary>
        /// Cast to float number
        /// </summary>
        public float GetFloatValue() => Convert.ToSingle(Value);
        /// <summary>
        /// Cast to decimal number
        /// </summary>
        public decimal GetDecimalValue() => Convert.ToDecimal(Value);


        /// <summary>
        /// Cast from byte number
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonNumber(byte value) => new JsonNumber(Convert.ToString(value));
        /// <summary>
        /// Cast from sbyte number
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonNumber(sbyte value) => new JsonNumber(Convert.ToString(value));
        /// <summary>
        /// Cast from short number
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonNumber(short value) => new JsonNumber(Convert.ToString(value));
        /// <summary>
        /// Cast from ushort number
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonNumber(ushort value) => new JsonNumber(Convert.ToString(value));
        /// <summary>
        /// Cast from int number
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonNumber(int value) => new JsonNumber(Convert.ToString(value));
        /// <summary>
        /// Cast from uint number
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonNumber(uint value) => new JsonNumber(Convert.ToString(value));
        /// <summary>
        /// Cast from long number
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonNumber(long value) => new JsonNumber(Convert.ToString(value));
        /// <summary>
        /// Cast from ulong number
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonNumber(ulong value) => new JsonNumber(Convert.ToString(value));
        /// <summary>
        /// Cast from double number
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonNumber(double value) => new JsonNumber(Convert.ToString(value));
        /// <summary>
        /// Cast from float number
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonNumber(float value) => new JsonNumber(Convert.ToString(value));
        /// <summary>
        /// Cast from decimal number
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonNumber(decimal value) => new JsonNumber(Convert.ToString(value));

        /// <summary>
        /// Cast to byte number
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator byte(JsonNumber value) => Convert.ToByte(value.Value);
        /// <summary>
        /// Cast to sbyte number
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator sbyte(JsonNumber value) => Convert.ToSByte(value.Value);
        /// <summary>
        /// Cast to short number
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator short(JsonNumber value) => Convert.ToInt16(value.Value);
        /// <summary>
        /// Cast to ushort number
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ushort(JsonNumber value) => Convert.ToUInt16(value.Value);
        /// <summary>
        /// Cast to int number
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator int(JsonNumber value) => Convert.ToInt32(value.Value);
        /// <summary>
        /// Cast to uint number
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator uint(JsonNumber value) => Convert.ToUInt32(value.Value);
        /// <summary>
        /// Cast to long number
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator long(JsonNumber value) => Convert.ToInt64(value.Value);
        /// <summary>
        /// Cast to ulong number
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ulong(JsonNumber value) => Convert.ToUInt64(value.Value);
        /// <summary>
        /// Cast to double number
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator double(JsonNumber value) => Convert.ToDouble(value.Value);
        /// <summary>
        /// Cast to float number
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator float(JsonNumber value) => Convert.ToSingle(value.Value);
        /// <summary>
        /// Cast to decimal number
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator decimal(JsonNumber value) => Convert.ToDecimal(value.Value);

        /// <inheritdoc/>
        public override int GetHashCode() => Tuple.Create(nameof(JsonNumber), Value).GetHashCode();

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is JsonNumber other && Equals(other);
    }
}