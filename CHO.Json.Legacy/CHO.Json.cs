using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace CHO.Json.Legacy
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
        public InvalidCharParseException(string message)
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
    /// 表示分析Json数据时某个数据未结束导致的异常
    /// </summary>
    public class NotClosedParseException : Exception
    {
        private readonly string message;
        public NotClosedParseException(string message)
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
    /// 表示函数错误调用错误 (此异常一般不会出现)
    /// </summary>
    public class ParseCallError : Exception
    {
        private readonly string message;
        public ParseCallError(string message)
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
    /// 表示分析时的未知错误 (此异常一般不会出现)
    /// </summary>
    public class ParseUnknownError : Exception
    {
        private readonly string message;
        public ParseUnknownError(string message)
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
    /// 表示分析Json数据时的Json格式异常
    /// </summary>
    public class JsonFormatParseException : Exception
    {
        private readonly string message;
        public JsonFormatParseException(string message)
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
    /// Json数据类型, 表示Json数据中包含数据的类型
    /// </summary>
    public enum JsonDataType
    {
        Object,
        Array,
        String,
        Integer,
        Float,
        Double,
        Boolean,
        Null
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
            get
            {
                return dataType;
            }
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


        public static T Deserialize<T>(JsonData jsonData) where T : new()
        {
            return (T)Deserialize(jsonData, typeof(T));
        }
        public static JsonData Serialize(object obj)
        {
            return Create(obj);
        }
        public static bool TryDeserialize<T>(JsonData jsonData, out T result) where T : new()
        {
            try
            {
                result = Deserialize<T>(jsonData);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
        public static bool TrySerialize(object obj, out JsonData result)
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

        protected static object Deserialize(JsonData jsonData, Type resultType)
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
                            (result as IDictionary)[Deserialize(i, geneticArgs[0])] = Deserialize((jsonData.content as IDictionary<JsonData, JsonData>)[i], geneticArgs[1]);
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
                                    field.SetValue(result, Deserialize(value, field.FieldType));
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
                            (result as IList).Add(Deserialize(i, geneticTypes[0]));
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
        /// <summary>
        /// 将JsonData实例中的数据转换成Json文本
        /// 注意: 本程序集可以读取, 但其他容错值较小的Json操作程序集或包可能无法读取它
        /// </summary>
        /// <returns>String类型的Json文本</returns>
        public string ToJsonText()
        {
            switch (dataType)
            {
                case JsonDataType.Object:
                    List<string> pairs = new List<string>();
                    foreach (JsonData key in (content as Dictionary<JsonData, JsonData>).Keys)
                    {
                        pairs.Add(string.Format("{0}: {1}", key.ToJsonText(), (content as Dictionary<JsonData, JsonData>)[key].ToJsonText()));
                    }
                    return string.Format("{0}{1}{2}", "{", string.Join(", ", pairs), "}");
                case JsonDataType.Array:
                    List<string> elements = new List<string>();
                    foreach (JsonData element in (content as List<JsonData>))
                    {
                        elements.Add(element.ToJsonText());
                    }
                    return string.Format("[{0}]", string.Join(", ", elements));
                case JsonDataType.String:
                    return string.Format("\"{0}\"", ((string)content).Replace("\\", "\\\\").Replace("\'", "\\\'").Replace("\"", "\\\"").Replace("\0", "\\0").Replace("\a", "\\a").Replace("\b", "\\b").Replace("\f", "\\f").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t").Replace("\v", "\\v"));
                case JsonDataType.Integer:
                    return ((int)content).ToString();
                case JsonDataType.Float:
                    return ((float)content).ToString();
                case JsonDataType.Double:
                    return ((double)content).ToString();
                case JsonDataType.Boolean:
                    return (bool)content ? "true" : "false";
                case JsonDataType.Null:
                    return "null";
                default:
                    throw new JsonDataTypeException("所访问数据类型未知");
            }
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

        private readonly static char[] EmptyChars = " \n\r\t\0".ToCharArray();
        private readonly static char[] ParseEndChars = " :,}]\" \n\r\t\0".ToCharArray();
        private readonly static char[] ParseNumberChars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm0123456789.-".ToCharArray();
        private readonly static char[] ParseWordChars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm".ToCharArray();
        private readonly static char[] NumberChars = "0123456789-".ToCharArray();
        private static bool CheckInterface(Type targetType, Type interfaceType, out Type[] geneticTypes)
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
                if (EmptyChars.Contains(source[offset]))
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
                    else if (NumberChars.Contains(source[offset]))
                    {
                        return ParseNumber(ref source, ref offset);
                    }
                    else if (ParseWordChars.Contains(source[offset]))
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
                        throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]));
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
                    if (EmptyChars.Contains(source[offset]))
                    {
                        continue;
                    }
                    else if (source[offset] == '"')
                    {
                        parsing = true;
                    }
                    else
                    {
                        throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]));
                    }
                }
            }
            throw new NotClosedParseException("String无结束符号");
        }
        protected static JsonData ParseNumber(ref char[] source, ref int offset)
        {
            bool parsing = false;
            StringBuilder content = new StringBuilder();

            for (; offset < source.Length; offset++)
            {
                if (parsing)
                {
                    if (ParseNumberChars.Contains(source[offset]))
                    {
                        content.Append(source[offset]);
                    }
                    else if (ParseEndChars.Contains(source[offset]))
                    {
                        offset--;
                        break;
                    }
                    else
                    {
                        throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]));
                    }
                }
                else
                {
                    if (EmptyChars.Contains(source[offset]))
                    {
                        continue;
                    }
                    else if (NumberChars.Contains(source[offset]))
                    {
                        parsing = true;
                        content.Append(source[offset]);
                    }
                    else
                    {
                        throw new ParseCallError("未知错误! 一般情况下,此错误不会被触发");
                    }
                }
            }
            try
            {
                double template = double.Parse(content.ToString());

                if (template == (int)template)
                {
                    return Create((int)template);
                }
                else if (template == (float)template)
                {
                    return Create((float)template);
                }
                else
                {
                    return Create(template);
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
                    if (ParseWordChars.Contains(source[offset]))
                    {
                        content.Append(source[offset]);
                    }
                    else if (ParseEndChars.Contains(source[offset]))
                    {
                        offset--;
                        break;
                    }
                }
                else
                {
                    if (EmptyChars.Contains(source[offset]))
                    {
                        continue;
                    }
                    else if (ParseWordChars.Contains(source[offset]))
                    {
                        parsing = true;
                        content.Append(source[offset]);
                    }
                    else
                    {
                        throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]));
                    }
                }
            }
            switch (content.ToString().ToUpper())
            {
                case "TRUE":
                    return Create(true);
                case "FALSE":
                    return Create(false);
                case "NULL":
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
                        if (EmptyChars.Contains(source[offset]))
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
                            offset++;
                            return result;
                        }
                        else
                        {
                            throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]));
                        }
                        break;
                    case ArrayParseState.NotStart:
                        if (EmptyChars.Contains(source[offset]))
                        {
                            continue;
                        }
                        else if (source[offset] == '[')
                        {
                            parseState = ArrayParseState.ElementStart;
                        }
                        else
                        {
                            throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]));
                        }
                        break;

                }
            }

            throw new NotClosedParseException("Array无结束符号");
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
                        if (EmptyChars.Contains(source[offset]))
                        {
                            continue;
                        }
                        else if (source[offset] == ':')
                        {
                            parseState = ObjectParseState.ValueStart;
                        }
                        else
                        {
                            throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]));
                        }
                        break;
                    case ObjectParseState.ValueStart:
                        resultContainer[tempKey] = ParseData(ref source, ref offset);
                        parseState = ObjectParseState.ValueEnd;
                        break;
                    case ObjectParseState.ValueEnd:
                        if (EmptyChars.Contains(source[offset]))
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
                            offset++;
                            return result;
                        }
                        else
                        {
                            throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]));
                        }
                        break;
                    case ObjectParseState.NotStart:
                        if (EmptyChars.Contains(source[offset]))
                        {
                            continue;
                        }
                        else if (source[offset] == '{')
                        {
                            parseState = ObjectParseState.KeyStart;
                        }
                        else
                        {
                            throw new InvalidCharParseException(string.Format("非法字符'{0}'", source[offset]));
                        }
                        break;
                }
            }

            throw new NotClosedParseException("Object无结束符号");
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

            for (; offset < source.Length; offset++)
            {
                if (!EmptyChars.Contains(source[offset]))
                {
                    throw new JsonFormatParseException("一个Json文本中不能出现两个元素并列的情况");
                }
            }

            return result;
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
}
