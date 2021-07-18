using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Test
{
    class DB
    {
        private List<Task> tasks;
        private List<GroupTask> groups;

        public void Save(string path)
        {
            using StreamWriter file = File.CreateText(path);
            JsonSerializer jsonSerializer = new JsonSerializer();
            jsonSerializer.Serialize(file, tasks);
        }

        public void Load(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException("Wrong path");
            }

            using StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd();
            tasks = JsonConvert.DeserializeObject<List<Task>>(json);

        }

        public DB()
        {
            tasks = new List<Task>();
            groups = new List<GroupTask>();
        }

        public List<Task> Get() => tasks;

        public List<GroupTask> GetfGroup() => groups;
 
        public void SearchCompleted()
        {
            foreach (Task task in tasks)
            {
                int count = task.SubTasks.Count(subTask => subTask.Status == TaskStatus.Finished);
                task.Completed = count;
            }
        }

        public bool SearchTask(string name)
        {
            bool check = false;
            foreach (Task task in tasks)
            {
                if (task.Name.ToLower() == name.ToLower())
                    check = true;
            }

            return check;
        }

        public void DeleteTask(int id)
        {
            var selectedTask = tasks.Where(task => task.Id == id);

            foreach (Task task in selectedTask)
            {
                task.SubTasks.Clear();
                tasks.Remove(task);
            }
        }

        public void Complete(int id)
        {
            bool check = tasks.Any(task => task.Id == id);
            if (check)
            {
                var completedTask = tasks.Where(task => task.Id == id);
                foreach (Task task in completedTask)
                    task.Status = TaskStatus.Finished;
            }
            else
            {
                var completedTask = from task in tasks
                                    from subTask in task.SubTasks
                                    where subTask.Id == id
                                    select subTask;

                foreach (SubTask task in completedTask)
                    task.Status = TaskStatus.Finished;
            }
        }

        public void DeleteFromGroup(int id, string name)
        {
            var selectedGroup = groups.Where(group => group.Name == name);

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
            var selectedTask = tasks.Where(task => task.Id == parentId);

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
            for (int i = 0; i < tasks.Count; i++)
            {
                for (int j = 0; j < tasks[i].SubTasks.Count; j++)
                {
                    if (tasks[i].SubTasks[j].Id == id)
                        tasks[i].SubTasks.RemoveAt(j);
                }
            }
        }

        public void DeleteGroup(string name)
        {
            var selectedGroup = groups.Where(group => group.Name == name);

            foreach (GroupTask group in selectedGroup)
            {
                group.Tasks.Clear();
                groups.Remove(group);
            }
        }

        public void All()
        {
            SearchCompleted();

            foreach (Task task in Get())
                Display.PrintTask(task);

            foreach (GroupTask group in GetfGroup())
                Display.PrintGroup(group);
        }

        public void AddToGroup(int id, string name)
        {
            bool check = tasks.Any(task => task.Id == id);
            var selectedGroup = groups.Where(group => group.Name == name);

            if (check)
            {
                var selectedTask = tasks.Where(task => task.Id == id);

                foreach (GroupTask group in selectedGroup)
                    foreach (Task task in selectedTask)
                        group.Tasks.Add(task);

            }
            else
            {
                var selectedTask = from task in tasks
                                   from subTask in task.SubTasks
                                   where subTask.Id == id
                                   select subTask;

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
            var completed = tasks.Where(task => task.Status == TaskStatus.Finished);

            Console.WriteLine("Completed ");

            foreach (Task task in completed)
                Display.PrintTask(task);
        }

        public void CompletedFromGroup(string name)
        {
            var selectedGroup = groups.Where(group => group.Name == name);
            var selected = from grouping in selectedGroup
                           from task in grouping.Tasks
                           where task.Status == TaskStatus.Finished
                           select task;

            Console.WriteLine($"Completed  in the group {name}");

            foreach (Task comletedTask in selected)
                Display.PrintTask(comletedTask);
        }

        public void Today()
        {
            foreach (Task task in tasks)
            {
                if (task.DueDate == DateTime.Today)
                    Display.PrintTask(task);
                else
                {
                    foreach (SubTask subTask in task.SubTasks)
                    {
                        if (subTask.DueDate == DateTime.Today)
                            Display.PrintSubTask(subTask);
                    }
                }
            }
        }
    }
}
