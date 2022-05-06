# EleCho.Json

[![nuget](https://img.shields.io/nuget/v/EleCho.Json)](https://www.nuget.org/packages/EleCho.Json/) [![nuget](https://img.shields.io/nuget/dt/EleCho.Json)](https://www.nuget.org/packages/EleCho.Json/) ![github last commit](https://img.shields.io/github/last-commit/SlimeNull/Elecho.Json) ![gitHub code size in bytes](https://img.shields.io/github/languages/code-size/SlimeNull/EleCho.Json)

Easy, Simple and Fast JSON reader and writer. You can also use model class to operate JSON data.
便捷, 简单以及高速的 JSON 读写器. 同时以也可以使用实体类来操作 JSON 数据.

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
Stream stream;  // Stream to write
IJsonData someJsonData;
JsonWriter jw = new JsonWriter(stream);
jw.Write(someJsonData);
```

Read JSON data from stream

```csharp
Stream stream; // Stream to read
JsonReader jr = new JsonReader(stream); // instantiate the JSON reader
IJsonData jsonData = jr.Read(); // Read a complete JSON data from the stream

// If you only need to temporarily read once, you can also use the static method of JsonReader
IJsonData jsonData = JsonReader.Read(stream);
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

Lex JSON tokens from stream

```csharp
Stream stream;     // Stream to read
JsonLexer jLexer = new JsonLexer(stream);
jLexer.PeekToken();   // peek a token from the stream
jLexer.ReadToken();   // read a token from the stream
```

Weakly typed operation on JSON
```csharp
// EleCho.Json also provides weakly typed operations on JSON (similar to dynamic, but strongly typed in nature)
JsonAny jAny = new JsonObject(); // JsonAny can be converted from any Json data

// JsonAny's Value member (of type IJsonData) stores the actual JSON data of JsonAny
var jAnyData = jAny.Value;

// You can index into JsonAny (if it's a JsonArray)
jAny[0]; // try to take the first value of JsonAny as a JsonArray

// You can also key index on JsonAny (if it's a JsonObject)
jAny["SomeKey"];

// Also, you can have multi-level access, because the JsonAny indexer always returns JsonAny
jAny["FirstKey"]["SecondKey"]["ThirdKey"];

// If you want to use iterators, before that, you need to try type conversion using AsXXX
// Of course, you can also use (JsonArray)jAny.Value to cast
foreach(var jdata in jAny.AsArray()) // Iterate over jAny as JsonArray
    _ = jdata;

// For the Add operation of JsonArray, JsonAny also supports
// At this point string is implicitly converted to JsonString
jAny.Add("A new data");

// If the above operation does not match the actual type of JsonAny, an InvalidOperationException will be thrown
// here jAny stores a JsonObject, so the Add method cannot be used
jAny.Add("try to add data"); // exception here

// In fact, JsonAny implements all the operations of JsonObject and JsonArray, and adds type conversion
// Below is a partial example
jAny.ContainsKey("Some keys"); // Operation of JsonObject
jAny.Contains("some data"); // Operation of JsonArray
jAny.Insert(0, "New data"); // Operation of JsonArray

// The static member StrictMode of JsonAny indicates whether it is strict mode
// If not in strict mode, any method of JsonAny operation will not throw exception related to type mismatch
// and return the appropriate return value associated with the method, here are some examples
jAny.Count; // If jAny is not a countable type, return 0
jAny.ContainsKey("some keys"); // if jAny is not a JsonObject, return false
jAny.IndexOf("some value"); // if jAny is not a JsonArray, return -1
jAny.GetEnumerator(); // If jAny is not enumerable, return an enumerator with no content
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

// 如果你只需要临时读取一次, 也可以使用 JsonReader 的静态方法
IJsonData jsonData = JsonReader.Read(stream);
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
JsonLexer jLexer = new JsonLexer(stream);
jLexer.PeekToken();   // 从流中读取一个 token, 但不变更当前的读取状态
jLexer.ReadToken();   // 从流中读取一个 token, 并移动到下一个 token
```

JSON 弱类型操作
```csharp
// EleCho.Json 还提供了 JSON 的弱类型操作 (类似于 dynamic, 但本质是强类型)
JsonAny jAny = new JsonObject();   // JsonAny 可以由任意 Json 数据转换而来

// JsonAny 的 Value 成员(类型为 IJsonData)存储了 JsonAny 的实际 JSON 数据
var jAnyData = jAny.Value;

// 你可以对 JsonAny 进行索引操作 (如果它是一个 JsonArray)
jAny[0];  // 尝试将 JsonAny 作为 JsonArray 取第一个值

// 你也可以对 JsonAny 进行键索引操作 (如果它是一个 JsonObject)
jAny["SomeKey"];

// 同时, 你也可以进行多级访问, 因为 JsonAny 索引器总是返回 JsonAny
jAny["FirstKey"]["SecondKey"]["ThirdKey"];

// 如果你希望使用迭代器, 那在这之前, 你需要使用 AsXXX 尝试进行类型转换
// 当然, 你也可以使用 (JsonArray)jAny.Value 来进行强制类型转换
foreach(var jdata in jAny.AsArray())   // 将 jAny 作为 JsonArray 进行迭代
    _ = jdata;

// 对于 JsonArray 的 Add 操作, JsonAny 也同样支持
// 此时 string 隐式转换为 JsonString
jAny.Add("一个新的数据");

// 如果上述操作与 JsonAny 的实际类型不匹配, 则会抛出 InvalidOperationException
// 在这里 jAny 存储了 JsonObject, 所以无法使用 Add 方法
jAny.Add("尝试添加数据");   // 此处异常

// 事实上, JsonAny 实现了所有的 JsonObject 和 JsonArray 的操作, 并且添加了类型转换
// 下面是一部分示例
jAny.ContainsKey("某些键");   // JsonObject 的操作
jAny.Contains("某些数据");    // JsonArray 的操作
jAny.Insert(0, "新的数据");   // JsonArray 的操作

// JsonAny 的静态成员 StrictMode 表示是否为严格模式
// 如果不处于严格模式, 任何 JsonAny 操作的方法都不会抛出与类型不匹配相关的异常
// 并且返回与方法相关的合适的返回值, 下面是一些示例
jAny.Count;   // 如果 jAny 不是可以计数的类型, 那么返回 0
jAny.ContainsKey("某些键");   // 如果 jAny 不是 JsonObject, 返回 false
jAny.IndexOf("某些值");       // 如果 jAny 不是 JsonArray, 返回 -1
jAny.GetEnumerator();         // 如果 jAny 不可枚举, 则返回一个无内容的枚举器
```