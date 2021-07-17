using System;
using System.Collections.Generic;

namespace Test
{
    class Commands
    {
        public void All(List<Task> tasks, List<GroupTask> group)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].SubTasks.Count != 0)
                {
                    Console.WriteLine($"id {tasks[i].Id} name {tasks[i].Name} date {tasks[i].DueDate.ToShortDateString()}" +
                        $" completed tasks {tasks[i].CompletedTasks}/{tasks[i].SubTasks.Count}");
                    for (int j = 0; j < tasks[i].SubTasks.Count; j++)
                    {
                        Console.WriteLine($"\t id {tasks[i].SubTasks[j].Id} name {tasks[i].SubTasks[j].Name} " +
                            $"date {tasks[i].SubTasks[j].DueDate}");
                    }
                }
                else
                {
                    Console.WriteLine($"id {tasks[i].Id} name {tasks[i].Name} date {tasks[i].DueDate.ToShortDateString()}");
                }
            }

            for (int i = 0; i < group.Count; i++)
            {
                Console.WriteLine($"Name {group[i].Name}");
                for (int j = 0; j < group[i].Tasks.Count; j++)
                {
                    Console.WriteLine($"\t id {group[i].Tasks[j].Id} name {group[i].Tasks[j].Name}" +
                        $" date {group[i].Tasks[j].DueDate}");
                }
            }
        }

        public void AddSubtasks(string id, string name, string idParent, List<Task> tasks)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Id == Convert.ToInt32(idParent))
                {
                    SubTask t = new SubTask { Name = name, Id = Convert.ToInt32(id), DueDate = DateTime.Today, 
                        ParentTask = tasks[i] };
                    tasks[i].SubTasks.Add(t);
                }
            }
        }

        public void AddToGroup(string id, string name, List<GroupTask> t, List<Task> t1)
        {

            for (int i = 0; i < t.Count; i++)
            {
                if (t[i].Name == name)
                {
                    for (int j = 0; j < t1.Count; j++)
                    {
                        if (t1[j].Id == Convert.ToInt32(id))
                        {
                            t[i].Tasks.Add(t1[j]);
                        }
                        else
                        {
                            for (int k = 0; k < t1[j].SubTasks.Count; k++)
                            {
                                if (t1[j].SubTasks[k].Id == Convert.ToInt32(id))
                                {
                                    Task l = new Task { Name = t1[j].SubTasks[k].Name, Id = t1[j].SubTasks[k].Id };
                                    t[i].Tasks.Add(l);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Complete(List<Task> tasks, int id)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Id == id)
                {
                    tasks[i].Status = TaskStatus.End;
                }
                else
                {
                    for (int j = 0; j < tasks[i].SubTasks.Count; j++)
                    {
                        if (tasks[i].SubTasks[j].Id == id)
                        {
                            tasks[i].SubTasks[j].Status = TaskStatus.End;
                        }
                    }
                }
            }
        }

        public void Completed(List<Task> tasks)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Status == TaskStatus.End)
                {
                    Console.WriteLine($"completed id {tasks[i].Id} name {tasks[i].Name}");
                }
            }
        }

        public void CompletedFromGroup(List<GroupTask> groups, string name)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i].Name == name)
                {
                    for (int j = 0; j < groups[i].Tasks.Count; j++)
                    {
                        if (groups[i].Tasks[j].Status == TaskStatus.End)
                        {
                            Console.WriteLine($"completed id {groups[i].Tasks[j].Id} name {groups[i].Tasks[j].Name}");
                        }
                    }
                }
            }
        }

        public void SearchCompleted(List<Task> t)
        {
            int count = 0;
            for (int i = 0; i < t.Count; i++)
            {
                for (int j = 0; j < t[i].SubTasks.Count; j++)
                {
                    if (t[i].SubTasks[j].Status == TaskStatus.End)
                    {
                        count++;
                    }
                }
                t[i].CompletedTasks = count;
            }

        }

        public bool SearchTask(List <Task> t, string name)
        {
            bool check = false;
            for (int i = 0; i < t.Count; i++)
            {
                if(t[i].Name.ToLower() == name.ToLower())
                {
                    check = true;
                }
            }

            return check;
        }

        public void Today(List<Task> t)
        {
            for (int i = 0; i < t.Count; i++)
            {
                if (t[i].DueDate == DateTime.Today)
                {
                    Console.WriteLine($"id {t[i].Id} name {t[i].Name} date {t[i].DueDate.ToShortDateString()}");
                    for (int j = 0; j < t[i].SubTasks.Count; j++)
                    {
                        if (t[i].SubTasks[j].DueDate.ToShortDateString() == DateTime.Today.ToShortDateString())
                        {
                            Console.WriteLine($"id {t[i].SubTasks[j].Id} name {t[i].SubTasks[j].Name} date {t[i].SubTasks[j].DueDate.ToShortDateString()}");
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < t[i].SubTasks.Count; j++)
                    {
                        if (t[i].SubTasks[j].DueDate.ToShortDateString() == DateTime.Today.ToShortDateString())
                        {
                            Console.WriteLine($"id {t[i].SubTasks[j].Id} name {t[i].SubTasks[j].Name} date {t[i].SubTasks[j].DueDate.ToShortDateString()}");
                        }
                    }
                }
            }
        }
    }
}
