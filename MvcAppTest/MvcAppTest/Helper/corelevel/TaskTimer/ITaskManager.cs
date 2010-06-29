using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAppTest.Helper.TaskTimer
{
    interface ITaskManager_WuQi
    {
        void RegisterTask(CTask_WuQi task);
        void UnRegisterTask(CTask_WuQi task);
    }
}
