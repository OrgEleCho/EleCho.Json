using System;
using System.IO;
using System.Text;

namespace EleCho.Json
{
    public class JsonParser
    {
        private readonly TextReader reader;
        private JsonToken current = JsonToken.None;

        public JsonParser(Stream source)
        {
            reader = new StreamReader(source);
        }
        
        public JsonParser(TextReader reader)
        {
            this.reader = reader;
        }
        
        internal string NextChar()
        {
            return ((char)reader.Read()).ToString();
        }

        internal string ReadString()
        {
            int cur;
            cur = reader.Read();   // skip the first '"'

            bool escape = false;
            StringBuilder sb = new StringBuilder();
            while (cur != -1)
            {
                cur = reader.Read();
                if (escape)
                {
                    escape = false;
                    sb.Append(cur switch
                    {
                        '0' => '\0',
                        'a' => '\a',
                        'b' => '\b',
                        't' => '\t',
                        'r' => '\r',
                        'f' => '\f',
                        'n' => '\n',
                        'v' => '\v',
                        _ => cur
                    });
                }
                else
                {
                    if (cur == '\\')
                    {
                        escape = true;
                    }
                    else if (cur == '"')
                    {
                        // " is already readed, so we need to read the next char
                        return sb.ToString();
                    }
                    else
                    {
                        sb.Append((char)cur);
                    }
                }
            }

            throw new InvalidOperationException("Unexpected end of stream");
        }

        internal string ReadNumber()
        {
            int cur;
            cur = reader.Peek();

            StringBuilder sb = new StringBuilder();
            sb.Append((char)cur);
            reader.Read();  // skip the first char

            while (cur != -1)
            {
                cur = reader.Peek();
                if (cur >= '0' && cur <= '9')
                {
                    reader.Read();   // skip the char
                    sb.Append((char)cur);
                }
                else
                {
                    break;
                }
            }

            if (cur == '.')
            {
                sb.Append((char)cur);
                while (cur != -1)
                {
                    reader.Peek();
                    if (cur >= '0' && cur <= '9')
                    {
                        reader.Read();   // skip the char
                        sb.Append((char)cur);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (cur == 'e' || cur == 'E')
            {
                sb.Append((char)cur);

                cur = reader.Peek();
                if (cur == '+' || cur == '-')
                {
                    reader.Read();   // skip the first '+' or '-'
                    sb.Append((char)cur);
                }

                while (cur != -1)
                {
                    reader.Peek();
                    if (cur >= '0' && cur <= '9')
                    {
                        reader.Read();   // skip the char
                        sb.Append((char)cur);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return sb.ToString();
        }

        internal string ReadWord(string expectedWord)
        {
            char[] word = new char[expectedWord.Length];
            int count = reader.ReadBlock(word, 0, word.Length);
            if (count < word.Length)
                throw new InvalidOperationException("Unexpected end of stream");
            string _word = new string(word);
            if (_word != expectedWord)
                throw new InvalidOperationException($"Unexpected word '{_word}'");

            return _word;
        }

        internal int SkipEmpty()
        {
            int cur;
            while (true)
            {
                cur = reader.Peek();  // skip the whitespace
                if (char.IsWhiteSpace((char)cur))
                {
                    reader.Read();
                }
                else
                {
                    break;
                }
            }

            return cur;
        }

        public JsonToken PeekToken()
        {
            if (current.IsNone())
            {
                current = ReadToken();
            }

            return current;
        }
        public JsonToken ReadToken()
        {
            if (!current.IsNone())
            {
                JsonToken rst = current;
                current = JsonToken.None;
                return rst;
            }
            
            int curChar = reader.Peek();
            if (curChar == -1)
                return new JsonToken(JsonTokenKind.None, string.Empty);
            if (char.IsWhiteSpace((char)curChar))
                curChar = SkipEmpty();          // skip the whitespace

            return curChar switch
            {
                '{' => new JsonToken(JsonTokenKind.ObjectStart, NextChar()),
                '}' => new JsonToken(JsonTokenKind.ObjectEnd, NextChar()),
                '[' => new JsonToken(JsonTokenKind.ArrayStart, NextChar()),
                ']' => new JsonToken(JsonTokenKind.ArrayEnd, NextChar()),
                '"' => new JsonToken(JsonTokenKind.String, ReadString()),
                ':' => new JsonToken(JsonTokenKind.Colon, NextChar()),
                ',' => new JsonToken(JsonTokenKind.Comma, NextChar()),
                't' => new JsonToken(JsonTokenKind.True, ReadWord("true")),
                'f' => new JsonToken(JsonTokenKind.False, ReadWord("false")),
                'n' => new JsonToken(JsonTokenKind.Null, ReadWord("null")),
                (>= '0' and <= '9') or '-' => new JsonToken(JsonTokenKind.Number, ReadNumber()),
                _ => throw new InvalidOperationException($"Unexpected char '{(char)curChar}'")
            };
        }
    }
}