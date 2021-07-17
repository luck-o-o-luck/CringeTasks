using System;
using System.Collections.Generic;
using System.IO;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string name;
            string Date;
            int id;
            int count;
            string path;

            List<Task> tasks = new List <Task>();
            List<GroupTask> groups = new List<GroupTask>();
            Commands c = new Commands();
            Delete d = new Delete();
            SaveAndLoad salo = new SaveAndLoad();

            for (int i = 1; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "/all":
                        salo.Load(@args[0], ref tasks);
                        c.SearchCompleted(tasks);
                        c.All(tasks, groups);
                        salo.Save(@args[0], tasks);
                        break;
                    case "/add":
                        salo.Load(@args[0], ref tasks);
                        count = tasks.Count + 1;
                        name = args[i + 1];
                        Date = args[i + 2];
                        Date = Date.Replace(".", string.Empty);
                        bool ch = c.SearchTask(tasks, name);
                        if (ch == false)
                        {
                            tasks.Add(new Task
                            {
                                Name = name,
                                DueDate = new DateTime(Convert.ToInt32(Date.Substring(4, 4)),
                                Convert.ToInt32(Date.Substring(2, 2)), Convert.ToInt32(Date.Substring(0, 2))),
                                Id = count
                            });
                        }
                        salo.Save(@args[0], tasks);
                        break;
                    case "/delete":
                        salo.Load(@args[0], ref tasks);
                        id = Convert.ToInt32(args[i + 1]);
                        d.DeleteTask(tasks, id);
                        salo.Save(@args[0], tasks);
                        break;
                    case "/complete":
                        salo.Load(@args[0], ref tasks);
                        id = Convert.ToInt32(args[i + 1]);
                        c.Complete(tasks, id);
                        c.SearchCompleted(tasks);
                        salo.Save(@args[0], tasks);
                        break;
                    case "/today":
                        salo.Load(@args[0], ref tasks);
                        c.Today(tasks);
                        salo.Save(@args[0], tasks);
                        break;
                    case "/create-group":
                        salo.Load(@args[0], ref tasks);
                        name = args[i + 1];
                        groups.Add(new GroupTask { Name = name });
                        salo.Save(@args[0], tasks);
                        break;
                    case "/delete-group":
                        salo.Load(@args[0], ref tasks);
                        name = args[i + 1];
                        d.DeleteGroup(groups, name);
                        salo.Save(@args[0], tasks);
                        break;
                    case "/add-to-group":
                        salo.Load(@args[0], ref tasks);
                        c.AddToGroup(args[i + 1], args[i + 2], groups, tasks);
                        salo.Save(@args[0], tasks);
                        break;
                    case "/delete-from-group":
                        salo.Load(@args[0], ref tasks);
                        d.DeleteFromGroup(groups, Convert.ToInt32(args[i + 1]), args[i + 2]);
                        salo.Save(@args[0], tasks);
                        break;
                    case "/completed":
                        salo.Load(@args[0], ref tasks);
                        c.Completed(tasks);
                        salo.Save(@args[0], tasks);
                        break;
                    case "/add-subtask":
                        salo.Load(@args[0], ref tasks);
                        c.AddSubtasks(args[i + 1], args[i + 2], args[i + 3], tasks);
                        salo.Save(@args[0], tasks);
                        break;
                    case "/save":
                        path = args[i + 1];
                        salo.Save(@path, tasks);
                        break;
                    case "/load":
                        path = args[i + 1];
                        salo.Load(@path, ref tasks);
                        break;
                }               
            }
        }
        
    }
}
