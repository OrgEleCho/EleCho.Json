using System;
using System.Collections.Generic;
using System.IO;

namespace EleCho.Json
{
    public class JsonWriter
    {
        private readonly TextWriter writer;
        public TextWriter Writer => writer;

        public JsonWriter(Stream stream)
        {
            writer = new StreamWriter(stream);
        }

        public JsonWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        public void WriteObjectData(JsonObject data)
        {
            writer.Write('{');  // start object

            Dictionary<string, IJsonData>.Enumerator enumerator = data.GetEnumerator();
            if (enumerator.MoveNext())
            {
                while (true)
                {
                    WriteStringData(new JsonString(enumerator.Current.Key));
                    writer.Write(':');
                    WriteData(enumerator.Current.Value);

                    if (!enumerator.MoveNext())
                        break;

                    writer.Write(',');
                }
            }

            writer.Write("}");  // end object
        }

        public void WriteArrayData(JsonArray data)
        {
            writer.Write('[');  // start array

            List<IJsonData>.Enumerator enumerator = data.GetEnumerator();
            if (enumerator.MoveNext())
            {
                while (true)
                {
                    WriteData(enumerator.Current);

                    if (!enumerator.MoveNext())
                        break;

                    writer.Write(',');
                }
            }

            writer.Write(']');
        }

        public void WriteStringData(JsonString data)
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

        public void WriteNumberData(JsonNumber data)
        {
            writer.Write(data.Value.ToString());
        }

        public void WriteBooleanData(JsonBoolean data)
        {
            writer.Write(data.Value ? "true" : "false");
        }

        public void WriteNullData()
        {
            writer.Write("null");
        }

        public void WriteData(IJsonData data)
        {
            switch (data)
            {
                case JsonObject obj:
                    WriteObjectData(obj);
                    break;
                case JsonArray arr:
                    WriteArrayData(arr);
                    break;
                case JsonString str:
                    WriteStringData(str);
                    break;
                case JsonNumber num:
                    WriteNumberData(num);
                    break;
                case JsonBoolean boolData:
                    WriteBooleanData(boolData);
                    break;
                case JsonNull:
                    WriteNullData();
                    break;
                default:
                    throw new InvalidOperationException("Unexpected data type");
            }
        }
    }
}