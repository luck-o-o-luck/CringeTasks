using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Test
{
    class SaveAndLoad
    {
        public void Save(string path, List<Task> tasks)
        {
            using StreamWriter file = File.CreateText(path);
            JsonSerializer jsonSerializer = new JsonSerializer();
            jsonSerializer.Serialize(file, tasks);
            Console.WriteLine(path);
        }

        public void Load(string path, ref List<Task> task)
        {
            if (!File.Exists(path)) return;
            using StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd();
            task = JsonConvert.DeserializeObject<List<Task>>(json);
        }
    }

}
