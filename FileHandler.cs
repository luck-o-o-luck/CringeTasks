using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace Test
{
    class FileHandler
    {
        private string _path { get; }

        public FileHandler(string filePath) { _path = @filePath; }

        public void Save(TaskDataStore data)
        {
            using StreamWriter file = File.CreateText(_path);
            JsonSerializer jsonSerializer = new JsonSerializer();
            jsonSerializer.Serialize(file, data.tasks);
        }

        public TaskDataStore Load()
        {
            if (!File.Exists(_path))
                throw new ArgumentException("Wrong path");

            TaskDataStore data = new TaskDataStore();

            using StreamReader reader = new StreamReader(_path);
            string json = reader.ReadToEnd();
            data.tasks = JsonConvert.DeserializeObject<List<Task>>(json);

            return data;
        }
    }
}
