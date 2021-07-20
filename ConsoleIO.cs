using System;

namespace Test
{
    class ConsoleInput
    {
        private TaskDataStore _mockData = new TaskDataStore();
        
        public void Input(string[] args)
        {
            string Date;

            for (int i = 1; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "/all":
                        _mockData.Load(@args[0]);
                        _mockData.All();
                        break;
                    case "/add":
                    {
                        _mockData.Load(@args[0]);
                        Date = args[i + 2];
                        Date = Date.Replace(".", string.Empty);
                        try
                        {
                                _mockData.AddTask(args[i + 1], new DateTime(Convert.ToInt32(Date.Substring(4, 4)),
                                    Convert.ToInt32(Date.Substring(2, 2)), Convert.ToInt32(Date.Substring(0, 2))));
                        }
                        catch (Exception ep)
                        {
                                Console.WriteLine(ep.Message);
                        }
                        _mockData.Save(@args[0]);
                        break;
                    }
                    case "/delete":
                        _mockData.Load(@args[0]);
                        try
                        {
                            _mockData.DeleteTask(Convert.ToInt32(args[i + 1]));
                        }
                        catch (Exception ep)
                        {
                            Console.WriteLine(ep.Message);
                        }
                        _mockData.Save(@args[0]);
                        break;
                    case "/complete":
                        _mockData.Load(@args[0]);
                        _mockData.Complete(Convert.ToInt32(args[i + 1]));
                        _mockData.Save(@args[0]);
                        break;
                    case "/today":
                        _mockData.Load(@args[0]);
                        _mockData.Today();
                        _mockData.Save(@args[0]);
                        break;
                    case "/create-group":
                        _mockData.Load(@args[0]);
                        try
                        {
                            _mockData.AddGroup(args[i + 1]);
                        }
                        catch (Exception ep)
                        {
                            Console.WriteLine(ep.Message);
                        }
                        _mockData.Save(@args[0]);
                        break;
                    case "/delete-group":
                        _mockData.Load(@args[0]);
                        try
                        {
                            _mockData.DeleteGroup(args[i + 1]);
                        }
                        catch (Exception ep)
                        {
                            Console.WriteLine(ep.Message);
                        }
                        _mockData.Save(@args[0]);
                        break;
                    case "/add-to-group":
                        _mockData.Load(@args[0]);
                        try
                        {
                            _mockData.AddToGroup(Convert.ToInt32(args[i + 1]), args[i + 2]);
                        }
                        catch (Exception ep)
                        {
                            Console.WriteLine(ep.Message);
                        }
                        _mockData.Save(@args[0]);
                        break;
                    case "/delete-from-group":
                        _mockData.Load(@args[0]);
                        try
                        {
                            _mockData.DeleteFromGroup(Convert.ToInt32(args[i + 1]), args[i + 2]);
                        }
                        catch (Exception ep)
                        {
                            Console.WriteLine(ep.Message);
                        }
                        _mockData.Save(@args[0]);
                        break;
                    case "/completed":
                        _mockData.Load(@args[0]);
                        _mockData.Completed();
                        _mockData.Save(@args[0]);
                        break;
                    case "/add-subtask":
                        _mockData.Load(@args[0]);
                        try
                        {
                            _mockData.AddSubTask(Convert.ToInt32(args[i + 1]), args[i + 2], Convert.ToInt32(args[i + 3]));
                        }
                        catch (Exception ep)
                        {
                            Console.WriteLine(ep.Message);
                        }
                        _mockData.Save(@args[0]);
                        break;
                    case "/save":
                        _mockData.Save(args[i + 1]);
                        break;
                    case "/load":
                        try
                        {
                            _mockData.Load(args[i + 1]);
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
