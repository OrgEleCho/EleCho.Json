using System;
using System.IO;
using System.Text;

namespace EleCho.Json
{
    /// <summary>
    /// JSON lexer, JSON tokens are produced by the lexer. <br/>
    /// JSON 此法分析器, JSON 单词由词法分析器产生。
    /// </summary>
    public class JsonLexer
    {
        private readonly TextReader reader;
        private JsonToken current = JsonToken.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLexer"/> class. <br/>
        /// 初始化 <see cref="JsonLexer"/> 类的新实例。
        /// </summary>
        /// <param name="source"><see cref="Stream"/> to read.  <br/>要读取的 <see cref="Stream"/></param>
        public JsonLexer(Stream source)
        {
            reader = new StreamReader(source);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLexer"/> class. <br/>
        /// 初始化 <see cref="JsonLexer"/> 类的新实例。
        /// </summary>
        /// <param name="reader"><see cref="TextReader"/> to use.  <br/>要使用的 <see cref="TextReader"/></param>
        public JsonLexer(TextReader reader)
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
            cur = reader.Read();   // skip the first '"' 跳过第一个双引号

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
                        // " is already readed, so we need to read the next char, 双引号已经被读取了, 所以我们不需要读取下一个字符
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
            reader.Read();  // skip the first char. 跳过第一个字符

            while (cur != -1)
            {
                cur = reader.Peek();
                if (cur >= '0' && cur <= '9')
                {
                    reader.Read();   // skip the char. 跳过字符
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
                        reader.Read();   // skip the char. 跳过字符
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
                    reader.Read();   // skip the first '+' or '-'. 跳过第一个'+'或'-'
                    sb.Append((char)cur);
                }

                while (cur != -1)
                {
                    reader.Peek();
                    if (cur >= '0' && cur <= '9')
                    {
                        reader.Read();   // skip the char. 跳过字符
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
                cur = reader.Peek();  // skip the whitespace. 跳过空白
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

        /// <summary>
        /// Reads the next token without changing the state of the lexer.
        /// Returns the next available token and cache the token. (Calling <see cref="ReadToken"/> will fist use the cache) <br/>
        /// 读取下一个令牌, 但不改变词法分析器的状态。
        /// 返回下一个可用的令牌, 并缓存该令牌。(调用 <see cref="ReadToken"/> 将优先使用缓存)
        /// </summary>
        /// <returns></returns>
        public JsonToken PeekToken()
        {
            if (current.IsNone())
            {
                current = ReadToken();
            }

            return current;
        }

        /// <summary>
        /// Reads the next token from the lexer. (If <see cref="PeekToken"/> was called before read, cache token will be first use) <br/>
        /// 从词法分析器读取下一个令牌。(如果 <see cref="PeekToken"/> 在读取之前被调用，缓存令牌将首先使用)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
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
                curChar = SkipEmpty();          // skip the whitespace. 跳过空白

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