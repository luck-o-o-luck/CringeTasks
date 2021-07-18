using System;

namespace Test
{
    class ConsoleInput
    {
        public string name;
        public string Date;
        public int id;
        public int count;
        public string path;

        private DB db = new DB();
        
        public void Input(string[] args)
        {
            for (int i = 1; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "/all":
                        db.Load(@args[0]);
                        db.SearchCompleted();
                        db.All();
                        break;
                    case "/add":
                    {
                        db.Load(@args[0]);
                        Console.WriteLine(args[0]);
                        count = db.Get().Count + 1;
                        name = args[i + 1];
                        Date = args[i + 2];
                        Date = Date.Replace(".", string.Empty);
                        bool ch = db.SearchTask(name);
                        if (ch == false)
                        {
                            db.Get().Add(new Task
                            {
                                Name = name,
                                DueDate = new DateTime(Convert.ToInt32(Date.Substring(4, 4)),
                                    Convert.ToInt32(Date.Substring(2, 2)), Convert.ToInt32(Date.Substring(0, 2))),
                                Id = count
                            });
                        }

                        db.Save(@args[0]);
                        break;
                    }
                    case "/delete":
                        db.Load(@args[0]);
                        id = Convert.ToInt32(args[i + 1]);
                        db.DeleteTask(id);
                        db.Save(@args[0]);
                        break;
                    case "/complete":
                        db.Load(@args[0]);
                        id = Convert.ToInt32(args[i + 1]);
                        db.Complete(id);
                        db.SearchCompleted();
                        db.Save(@args[0]);
                        break;
                    case "/today":
                        db.Load(@args[0]);
                        db.Today();
                        db.Save(@args[0]);
                        break;
                    case "/create-group":
                        db.Load(@args[0]);
                        name = args[i + 1];
                        db.GetfGroup().Add(new GroupTask {Name = name});
                        db.Save(@args[0]);
                        break;
                    case "/delete-group":
                        db.Load(@args[0]);
                        name = args[i + 1];
                        db.DeleteGroup(name);
                        db.Save(@args[0]);
                        break;
                    case "/add-to-group":
                        db.Load(@args[0]);
                        db.AddToGroup(Convert.ToInt32(args[i + 1]), args[i + 2]);
                        db.Save(@args[0]);
                        break;
                    case "/delete-from-group":
                        db.Load(@args[0]);
                        db.DeleteFromGroup(Convert.ToInt32(args[i + 1]), args[i + 2]);
                        db.Save(@args[0]);
                        break;
                    case "/completed":
                        db.Load(@args[0]);
                        db.Completed();
                        db.Save(@args[0]);
                        break;
                    case "/add-subtask":
                        db.Load(@args[0]);
                        db.AddSubTask(Convert.ToInt32(args[i + 1]), args[i + 2], Convert.ToInt32(args[i + 3]));
                        db.Save(@args[0]);
                        break;
                    case "/save":
                        path = args[i + 1];
                        db.Save(path);
                        break;
                    case "/load":
                        path = args[i + 1];
                        try
                        {
                            db.Load(path);
                        }
                        catch (ArgumentException ep)
                        {
                            Console.WriteLine(ep.Message);
                        }
                        break;
                }
            }
        }
    }
}
