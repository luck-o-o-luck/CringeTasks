using System;
using System.Collections.Generic;

namespace Test
{
    abstract class TaskBase
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime DueDate { get; set; }

        public bool IsChild = false; 

    }
}
