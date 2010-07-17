using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace MvcAppTest.Helper.corelevel.TaskTimer
{
    /// <summary>
    /// 公共任务定时器，定时触发已经注册的任务
    /// </summary>
    public class CTaskManager_WuQi : ITaskManager_WuQi
    {
        private List<CTask_WuQi> l_tasks =null;
        private object task_lock = null;
        private int timer_lock = 0;//定时器循环重复进入锁
        private  Timer ticker =null;
        private TimerCallback timerDelegate = null;
        private int peroid = 60000;
        private CTaskManager_WuQi()
        {
            l_tasks = new List<CTask_WuQi>();
            task_lock = new object();
            timerDelegate = new TimerCallback(TimerMethod);
            ticker = new Timer(timerDelegate,this,30000,peroid);
        }
        private static CTaskManager_WuQi taskManager = null;
        public static CTaskManager_WuQi GetTaskManager()
        {
            if (null != taskManager)
                return taskManager;
            else
            {
                taskManager = new CTaskManager_WuQi();
                return taskManager;
            }
        }
        public void SetTimerInternal(int peroid)
        {
            if (null != taskManager)
                this.peroid = peroid;
        }
        public void CloseTimer()
        {
            if(null != taskManager)
                ticker.Dispose();
        }
        private void TimerMethod(object state)
        {
            if(Interlocked.Exchange(ref this.timer_lock ,1) == 0)
            {
                lock (this.task_lock)
                {
                    foreach (CTask_WuQi task in l_tasks)
                    {
                        if (null != task.pdel)
                            task.pdel();
                        Thread.Sleep(0);
                    }

                }

                Interlocked.Exchange(ref this.timer_lock, 0);
            }
        }
        public void RegisterTask(CTask_WuQi ct)
        {
            lock(this.task_lock)
            {
                this.l_tasks.Add(ct);
            }
            
        }
        public void UnRegisterTask(CTask_WuQi ct)
        {
            lock(this.task_lock)
            {
                this.l_tasks.Remove(ct);
            }
            
        }
    }
}
