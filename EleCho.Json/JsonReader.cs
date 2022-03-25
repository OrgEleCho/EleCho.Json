using System;
using System.Collections.Generic;
using System.IO;

namespace EleCho.Json
{

    public class JsonReader
    {
        private JsonParser parser;
        public JsonParser Parser => parser;

        public JsonReader(Stream stream)
        {
            parser = new JsonParser(stream);
        }
        public JsonReader(TextReader reader)
        {
            parser = new JsonParser(reader);
        }

        internal JsonObject InternalReadObject()
        {
            Dictionary<string, IJsonData> dataDict = new ();
            parser.ReadToken();  // skip object start

            while (true)
            {
                JsonToken keyToken = parser.PeekToken();   // move to key
                if (keyToken.Kind == JsonTokenKind.ObjectEnd)
                    break;
                if (keyToken.Kind != JsonTokenKind.String)
                    throw new InvalidOperationException("Unexpected token " + keyToken.Kind);

                string key = parser.ReadToken().Value;
                JsonToken colonToken = parser.ReadToken();
                if (colonToken.Kind != JsonTokenKind.Colon)
                    throw new InvalidOperationException("Unexpected token " + keyToken.Kind);

                dataDict[key] = InternalRead();

                JsonToken endOrCommaToken = parser.PeekToken();
                if (endOrCommaToken.Kind == JsonTokenKind.ObjectEnd)
                    break;
                if (endOrCommaToken.Kind == JsonTokenKind.Comma)
                    parser.ReadToken();   // skip comma
                else
                    throw new InvalidOperationException("Unexpected token " + endOrCommaToken.Kind);
            }

            parser.ReadToken();  // skip object end
            return new JsonObject(dataDict);
        }

        /// <summary>
        /// take a array from stream (thw following token must be array start, it will be skipped without any check)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal JsonArray InternalReadArray()
        {
            List<IJsonData> dataArr = new ();
            parser.ReadToken(); // skip array start

            while (true)
            {
                JsonToken valueToken = parser.PeekToken();   // move to value
                if (valueToken.Kind == JsonTokenKind.ArrayEnd)
                    break;

                dataArr.Add(InternalRead());

                JsonToken endOrCommaToken = parser.PeekToken();
                if (endOrCommaToken.Kind == JsonTokenKind.ArrayEnd)
                    break;
                if (endOrCommaToken.Kind == JsonTokenKind.Comma)
                    parser.ReadToken();    // skip comma
                else
                    throw new InvalidOperationException("Unexpected token " + endOrCommaToken.Kind);
            }

            parser.ReadToken(); // skip array end
            return new JsonArray(dataArr);
        }

        internal JsonString InternalReadString()
        {
            return new JsonString(parser.ReadToken().Value);
        }

        internal JsonNumber InternalReadNumber()
        {
            return new JsonNumber(double.Parse(parser.ReadToken().Value));
        }

        internal JsonBoolean InternalReadBoolean()
        {
            JsonToken token = parser.ReadToken();
            if (token.Kind == JsonTokenKind.True)
            {
                return new JsonBoolean(true);
            }
            else if (token.Kind == JsonTokenKind.False)
            {
                return new JsonBoolean(false);
            }
            else
            {
                throw new InvalidOperationException("Unexpected token " + parser.PeekToken().Kind);
            }
        }

        internal JsonNull InternalReadNull()
        {
            parser.ReadToken();   // skip null
            return new JsonNull();
        }

        public IJsonData InternalRead()
        {
            JsonToken token = parser.PeekToken();
            return token.Kind switch
            {
                JsonTokenKind.ObjectStart => InternalReadObject(),
                JsonTokenKind.ArrayStart => InternalReadArray(),
                JsonTokenKind.String => InternalReadString(),
                JsonTokenKind.Number => InternalReadNumber(),
                JsonTokenKind.True => InternalReadBoolean(),
                JsonTokenKind.False => InternalReadBoolean(),
                JsonTokenKind.Null => InternalReadNull(),
                _ => throw new InvalidOperationException("Unexpected token " + parser.PeekToken().Kind)
            };
        }

        public JsonObject ReadObject()
        {
            JsonToken peekedToken = parser.PeekToken();
            if (peekedToken.Kind != JsonTokenKind.ObjectStart)
                throw new InvalidOperationException("Unexpected token " + peekedToken.Kind);

            return InternalReadObject();
        }

        public JsonArray ReadArray()
        {
            JsonToken peekedToken = parser.PeekToken();
            if (peekedToken.Kind != JsonTokenKind.ArrayStart)
                throw new InvalidOperationException("Unexpected token " + peekedToken.Kind);

            return InternalReadArray();
        }

        public JsonString ReadString()
        {
            JsonToken token = parser.ReadToken();
            if (token.Kind != JsonTokenKind.String)
                throw new InvalidOperationException("Unexpected token " + token.Kind);

            return InternalReadString();
        }

        public JsonNumber ReadNumber()
        {
            JsonToken token = parser.ReadToken();
            if (token.Kind != JsonTokenKind.Number)
                throw new InvalidOperationException("Unexpected token " + token.Kind);

            return InternalReadNumber();
        }

        public JsonBoolean ReadBoolean()
        {
            JsonToken token = parser.ReadToken();
            if (token.Kind != JsonTokenKind.True && token.Kind != JsonTokenKind.False)
                throw new InvalidOperationException("Unexpected token " + token.Kind);

            return InternalReadBoolean();
        }

        public JsonNull ReadNull()
        {
            JsonToken token = parser.ReadToken();
            if (token.Kind != JsonTokenKind.Null)
                throw new InvalidOperationException("Unexpected token " + token.Kind);

            return InternalReadNull();
        }

        public IJsonData Read()
        {
            return InternalRead();
        }
    }
}