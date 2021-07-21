using System;
using System.Collections.Generic;

namespace Test
{
    class GroupTasks
    {
        public string Name { get; }

        public GroupTasks(string name) { Name = name; }

        public List<Task> Tasks = new List<Task>();
    }
}
