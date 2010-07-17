using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using MvcAppTest.Helper.corelevel.TaskTimer;
using MvcAppTest.Helper.corelevel.Exception;
namespace MvcAppTest.Helper.corelevel.Cache
{
    /// <summary>
    /// 对象容器，用于缓存外部对象
    /// </summary>
    /// <typeparam name="T">泛型对象</typeparam>
    public class ObjectContainer_WuQi<K,T> where T : IObjectAdapter_WuQi<K>
    {
        private ICacheDependency_WuQi<K, T> obj_dependency = null;//过期策略
        private ICacheStorage_WuQi<K, T> obj_containers = null;//缓冲区
        private System.Threading.ReaderWriterLock obj_rwl =null;//读写锁        
        private int intervalMinuteTime = 24;
        /// <summary>
        /// 默认改造函数
        /// </summary>
        public ObjectContainer_WuQi(ICacheStorage_WuQi<K, T> ics, ICacheDependency_WuQi<K, T> icd)
        {
            this.obj_dependency = icd;
            this.obj_containers = ics;
            this.obj_rwl = new System.Threading.ReaderWriterLock();
        }

        /// <summary>
        /// 将容器注册到任务定时器，与定时策略CCacheDependencyTime_WuQi同时使用，将定时检查容器内数据是否过期.
        /// </summary>
        /// <param name="intervaltime">对象失效时间以分钟为单位</param>
        public void RegisterTimeDependency(int intervaltime)
        {
            this.intervalMinuteTime = intervaltime;
            CTask_WuQi task = new CTask_WuQi();
            task.pdel = this.ProcessStorage;
            CTaskManager_WuQi taskmanager = CTaskManager_WuQi.GetTaskManager();
            taskmanager.RegisterTask(task);
        }
        private void ProcessStorage()
        {
            try
            {
                obj_rwl.AcquireWriterLock(System.Threading.Timeout.Infinite);
                //在对象集合中寻找
                List<CCacheItem_WuQi<K, T>> result = new List<CCacheItem_WuQi<K, T>>();
                foreach (KeyValuePair<K, CCacheItem_WuQi<K, T>> defront in obj_containers)
                {
                    CCacheItem_WuQi<K, T> item = (CCacheItem_WuQi<K, T>)defront.Value;
                    if (false != item.IsExpire(intervalMinuteTime))
                    {
                        result.Add(item);
                    }
                }
                foreach (CCacheItem_WuQi<K, T> item in result)
                {
                    obj_containers.Remove(item.key);
                }
            }
            catch (System.Exception e)
            {
                CExceptionContainer_WuQi.ProcessException(e);
                throw e;
            }
            finally
            {
                obj_rwl.ReleaseWriterLock();
            }
        }


        /// <summary>
        /// 同步数据库，即从数据库中得到全部对象
        /// 注意同步数据库时必须回调一下对象的SetMyGuide（）来得到最后的guid。
        /// </summary>
        /// <returns>所有对象组成的数据字典</returns>
        public virtual Dictionary<K, T> SynchronousDB()
        {
            return null;
        }
        /// <summary>
        /// 与数据库同步所有的对象
        /// </summary>
        /// <param name="objs"></param>
        public int SynchronousAllObject()
        {
            try
            {
                obj_rwl.AcquireWriterLock(System.Threading.Timeout.Infinite);
                Dictionary<K, T> objs = SynchronousDB();
                if (null == objs || 0 == objs.Count)
                    return 0;
                List<CCacheItem_WuQi<K, T>> litem = new List<CCacheItem_WuQi<K,T>>();
                foreach (KeyValuePair<K, T> kvp in objs)
                {
                    litem.Add(new CCacheItem_WuQi<K,T>(kvp.Key, kvp.Value));                    
                }
                obj_dependency.SynchronousAllObject(litem,ref this.obj_containers);
            }
            catch (System.Exception e)
            {
                CExceptionContainer_WuQi.ProcessException(e);
                throw e;
            }
            finally
            {
                obj_rwl.ReleaseWriterLock();
            }
            return this.obj_containers.Count;
        }

        protected virtual bool InsertDB(int condition, List<T> lt)
        {
            return true;
        }
        public bool Insert(int condition,List<T> lt)
        {
            if (0 == lt.Count)
                return true;
            //设置回滚标记；用来同步数据库操作和内存操作
            int rollback = 0;
            try
            {
                obj_rwl.AcquireWriterLock(System.Threading.Timeout.Infinite);
                //如果插入数据库失败，直接返回
                if (false == InsertDB(condition, lt))
                    return false;
                else
                    rollback = 1;

                List<CCacheItem_WuQi<K,T>> litem = new List<CCacheItem_WuQi<K,T>>();
                foreach (T t in lt)
                {
                    litem.Add(new CCacheItem_WuQi<K,T>(t.GetMyGuid(),t));
                }
                obj_dependency.Insert(litem, ref obj_containers);

            }
            catch (System.Exception e)
            {
                if (0 != rollback)
                    DeleteDB(condition, lt);
                CExceptionContainer_WuQi.ProcessException(e);
                throw e;
            }
            finally
            {
                obj_rwl.ReleaseWriterLock();
            }
            return true;
        }
        /// <summary>
        /// 插入对象到数据库中，由子类实现
        /// </summary>
        /// <param name="t">对象的实例</param>
        /// <param name="k">对象的键</param>
        /// <returns></returns>
        protected virtual bool InsertDB( K k,T t)
        {
            return true;
        }
        /// <summary>
        /// 插入一个对象到容器中
        /// </summary>
        /// <param name="t">对象的值</param>
        /// <param name="k">对象的键，用于检索对象</param>
        public bool Insert(K k, T t)
        {
            //设置回滚标记；用来同步数据库操作和内存操作
            int rollback = 0;
            try
            {
                obj_rwl.AcquireWriterLock(System.Threading.Timeout.Infinite);
                //如果插入数据库失败，直接返回
                if (false == InsertDB(k,t))
                    return false;
                else
                    rollback = 1;                
                obj_dependency.Insert(k, new CCacheItem_WuQi<K,T>(k, t), ref obj_containers);                

            }
            catch (System.Exception e)
            {
                if(0 != rollback)
                    DeleteDB(k,t);
                CExceptionContainer_WuQi.ProcessException(e);
                throw e;
            }
            finally
            {
                obj_rwl.ReleaseWriterLock();
            }
            return true;
        }

        protected virtual bool DeleteDB(int condition,List<T> lt) 
        {
            return true;
        }
        public int Delete(int condition,List<T> lt)
        {
            if (0 == lt.Count)
                return 0;
            int rollback = 0;//设置回滚标记；用来同步数据库操作和内存操作
            int count = 0;
            try
            {
                obj_rwl.AcquireWriterLock(System.Threading.Timeout.Infinite);
                //首先从数据库中删除，如果成功将回滚置1
                if (false == DeleteDB(condition, lt))
                    return 0;
                else
                    rollback = 1;
                //在对象集合中寻找
                List<CCacheItem_WuQi<K,T>> litem = new List<CCacheItem_WuQi<K,T>>();
                foreach (T t in lt)
                {
                    litem.Add(new CCacheItem_WuQi<K,T>(t.GetMyGuid(), t));
                }

               count = obj_dependency.Delete(litem, ref this.obj_containers);

            }
            catch (System.Exception e)
            {
                //如果回滚标志置1
                if (0 != rollback)
                {
                    InsertDB(condition, lt);
                    count = 0;
                }
                CExceptionContainer_WuQi.ProcessException(e);
                throw e;
            }
            finally
            {
                obj_rwl.ReleaseWriterLock();
            }
            return count;
        }

        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <param name="t">对象的值</param>
        /// <param name="k">对象的键</param>
        /// <returns></returns>
        public bool Delete(K k, T t)
        {
            int rollback = 0;//设置回滚标记；用来同步数据库操作和内存操作
            try
            {
                obj_rwl.AcquireWriterLock(System.Threading.Timeout.Infinite);
                //首先从数据库中删除，如果成功将回滚置1
                if (false == DeleteDB(k, t))
                    return false;
                else
                    rollback = 1;
                //在对象集合中寻找
                if (false != this.obj_containers.Contains(k))
                {
                    obj_dependency.Delete(k, ref this.obj_containers);
                    return true;
                }

            }
            catch (System.Exception e)
            {
                //如果回滚标志置1
                if (0 != rollback)
                    InsertDB(k, t);
                CExceptionContainer_WuQi.ProcessException(e);
                throw e;
            }
            finally
            {
                obj_rwl.ReleaseWriterLock();
            }
            return true;
        }
        /// <summary>
        /// 从数据库删除一个对象，由子类实现
        /// </summary>
        /// <param name="t">对象的值</param>
        /// <param name="k">对象的键</param>
        /// <returns></returns>
        protected virtual bool DeleteDB( K k,T t)
        {
            return true;
        }

        protected virtual bool UpdateDB(int condition,List<T> lt) 
        {
            return true;
        }
        public bool Update(int condition, List<T> lt)
        {
            if (0 == lt.Count)
                return true;
            List<CCacheItem_WuQi<K,T>> backT = new List<CCacheItem_WuQi<K,T>>();
            try
            {
                obj_rwl.AcquireWriterLock(System.Threading.Timeout.Infinite);
                //在对象集合中寻找，此处不需要过期策略
                foreach(T t in lt)
                {
                    if(false != this.obj_containers.Contains(t.GetMyGuid()))
                    {
                        backT.Add((CCacheItem_WuQi<K,T>)this.obj_containers[t.GetMyGuid()]);
                        this.obj_containers[t.GetMyGuid()] = new CCacheItem_WuQi<K,T>(t.GetMyGuid(), t);
                    }
                    
                }
                //从数据库中更新，如果成功将回滚置1
                //该操作将同时更新该对象的所有数据项，而没有排除不需要更新的数据项；
                //所以该操作要求数据库表存储在一个数据服务器上。
                //不适用于分布式服务器，因为分布式服务器常采用分表策略，该操作将大大降低效率。
                if (false == UpdateDB(condition, lt))
                {
                    foreach( CCacheItem_WuQi<K,T> item in backT)
                    {
                        this.obj_containers[item.key] = item;
                    }
                }
            }
            catch (System.Exception e)
            {
                CExceptionContainer_WuQi.ProcessException(e);
                throw e;
            }
            finally
            {
                obj_rwl.ReleaseWriterLock();
            }
            return true;
        }
        /// <summary>
        /// 更新一个对象
        /// </summary>
        /// <param name="t">对象的值</param>
        /// <param name="k">对象的键</param>
        /// <returns></returns>
        public bool Update(K k, T t)
        {
            int rollback = 0;//设置回滚标记；用来同步数据库操作和内存操作
            CCacheItem_WuQi<K, T> backT = null;
            try
            {
                obj_rwl.AcquireWriterLock(System.Threading.Timeout.Infinite);
                //在对象集合中寻找，此处不需要过期策略
                if (false != this.obj_containers.Contains(k))
                {
                    backT = this.obj_containers[k];
                    this.obj_containers[k] = new CCacheItem_WuQi<K,T>(k,t);
                    rollback = 1;
                }
                //从数据库中更新，如果成功将回滚置1
                //该操作将同时更新该对象的所有数据项，而没有排除不需要更新的数据项；
                //所以该操作要求数据库表存储在一个数据服务器上。
                //不适用于分布式服务器，因为分布式服务器常采用分表策略，该操作将大大降低效率。
                if (false == UpdateDB(k, t))
                {
                    this.obj_containers[k] = backT;
                }
            }
            catch (System.Exception e)
            {
                //如果回滚标志置1
                if (0 != rollback)
                    this.obj_containers[k] = backT;
                CExceptionContainer_WuQi.ProcessException(e);
                throw e;
            }
            finally
            {
                obj_rwl.ReleaseWriterLock();
            }
            return true;
        }
        /// <summary>
        /// 从数据库更新一个对象，由子类实现
        /// </summary>
        /// <param name="t"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        protected virtual bool UpdateDB(K k, T t)
        {
            return true;
        }
        /// <summary>
        /// 删除所有对象
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            try
            {
                obj_rwl.AcquireWriterLock(System.Threading.Timeout.Infinite);
                ClearDB();
                this.obj_dependency.Clear(ref this.obj_containers);                

            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                obj_rwl.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 删除数据库中所有对象
        /// </summary>
        /// <returns></returns>
        protected virtual void ClearDB()
        {
            
        }
        /// <summary>
        /// 查找符合条件的对象，并将它们存入一个list中
        /// </summary>
        /// <param name="adapter">查找条件</param>
        /// <returns>如果返回的list的长度为0，表示没有找到符合条件的对象</returns>
        public List<T> Search(int adapter,Hashtable paraset)
        {
            List<T> result = null;
            try
            {
                obj_rwl.AcquireReaderLock(System.Threading.Timeout.Infinite);
                //在对象集合中寻找
                int getall;//根据管理策略的不同而选择不同的值，如果该策略缓存了所有的数据，则getall为1，表示不需要再到数据库中查询了。
                result = this.obj_dependency.Search(ref this.obj_containers, adapter, paraset,out getall);
                obj_rwl.ReleaseReaderLock();

                if (0 == getall)
                {
                    obj_rwl.AcquireWriterLock(System.Threading.Timeout.Infinite);
                    result.Clear();
                    result = SearchDB(adapter, paraset);
                    if(result.Count > 0)
                    {
                        foreach ( T t in result)
                        {
                            this.obj_dependency.Insert(t.GetMyGuid(), new CCacheItem_WuQi<K, T>(t.GetMyGuid(), t), ref this.obj_containers);
                        }
                    }
                    obj_rwl.ReleaseWriterLock();
                }
            }
            catch (System.Exception e)
            {
                CExceptionContainer_WuQi.ProcessException(e);
                throw e;
            }
            finally
            {
                if (obj_rwl.IsReaderLockHeld)
                    obj_rwl.ReleaseReaderLock();
                if (obj_rwl.IsWriterLockHeld)
                    obj_rwl.ReleaseWriterLock();
            }
            return result;
        }

        protected virtual List<T> SearchDB(int adapter,Hashtable paraset)
        {
            return new List<T>();
        }

        /// <summary>
        /// 查找单个对象
        /// </summary>
        /// <param name="k">对象的键，用于检索对象</param>
        /// <returns>为null时表示未找到对象</returns>
        public T SelectSingleObject(K k)
        {
            T result= default(T);
            try
            {
                obj_rwl.AcquireReaderLock(System.Threading.Timeout.Infinite);
                int getall;
                result = this.obj_dependency.SelectSingleObject(ref this.obj_containers, k,out getall);
                obj_rwl.ReleaseReaderLock();
                //如果已经查询到对象，则不需要再到数据库中查询了
                if (!result.Equals(default(T)))
                    return result;
                if (0 == getall)
                {
                    obj_rwl.AcquireWriterLock(System.Threading.Timeout.Infinite);
                    result = SelectSingleObjectDB(k);
                    if (!result.Equals(default(T)))
                    {
                        this.obj_dependency.Insert(result.GetMyGuid(), new CCacheItem_WuQi<K, T>(result.GetMyGuid(), result), ref this.obj_containers);
                    }
                    obj_rwl.ReleaseWriterLock();
                }
            }
            catch (System.Exception e)
            {
                CExceptionContainer_WuQi.ProcessException(e);
                throw e;            	
            }
            finally
            {
                if (obj_rwl.IsReaderLockHeld)
                    obj_rwl.ReleaseReaderLock();
                if (obj_rwl.IsWriterLockHeld)
                    obj_rwl.ReleaseWriterLock();
            }            
            return result;
        }
        protected virtual T SelectSingleObjectDB(K key)
        {
            return default(T);
        }
    }
}
