using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Test
{
    class TaskDataStore
    {
        private List<Task> _tasks { get; set; }
        private List<GroupTask> _groups { get; set; }

        public TaskDataStore()
        {
            _tasks = new List<Task>();
            _groups = new List<GroupTask>();
        }

        public void Save(string path)
        {
            using StreamWriter file = File.CreateText(path);
            JsonSerializer jsonSerializer = new JsonSerializer();
            jsonSerializer.Serialize(file, _tasks);
        }

        public void Load(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException("Wrong path");
            }

            using StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd();
            _tasks = JsonConvert.DeserializeObject<List<Task>>(json);
            
        }

        public bool SearchTask(string name) => _tasks.Any(task => task.Name.ToLower() == name.ToLower());

        public bool SearchGroup(string name) => _groups.Any(group => group.Name.ToLower() == name.ToLower());

        public bool SearchTaskId(int id) => _tasks.Any(task => task.Id == id);

        public void AddTask (string name, DateTime date)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new Exception("String is null or empty");
            if (SearchTask(name))
                throw new Exception("The name already exists");

            _tasks.Add(new Task
            {
                Name = name,
                DueDate = date,
                Id = _tasks.Count + 1
            });
        }

        public void AddGroup (string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("String is null or empty");
            if (SearchGroup(name))
                throw new Exception("The name already exists");

            _groups.Add(new GroupTask { Name = name });
        }

        public void All()
        {
            Display.PrintAll(_tasks, _groups);
        }
       
        public void DeleteTask(int id)
        {
            if (!SearchTaskId(id))
                throw new Exception("id doesn't exist");

            var selectedTask = _tasks.Where(task => task.Id == id);

            foreach (Task task in selectedTask)
            {
                _tasks.Remove(task);
                break;
            }
        }

        public void Complete(int id)
        {
            if (_tasks.Any(task => task.Id == id))
            {
                var completedTask = _tasks.Where(task => task.Id == id);
                foreach (Task task in completedTask)
                    task.Status = TaskStatus.Finished;
            }
            else
            {
                var completedTask = _tasks.SelectMany(task => task.SubTasks, (task, subTask) => new { task, subTask })
                    .Where(task => task.subTask.Id == id).Select(task => task.subTask);
              
                foreach (SubTask task in completedTask)
                    task.Status = TaskStatus.Finished;
            }

            foreach (Task task in _tasks)
                task.Completed = task.SubTasks.Count(subTask => subTask.Status == TaskStatus.Finished);
        }

        public void DeleteFromGroup(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("String is null or empty");
            if (!SearchGroup(name))
                throw new Exception("There's no group with this name");

            var selectedGroup = _groups.Where(group => group.Name == name);

            foreach (GroupTask group in selectedGroup)
            {
                for (int i = 0; i < group.Tasks.Count; i++)
                {
                    if (group.Tasks[i].Id == id)
                        group.Tasks.RemoveAt(i);
                }
            }
        }

        public void AddSubTask(int id, string name, int parentId)
        {
            if (!SearchTaskId(parentId))
                throw new Exception("id doesn't exist");

            var selectedTask = _tasks.Where(task => task.Id == parentId);

            foreach (Task task in selectedTask)
            {
                task.SubTasks.Add(new SubTask
                {
                    Name = name,
                    Id = id,
                    DueDate = DateTime.Today,
                    ParentTask = task
                });
            }
        }

        public void DeleteSubTask(int id)
        {
            for (int i = 0; i < _tasks.Count; i++)
            {
                for (int j = 0; j < _tasks[i].SubTasks.Count; j++)
                {
                    if (_tasks[i].SubTasks[j].Id == id)
                        _tasks[i].SubTasks.RemoveAt(j);
                }
            }
        }

        public void DeleteGroup(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("String is null or empty");
            if (!SearchGroup(name))
                throw new Exception("There's no group with this name");

            var selectedGroup = _groups.Where(group => group.Name == name);

            foreach (GroupTask group in selectedGroup)
                _groups.Remove(group);
        }

        public void AddToGroup(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("String is null or empty");
            if (!SearchGroup(name))
                throw new Exception("There's no group with this name");

            var selectedGroup = _groups.Where(group => group.Name == name);

            if (_tasks.Any(task => task.Id == id))
            {
                var selectedTask = _tasks.Where(task => task.Id == id);

                foreach (GroupTask group in selectedGroup)
                    foreach (Task task in selectedTask)
                        group.Tasks.Add(task);

            }
            else
            {
                var selectedTask = _tasks.SelectMany(task => task.SubTasks, (task, subTask) => new { task, subTask })
                    .Where(task => task.subTask.Id == id)
                    .Select(task => task.subTask);

                foreach (GroupTask group in selectedGroup)
                {
                    foreach (SubTask task in selectedTask)
                        group.Tasks.Add(new Task
                        {
                            Name = task.Name,
                            Id = task.Id,
                            DueDate = task.DueDate,
                            Status = task.Status
                        });
                }
            }
        }

        public void Completed()
        {
            var completed = _tasks.Where(task => task.Status == TaskStatus.Finished);

            Console.WriteLine("Completed ");

            foreach (Task task in completed)
                Display.PrintTask(task);
        }

        public void CompletedFromGroup(string name)
        {
            var selectedGroup = _groups.Where(group => group.Name == name);
            var selected = selectedGroup.SelectMany(group => group.Tasks, (group, task) => new { group, task })
                .Where(group => group.task.Status == TaskStatus.Finished)
                .Select(group => group.task);

            Console.WriteLine($"Completed in the group {name}");

            foreach (Task comletedTask in selected)
                Display.PrintTask(comletedTask);
        }

        public void Today()
        {
            foreach (Task task in _tasks)
            {
                if (task.DueDate == DateTime.Today)
                {
                    Display.PrintTask(task);
                    continue;
                }

                foreach (SubTask subTask in task.SubTasks)
                    if (subTask.DueDate == DateTime.Today)
                        Display.PrintSubTask(subTask);
            }
        }
    }
}
