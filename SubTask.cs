using System;
using System.Collections.Generic;

namespace Test
{
    class SubTask : TaskBase
    {
        public Task ParentTask { get; set; }
    }
}
