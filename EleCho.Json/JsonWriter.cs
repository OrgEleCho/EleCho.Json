using System;
using System.Collections.Generic;
using System.IO;

namespace EleCho.Json
{
    /// <summary>
    /// Represents a writer that provides a fast, non-cached, forward-only way of generating Json data.
    /// 表示一个提供快速、非缓存、向前的方式来生成 Json 数据的写入器。
    /// </summary>
    public class JsonWriter
    {
        private readonly TextWriter writer;
        /// <summary>
        /// Base text writer
        /// </summary>
        public TextWriter Writer => writer;

        /// <summary>
        /// Create a new instance of the <see cref="JsonWriter"/> class.
        /// 创建一个新的 <see cref="JsonWriter"/> 实例。
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> to write. 要写入的 <see cref="Stream"/></param>
        public JsonWriter(Stream stream)
        {
            writer = new StreamWriter(stream);
        }

        /// <summary>
        /// Create a new instance of the <see cref="JsonWriter"/> class.
        /// 创建一个新的 <see cref="JsonWriter"/> 实例。
        /// </summary>
        /// <param name="writer"><see cref="TextWriter"/> to use. 要使用的 <see cref="TextWriter"/></param>
        public JsonWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Write the JSON object to the underlying <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="data"></param>
        public void WriteObject(JsonObject data)
        {
            writer.Write('{');  // start object

            Dictionary<string, IJsonData>.Enumerator enumerator = data.GetEnumerator();
            if (enumerator.MoveNext())
            {
                while (true)
                {
                    WriteString(new JsonString(enumerator.Current.Key));
                    writer.Write(':');
                    Write(enumerator.Current.Value);

                    if (!enumerator.MoveNext())
                        break;

                    writer.Write(',');
                }
            }

            writer.Write("}");  // end object
        }

        /// <summary>
        /// Write the JSON array to the underlying <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="data"></param>
        public void WriteArray(JsonArray data)
        {
            writer.Write('[');  // start array

            List<IJsonData>.Enumerator enumerator = data.GetEnumerator();
            if (enumerator.MoveNext())
            {
                while (true)
                {
                    Write(enumerator.Current);

                    if (!enumerator.MoveNext())
                        break;

                    writer.Write(',');
                }
            }

            writer.Write(']');
        }

        /// <summary>
        /// Write the JSON string to the underlying <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="data"></param>
        public void WriteString(JsonString data)
        {
            writer.Write('"');

            char[] chars = data.Value.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                writer.Write(chars[i] switch
                {
                    '\0' => "\\0",
                    '\a' => "\\a",
                    '\b' => "\\b",
                    '\t' => "\\t",
                    '\r' => "\\r",
                    '\f' => "\\f",
                    '\n' => "\\n",
                    '"' => "\\\"",
                    '\\' => "\\\\",
                    _ => chars[i].ToString()
                });
            }

            writer.Write('"');
        }

        /// <summary>
        /// Write the JSON number to the underlying <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="data"></param>
        public void WriteNumber(JsonNumber data)
        {
            writer.Write(data.Value.ToString());
        }

        /// <summary>
        /// Write the JSON boolean to the underlying <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="data"></param>
        public void WriteBoolean(JsonBoolean data)
        {
            writer.Write(data.Value ? "true" : "false");
        }

        /// <summary>
        /// Write the JSON null to the underlying <see cref="TextWriter"/>.
        /// </summary>
        public void WriteNull()
        {
            writer.Write("null");
        }

        /// <summary>
        /// Write the JSON data to the underlying <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Write(IJsonData data)
        {
            switch (data)
            {
                case JsonObject obj:
                    WriteObject(obj);
                    break;
                case JsonArray arr:
                    WriteArray(arr);
                    break;
                case JsonString str:
                    WriteString(str);
                    break;
                case JsonNumber num:
                    WriteNumber(num);
                    break;
                case JsonBoolean boolData:
                    WriteBoolean(boolData);
                    break;
                case JsonNull:
                    WriteNull();
                    break;
                default:
                    throw new InvalidOperationException("Unexpected data type");
            }
        }
    }
}