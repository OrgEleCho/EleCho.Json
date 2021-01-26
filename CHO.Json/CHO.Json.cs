using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace CHO.Json
{
    /// <summary>
    /// 表示Json数据类型异常
    /// </summary>
    public class JsonDataTypeException : Exception
    {
        private readonly string message;
        public JsonDataTypeException(string message)
        {
            this.message = message;
        }
        public override string Message
        {
            get
            {
                return message;
            }
        }
    }

    /// <summary>
    /// 表示分析Json数据时的非法字符异常
    /// </summary>
    public class InvalidCharParseException : Exception
    {
        private readonly string message;
        private readonly int index;
        public InvalidCharParseException(string message, int index)
        {
            this.message = message;
            this.index = index;
        }
        public override string Message
        {
            get => message;
        }
        public int Index
        {
            get => index;
        }
    }

    /// <summary>
    /// 表示分析Json数据时某个数据未结束导致的异常
    /// </summary>
    public class NotClosedParseException : Exception
    {
        private readonly string message;
        private readonly int index;
        public NotClosedParseException(string message, int index)
        {
            this.message = message;
            this.index = index;
        }
        public override string Message
        {
            get => message;
        }
        public int Index
        {
            get => index;
        }
    }

    /// <summary>
    /// 表示函数错误调用错误 (此异常一般不会出现)
    /// </summary>
    public class ParseCallError : Exception
    {
        private readonly string message;
        private readonly int index;
        public ParseCallError(string message, int index)
        {
            this.message = message;
            this.index = index;
        }
        public override string Message
        {
            get => message;
        }
        public int Index
        {
            get => index;
        }
    }

    /// <summary>
    /// 表示分析时的未知错误 (此异常一般不会出现)
    /// </summary>
    public class ParseUnknownError : Exception
    {
        private readonly string message;
        private readonly int index;
        public ParseUnknownError(string message, int index)
        {
            this.message = message;
            this.index = index;
        }
        public override string Message
        {
            get => message;
        }
        public int Index
        {
            get => index;
        }
    }

    /// <summary>
    /// 表示分析Json数据时的Json格式异常
    /// </summary>
    public class JsonFormatParseException : FormatException
    {
        private readonly string message;
        private readonly int index;
        public JsonFormatParseException(string message, int index)
        {
            this.message = message;
            this.index = index;
        }
        public override string Message
        {
            get => message;
        }
        public int Index
        {
            get => index;
        }
    }
    /// <summary>
    /// Json数据类型, 表示Json数据中包含数据的类型
    /// </summary>
    public enum JsonDataType
    {
        Null,
        Object,
        Array,
        String,
        Integer,
        Float,
        Double,
        Boolean
    }

    /// <summary>
    /// 单个Json数据
    /// </summary>
    public class JsonData
    {
        protected JsonDataType dataType;
        protected Object content;

        public JsonDataType DataType
        {
            get => dataType;
        }

        protected JsonData() { }

        /// <summary>
        /// 创建一个Object类型的空Json数据
        /// </summary>
        /// <returns>包含Object类型Json数据的JsonData实例</returns>
        public static JsonData CreateObject()
        {
            JsonData result = new JsonData
            {
                dataType = JsonDataType.Object,
                content = new Dictionary<JsonData, JsonData>()
            };
            return result;
        }
        /// <summary>
        /// 创建一个Array类型的空Json数据
        /// </summary>
        /// <returns>包含Array类型Json数据的JsonData实例</returns>
        public static JsonData CreateArray()
        {
            JsonData result = new JsonData
            {
                dataType = JsonDataType.Array,
                content = new List<JsonData>()
            };
            return result;
        }
        /// <summary>
        /// 创建一个String类型的空Json数据
        /// </summary>
        /// <returns>包含String类型Json数据的JsonData实例</returns>
        public static JsonData CreateString()
        {
            JsonData result = new JsonData
            {
                dataType = JsonDataType.String,
                content = string.Empty
            };
            return result;
        }
        /// <summary>
        /// 创建一个Integer类型的空Json数据
        /// </summary>
        /// <returns></returns>
        public static JsonData CreateInteger()
        {
            JsonData result = new JsonData
            {
                dataType = JsonDataType.Integer,
                content = (int)0
            };
            return result;
        }
        /// <summary>
        /// 创建一个Float类型的空Json数据
        /// </summary>
        /// <returns></returns>
        public static JsonData CreateFloat()
        {
            JsonData result = new JsonData
            {
                dataType = JsonDataType.Integer,
                content = (float)0
            };
            return result;
        }
        /// <summary>
        /// 创建一个Double类型的空Json数据
        /// </summary>
        /// <returns>包含Double类型Json数据的JsonData实例</returns>
        public static JsonData CreateDouble()
        {
            JsonData result = new JsonData
            {
                dataType = JsonDataType.Double,
                content = (double)0.0
            };
            return result;
        }
        /// <summary>
        /// 创建一个Bool类型的空Json数据
        /// </summary>
        /// <returns>包含Bool类型Json数据的JsonData实例</returns>
        public static JsonData CreateBoolean()
        {
            JsonData result = new JsonData
            {
                dataType = JsonDataType.Boolean,
                content = false
            };
            return result;
        }
        /// <summary>
        /// 创建一个空Json数据
        /// </summary>
        /// <returns>包含空Json数据的JsonData实例</returns>
        public static JsonData CreateNull()
        {
            JsonData result = new JsonData
            {
                dataType = JsonDataType.Null,
                content = null
            };
            return result;
        }
        /// <summary>
        /// 根据参数创建一个包含相应类型Json数据的JsonData实例
        /// </summary>
        /// <param name="obj">参数</param>
        /// <returns>对应JsonData实例</returns>
        public static JsonData Create(object obj)
        {
            if (obj == null)
            {
                return CreateNull();
            }
            else
            {
                Type objType = obj.GetType();

                JsonData result = new JsonData();
                if (typeof(string).IsAssignableFrom(objType))
                {
                    result.dataType = JsonDataType.String;
                    result.content = obj;
                    return result;
                }
                else if (typeof(bool).IsAssignableFrom(objType))
                {
                    result.dataType = JsonDataType.Boolean;
                    result.content = obj;
                    return result;
                }
                else if (typeof(int).IsAssignableFrom(objType))
                {
                    result.dataType = JsonDataType.Integer;
                    result.content = obj;
                    return result;
                }
                else if (typeof(float).IsAssignableFrom(objType))
                {
                    result.dataType = JsonDataType.Float;
                    result.content = obj;
                    return result;
                }
                else if (typeof(double).IsAssignableFrom(objType))
                {
                    result.dataType = JsonDataType.Double;
                    result.content = obj;
                    return result;
                }
                else if (typeof(IDictionary).IsAssignableFrom(objType))
                {
                    Dictionary<JsonData, JsonData> objcntnt = new Dictionary<JsonData, JsonData>();
                    foreach (object i in (obj as IDictionary).Keys)
                    {
                        objcntnt[Create(i)] = Create((obj as IDictionary)[i]);
                    }

                    result.dataType = JsonDataType.Object;
                    result.content = objcntnt;
                    return result;
                }
                else if (typeof(IList).IsAssignableFrom(objType))
                {
                    List<JsonData> objcntnt = new List<JsonData>();
                    foreach (object i in obj as IList)
                    {
                        objcntnt.Add(Create(i));
                    }

                    result.dataType = JsonDataType.Array;
                    result.content = objcntnt;
                    return result;
                }
                else
                {
                    Dictionary<JsonData, JsonData> objcntnt = new Dictionary<JsonData, JsonData>();
                    FieldInfo[] fs = objType.GetFields();

                    foreach (FieldInfo i in fs)
                    {
                        objcntnt[Create(i.Name)] = Create(i.GetValue(obj));
                    }

                    result.dataType = JsonDataType.Object;
                    result.content = objcntnt;
                    return result;
                }
            }
        }


        public static T Deserialize<T>(string jsonText)
        {
            return (T)ConvertToInstance(Parse(jsonText), typeof(T));
        }
        public static bool TryDeserialize<T>(string jsonText, out T result)
        {
            try
            {
                result = Deserialize<T>(jsonText);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
        public static string Serialize(object obj)
        {
            return ConvertToText(Create(obj));
        }
        public static bool TrySerialize(object obj, out string result)
        {
            try
            {
                result = Serialize(obj);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        protected static object ConvertToInstance(JsonData jsonData, Type resultType)
        {
            if (jsonData.DataType == JsonDataType.Null || jsonData.content == null)
            {
                return null;
            }
            else if (resultType == typeof(JsonData))
            {
                return jsonData;
            }
            else if (resultType.IsAssignableFrom(jsonData.content.GetType()))
            {
                return Convert.ChangeType(jsonData.content, resultType);
            }
            else
            {
                object result = System.Activator.CreateInstance(resultType);
                if (jsonData.dataType == JsonDataType.Object)
                {
                    if (CheckInterface(resultType, typeof(IDictionary), out Type[] _) && CheckInterface(resultType, typeof(IDictionary<,>), out Type[] geneticArgs))
                    {
                        foreach (JsonData i in (jsonData.content as IDictionary<JsonData, JsonData>).Keys)
                        {
                            (result as IDictionary)[ConvertToInstance(i, geneticArgs[0])] = ConvertToInstance((jsonData.content as IDictionary<JsonData, JsonData>)[i], geneticArgs[1]);
                        }
                        return result;
                    }
                    else
                    {
                        foreach (JsonData i in (jsonData.content as IDictionary<JsonData, JsonData>).Keys)
                        {
                            if (i.dataType == JsonDataType.String)
                            {
                                FieldInfo field = resultType.GetField(i.content as string);
                                if (field != null)
                                {
                                    JsonData value = (jsonData.content as IDictionary<JsonData, JsonData>)[i];
                                    field.SetValue(result, ConvertToInstance(value, field.FieldType));
                                }
                                else
                                {
                                    // 表示遍历JsonData时, 未找到用户指定类中的对应字段
                                }
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException("Object类型的JsonData在反序列化为自定义类型时, 要求JsonData的键必须是String类型");
                            }
                        }
                        return result;
                    }
                }
                else if (jsonData.dataType == JsonDataType.Array)
                {
                    if (CheckInterface(resultType, typeof(IList), out Type[] _) && CheckInterface(resultType, typeof(IList<>), out Type[] geneticTypes))
                    {
                        foreach (JsonData i in jsonData.content as IList<JsonData>)
                        {
                            (result as IList).Add(ConvertToInstance(i, geneticTypes[0]));
                        }
                        return result;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Array类型的JsonData在反序列化为类实例时, 要求类必须继承IList与IList<T>接口");
                    }
                }
                else
                {
                    throw new JsonDataTypeException("未知类型的JsonData数据");
                }
            }
        }
        /// <summary>
        /// 将JsonData实例中的数据反序列化为指定类型的实例
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="jsonData">要进行操作的JsonData实例</param>
        /// <returns></returns>
        public static T ConvertToInstance<T>(JsonData jsonData)
        {
            return (T)ConvertToInstance(jsonData, typeof(T));
        }
        /// <summary>
        /// 将JsonData实例中的数据转换成Json文本
        /// 注意: 本程序集可以读取, 但其他容错值较小的Json操作程序集或包可能无法读取它
        /// </summary>
        /// <returns>String类型的Json文本</returns>
        public static string ConvertToText(JsonData jsonData)
        {
            switch (jsonData.dataType)
            {
                case JsonDataType.Object:
                    List<string> pairs = new List<string>();
                    foreach (JsonData key in (jsonData.content as Dictionary<JsonData, JsonData>).Keys)
                    {
                        pairs.Add(string.Format("{0}: {1}", ConvertToText(key), ConvertToText((jsonData.content as Dictionary<JsonData, JsonData>)[key])));
                    }
                    return string.Format("{0}{1}{2}", "{", string.Join(", ", pairs), "}");
                case JsonDataType.Array:
                    List<string> elements = new List<string>();
                    foreach (JsonData element in (jsonData.content as List<JsonData>))
                    {
                        elements.Add(ConvertToText(element));
                    }
                    return string.Format("[{0}]", string.Join(", ", elements));
                case JsonDataType.String:
                    return string.Format("\"{0}\"", ((string)jsonData.content).Replace("\\", "\\\\").Replace("\'", "\\\'").Replace("\"", "\\\"").Replace("\0", "\\0").Replace("\a", "\\a").Replace("\b", "\\b").Replace("\f", "\\f").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t").Replace("\v", "\\v"));
                case JsonDataType.Integer:
                    return ((int)jsonData.content).ToString();
                case JsonDataType.Float:
                    return ((float)jsonData.content).ToString();
                case JsonDataType.Double:
                    return ((double)jsonData.content).ToString();
                case JsonDataType.Boolean:
                    return (bool)jsonData.content ? "true" : "false";
                case JsonDataType.Null:
                    return "null";
                default:
                    throw new JsonDataTypeException("所访问数据类型未知");
            }
        }
        public static bool TryConvertToInstance<T>(JsonData jsonData, out T result)
        {
            try
            {
                result = ConvertToInstance<T>(jsonData);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
        public static bool TryConvertToText(JsonData jsonData, out string result)
        {
            try
            {
                result = ConvertToText(jsonData);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }


        /// <summary>
        /// 从包含Object类型Json数据的JsonData实例中获取所包含的数据
        /// </summary>
        /// <returns>Dictionary<JsonData, JsonData>实例</returns>
        public Dictionary<JsonData, JsonData> GetObject()
        {
            if (dataType == JsonDataType.Object)
            {
                return (Dictionary<JsonData, JsonData>)content;
            }
            else
            {
                throw new JsonDataTypeException("所访问数据不是Object类型");
            }
        }
        /// <summary>
        /// 从包含Object类型Json数据的JsonData实例中获取所包含的数据
        /// </summary>
        /// <returns>List<JsonData>实例</returns>
        public List<JsonData> GetArray()
        {
            if (dataType == JsonDataType.Array)
            {
                return (List<JsonData>)content;
            }
            else
            {
                throw new JsonDataTypeException("所访问数据不是Array类型");
            }
        }
        /// <summary>
        /// 从包含String类型Json数据的JsonData实例中获取所包含的数据
        /// </summary>
        /// <returns>String值</returns>
        public string GetString()
        {
            if (dataType == JsonDataType.String)
            {
                return (string)content;
            }
            else
            {
                throw new JsonDataTypeException("所访问数据不是String类型");
            }
        }
        /// <summary>
        /// 从包含Interger类型Json数据的JsonData实例中获取所包含的数据
        /// </summary>
        /// <returns>Integer值</returns>
        public int GetInteger()
        {
            if (dataType == JsonDataType.Integer)
            {
                return (int)content;
            }
            else
            {
                throw new JsonDataTypeException("所访问数据不是Integer类型");
            }
        }
        /// <summary>
        /// 从包含Float类型Json数据的JsonData实例中获取所包含的数据
        /// </summary>
        /// <returns>Float值</returns>
        public float GetFloat()
        {
            if (dataType == JsonDataType.Float)
            {
                return (float)content;
            }
            else
            {
                throw new JsonDataTypeException("所访问数据不是Float类型");
            }
        }
        /// <summary>
        /// 从包含Double类型Json数据的JsonData实例中获取所包含的数据
        /// </summary>
        /// <returns>Double值</returns>
        public double GetDouble()
        {
            if (dataType == JsonDataType.Double)
            {
                return (double)content;
            }
            else
            {
                throw new JsonDataTypeException("所访问数据不是Double类型");
            }
        }
        /// <summary>
        /// 从包含Bool类型Json数据的JsonData实例中获取所包含的数据
        /// </summary>
        /// <returns>Boolean值</returns>
        public bool GetBoolean()
        {
            if (dataType == JsonDataType.Boolean)
            {
                return (bool)content;
            }
            else
            {
                throw new JsonDataTypeException("所访问数据不是Bool类型");
            }
        }

        /// <summary>
        /// 从包含Json数据的JsonData实例中获取所包含的数据
        /// </summary>
        /// <returns>对应数据的实例</returns>
        public object GetData()
        {
            return content;
        }

        public static implicit operator JsonData(Dictionary<JsonData, JsonData> value)
        {
            return Create(value);
        }
        public static implicit operator JsonData(List<JsonData> value)
        {
            return Create(value);
        }
        public static implicit operator JsonData(string value)
        {
            return Create(value);
        }
        public static implicit operator JsonData(int value)
        {
            return Create(value);
        }
        public static implicit operator JsonData(float value)
        {
            return Create(value);
        }
        public static implicit operator JsonData(double value)
        {
            return Create(value);
        }
        public static implicit operator JsonData(bool value)
        {
            return Create(value);
        }

        public JsonData this[JsonData by]
        {
            get
            {
                switch (dataType)
                {
                    case JsonDataType.Object:
                        return (content as Dictionary<JsonData, JsonData>)[by];
                    case JsonDataType.Array:
                        if (by.dataType == JsonDataType.Integer)
                        {
                            return (content as List<JsonData>)[(int)by.content];
                        }
                        else
                        {
                            throw new JsonDataTypeException("应使用Integer数据");
                        }
                    default:
                        throw new JsonDataTypeException("操作对该Json数据无效");
                }
            }
            set
            {
                switch (dataType)
                {
                    case JsonDataType.Object:
                        (content as Dictionary<JsonData, JsonData>)[by] = value;
                        break;
                    case JsonDataType.Array:
                        if (by.dataType == JsonDataType.Double)
                        {
                            (content as List<JsonData>)[by.GetInteger()] = value;
                        }
                        else
                        {
                            throw new JsonDataTypeException("应使用Integer数据");
                        }
                        break;
                    default:
                        throw new JsonDataTypeException("操作对该Json数据无效");
                }
            }
        }

        /// <summary>
        /// 获取Array或Object的内容个数
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            if (dataType == JsonDataType.Array)
            {
                return ((List<JsonData>)this.content).Count;
            }
            else if (dataType == JsonDataType.Object)
            {
                return ((Dictionary<JsonData, JsonData>)this.content).Count;
            }
            else
            {
                throw new JsonDataTypeException("所访问数据不是Array或Object类型");
            }
        }
        /// <summary>
        /// 获取Object的键集合
        /// </summary>
        /// <returns>Dictionary<JsonData, JsonData>.KeyCollection</returns>
        public JsonData[] GetKeys()
        {
            if (dataType == JsonDataType.Object)
            {
                return (content as Dictionary<JsonData, JsonData>).Keys.ToArray();
            }
            else
            {
                throw new JsonDataTypeException("所访问数据不是Object类型");
            }
        }

        /// <summary>
        /// 获取Object的值集合
        /// </summary>
        /// <returns>Dictionary<JsonData, JsonData>.ValueCollection</returns>
        public JsonData[] GetValues()
        {
            if (dataType == JsonDataType.Object)
            {
                return (content as Dictionary<JsonData, JsonData>).Values.ToArray();
            }
            else
            {
                throw new JsonDataTypeException("所访问数据不是Object类型");
            }
        }

        /// <summary>
        /// 在Object设置或更改原有键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Set(JsonData key, JsonData value)
        {
            if (dataType == JsonDataType.Object)
            {
                if ((content as Dictionary<JsonData, JsonData>).ContainsKey(key))
                {
                    (content as Dictionary<JsonData, JsonData>)[key] = value;
                }
            }
            else
            {
                throw new JsonDataTypeException("所访问数据不是Object类型");
            }
        }

        /// <summary>
        /// 在Array中添加一个元素
        /// </summary>
        /// <param name="element">元素</param>
        public void Add(JsonData element)
        {
            if (dataType == JsonDataType.Array)
            {
                (content as List<JsonData>).Add(element);
            }
            else
            {
                throw new JsonDataTypeException("所访问数据不是Array类型");
            }
        }

        /// <summary>
        /// 根据键从Object删除匹配键值对或在Array中删除第一个匹配元素
        /// </summary>
        /// <param name="basis">检索一句</param>
        public void Remove(JsonData basis)
        {
            switch (dataType)
            {
                case JsonDataType.Object:
                    (content as Dictionary<JsonData, JsonData>).Remove(basis);
                    break;
                case JsonDataType.Array:
                    (content as List<JsonData>).Remove(basis);
                    break;
                default:
                    throw new JsonDataTypeException("操作对该Json数据无效");
            }
        }

        /// <summary>
        /// 从Array中删除指定索引的元素
        /// </summary>
        /// <param name="index">索引</param>
        public void RemoveAt(int index)
        {
            if (dataType == JsonDataType.Array)
            {
                (content as List<JsonData>).RemoveAt(index);
            }
            else
            {
                throw new JsonDataTypeException("所访问数据不是Array类型");
            }
        }

        /// <summary>
        /// 检查Object是否含有某键或Array是否含有某元素
        /// </summary>
        /// <param name="basis">检索依据</param>
        public bool Contains(JsonData basis)
        {
            switch (dataType)
            {
                case JsonDataType.Object:
                    return (content as Dictionary<JsonData, JsonData>).ContainsKey(basis);
                case JsonDataType.Array:
                    return (content as List<JsonData>).Contains(basis);
                default:
                    throw new JsonDataTypeException("操作对该Json数据无效");
            }
        }

        //private readonly static char[] EmptyChars = " \n\r\t\0".ToCharArray();
        //private readonly static char[] ParseEndChars = " :,}]\" \n\r\t\0".ToCharArray();
        //private readonly static char[] ParseNumberChars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm0123456789.-".ToCharArray();
        //private readonly static char[] ParseWordChars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm".ToCharArray();
        //private readonly static char[] NumberChars = "0123456789-".ToCharArray();

        private static bool IsEmptyChar(char c)
        {
            return c == ' ' || c == '\n' || c == '\r' || c == '\t' || c == '\0';
        }
        private static bool IsLetterChar(char c)
        {
            return c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z';
        }
        private static bool IsNumberStartChar(char c)
        {
            return c >= '0' && c <= '9';
        }
        private static bool IsNumberParsingChar(char c)
        {
            return IsNumberStartChar(c) || c == '.' || c == '-' || IsLetterChar(c);
        }
        private static bool IsWordParseChar(char c)
        {
            return IsLetterChar(c) || c == '_';
        }
        private static bool IsEndChar(char c)
        {
            return IsEmptyChar(c) || c == '"' || c == ':' || c == ',' || c == ']' || c == '}';
        }
        protected static bool CheckInterface(Type targetType, Type interfaceType, out Type[] geneticTypes)
        {
            foreach (Type i in targetType.GetInterfaces())
            {
                if (i.IsGenericType)
                {
                    if (i.GetGenericTypeDefinition().Equals(interfaceType))
                    {
                        geneticTypes = i.GetGenericArguments();
                        return true;
                    }
                }
                else
                {
                    if (i.Equals(interfaceType))
                    {
                        geneticTypes = null;
                        return true;
                    }
                }
            }
            geneticTypes = null;
            return false;
        }

        private enum ArrayParseState
        {
            NotStart,
            ElementStart,
            ElementEnd
        }
        private enum ObjectParseState
        {
            NotStart,
            KeyStart,
            KeyEnd,
            ValueStart,
            ValueEnd
        }
        protected static JsonData ParseData(ref char[] source, ref int offset)
        {
            for (; offset < source.Length; offset++)
            {
                if (IsEmptyChar(source[offset]))
                {
                    continue;
                }
                else
                {
                    if (source[offset] == '{')
                    {
                        return ParseObject(ref source, ref offset);
                    }
                    else if (source[offset] == '[')
                    {
                        return ParseArray(ref source, ref offset);
                    }
                    else if (source[offset] == '"')
                    {
                        return ParseString(ref source, ref offset);
                    }
                    else if (IsNumberStartChar(source[offset]))
                    {
                        return ParseNumber(ref source, ref offset);
                    }
                    else if (IsWordParseChar(source[offset]))
                    {
                        return ParseWord(ref source, ref offset);
                    }
                    else if (source[offset] == '}' || source[offset] == ']')
                    {
                        offset--;
                        return null;
                    }
                    else
                    {
                        throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]), offset);
                    }
                }
            }
            return CreateNull();
        }
        protected static JsonData ParseString(ref char[] source, ref int offset)
        {
            bool parsing = false;
            bool escape = false;
            StringBuilder content = new StringBuilder();

            for (; offset < source.Length; offset++)
            {
                if (escape)
                {
                    switch (source[offset])
                    {
                        case 'a':
                            content.Append('\a');
                            break;
                        case 'b':
                            content.Append('\b');
                            break;
                        case 'f':
                            content.Append('\f');
                            break;
                        case 'n':
                            content.Append('\n');
                            break;
                        case 'r':
                            content.Append('\r');
                            break;
                        case 't':
                            content.Append('\t');
                            break;
                        case 'v':
                            content.Append('\v');
                            break;
                        default:
                            content.Append(source[offset]);
                            break;
                    }
                    escape = false;
                }
                else if (parsing)
                {
                    if (source[offset] == '"')
                    {
                        return Create(content.ToString());
                    }
                    else if (source[offset] == '\\')
                    {
                        escape = true;
                    }
                    else
                    {
                        content.Append(source[offset]);
                    }
                }
                else
                {
                    if (IsEmptyChar(source[offset]))
                    {
                        continue;
                    }
                    else if (source[offset] == '"')
                    {
                        parsing = true;
                    }
                    else
                    {
                        throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]), offset);
                    }
                }
            }
            throw new NotClosedParseException("String无结束符号", offset);
        }
        protected static JsonData ParseNumber(ref char[] source, ref int offset)
        {
            bool parsing = false;
            StringBuilder content = new StringBuilder();

            for (; offset < source.Length; offset++)
            {
                if (parsing)
                {
                    if (IsNumberParsingChar(source[offset]))
                    {
                        content.Append(source[offset]);
                    }
                    else if (IsEndChar(source[offset]))
                    {
                        offset--;
                        break;
                    }
                    else
                    {
                        throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]), offset);
                    }
                }
                else
                {
                    if (IsEmptyChar(source[offset]))
                    {
                        continue;
                    }
                    else if (IsNumberStartChar(source[offset]))
                    {
                        parsing = true;
                        content.Append(source[offset]);
                    }
                    else
                    {
                        throw new ParseCallError("未知错误! 一般情况下,此错误不会被触发", offset);
                    }
                }
            }
            try
            {
                double doubleValue = double.Parse(content.ToString());
                int intValue = (int)doubleValue;
                float floatValue = (float)doubleValue;

                if (doubleValue == intValue)
                {
                    return Create(intValue);
                }
                else if (doubleValue == floatValue)
                {
                    return Create(floatValue);
                }
                else
                {
                    return Create(doubleValue);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected static JsonData ParseWord(ref char[] source, ref int offset)
        {
            bool parsing = false;
            StringBuilder content = new StringBuilder();

            for (; offset < source.Length; offset++)
            {
                if (parsing)
                {
                    if (IsWordParseChar(source[offset]))
                    {
                        content.Append(source[offset]);
                    }
                    else if (IsEndChar(source[offset]))
                    {
                        offset--;
                        break;
                    }
                }
                else
                {
                    if (IsEmptyChar(source[offset]))
                    {
                        continue;
                    }
                    else if (IsWordParseChar(source[offset]))
                    {
                        parsing = true;
                        content.Append(source[offset]);
                    }
                    else
                    {
                        throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]), offset);
                    }
                }
            }
            switch (content.ToString())
            {
                case "true":
                    return Create(true);
                case "false":
                    return Create(false);
                case "null":
                    return CreateNull();
                default:
                    throw new JsonDataTypeException(string.Format("未知的关键词'{0}'", content.ToString()));
            }
        }
        protected static JsonData ParseArray(ref char[] source, ref int offset)
        {
            ArrayParseState parseState = ArrayParseState.NotStart;
            List<JsonData> resultContainer = new List<JsonData>();

            for (; offset < source.Length; offset++)
            {
                switch (parseState)
                {
                    case ArrayParseState.ElementStart:
                        JsonData tempJson = ParseData(ref source, ref offset);
                        if (tempJson != null)
                        {
                            resultContainer.Add(tempJson);
                        }
                        parseState = ArrayParseState.ElementEnd;
                        break;
                    case ArrayParseState.ElementEnd:
                        if (IsEmptyChar(source[offset]))
                        {
                            continue;
                        }
                        else if (source[offset] == ',')
                        {
                            parseState = ArrayParseState.ElementStart;
                        }
                        else if (source[offset] == ']')
                        {
                            JsonData result = new JsonData
                            {
                                dataType = JsonDataType.Array,
                                content = resultContainer
                            };
                            return result;
                        }
                        else
                        {
                            throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]), offset);
                        }
                        break;
                    case ArrayParseState.NotStart:
                        if (IsEmptyChar(source[offset]))
                        {
                            continue;
                        }
                        else if (source[offset] == '[')
                        {
                            parseState = ArrayParseState.ElementStart;
                        }
                        else
                        {
                            throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]), offset);
                        }
                        break;

                }
            }

            throw new NotClosedParseException("Array无结束符号", offset);
        }
        protected static JsonData ParseObject(ref char[] source, ref int offset)
        {
            ObjectParseState parseState = ObjectParseState.NotStart;
            Dictionary<JsonData, JsonData> resultContainer = new Dictionary<JsonData, JsonData>();
            JsonData tempKey = null;

            for (; offset < source.Length; offset++)
            {
                switch (parseState)
                {
                    case ObjectParseState.KeyStart:
                        tempKey = ParseData(ref source, ref offset);
                        if (tempKey != null)
                        {
                            parseState = ObjectParseState.KeyEnd;
                        }
                        else
                        {
                            parseState = ObjectParseState.ValueEnd;
                        }
                        break;
                    case ObjectParseState.KeyEnd:
                        if (IsEmptyChar(source[offset]))
                        {
                            continue;
                        }
                        else if (source[offset] == ':')
                        {
                            parseState = ObjectParseState.ValueStart;
                        }
                        else
                        {
                            throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]), offset);
                        }
                        break;
                    case ObjectParseState.ValueStart:
                        resultContainer[tempKey] = ParseData(ref source, ref offset);
                        parseState = ObjectParseState.ValueEnd;
                        break;
                    case ObjectParseState.ValueEnd:
                        if (IsEmptyChar(source[offset]))
                        {
                            continue;
                        }
                        else if (source[offset] == ',')
                        {
                            parseState = ObjectParseState.KeyStart;
                        }
                        else if (source[offset] == '}')
                        {
                            JsonData result = new JsonData
                            {
                                dataType = JsonDataType.Object,
                                content = resultContainer
                            };
                            return result;
                        }
                        else
                        {
                            throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]), offset);
                        }
                        break;
                    case ObjectParseState.NotStart:
                        if (IsEmptyChar(source[offset]))
                        {
                            continue;
                        }
                        else if (source[offset] == '{')
                        {
                            parseState = ObjectParseState.KeyStart;
                        }
                        else
                        {
                            throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]), offset);
                        }
                        break;
                }
            }

            throw new NotClosedParseException("Object无结束符号", offset);
        }

        /// <summary>
        /// 从包含Json文本的字符数组中分析Json数据
        /// </summary>
        /// <param name="jsonText">Json文本</param>
        /// <returns>包含Json数据的JsonData实例</returns>
        public static JsonData Parse(char[] jsonText)
        {
            char[] source = jsonText;
            int offset = 0;

            JsonData result = ParseData(ref source, ref offset);

            for (offset++; offset < source.Length; offset++)
            {
                if (!IsEmptyChar(source[offset]))
                {
                    throw new JsonFormatParseException("一个Json文本中不能出现两个元素并列的情况", offset);
                }
            }

            return result;
        }
        /// <summary>
        /// 从Json文本中分析Json数据
        /// </summary>
        /// <param name="jsonText">Json文本</param>
        /// <returns>包含Json数据的JsonData实例</returns>
        public static JsonData Parse(string jsonText)
        {
            char[] source = jsonText.ToCharArray();
            int offset = 0;

            JsonData result = ParseData(ref source, ref offset);

            for (offset++; offset < source.Length; offset++)
            {
                if (!IsEmptyChar(source[offset]))
                {
                    throw new JsonFormatParseException("一个Json文本中不能出现两个元素并列的情况", offset);
                }
            }

            return result;
        }

        enum RapidParseType
        {
            Object,
            Array,
            String,
            Number,
            Word,
            None = -1
        }
        enum RapidArrayParseState
        {
            ItemAssignment,
        }
        enum RapidObjectParseState
        {
            KeyAssignment,
            ValueAssignment,
        }
        class RapidParseStackInfo
        {
            public int ParseType = -1;
            public int ParseState = 0;
            public object ParseInfo = null;
        }
        class RapidStringParseInfo
        {
            public bool Escape;
            public StringBuilder String;
        }
        class RapidNumberParseInfo
        {
            public StringBuilder Number;
        }
        class RapidWordParseInfo
        {
            public StringBuilder Word;
        }
        class RapidObjectParseInfo
        {
            public Dictionary<JsonData, JsonData> Object;
            public JsonData TempKey;
        }
        class RapidArrayParseInfo
        {
            public List<JsonData> Array;
        }
        public static JsonData RapidParseData(ref char[] source, ref int offset)
        {
            Stack<RapidParseStackInfo> infos = new Stack<RapidParseStackInfo>();
            JsonData popData = null;

            infos.Push(new RapidParseStackInfo());

            for (; offset < source.Length && infos.Count > 0; offset++)
            {
                RapidParseStackInfo info = infos.Peek();
                switch(info.ParseType)
                {
                    case (int)RapidParseType.None:
                        if (IsEmptyChar(source[offset]))
                        {
                            continue;
                        }
                        else
                        {
                            if (source[offset] == '{')
                            {
                                info.ParseType = (int)RapidParseType.Object;
                                info.ParseState = (int)RapidObjectParseState.KeyAssignment;
                                info.ParseInfo = new RapidObjectParseInfo()
                                {
                                    Object = new Dictionary<JsonData, JsonData>()
                                };

                                infos.Push(new RapidParseStackInfo());
                            }
                            else if (source[offset] == '[')
                            {
                                info.ParseType = (int)RapidParseType.Array;
                                info.ParseState = (int)RapidArrayParseState.ItemAssignment;
                                info.ParseInfo = new RapidArrayParseInfo()
                                {
                                    Array = new List<JsonData>()
                                };

                                infos.Push(new RapidParseStackInfo());
                            }
                            else if (source[offset] == '"')
                            {
                                info.ParseType = (int)RapidParseType.String;
                                info.ParseInfo = new RapidStringParseInfo()
                                {
                                    Escape = false,
                                    String = new StringBuilder()
                                };
                            }
                            else if (IsNumberStartChar(source[offset]))
                            {
                                RapidNumberParseInfo newInfo = new RapidNumberParseInfo()
                                {
                                    Number = new StringBuilder()
                                };

                                info.ParseType = (int)RapidParseType.Number;
                                info.ParseInfo = newInfo;

                                newInfo.Number.Append(source[offset]);
                            }
                            else if (IsWordParseChar(source[offset]))
                            {
                                RapidWordParseInfo newInfo = new RapidWordParseInfo()
                                {
                                    Word = new StringBuilder()
                                };

                                info.ParseType = (int)RapidParseType.Word;
                                info.ParseInfo = newInfo;

                                newInfo.Word.Append(source[offset]);
                            }
                            else if (source[offset] == '}' || source[offset] == ']')
                            {
                                offset--;

                                popData = null;
                                infos.Pop();
                            }
                            else
                            {
                                throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]), offset);
                            }
                        }
                        break;
                    case (int)RapidParseType.Object:
                        {
                            RapidObjectParseInfo parseInfo = info.ParseInfo as RapidObjectParseInfo;

                            if (IsEmptyChar(source[offset]))
                            {
                                continue;
                            }
                            else
                            {
                                if (source[offset] == '}')
                                {
                                    JsonData thisJsonObject = new JsonData()
                                    {
                                        dataType = JsonDataType.Object,
                                        content = parseInfo.Object
                                    };

                                    popData = thisJsonObject;

                                    infos.Pop();
                                }
                                else
                                {
                                    switch (info.ParseState)
                                    {
                                        case (int)RapidObjectParseState.KeyAssignment:
                                            if (source[offset] == ':')
                                            {
                                                parseInfo.TempKey = popData;
                                                info.ParseState = (int)RapidObjectParseState.ValueAssignment;

                                                infos.Push(new RapidParseStackInfo());
                                            }
                                            else
                                            {
                                                throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]), offset);
                                            }
                                            break;
                                        case (int)RapidObjectParseState.ValueAssignment:
                                            if (source[offset] == ',')
                                            {
                                                parseInfo.Object[parseInfo.TempKey] = popData;
                                                info.ParseState = (int)RapidObjectParseState.KeyAssignment;

                                                infos.Push(new RapidParseStackInfo());
                                            }
                                            else
                                            {
                                                throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]), offset);
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        }
                    case (int)RapidParseType.Array:
                        {
                            RapidArrayParseInfo parseInfo = info.ParseInfo as RapidArrayParseInfo;

                            if (IsEmptyChar(source[offset]))
                            {
                                continue;
                            }
                            else
                            {
                                if (popData != null)
                                    parseInfo.Array.Add(popData);

                                if (source[offset] == ']')
                                {
                                    JsonData thisJsonArray = new JsonData()
                                    {
                                        dataType = JsonDataType.Object,
                                        content = parseInfo.Array
                                    };

                                    popData = thisJsonArray;

                                    infos.Pop();
                                }
                                else
                                {
                                    if (source[offset] == ',')
                                    {
                                        infos.Push(new RapidParseStackInfo());
                                    }
                                    else
                                    {
                                        throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]), offset);
                                    }
                                }
                            }
                            break;
                        }
                    case (int)RapidParseType.String:
                        {
                            RapidStringParseInfo parseInfo = info.ParseInfo as RapidStringParseInfo;

                            if (parseInfo.Escape)
                            {
                                switch(source[offset])
                                {
                                    case 'a':
                                        parseInfo.String.Append('\a');
                                        break;
                                    case 'b':
                                        parseInfo.String.Append('\b');
                                        break;
                                    case 'f':
                                        parseInfo.String.Append('\f');
                                        break;
                                    case 'n':
                                        parseInfo.String.Append('\n');
                                        break;
                                    case 'r':
                                        parseInfo.String.Append('\r');
                                        break;
                                    case 't':
                                        parseInfo.String.Append('\t');
                                        break;
                                    case 'v':
                                        parseInfo.String.Append('\v');
                                        break;
                                    default:
                                        parseInfo.String.Append(source[offset]);
                                        break;
                                }
                            }
                            else
                            {
                                if (source[offset] == '"')
                                {
                                    JsonData thisJsonString = new JsonData()
                                    {
                                        dataType = JsonDataType.String,
                                        content = parseInfo.String.ToString()
                                    };

                                    popData = thisJsonString;

                                    infos.Pop();
                                }
                                else if (source[offset] == '\\')
                                {
                                    parseInfo.Escape = true;
                                }
                                else
                                {
                                    parseInfo.String.Append(source[offset]);
                                }
                            }
                            break;
                        }
                    case (int)RapidParseType.Word:
                        {
                            RapidWordParseInfo parseInfo = info.ParseInfo as RapidWordParseInfo;

                            if (IsWordParseChar(source[offset]))
                            {
                                parseInfo.Word.Append(source[offset]);
                            }
                            else
                            {
                                string wordCntnt = parseInfo.Word.ToString();
                                JsonData thisJsonWord;

                                if (wordCntnt == "true")
                                    thisJsonWord = new JsonData()
                                    {
                                        dataType = JsonDataType.Boolean,
                                        content = true
                                    };
                                else if (wordCntnt == "false")
                                    thisJsonWord = new JsonData()
                                    {
                                        dataType = JsonDataType.Boolean,
                                        content = false
                                    };
                                else if (wordCntnt == "null")
                                    thisJsonWord = JsonData.CreateNull();
                                else
                                    throw new JsonDataTypeException(string.Format("未知的关键词'{0}'", wordCntnt));

                                popData = thisJsonWord;

                                offset--;
                                infos.Pop();
                            }
                            break;
                        }
                    case (int)RapidParseType.Number:
                        {
                            RapidNumberParseInfo parseInfo = info.ParseInfo as RapidNumberParseInfo;

                            if (IsNumberParsingChar(source[offset]))
                            {
                                parseInfo.Number.Append(source[offset]);
                            }
                            else
                            {
                                string numCntnt = parseInfo.Number.ToString();
                                JsonData thisJsonNumber;

                                double doubleValue = double.Parse(numCntnt);
                                int intValue = (int)doubleValue;
                                float floatValue = (float)doubleValue;

                                if (doubleValue == intValue)
                                    thisJsonNumber = new JsonData()
                                    {
                                        dataType = JsonDataType.Integer,
                                        content = intValue
                                    };
                                else if (doubleValue == floatValue)
                                    thisJsonNumber = new JsonData()
                                    {
                                        dataType = JsonDataType.Float,
                                        content = floatValue
                                    };
                                else
                                    thisJsonNumber = new JsonData()
                                    {
                                        dataType = JsonDataType.Double,
                                        content = doubleValue
                                    };

                                popData = thisJsonNumber;

                                offset--;
                                infos.Pop();
                            }
                            break;
                        }
                }
            }

            if (infos.Count > 0)
            {
                RapidParseStackInfo info = infos.Peek();

                switch(info.ParseType)
                {
                    case (int)RapidParseType.Word:
                        {
                            RapidWordParseInfo parseInfo = info.ParseInfo as RapidWordParseInfo;

                            string wordCntnt = parseInfo.Word.ToString();
                            JsonData thisJsonWord;

                            if (wordCntnt == "true")
                                thisJsonWord = new JsonData()
                                {
                                    dataType = JsonDataType.Boolean,
                                    content = true
                                };
                            else if (wordCntnt == "false")
                                thisJsonWord = new JsonData()
                                {
                                    dataType = JsonDataType.Boolean,
                                    content = false
                                };
                            else if (wordCntnt == "null")
                                thisJsonWord = JsonData.CreateNull();
                            else
                                throw new JsonDataTypeException(string.Format("未知的关键词'{0}'", wordCntnt));

                            popData = thisJsonWord;

                            infos.Pop();
                        }
                        break;
                    case (int)RapidParseType.Number:
                        {
                            RapidNumberParseInfo parseInfo = info.ParseInfo as RapidNumberParseInfo;

                            string numCntnt = parseInfo.Number.ToString();
                            JsonData thisJsonNumber;

                            double doubleValue = double.Parse(numCntnt);
                            int intValue = (int)doubleValue;
                            float floatValue = (float)doubleValue;

                            if (doubleValue == intValue)
                                thisJsonNumber = new JsonData()
                                {
                                    dataType = JsonDataType.Integer,
                                    content = intValue
                                };
                            else if (doubleValue == floatValue)
                                thisJsonNumber = new JsonData()
                                {
                                    dataType = JsonDataType.Float,
                                    content = floatValue
                                };
                            else
                                thisJsonNumber = new JsonData()
                                {
                                    dataType = JsonDataType.Double,
                                    content = doubleValue
                                };

                            popData = thisJsonNumber;

                            infos.Pop();
                        }
                        break;
                    default:
                        throw new NotClosedParseException("未能从中分析出数据", offset);
                }
            }

            return popData;
        }
        public static JsonData RapidParse(char[] jsonText)
        {
            char[] source = jsonText;
            int offset = 0;

            JsonData result = RapidParseData(ref source, ref offset);

            for (offset++; offset < source.Length; offset++)
            {
                if (!IsEmptyChar(source[offset]))
                {
                    throw new JsonFormatParseException("一个Json文本中不能出现两个元素并列的情况", offset);
                }
            }

            return result;
        }
        public static JsonData RapidParse(string jsonText)
        {
            char[] source = jsonText.ToCharArray();
            int offset = 0;

            JsonData result = RapidParseData(ref source, ref offset);

            for (offset++; offset < source.Length; offset++)
            {
                if (!IsEmptyChar(source[offset]))
                {
                    throw new JsonFormatParseException("一个Json文本中不能出现两个元素并列的情况", offset);
                }
            }

            return result;
        }
        public static JsonData[] ParseStream(char[] jsonText)
        {
            char[] source = jsonText;
            int offset = 0;

            List<JsonData> result = new List<JsonData>();

            for (; offset < source.Length; offset++)
            {
                JsonData currentJson = JsonData.ParseData(ref source, ref offset);
                result.Add(currentJson);
            }

            return result.ToArray();
        }
        public static JsonData[] ParseStream(string jsonText)
        {
            char[] source = jsonText.ToCharArray();
            int offset = 0;

            List<JsonData> result = new List<JsonData>();

            for (; offset < source.Length; offset++)
            {
                JsonData currentJson = JsonData.ParseData(ref source, ref offset);
                result.Add(currentJson);
            }

            return result.ToArray();
        }
        /// <summary>
        /// 尝试从包含Json文本的字符数组中分析Json数据
        /// </summary>
        /// <param name="jsonText">包含Json数据的文本</param>
        /// <param name="jsonObj">返回的结果</param>
        /// <returns>是否分析成功</returns>
        public static bool TryParse(char[] jsonText, out JsonData jsonObj)
        {
            try
            {
                jsonObj = Parse(jsonText);
                return true;
            }
            catch
            {
                jsonObj = null;
                return false;
            }
        }
        /// <summary>
        /// 尝试从Json文本中分析Json数据
        /// </summary>
        /// <param name="jsonText">包含Json数据的文本</param>
        /// <param name="jsonObj">返回的结果</param>
        /// <returns>是否分析成功</returns>
        public static bool TryParse(string jsonText, out JsonData jsonObj)
        {
            try
            {
                jsonObj = Parse(jsonText);
                return true;
            }
            catch
            {
                jsonObj = null;
                return false;
            }
        }
        public static bool TryParseStream(char[] jsonText, out JsonData[] jsonObjs)
        {
            try
            {
                jsonObjs = ParseStream(jsonText);
                return true;
            }
            catch
            {
                jsonObjs = null;
                return false;
            }
        }
        public static bool TryParseStream(string jsonText, out JsonData[] jsonObjs)
        {
            try
            {
                jsonObjs = ParseStream(jsonText);
                return true;
            }
            catch
            {
                jsonObjs = null;
                return false;
            }
        }

        /// <summary>
        /// 确定指定对象是否等于当前对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (GetType() == obj.GetType())
            {
                if (dataType == (obj as JsonData).dataType)
                {
                    switch (dataType)
                    {
                        case JsonDataType.Object:
                            Dictionary<JsonData, JsonData> selfPairs = (Dictionary<JsonData, JsonData>)content;
                            Dictionary<JsonData, JsonData> thatPairs = (Dictionary<JsonData, JsonData>)(obj as JsonData).content;

                            bool Removed = false;

                            foreach (JsonData key in selfPairs.Keys)
                            {
                                if (thatPairs.ContainsKey(key))
                                {
                                    foreach (JsonData key2 in thatPairs.Keys)
                                    {
                                        if (key.Equals(key2))
                                        {
                                            if (selfPairs[key].Equals(thatPairs[key2]))
                                            {
                                                thatPairs.Remove(key2);
                                                Removed = true;
                                                break;
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    if (Removed)
                                    {
                                        Removed = false;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                            return (thatPairs.Count == 0);
                        case JsonDataType.Array:
                            List<JsonData> selfList = (List<JsonData>)content;
                            List<JsonData> thatList = (List<JsonData>)(obj as JsonData).content;

                            bool Deleted = false;

                            foreach (JsonData element in selfList)
                            {
                                foreach (JsonData element2 in thatList)
                                {
                                    if (element.Equals(element2))
                                    {
                                        thatList.Remove(element2);
                                        Deleted = true;
                                        break;
                                    }
                                }
                                if (Deleted)
                                {
                                    Deleted = false;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            return (thatList.Count == 0);
                        case JsonDataType.String:
                            return ((string)content) == (string)((obj as JsonData).content);
                        case JsonDataType.Double:
                            return ((double)content) == (double)((obj as JsonData).content);
                        case JsonDataType.Boolean:
                            return ((bool)content) == (bool)((obj as JsonData).content);
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 获取JsonData实例所包含数据的HashCode
        /// </summary>
        /// <returns>int类型的HashCode值</returns>
        public override int GetHashCode()
        {
            switch (dataType)
            {
                case JsonDataType.Object:
                    return (content as Dictionary<JsonData, JsonData>).GetHashCode();
                case JsonDataType.Array:
                    return (content as List<JsonData>).GetHashCode();
                case JsonDataType.String:
                    return ((string)content).GetHashCode();
                case JsonDataType.Double:
                    return ((double)content).GetHashCode();
                case JsonDataType.Boolean:
                    return ((bool)content).GetHashCode();
            }
            return content.GetHashCode();
        }
    }
    public class JsonDataParser
    {

    }
}
