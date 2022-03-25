using System.Collections.Generic;

namespace EleCho.Json
{
    public class JsonArray : List<IJsonData>, IJsonData
    {
        public JsonDataKind DataKind => JsonDataKind.Array;
        
        
        public JsonArray(List<IJsonData> data) => AddRange(data);

        public JsonArray()
        {
        }

        public void Add(JsonObject data) => Add(data as IJsonData);
        public void Add(JsonArray data) => Add(data as IJsonData);
        public void Add(JsonString data) => Add(data as IJsonData);
        public void Add(JsonNumber data) => Add(data as IJsonData);
        public void Add(JsonBoolean data) => Add(data as IJsonData);
        public void Add(JsonNull data) => Add(data as IJsonData);

        public List<object?> GetValue()
        {
            return new List<object?>(ConvertAll(x => x.GetValue()));
        }

        object? IJsonData.GetValue() => GetValue();
    }
}