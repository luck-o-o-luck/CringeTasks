using System;
using System.Collections.Generic;

namespace Test
{
    class Delete
    {
        public void DeleteTask(List<Task> tasks, int id)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Id == Convert.ToInt32(id))
                {
                    for (int j = 0; j < tasks[i].SubTasks.Count; j++)
                    {
                        tasks[i].SubTasks.RemoveAt(j);
                    }
                    tasks.RemoveAt(Convert.ToInt32(id) - 1);
                }
            }
        }

        public void DeleteSubTask(List<Task> tasks, int id)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                for (int j = 0; j < tasks[i].SubTasks.Count; j++)
                {
                    if (tasks[i].SubTasks[j].Id == id)
                    {
                        tasks[i].SubTasks.RemoveAt(j);
                    }
                }
            }
        }

        public void DeleteFromGroup(List<GroupTask> group, int id, string name)
        {
            for (int i = 0; i < group.Count; i++)
            {
                if (group[i].Name == name)
                {
                    for (int j = 0; j < group[i].Tasks.Count; j++)
                    {
                        if (group[i].Tasks[j].Id == id)
                        {
                            group[i].Tasks.RemoveAt(j);
                        }
                    }
                }
            }
        }

        public void DeleteGroup(List<GroupTask> groups, string name)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i].Name == name)
                {
                    for (int j = 0; j < groups[i].Tasks.Count; j++)
                    {
                        groups[i].Tasks.RemoveAt(j);
                    }
                    groups.RemoveAt(i);
                }
            }
        }
    }
}
