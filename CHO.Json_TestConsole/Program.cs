using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using CHO.Json;
using CHO_Json_TestConsole.Properties;

namespace CHO_Json_TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string strToParse = "{\"paramz\": {\"feeds\": [{\"id\": 299076, \"oid\": 288340, \"category\": \"article\", \"data\": {\"subject\": \"Benchmark\", \"summary\": \" abc\", \"cover\": \"/oman001/article/details/79063278\", \"pic\": \"null\", \"format\": \"txt\", \"changed\": \"2015-09-22 16:01:41\"}}, {\"id\": 299078, \"oid\": 288340, \"category\": \"article\", \"data\": {\"subject\": \"Benchmark\", \"summary\": \" abc\", \"cover\": \"/oman001/article/details/79063278\", \"pic\": \"null\", \"format\": \"txt\", \"changed\": \"2012-09-22 16:01:41\"}}, {\"id\": 299065, \"oid\": 288340, \"category\": \"article\", \"data\": {\"subject\": \"Benchmark\", \"summary\": \" abc\", \"cover\": \"/oman001/article/details/79063278\", \"pic\": \"null\", \"format\": \"txt\", \"changed\": \"2019-09-22 16:01:41\"}}], \"PageIndex\": 1, \"PageSize\": 20, \"TotalCount\": 535821, \"TotalPage\": 2677}}";
            strToParse = Resources.Untitled_1;
            JsonData qwq = JsonData.RapidParse("{548426:489}");
            Console.WriteLine(qwq);
            //return;
            Stopwatch watch = new Stopwatch();
            foreach (int i in new int[10])
            {
                watch.Start();
                JsonData data = JsonData.Parse(strToParse);
                watch.Stop();
                Console.WriteLine($"Parse: {watch.ElapsedMilliseconds}ms");
            }
            foreach (int i in new int[10])
            {
                watch.Start();
                JsonData data = JsonData.RapidParse(strToParse);
                watch.Stop();
                Console.WriteLine($"Parse: {watch.ElapsedMilliseconds}ms");
            }

            Console.ReadLine();
        }
    }
}
