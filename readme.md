# EleCho.Json

![nuget](https://img.shields.io/nuget/v/EleCho.Json) ![nuget](https://img.shields.io/nuget/dt/EleCho.Json) ![github last commit](https://img.shields.io/github/last-commit/SlimeNull/Elecho.Json) ![gitHub code size in bytes](https://img.shields.io/github/languages/code-size/SlimeNull/EleCho.Json)


Easy, Simple and Fast JSON reader and writer. You can also use model class to operate JSON data.
便捷, 简单以及高速的 JSON 读写器. 同时以也可以使用实体类来操作 JSON 数据.

It is about 1.89 times faster than Newtonsoft.Json and 7.92 times faster than System.Text.Json, You can also run the test yourself in the TestConsole project
速度约为 Newtonsoft.Json 的 1.89 倍, System.Text.Json 的 7.92 倍, 你也可以在 TestConsole 项目中自己运行该测试

The document is available in English and Chinese, just scroll down.
文档有英文和中文, 往下翻阅即可.

## English

This library is primarily for simple JSON operations that are fast and don't require complex features, such as primitive type conversions.

Various JSON object stores
```csharp
JsonObject jObj = new JsonObject();    // JSON Object (Key-value pair storage)
jObj.Add("some_key", "any_value");     // Set value with string key
jObj["some_key"] = new JsonString("Some string value");   // Set key with "this[...]", assign with IJsonData

JsonArray jArr = new JsonArray();      // JSON Array (List storage)
jArr.Add("Any value");                 // Add any JSON data.
jArr.Add(new JsonString("Anything"));  // Or use JsonString

JsonString jStr = "QWQ";               // Create JSON string with implicit type conversion
JsonBoolean jBl = true;                // Create JSON boolean with implicit type conversion
JsonNull = new JsonNull();       // JSON null, or use JsonData.Null

// Read JSON data:
string someStr = jObj["some_key"] as JsonString;   // Read JSON string from JSON object.
int someNum = jObj["some_key_num"] as JsonNumber;  // Read JSON number from JSON object.
```

Write JSON data to stream
```csharp
Stream stream;  // 要写入的 Stream
IJsonData someJsonData;
JsonWriter jw = new JsonWriter(stream);
jw.Write(someJsonData);
```

Read JSON data from stream
```csharp
Stream stream;  // 要读取的 Stream
JsonReader jr = new JsonReader(stream);   // 实例化 JSON 读取器
IJsonData jsonData = jr.Read();           // 从流中读取一个完整的 JSON 数据
```

Create JSON data with any object
```csharp
IJsonData jsonData = JsonData.FromValue(new()
{
    Name = "SlimeNull",
    Age = 18,
    Description = "CSharper! love .NET (*/ω＼*)"
});
```

Create model object with JSON data
```csharp
class One
{
    public string Name;
    public int Age;
    public string Description;
    
    // you can also use Property here
}

void LogicalCode()
{
    JsonObject someJsonData;   // JSON data to process
    One model = JsonData.ToValue(typeof(One), someJsonData);
}
```

Serialize model object to JSON string
```csharp
class One
{
    public string Name;
    public int Age;
    public string Description;
}

void LogicalCode()
{
    One model = new One() { Name = "SlimeNull", Age = 18, Description = "Some text" };
    string jsonText = JsonSerializer.Serialize(model);
}
```

Deserialize from JSON string
```csharp
class One
{
    public string Name;
    public int Age;
    public string Description;
}

void LogicalCode()
{
    string jsonTxt;
    One model = JsonSerializer.Deserialize<One>(jsonTxt);
}
```

Parse JSON tokens from stream
```csharp
Stream stream;     // Stream to read
JsonParser jParser = new JsonParser(stream);
jParser.PeekToken();   // peek a token from the stream
jParser.ReadToken();   // read a token from the stream
```

## 简体中文

这个库主要是追求较高速率的, 不要求复杂特性的简易 JSON 操作, 例如最基本类型的转换.

各种 JSON 对象存储
```csharp
JsonObject jObj = new JsonObject();    // JSON 对象 (键值对存储)
jObj.Add("some_key", "any_value");     // 使用字符串键设置值
jObj["some_key"] = new JsonString("Some string value");   // 或者使用 "this[...]" 和 IJsonData 设置

JsonArray jArr = new JsonArray();      // JSON 数组 (列表存储)
jArr.Add("Any value");                 // 添加任何 JSON 数据
jArr.Add(new JsonString("Anything"));  // 或者使用 JsonString

JsonString jStr = "QWQ";               // 使用隐式类型转换创建 JSON 字符串
JsonNumber jNum = 114514;              // 床架 JSON 数字 (它使用 double 存储)
JsonBoolean jBl = true;                // 创建 JSON 逻辑值(布尔值)
JsonNull = new JsonNull();             // JSON null, 或者使用 JsonData.Null

// Read JSON data:
string someStr = jObj["some_key"] as JsonString;   // 从 JSON 对象中读取 JSON 字符串
int someNum = jObj["some_key_num"] as JsonNumber;  // 从 JSON 对象中读取 JSON 数字
```

将 JSON 数据写入到流
```csharp
Stream stream;  // 要写入的 Stream
IJsonData someJsonData;
JsonWriter jw = new JsonWriter(stream);
jw.Write(someJsonData);
```

从流中读取 IJsonData:
```csharp
Stream stream;  // 要读取的 Stream
JsonReader jr = new JsonReader(stream);   // 实例化 JSON 读取器
IJsonData jsonData = jr.Read();           // 从流中读取一个完整的 JSON 数据
```

从任意创建 JSON 数据
```csharp
IJsonData jsonData = JsonData.FromValue(new()
{
    Name = "SlimeNull",
    Age = 18,
    Description = "CSharper! love .NET (*/ω＼*)"
});
```

从 JSON 数据创建模型对象
```csharp
class One
{
    public string Name;
    public int Age;
    public string Description;
    
    // 在这里你也可以使用属性
}

void LogicalCode()
{
    JsonObject someJsonData;   // 要处理的 JSON 数据
    One model = JsonData.ToValue(typeof(One), someJsonData);
}
```

将对象序列化为 JSON 字符串
```csharp
class One
{
    public string Name;
    public int Age;
    public string Description;
}

void LogicalCode()
{
    One model = new One() { Name = "SlimeNull", Age = 18, Description = "Some text" };
    string jsonText = JsonSerializer.Serialize(model);
}
```

从 JSON 字符串反序列化
```csharp
class One
{
    public string Name;
    public int Age;
    public string Description;
}

void LogicalCode()
{
    string jsonTxt;
    One model = JsonSerializer.Deserialize<One>(jsonTxt);
}
```

从流中解析 JSON token
```csharp
Stream stream;     // 要读取的流
JsonParser jParser = new JsonParser(stream);
jParser.PeekToken();   // 从流中读取一个 token, 但不变更当前的读取状态
jParser.ReadToken();   // 从流中读取一个 token, 并移动到下一个 token
```