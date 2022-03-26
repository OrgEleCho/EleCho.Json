using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using EleCho.Json;
using Newtonsoft.Json.Linq;

namespace TestConsole
{
    class Program
    {
        public class Person
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public int Age { get; set; }
        }
        static void Main(string[] args)
        {
            string jsonToRead = "{\"paramz\": {\"feeds\": [{\"id\": 299076, \"oid\": 288340, \"category\": \"article\", \"data\": {\"subject\": \"Benchmark\", \"summary\": \" abc\", \"cover\": \"/oman001/article/details/79063278\", \"pic\": \"null\", \"format\": \"txt\", \"changed\": \"2015-09-22 16:01:41\"}}, {\"id\": 299078, \"oid\": 288340, \"category\": \"article\", \"data\": {\"subject\": \"Benchmark\", \"summary\": \" abc\", \"cover\": \"/oman001/article/details/79063278\", \"pic\": \"null\", \"format\": \"txt\", \"changed\": \"2012-09-22 16:01:41\"}}, {\"id\": 299065, \"oid\": 288340, \"category\": \"article\", \"data\": {\"subject\": \"Benchmark\", \"summary\": \" abc\", \"cover\": \"/oman001/article/details/79063278\", \"pic\": \"null\", \"format\": \"txt\", \"changed\": \"2019-09-22 16:01:41\"}}], \"PageIndex\": 1, \"PageSize\": 20, \"TotalCount\": 535821, \"TotalPage\": 2677}}";
            JsonReader reader = new JsonReader(new StringReader(jsonToRead));
            IJsonData jsonData = reader.Read();
            StringWriter sr = new StringWriter();
            JsonWriter writer = new JsonWriter(sr);
            writer.WriteData(jsonData);
            Console.WriteLine(sr.ToString());
            Console.WriteLine(JsonSerializer.Serialize(new
            {
                Test = "Test",
                Address = "China",
                Age = 123123,
                QWQ = new
                {
                    You = "Fuck"
                }
            }));

            Stopwatch stopwatch = new Stopwatch();

            Console.Write("\n\n\n\n\n");
            Console.WriteLine("SpeedTest:");

            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                _ = JsonSerializer.Deserialize(jsonToRead);
            }
            stopwatch.Stop();
            Console.WriteLine("EleCho.Json > JsonSerializer.Deserialize: " + stopwatch.ElapsedMilliseconds + "ms");

            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                _ = System.Text.Json.JsonDocument.Parse(jsonToRead);
            }
            stopwatch.Stop();
            Console.WriteLine("System.Text.Json > JsonDocument.Parse: " + stopwatch.ElapsedMilliseconds + "ms");

            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                _ = JObject.Parse(jsonToRead);
            }
            stopwatch.Stop();
            Console.WriteLine("Newtonsoft.Json > JObject.Parse: " + stopwatch.ElapsedMilliseconds + "ms");

            Console.ReadLine();
        }
    }
}
