using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAppTest.Helper.corelevel.TaskTimer
{
    interface ITaskManager_WuQi
    {
        void RegisterTask(CTask_WuQi task);
        void UnRegisterTask(CTask_WuQi task);
    }
}
