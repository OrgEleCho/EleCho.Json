using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Xml.Schema;

namespace EleCho.Json
{
    /// <summary>
    /// Any JSON data
    /// </summary>
    public class JsonAny : IDictionary<string, JsonAny>, IList<JsonAny>
    {
        private static bool strictMode = true;
        /// <summary>
        /// Get or set if exception will be thrown when error
        /// 获取或设置当错误时是否抛出异常.
        /// </summary>
        public static bool StrictMode { get => strictMode; set => strictMode = value; }

        /// <summary>
        /// Value of this instance
        /// </summary>
        public readonly IJsonData Value;

        /// <inheritdoc/>
        public ICollection<string> Keys
        {
            get
            {
                if (Value is JsonObject jObj)
                    return jObj.Keys;

                if (strictMode)
                    throw new InvalidOperationException("Not a JsonObject");

                return Array.Empty<string>();
            }
        }

        /// <inheritdoc/>
        public ICollection<JsonAny> Values
        {
            get
            {
                if (Value is JsonObject jObj)
                    return new ValueCollection(jObj);

                if (strictMode)
                    throw new InvalidOperationException("Not a JsonObject");

                return Array.Empty<JsonAny>();
            }
        }

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                if (Value is JsonObject jObj)
                    return jObj.Count;
                else if (Value is JsonArray jArr)
                    return jArr.Count;
                else if (Value is JsonString jStr)
                    return jStr.Value.Length;

                if (strictMode)
                    throw new InvalidOperationException("Not a JsonObject, JsonArray or JsonString");

                return 0;
            }
        }

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <summary>
        /// Access as a JsonArray
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public JsonAny this[int index]
        {
            get
            {
                if (Value is JsonArray jArr)
                    return new JsonAny(jArr[index]);
                if (strictMode)
                    throw new InvalidOperationException("Not a JsonArray");
                return JsonNull.Null;
            }
            set
            {
                if (this.Value is JsonArray jArr)
                    jArr[index] = value.Value;
                else if (strictMode)
                    throw new InvalidOperationException("Not a JsonArray");
            }
        }

        /// <summary>
        /// Access as a JsonObject
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public JsonAny this[string key]
        {
            get
            {
                if (Value is JsonObject jObj)
                    return new JsonAny(jObj[key]);
                if (strictMode)
                    throw new InvalidOperationException("Not a JsonObject");
                return JsonNull.Null;
            }

            set
            {
                if (Value is JsonObject jObj)
                    jObj[key] = value.Value;
                else if (strictMode)
                    throw new InvalidOperationException("Not a JsonObject");
            }
        }

        /// <summary>
        /// Create an instance of <see cref="JsonAny"/>
        /// </summary>
        /// <param name="value"></param>
        public JsonAny(IJsonData value)
        {
            Value = value;
        }

        /// <summary>
        /// Create an instance of <see cref="JsonAny"/>
        /// </summary>
        /// <param name="value"></param>
        public JsonAny(JsonObject value) => Value = value;
        /// <summary>
        /// Create an instance of <see cref="JsonAny"/>
        /// </summary>
        /// <param name="value"></param>
        public JsonAny(JsonArray value) => Value = value;
        /// <summary>
        /// Create an instance of <see cref="JsonAny"/>
        /// </summary>
        /// <param name="value"></param>
        public JsonAny(JsonString value) => Value = value;
        /// <summary>
        /// Create an instance of <see cref="JsonAny"/>
        /// </summary>
        /// <param name="value"></param>
        public JsonAny(JsonNumber value) => Value = value;
        /// <summary>
        /// Create an instance of <see cref="JsonAny"/>
        /// </summary>
        /// <param name="value"></param>
        public JsonAny(JsonBoolean value) => Value = value;
        /// <summary>
        /// Create an instance of <see cref="JsonAny"/>
        /// </summary>
        /// <param name="value"></param>
        public JsonAny(JsonNull value) => Value = value;

        /// <summary>
        /// Create an instance of <see cref="JsonAny"/>
        /// </summary>
        /// <param name="data"></param>
        public JsonAny(IEnumerable<KeyValuePair<string, IJsonData>> data)
        {
            Value = new JsonObject(data);
        }

        /// <summary>
        /// Create an instance of <see cref="JsonAny"/>
        /// </summary>
        /// <param name="data"></param>
        public JsonAny(IEnumerable<IJsonData> data)
        {
            Value = new JsonArray(data);
        }

        /// <summary>
        /// Create an instance of <see cref="JsonAny"/>
        /// </summary>
        /// <param name="data"></param>
        public JsonAny(string data)
        {
            Value = new JsonString(data);
        }

        /// <summary>
        /// Create an instance of <see cref="JsonAny"/>
        /// </summary>
        /// <param name="data"></param>
        public JsonAny(double data)
        {
            Value = new JsonNumber(data);
        }

        /// <summary>
        /// Create an instance of <see cref="JsonAny"/>
        /// </summary>
        /// <param name="data"></param>
        public JsonAny(bool data)
        {
            Value = (JsonBoolean)data;
        }

        /// <summary>
        /// Create an instance of <see cref="JsonAny"/>
        /// </summary>
        /// <param name="data"></param>
        public JsonAny(object data)
        {
            if (data == null)
                Value = JsonData.Null;
            else
                Value = JsonData.FromValue(data);
        }

        /// <inheritdoc/>        
        public void Add(string key, JsonAny value)
        {
            if (Value is JsonObject jObj)
                jObj.Add(key, value.Value);
            else if (strictMode)
                throw new InvalidOperationException("Not a JsonObject");
        }

        /// <summary>
        /// Add a new key value pair to the object.
        /// </summary>
        /// <param name="key">String key</param>
        /// <param name="data">JSON data</param>
        public void Add(string key, JsonObject data) => Add(key, new JsonAny((IJsonData)data));
        /// <summary>
        /// Add a new key value pair to the object.
        /// </summary>
        /// <param name="key">String key</param>
        /// <param name="data">JSON data</param>
        public void Add(string key, JsonArray data) => Add(key, new JsonAny((IJsonData)data));
        /// <summary>
        /// Add a new key value pair to the object.
        /// </summary>
        /// <param name="key">String key</param>
        /// <param name="data">JSON data</param>
        public void Add(string key, JsonString data) => Add(key, new JsonAny((IJsonData)data));
        /// <summary>
        /// Add a new key value pair to the object.
        /// </summary>
        /// <param name="key">String key</param>
        /// <param name="data">JSON data</param>
        public void Add(string key, JsonNumber data) => Add(key, new JsonAny((IJsonData)data));
        /// <summary>
        /// Add a new key value pair to the object.
        /// </summary>
        /// <param name="key">String key</param>
        /// <param name="data">JSON data</param>
        public void Add(string key, JsonBoolean data) => Add(key, new JsonAny((IJsonData)data));
        /// <summary>
        /// Add a new key value pair to the object.
        /// </summary>
        /// <param name="key">String key</param>
        /// <param name="data">JSON data</param>
        public void Add(string key, JsonNull data) => Add(key, new JsonAny((IJsonData)data));

        /// <inheritdoc/>
        public bool ContainsKey(string key)
        {
            if (Value is JsonObject jObj)
                return jObj.ContainsKey(key);
            if (strictMode)
                throw new InvalidOperationException("Not a JsonObject");
            return false;
        }

        /// <inheritdoc/>
        public bool Remove(string key)
        {
            if (Value is JsonObject jObj)
                return jObj.Remove(key);
            if (strictMode)
                throw new InvalidOperationException("Not a JsonObject");
            return false;
        }

        /// <inheritdoc/>
        public bool TryGetValue(string key, out JsonAny value)
        {
            if (this.Value is JsonObject jObj)
            {
                if (jObj.TryGetValue(key, out IJsonData? jValue))
                {
                    value = new JsonAny(jValue);
                    return true;
                }

                value = null!;
                return false;
            }

            if (strictMode)
                throw new InvalidOperationException("Not a JsonObject");

            value = null!;   // make compiler happy
            return false;
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<string, JsonAny> item)
        {
            if (this.Value is JsonObject jObj)
                jObj.Add(item.Key, item.Value.Value);
            else if (strictMode)
                throw new InvalidOperationException("Not a JsonObject");
        }

        /// <inheritdoc/>
        public void Clear()
        {
            if (Value is JsonObject jObj)
                jObj.Clear();
            else if (Value is JsonArray jArr)
                jArr.Clear();
            else if (strictMode)
                throw new InvalidOperationException("Not a JsonObject or JsonArray");
        }

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<string, JsonAny> item)
        {
            if (Value is JsonObject jObj)
                return jObj.Contains(new KeyValuePair<string, IJsonData>(item.Key, item.Value.Value));

            if (strictMode)
                throw new InvalidOperationException("Not a JsonObject");

            return false;
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<string, JsonAny>[] array, int arrayIndex)
        {
            if (Value is JsonObject jObj)
            {
                KeyValuePair<string, JsonAny>[] source = new KeyValuePair<string, JsonAny>[jObj.Count];
                int _index = 0;
                foreach (var pair in jObj)
                {
                    source[_index] = new KeyValuePair<string, JsonAny>(pair.Key, new JsonAny(pair.Value));
                    _index++;
                }

                Array.Copy(source, 0, array, arrayIndex, source.Length);
                return;
            }

            if (strictMode)
                throw new InvalidOperationException("Not a JsonObject");
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<string, JsonAny> item)
        {
            if (Value is JsonObject jObj && jObj.TryGetValue(item.Key, out IJsonData? jValue) && jValue == item.Value.Value)
                return jObj.Remove(item.Key);
            if (strictMode)
                throw new InvalidOperationException("Not a JsonObject");
            return false;
        }

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<string, JsonAny>> IEnumerable<KeyValuePair<string, JsonAny>>.GetEnumerator()
        {
            if (Value is JsonObject jObj)
            {
                foreach (var pair in jObj)
                    yield return new KeyValuePair<string, JsonAny>(pair.Key, new JsonAny(pair.Value));
                yield break;
            }
            
            if (strictMode)
                throw new InvalidOperationException("Not a JsonObject");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (Value is JsonObject jObj)
            {
                foreach (var pair in jObj)
                    yield return new KeyValuePair<string, JsonAny>(pair.Key, new JsonAny(pair.Value));
                yield break;
            }
            else if (Value is JsonArray jArr)
            {
                foreach (var item in jArr)
                    yield return new JsonAny(item);
                yield break;
            }

            if (strictMode)
                throw new InvalidOperationException("Not a JsonObject or JsonArray");
            yield break;
        }

        /// <inheritdoc/>
        public int IndexOf(JsonAny item)
        {
            if (Value is JsonArray jArr)
                return jArr.IndexOf(item.Value);
            if (strictMode)
                throw new InvalidOperationException("Not a JsonArray");
            return -1;
        }

        /// <inheritdoc/>
        public void Insert(int index, JsonAny item)
        {
            if (Value is JsonArray jArr)
                jArr.Insert(index, item.Value);
            else if (strictMode)
                throw new InvalidOperationException("Not a JsonArray");
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            if (Value is JsonArray jArr)
                jArr.RemoveAt(index);
            else if (strictMode)
                throw new InvalidOperationException("Not a JsonArray");
        }

        /// <inheritdoc/>
        public void Add(JsonAny item)
        {
            if (Value is JsonArray jArr)
                jArr.Add(item.Value);
            else if (strictMode)
                throw new InvalidOperationException("Not a JsonArray");
        }

        /// <summary>
        /// Add a JSON data to the array. <br/>
        /// 将 JSON 数据添加到数组中。
        /// </summary>
        /// <param name="data">Data to add. <br/>要添加的数据.</param>
        public void Add(JsonObject data) => Add(new JsonAny((IJsonData)data));

        /// <summary>
        /// Add a JSON data to the array. <br/>
        /// 将 JSON 数据添加到数组中。
        /// </summary>
        /// <param name="data">Data to add. <br/>要添加的数据.</param>
        public void Add(JsonArray data) => Add(new JsonAny((IJsonData)data));

        /// <summary>
        /// Add a JSON data to the array. <br/>
        /// 将 JSON 数据添加到数组中。
        /// </summary>
        /// <param name="data">Data to add. <br/>要添加的数据.</param>        
        public void Add(JsonString data) => Add(new JsonAny((IJsonData)data));

        /// <summary>
        /// Add a JSON data to the array. <br/>
        /// 将 JSON 数据添加到数组中。
        /// </summary>
        /// <param name="data">Data to add. <br/>要添加的数据.</param>        
        public void Add(JsonNumber data) => Add(new JsonAny((IJsonData)data));

        /// <summary>
        /// Add a JSON data to the array. <br/>
        /// 将 JSON 数据添加到数组中。
        /// </summary>
        /// <param name="data">Data to add. <br/>要添加的数据.</param>
        public void Add(JsonBoolean data) => Add(new JsonAny((IJsonData)data));

        /// <summary>
        /// Add a JSON data to the array. <br/>
        /// 将 JSON 数据添加到数组中。
        /// </summary>
        /// <param name="data">Data to add. <br/>要添加的数据.</param>
        public void Add(JsonNull data) => Add(new JsonAny((IJsonData)data));

        /// <inheritdoc/>
        public bool Contains(JsonAny item)
        {
            if (Value is JsonArray jArr)
                return jArr.Contains(item.Value);
            if (strictMode)
                throw new InvalidOperationException("Not a JsonArray");
            return false;
        }

        /// <inheritdoc/>
        public void CopyTo(JsonAny[] array, int arrayIndex)
        {
            if (Value is JsonArray jArr)
            {
                JsonAny[] source = new JsonAny[jArr.Count];
                int _index = 0;
                foreach (var jValue in jArr)
                {
                    source[_index] = new JsonAny(jValue);
                    _index++;
                }

                Array.Copy(source, 0, array, arrayIndex, source.Length);
                return;
            }

            if (strictMode)
                throw new InvalidOperationException("Not a JsonArray");
        }

        /// <inheritdoc/>
        public bool Remove(JsonAny item)
        {
            if (Value is JsonArray jArr)
                return jArr.Remove(item.Value);
            if (strictMode)
                throw new InvalidOperationException("Not a JsonArray");
            return false;
        }

        IEnumerator<JsonAny> IEnumerable<JsonAny>.GetEnumerator()
        {
            if (Value is JsonArray jArr)
            {
                foreach (var jValue in jArr)
                    yield return new JsonAny(jValue);
                yield break;
            }
            
            if (strictMode)
                throw new InvalidOperationException("Not a JsonArray");
            yield break;
        }

        /// <summary>
        /// Cast value to JsonObject
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Value is not JsonObject</exception>
        public JsonObject AsObject()
        {
            if (Value is JsonObject jObj)
                return jObj;
            if (strictMode)
                throw new InvalidOperationException("Not a JsonObject");
            return new JsonObject();
        }
        /// <summary>
        /// Cast value to JsonArray
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Value is not JsonArray</exception>
        public JsonArray AsArray()
        {
            if (Value is JsonArray jArr)
                return jArr;
            if (strictMode)
                throw new InvalidOperationException("Not a JsonArray");
            return new JsonArray();
        }
        /// <summary>
        /// Cast value to JsonString
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Value is not JsonString</exception>
        public JsonString AsString()
        {
            if (Value is JsonString jStr)
                return jStr;
            if (strictMode)
                throw new InvalidOperationException("Not a JsonString");
            return new JsonString(string.Empty);
        }
        /// <summary>
        /// Cast value to JsonNumber
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Value is not JsonNumber</exception>
        public JsonNumber AsNumber()
        {
            if (Value is JsonNumber jNum)
                return jNum;
            if (strictMode)
                throw new InvalidOperationException("Not a JsonNumber");
            return new JsonNumber(0);
        }
        /// <summary>
        /// Cast value to JsonBoolean
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Value is not JsonBoolean</exception>
        public JsonBoolean AsBoolean()
        {
            if (Value is JsonBoolean jBol)
                return jBol;
            if (strictMode)
                throw new InvalidOperationException("Not a JsonBoolean");
            return JsonBoolean.False;
        }
        /// <summary>
        /// Cast value to JsonNull
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Value is not JsonNull</exception>
        public JsonNull AsNull()
        {
            if (Value is JsonNull jNul)
                return jNul;
            if (strictMode)
                throw new InvalidOperationException("Not a JsonNull");
            return JsonNull.Null;
        }

        /// <summary>
        /// Convert from any JsonObject
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator JsonAny(JsonObject data) => new JsonAny((IJsonData)data);
        /// <summary>
        /// Convert from any JsonArray
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator JsonAny(JsonArray data) => new JsonAny((IJsonData)data);
        /// <summary>
        /// Convert from any JsonString
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator JsonAny(JsonString data) => new JsonAny((IJsonData)data);
        /// <summary>
        /// Convert from any JsonNumber
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator JsonAny(JsonNumber data) => new JsonAny((IJsonData)data);
        /// <summary>
        /// Convert from any JsonBoolean
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator JsonAny(JsonBoolean data) => new JsonAny((IJsonData)data);
        /// <summary>
        /// Convert from any JsonNull
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator JsonAny(JsonNull data) => new JsonAny(data);


        /// <summary>
        /// Convert from Dictionary
        /// </summary>
        /// <param name="data"></param>
        public static explicit operator JsonAny(Dictionary<string, IJsonData> data) => new JsonAny(data);
        /// <summary>
        /// Convert from List
        /// </summary>
        /// <param name="data"></param>
        public static explicit operator JsonAny(List<IJsonData> data) => new JsonAny(data);

        /// <summary>
        /// Convert from string
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator JsonAny(string data) => new JsonAny(data);
        /// <summary>
        /// Convert from double
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator JsonAny(double data) => new JsonAny(data);
        /// <summary>
        /// Convert from boolean
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator JsonAny(bool data) => new JsonAny(data);


        /// <summary>
        /// Represents the collection of values in a <see cref="JsonAny" />. This class cannot be inherited.
        /// </summary>
        public class ValueCollection : ICollection<JsonAny>, IEnumerable<JsonAny>, IEnumerable, IReadOnlyCollection<JsonAny>, ICollection
        {
            private JsonObject value;

            /// <summary>
            /// Create an instance of <see cref="ValueCollection"/>
            /// </summary>
            /// <param name="jsonObject"></param>
            public ValueCollection(JsonObject jsonObject)
            {
                value = jsonObject;
            }


            /// <inheritdoc/>
            public int Count => value.Count;

            /// <inheritdoc/>
            public bool IsReadOnly => true;

            /// <inheritdoc/>
            public bool IsSynchronized => false;

            /// <inheritdoc/>
            public object SyncRoot => ((ICollection)value).SyncRoot;

            void ICollection<JsonAny>.Add(JsonAny item) => throw new InvalidOperationException();
            void ICollection<JsonAny>.Clear() => throw new InvalidOperationException();

            /// <inheritdoc/>
            public bool Contains(JsonAny item) => value.ContainsValue(item.Value);

            /// <inheritdoc/>
            public void CopyTo(JsonAny[] array, int arrayIndex)
            {
                CopyTo((Array)array, arrayIndex);
            }

            /// <inheritdoc/>
            public IEnumerator<JsonAny> GetEnumerator()
            {
                foreach (var item in value.Values)
                    yield return new JsonAny(item);
            }

            bool ICollection<JsonAny>.Remove(JsonAny item) => throw new InvalidOperationException();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            /// <inheritdoc/>
            public void CopyTo(Array array, int index)
            {
                JsonAny[] source = new JsonAny[value.Count];
                int _index = 0;
                foreach (var item in value)
                {
                    source[_index] = new JsonAny(item.Value);
                    _index++;
                }

                Array.Copy(source, 0, array, index, source.Length);
            }
        }
    }
}
