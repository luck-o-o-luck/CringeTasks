using System;

namespace Test
{
    static class Display
    {
        public static void PrintTask(Task task)
        {
            if (task.SubTasks.Count != 0)
            {
                Console.WriteLine($"id {task.Id} name {task.Name} date {task.DueDate.ToShortDateString()}" +
                                  $" completed  {task.Completed}/{task.SubTasks.Count}");
            }
            else
                Console.WriteLine($"id {task.Id} name {task.Name} date {task.DueDate.ToShortDateString()}");

            foreach (SubTask subTask in task.SubTasks)
                PrintSubTask(subTask);
        }

        public static void PrintSubTask(SubTask subTask)
        {
            Console.WriteLine($"\t id {subTask.Id} name {subTask.Name} " +
                              $"date {subTask.DueDate}");
        }

        public static void PrintGroup(GroupTask group)
        {
            Console.WriteLine($"Name {group.Name}");

            foreach (Task task in group.Tasks)
                Console.WriteLine($"\t id {task.Id} name {task.Name}" +
                                  $" date {task.DueDate}");
        }
    }
}
