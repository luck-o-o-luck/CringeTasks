using System;

namespace Test
{
    class ConsoleInput
    {
        private TaskDataStore _data = new TaskDataStore();
        
        public void Input(string[] args)
        {
            FileHandler fileHandler = new FileHandler(@args[0]);

            for (int i = 1; i < args.Length; i++)
            {
                try
                {
                    switch (args[i])
                    {
                        case "/all":
                            _data = fileHandler.Load();
                            _data.All();
                            break;
                        case "/add":
                            {
                                string Date = args[i + 2];
                                fileHandler.Load();
                                Date = Date.Replace(".", string.Empty);
                                _data.AddTask(args[i + 1], new DateTime(Convert.ToInt32(Date.Substring(4, 4)),
                                        Convert.ToInt32(Date.Substring(2, 2)), Convert.ToInt32(Date.Substring(0, 2))));

                                fileHandler.Save(_data);
                                break;
                            }
                        case "/delete":
                            _data = fileHandler.Load();
                            _data.DeleteTask(Convert.ToInt32(args[i + 1]));
                            fileHandler.Save(_data);
                            break;
                        case "/complete":
                            _data = fileHandler.Load();
                            _data.Complete(Convert.ToInt32(args[i + 1]));
                            fileHandler.Save(_data);
                            break;
                        case "/today":
                            _data = fileHandler.Load();
                            _data.Today();
                            fileHandler.Save(_data);
                            break;
                        case "/create-group":
                            _data = fileHandler.Load();
                            _data.AddGroup(args[i + 1]);
                            fileHandler.Save(_data);
                            break;
                        case "/delete-group":
                            _data = fileHandler.Load();
                            _data.DeleteGroup(args[i + 1]);
                            fileHandler.Save(_data);
                            break;
                        case "/add-to-group":
                            _data = fileHandler.Load();
                            _data.AddToGroup(Convert.ToInt32(args[i + 1]), args[i + 2]);
                            fileHandler.Save(_data);
                            break;
                        case "/delete-from-group":
                            _data = fileHandler.Load();
                            _data.DeleteFromGroup(Convert.ToInt32(args[i + 1]), args[i + 2]);
                            fileHandler.Save(_data);
                            break;
                        case "/completed":
                            _data = fileHandler.Load();
                            _data.Completed();
                            fileHandler.Save(_data);
                            break;
                        case "/add-subtask":
                            _data = fileHandler.Load();
                            _data.AddSubTask(Convert.ToInt32(args[i + 1]), args[i + 2], Convert.ToInt32(args[i + 3]));
                            fileHandler.Save(_data);
                            break;
                        case "/save":
                            FileHandler fileHandlerForSave = new FileHandler(args[i + 1]);
                            fileHandlerForSave.Save(_data);
                            break;
                        case "/load":
                            FileHandler fileHandlerForLoad = new FileHandler(args[i + 1]);
                            _data = fileHandlerForLoad.Load();
                            fileHandler.Save(_data);
                            break;
                    }
                }
                catch (ArgumentException ep)
                {
                    Console.WriteLine(ep.Message);
                }
            }
        }
    }
}
