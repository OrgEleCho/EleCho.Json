using System;
using System.Collections.Generic;
using System.Text;

namespace EleCho.Json
{
    public interface IJsonDataHandler
    {
        object ToValue(IJsonData jsonData);
        IJsonData FromValue(object obj);
    }

    public class JsonDataHandler : IJsonDataHandler
    {
        private Func<object, IJsonData> fromValue;
        private Func<IJsonData, object> toValue;

        public JsonDataHandler(Func<object, IJsonData> fromValue, Func<IJsonData, object> toValue)
        {
            this.fromValue = fromValue;
            this.toValue = toValue;
        }
        public IJsonData FromValue(object obj) => fromValue.Invoke(obj);
        public object ToValue(IJsonData jsonData) => toValue(jsonData);
    }
}
