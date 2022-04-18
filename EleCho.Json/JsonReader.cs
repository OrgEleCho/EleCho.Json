using System;
using System.Collections.Generic;
using System.IO;

namespace EleCho.Json
{
    /// <summary>
    /// Represents 
    /// </summary>
    public class JsonReader
    {
        private JsonLexer lexer;

        /// <summary>
        /// Base lexer
        /// </summary>
        public JsonLexer Lexer => lexer;

        /// <summary>
        /// Create a new instance of the <see cref="JsonReader"/> class.
        /// </summary>
        /// <param name="stream"></param>
        public JsonReader(Stream stream)
        {
            lexer = new JsonLexer(stream);
        }

        /// <summary>
        /// Create a new instance of the <see cref="JsonReader"/> class.
        /// </summary>
        /// <param name="reader"></param>
        public JsonReader(TextReader reader)
        {
            lexer = new JsonLexer(reader);
        }

        /// <summary>
        /// Create a new instance of the <see cref="JsonReader"/> class.
        /// </summary>
        /// <param name="json">JSON text</param>
        public JsonReader(string json)
        {
            lexer = new JsonLexer(new StringReader(json));
        }

        /// <summary>
        /// Create a new <see cref="JsonReader"/> and use it to read a <see cref="IJsonData"/> from the stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static IJsonData Read(Stream stream)
        {
            return new JsonReader(stream).Read();
        }

        /// <summary>
        /// Create a new <see cref="JsonReader"/> and use it to read a <see cref="IJsonData"/> from the TextReader.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IJsonData Read(TextReader reader)
        {
            return new JsonReader(reader).Read();
        }

        /// <summary>
        /// Create a new <see cref="JsonReader"/> and use it to read a <see cref="IJsonData"/> from the string.
        /// </summary>
        /// <param name="json">JSON text</param>
        /// <returns></returns>
        public static IJsonData Read(string json)
        {
            return new JsonReader(json).Read();
        }

        internal JsonObject InternalReadObject()
        {
            Dictionary<string, IJsonData> dataDict = new ();
            lexer.ReadToken();  // skip object start

            while (true)
            {
                JsonToken keyToken = lexer.PeekToken();   // move to key
                if (keyToken.Kind == JsonTokenKind.ObjectEnd)
                    break;
                if (keyToken.Kind != JsonTokenKind.String)
                    throw new InvalidOperationException("Unexpected token " + keyToken.Kind);

                string? key = lexer.ReadToken().Value;
                JsonToken colonToken = lexer.ReadToken();
                if (colonToken.Kind != JsonTokenKind.Colon)
                    throw new InvalidOperationException("Unexpected token " + keyToken.Kind);

                dataDict[key!] = InternalRead();

                JsonToken endOrCommaToken = lexer.PeekToken();
                if (endOrCommaToken.Kind == JsonTokenKind.ObjectEnd)
                    break;
                if (endOrCommaToken.Kind == JsonTokenKind.Comma)
                    lexer.ReadToken();   // skip comma
                else
                    throw new InvalidOperationException("Unexpected token " + endOrCommaToken.Kind);
            }

            lexer.ReadToken();  // skip object end
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
            lexer.ReadToken(); // skip array start

            while (true)
            {
                JsonToken valueToken = lexer.PeekToken();   // move to value
                if (valueToken.Kind == JsonTokenKind.ArrayEnd)
                    break;

                dataArr.Add(InternalRead());

                JsonToken endOrCommaToken = lexer.PeekToken();
                if (endOrCommaToken.Kind == JsonTokenKind.ArrayEnd)
                    break;
                if (endOrCommaToken.Kind == JsonTokenKind.Comma)
                    lexer.ReadToken();    // skip comma
                else
                    throw new InvalidOperationException("Unexpected token " + endOrCommaToken.Kind);
            }

            lexer.ReadToken(); // skip array end
            return new JsonArray(dataArr);
        }

        /// <summary>
        /// Only call when next token is <see cref="JsonTokenKind.String"/>
        /// </summary>
        /// <returns></returns>
        internal JsonString InternalReadString()
        {
            return new JsonString(lexer.ReadToken().Value!);
        }

        /// <summary>
        /// Only call when next token is <see cref="JsonTokenKind.Number"/>
        /// </summary>
        /// <returns></returns>
        internal JsonNumber InternalReadNumber()
        {
            return new JsonNumber(double.Parse(lexer.ReadToken().Value!));
        }

        /// <summary>
        /// Only call when next token is <see cref="JsonTokenKind.True"/> or <see cref="JsonTokenKind.False"/>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal JsonBoolean InternalReadBoolean()
        {
            JsonToken token = lexer.ReadToken();
            if (token.Kind == JsonTokenKind.True)
            {
                return (JsonBoolean)true;
            }
            else if (token.Kind == JsonTokenKind.False)
            {
                return (JsonBoolean)false;
            }
            else
            {
                throw new InvalidOperationException("Unexpected token " + lexer.PeekToken().Kind);
            }
        }

        /// <summary>
        /// Only call when next token is <see cref="JsonTokenKind.Null"/>
        /// </summary>
        /// <returns></returns>
        internal JsonNull InternalReadNull()
        {
            lexer.ReadToken();   // skip null
            return JsonNull.Null;
        }

        internal IJsonData InternalRead()
        {
            JsonToken token = lexer.PeekToken();
            return token.Kind switch
            {
                JsonTokenKind.ObjectStart => InternalReadObject(),
                JsonTokenKind.ArrayStart => InternalReadArray(),
                JsonTokenKind.String => InternalReadString(),
                JsonTokenKind.Number => InternalReadNumber(),
                JsonTokenKind.True => InternalReadBoolean(),
                JsonTokenKind.False => InternalReadBoolean(),
                JsonTokenKind.Null => InternalReadNull(),
                _ => throw new InvalidOperationException("Unexpected token " + lexer.PeekToken().Kind)
            };
        }

        /// <summary>
        /// Read a JSON object
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public JsonObject ReadObject()
        {
            JsonToken peekedToken = lexer.PeekToken();
            if (peekedToken.Kind != JsonTokenKind.ObjectStart)
                throw new InvalidOperationException("Unexpected token " + peekedToken.Kind);

            return InternalReadObject();
        }

        /// <summary>
        /// Read a JSON array
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public JsonArray ReadArray()
        {
            JsonToken peekedToken = lexer.PeekToken();
            if (peekedToken.Kind != JsonTokenKind.ArrayStart)
                throw new InvalidOperationException("Unexpected token " + peekedToken.Kind);

            return InternalReadArray();
        }

        /// <summary>
        /// Read a JSON string
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public JsonString ReadString()
        {
            JsonToken token = lexer.ReadToken();
            if (token.Kind != JsonTokenKind.String)
                throw new InvalidOperationException("Unexpected token " + token.Kind);

            return InternalReadString();
        }

        /// <summary>
        /// Read a JSON number
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public JsonNumber ReadNumber()
        {
            JsonToken token = lexer.ReadToken();
            if (token.Kind != JsonTokenKind.Number)
                throw new InvalidOperationException("Unexpected token " + token.Kind);

            return InternalReadNumber();
        }

        /// <summary>
        /// Read a JSON boolean
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public JsonBoolean ReadBoolean()
        {
            JsonToken token = lexer.ReadToken();
            if (token.Kind != JsonTokenKind.True && token.Kind != JsonTokenKind.False)
                throw new InvalidOperationException("Unexpected token " + token.Kind);

            return InternalReadBoolean();
        }

        /// <summary>
        /// Read a JSON null
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public JsonNull ReadNull()
        {
            JsonToken token = lexer.ReadToken();
            if (token.Kind != JsonTokenKind.Null)
                throw new InvalidOperationException("Unexpected token " + token.Kind);

            return InternalReadNull();
        }

        /// <summary>
        /// Read a JSON data
        /// </summary>
        /// <returns></returns>
        public IJsonData Read()
        {
            return InternalRead();
        }
    }
}