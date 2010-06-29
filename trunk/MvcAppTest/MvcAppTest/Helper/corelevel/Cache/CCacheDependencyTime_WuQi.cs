using System;
using System.Data;
using System.Collections;
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
    /// 缓存长度策略主要是控制缓存数据的时间是否过期
    /// </summary>
    public class CCacheDependencyTime_WuQi<K, T> : ICacheDependency_WuQi<K, T> where T : IObjectAdapter_WuQi<K>
    {

        public T SelectSingleObject(ref ICacheStorage_WuQi<K, T> container, K k, out int getall)
        {
            if (false != container.Contains(k))
            {
                getall = 1;//该数据已经查询到了，不在需要到数据库中查询了。
                CCacheItem_WuQi<K, T> item = container[k];
                //更新元素的访问数和最后访问时间
                item.hits++;
                item.d_lastaccesstime = DateTime.Now;
                return item.t_value;
            }
            else
                getall = 0;
            return default(T);
        }
        public List<T> Search(ref ICacheStorage_WuQi<K, T> container, int adapter, Hashtable al,out int getall)
        {
            List<T> result = new List<T>();
            getall = 0;//该策略没有缓存所有数据库数据，需要到数据库中查询。
            return result;
        }
        public int SynchronousAllObject(List<CCacheItem_WuQi<K, T>> litem, ref ICacheStorage_WuQi<K, T> container)
        {
            container.Clear();
            foreach (CCacheItem_WuQi<K, T> item in litem)
            {
                container.Add(item.key, item);
            }
            return container.Count;
        }
        public bool Insert(K k, CCacheItem_WuQi<K, T> item, ref ICacheStorage_WuQi<K, T> container)
         {
            if(!container.Contains(k))
                container.Add(k, item);             
             return true;
         }
        public int Insert(List<CCacheItem_WuQi<K, T>> listitem, ref ICacheStorage_WuQi<K, T> container)
         {
             int result = 0;
             foreach (CCacheItem_WuQi<K, T> item in listitem)
             {
                 if(!container.Contains(item.key))
                     container.Add(item.key, item);
                 result++;
             }
             return result;
         }
        public bool Delete(K k, ref ICacheStorage_WuQi<K, T> container)
         {
            if(container.Contains(k))
                container.Remove(k);
             return true;
         }

        public int Delete(List<CCacheItem_WuQi<K, T>> listitem, ref ICacheStorage_WuQi<K, T> container)
         {
             foreach (CCacheItem_WuQi<K, T> item in listitem)
             {
                 if(container.Contains(item.key))
                     container.Remove(item.key);
             }
             return listitem.Count;
         }
        public void Clear(ref ICacheStorage_WuQi<K, T> container)
         {
             container.Clear();
         }
    }
}
