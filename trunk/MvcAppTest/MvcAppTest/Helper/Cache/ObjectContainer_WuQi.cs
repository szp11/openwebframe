using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;

namespace MvcAppTest.Helper.Cache
{
    /// <summary>
    /// 对象容器，用于缓存外部对象
    /// </summary>
    /// <typeparam name="T">泛型对象</typeparam>
    public class ObjectContainer_WuQi<T> where T : IObjectAdapter_WuQi
    {
        private ICacheDependency_WuQi obj_dependency = null;//过期策略
        private ICacheStorage_WuQi obj_containers = null;//缓冲区
        private System.Threading.ReaderWriterLock obj_rwl;//读写锁        

        /// <summary>
        /// 默认改造函数
        /// </summary>
        public ObjectContainer_WuQi(ICacheStorage_WuQi ics,ICacheDependency_WuQi icd)
        {
            this.obj_dependency = icd;
            this.obj_containers = ics;
            this.obj_rwl = new System.Threading.ReaderWriterLock();
        }

        public object[] GetAllObject()
        {
            return this.obj_containers.GetAllValues();
        }
        /// <summary>
        /// 同步数据库，即从数据库中得到全部对象
        /// </summary>
        /// <returns>所有对象组成的数据字典</returns>
        public virtual Dictionary<object, T> SynchronousDB()
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
                Dictionary<object, T> objs = SynchronousDB();
                if (null == objs || 0 == objs.Count)
                    return 0;
                //首先清空缓存区
                obj_containers.Clear();
                foreach (KeyValuePair<object, T> kvp in objs)
                {
                    this.obj_containers.Add(kvp.Key, new CCacheItem_WuQi(kvp.Key, kvp.Value));
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                obj_rwl.ReleaseWriterLock();
            }
            return this.obj_containers.Count;
        }
        /// <summary>
        /// 插入对象到数据库中，由子类实现
        /// </summary>
        /// <param name="t">对象的实例</param>
        /// <param name="k">对象的键</param>
        /// <returns></returns>
        protected virtual bool InsertDB( object k,T t)
        {
            return true;
        }
        /// <summary>
        /// 插入一个对象到容器中
        /// </summary>
        /// <param name="t">对象的值</param>
        /// <param name="k">对象的键，用于检索对象</param>
        public bool Insert(object k, T t)
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
                obj_containers.Add(k,new CCacheItem_WuQi(k,t));

            }
            catch (System.Exception e)
            {
                if(0 != rollback)
                    DeleteDB(k,t);
                throw e;
            }
            finally
            {
                obj_rwl.ReleaseWriterLock();
            }
            return true;
        }
        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <param name="t">对象的值</param>
        /// <param name="k">对象的键</param>
        /// <returns></returns>
        public bool Delete(object k, T t)
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
                    this.obj_containers.Remove(k);
                    return true;
                }

            }
            catch (System.Exception e)
            {
                //如果回滚标志置1
                if (0 != rollback)
                    InsertDB(k, t);
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
        protected virtual bool DeleteDB( object k,T t)
        {
            return true;
        }
        /// <summary>
        /// 更新一个对象
        /// </summary>
        /// <param name="t">对象的值</param>
        /// <param name="k">对象的键</param>
        /// <returns></returns>
        public bool Update(object k, T t)
        {
            int rollback = 0;//设置回滚标记；用来同步数据库操作和内存操作
            object backT =null;
            try
            {
                obj_rwl.AcquireWriterLock(System.Threading.Timeout.Infinite);
                //在对象集合中寻找
                if (false != this.obj_containers.Contains(k))
                {
                    backT = this.obj_containers[k];
                    this.obj_containers[k] = new CCacheItem_WuQi(k,t);
                    rollback = 1;
                }
                //从数据库中更新，如果成功将回滚置1
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
        protected virtual bool UpdateDB(object k, T t)
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
                this.obj_containers.Clear();

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
        public ArrayList Search(int adapter,ArrayList paraset)
        {
            ArrayList al = new ArrayList();
            
            try
            {
                obj_rwl.AcquireReaderLock(System.Threading.Timeout.Infinite);
                //在对象集合中寻找
                foreach (DictionaryEntry defront in obj_containers)
                {
                    CCacheItem_WuQi item = (CCacheItem_WuQi)defront.Value;
                    T t = (T)item.t_value;
                    if (false != t.IsMe(adapter, paraset))
                    {
                        //更新元素的访问数和最后访问时间
                        item.hits++;
                        item.d_lastaccesstime = DateTime.Now;
                        al.Add(t);
                    }
                }

            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                obj_rwl.ReleaseReaderLock();
            }
            return al;
        }
        /// <summary>
        /// 查找单个对象
        /// </summary>
        /// <param name="k">对象的键，用于检索对象</param>
        /// <returns>为null时表示未找到对象</returns>
        public object SelectSingleObject(object k)
        {
            object result = null;
            try
            {
                obj_rwl.AcquireReaderLock(System.Threading.Timeout.Infinite);
                if(false != this.obj_containers.Contains(k))
                {
                    CCacheItem_WuQi item = (CCacheItem_WuQi)this.obj_containers[k];
                    //更新元素的访问数和最后访问时间
                    item.hits++;
                    item.d_lastaccesstime = DateTime.Now;
                    result = item.t_value;
                }
            }
            catch (System.Exception e)
            {
                throw e;            	
            }
            finally
            {
                obj_rwl.ReleaseReaderLock();
            }            
            return result;
        }

    }
}
