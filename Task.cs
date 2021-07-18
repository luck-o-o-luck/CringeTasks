using System;
using System.Collections.Generic;

namespace Test
{
    class Task : TaskBase
    {
        public List<SubTask> SubTasks = new List<SubTask>();
        public int Completed = 0;
    }
}
