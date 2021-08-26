using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Test
{
    class TaskDataStore
    {
        public List<Task> tasks { get; set; }
        public List<GroupTasks> groups { get; set; }

        public TaskDataStore()
        {
            tasks = new List<Task>();
            groups = new List<GroupTasks>();
        }

        public bool TaskExists(string name) => tasks.Any(task => task.Name.ToLower() == name.ToLower());

        public bool GroupExists(string name) => groups.Any(group => group.Name.ToLower() == name.ToLower());

        public bool TaskIdExists(int id) => tasks.Any(task => task.Id == id);

        public void AddTask(string name, DateTime date)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new Exception("String is null or empty");
            if (TaskExists(name))
                throw new Exception("The name already exists");

            tasks.Add(new Task
            {
                Name = name,
                DueDate = date,
                Id = tasks.Count + 1
            });
        }

        public void AddGroup(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("String is null or empty");
            if (GroupExists(name))
                throw new Exception("The name already exists");

            groups.Add(new GroupTasks (name));
        }

        public void All()
        {
            Display.PrintAll(tasks, groups);
        }
       
        public void DeleteTask(int id)
        {
            if (!TaskIdExists(id))
                throw new Exception("id doesn't exist");

            var selectedTask = tasks.Where(task => task.Id == id).Single();

            tasks.Remove(selectedTask);
        }

        public void Complete(int id)
        {
            if (tasks.Any(task => task.Id == id))
            {
                Task completedTask = tasks.Where(task => task.Id == id).Single();
                completedTask.Status = TaskStatus.Finished;
            }
            else
            {
                SubTask completedTask = tasks.SelectMany(task => task.SubTasks, (task, subTask) => new { task, subTask })
                    .Where(task => task.subTask.Id == id).Select(task => task.subTask).Single();
                completedTask.Status = TaskStatus.Finished;

                completedTask.ParentTask.Completed++;
            }
        }

        public void DeleteFromGroup(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("String is null or empty");
            if (!GroupExists(name))
                throw new Exception("There's no group with this name");

            GroupTasks selectedGroup = groups.Where(group => group.Name == name).Single();

            for (int i = 0; i < selectedGroup.Tasks.Count; i++)
            {
                if (selectedGroup.Tasks[i].Id == id)
                    selectedGroup.Tasks.RemoveAt(i);
            }
        }

        public void AddSubTask(int id, string name, int parentId)
        {
            if (!TaskIdExists(parentId))
                throw new Exception("id doesn't exist");

            Task selectedTask = tasks.Where(task => task.Id == parentId).Single();

            selectedTask.SubTasks.Add(new SubTask
            {
                Name = name,
                Id = id,
                DueDate = DateTime.Today,
                ParentTask = selectedTask
            });
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
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("String is null or empty");
            if (!GroupExists(name))
                throw new Exception("There's no group with this name");

            GroupTasks group = groups.Where(group => group.Name == name).Single();
            groups.Remove(group);
        }

        public void AddToGroup(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("String is null or empty");
            if (!GroupExists(name))
                throw new Exception("There's no group with this name");

            GroupTasks selectedGroup = groups.Where(group => group.Name == name).Single();

            if (tasks.Any(task => task.Id == id))
            {
                Task selectedTask = tasks.Where(task => task.Id == id).Single();
                selectedGroup.Tasks.Add(selectedTask);
                selectedTask.IsChild = true;
            }
            else
            {
                SubTask selectedTask = tasks.SelectMany(task => task.SubTasks, (task, subTask) => new { task, subTask })
                    .Where(task => task.subTask.Id == id)
                    .Select(task => task.subTask).Single();

                selectedGroup.Tasks.Add(new Task
                {
                    Name = selectedTask.Name,
                    Id = selectedTask.Id,
                    DueDate = selectedTask.DueDate,
                    Status = selectedTask.Status
                });

                selectedTask.IsChild = true;
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
            var selected = selectedGroup.SelectMany(group => group.Tasks, (group, task) => new { group, task })
                .Where(group => group.task.Status == TaskStatus.Finished)
                .Select(group => group.task);

            Console.WriteLine($"Completed in the group {name}");

            foreach (Task comletedTask in selected)
                Display.PrintTask(comletedTask);
        }

        public void Today()
        {
            List<Task> tasks = new List<Task>();
            foreach (Task task in tasks)
            {
                if (task.DueDate == DateTime.Today)
                {
                    tasks.Add(task);
                    continue;
                }

                foreach (SubTask subTask in task.SubTasks)
                    if (subTask.DueDate == DateTime.Today)
                        tasks.Add(new Task {
                        Name = subTask.Name,
                        Id = subTask.Id,
                        DueDate = subTask.DueDate
                        });        
            }

            Display.PrintAll(tasks);
        }
    }
}
