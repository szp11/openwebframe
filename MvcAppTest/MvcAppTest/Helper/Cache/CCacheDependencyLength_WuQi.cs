using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace MvcAppTest.Helper.Cache
{
    /// <summary>
    /// 缓存长度策略主要是控制缓存的长度在允许范围内；以满足内存占用的限制
    /// </summary>
    public class CCacheDependencyLength_WuQi : ICacheDependency_WuQi
    {
        private int i_objectcountmax = 1000;//容器内缓存对象的上限
        private Queue<CCacheItem_WuQi> o_objectqueue;
        public CCacheDependencyLength_WuQi(int capacity)
        {
            this.i_objectcountmax = capacity;
            this.o_objectqueue = new Queue<CCacheItem_WuQi>(this.i_objectcountmax);
        }

        public int ContainerCapacity
        {
            get { return this.i_objectcountmax; }
            set { this.i_objectcountmax = value; }
        }
        public int SynchronousAllObject(List<CCacheItem_WuQi> litem, ref ICacheStorage_WuQi container)
        {
            container.Clear();
            foreach (CCacheItem_WuQi item in litem)
            {
                if (this.o_objectqueue.Count < this.i_objectcountmax)
                {
                    container.Add(item.key, item);
                    this.o_objectqueue.Enqueue(item);
                }
                else
                {
                    CCacheItem_WuQi ci = this.o_objectqueue.Dequeue();
                    container.Remove(ci.key);
                    this.o_objectqueue.Enqueue(item);
                    container.Add(item.key, item);
                }
            }
            return container.Count;
        }
         public bool Insert(object k, CCacheItem_WuQi item, ref ICacheStorage_WuQi container)
         {
             if(this.o_objectqueue.Count < this.i_objectcountmax)
             {
                 container.Add(k, item);
                 this.o_objectqueue.Enqueue(item);
                 return true;
             }
             else
             {
                 CCacheItem_WuQi ci = this.o_objectqueue.Dequeue();
                 container.Remove(ci.key);
                 this.o_objectqueue.Enqueue(item);
                 container.Add(k, item);
             }
             
             return true;
         }
         public int Insert(List<CCacheItem_WuQi> listitem, ref ICacheStorage_WuQi container)
         {
             int result = 0;
             foreach (CCacheItem_WuQi item in listitem)
             {
                 if (this.o_objectqueue.Count < this.i_objectcountmax)
                 {
                     container.Add(item.key, item);
                     this.o_objectqueue.Enqueue(item);
                     result++;
                 }
                 else
                 {
                     CCacheItem_WuQi ci = this.o_objectqueue.Dequeue();
                     container.Remove(ci.key);
                     this.o_objectqueue.Enqueue(item);
                     container.Add(item.key, item);
                     result++;
                 }
             }
             return result;
         } 
         public bool Delete(object k, ref ICacheStorage_WuQi container)
         {
             container.Remove(k);
             return true;
         }

         public int Delete(List<CCacheItem_WuQi> listitem, ref ICacheStorage_WuQi container)
         {
             
             foreach (CCacheItem_WuQi item in listitem)
             {
                 container.Remove(item.key);

             }
             return listitem.Count;
         } 
         public void Clear(ref ICacheStorage_WuQi container)
         {
             container.Clear();
             this.o_objectqueue.Clear();
         }
    }
}
