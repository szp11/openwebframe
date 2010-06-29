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
    public class CCacheDependencyNull_WuQi<K, T> : ICacheDependency_WuQi<K, T> where T : IObjectAdapter_WuQi<K>
    {

        public T SelectSingleObject(ref ICacheStorage_WuQi<K, T> container, K k, out int getall)
        {
            getall = 1;//该策略缓存了所有数据库数据，不需要再到数据库中查询了。
            if (false != container.Contains(k))
            {
                CCacheItem_WuQi<K, T> item = container[k];
                //更新元素的访问数和最后访问时间
                item.hits++;
                item.d_lastaccesstime = DateTime.Now;
                return item.t_value;
            }
            return default(T);
        }

        public List<T> Search(ref ICacheStorage_WuQi<K, T> container, int adapter, Hashtable al,out int getall)
        {
            getall =1;//该策略缓存了所有数据库数据，不需要再到数据库中查询了。
            List<T> result = new List<T>();
            foreach (KeyValuePair<K, CCacheItem_WuQi<K, T>> defront in container)
            {
                CCacheItem_WuQi<K, T> item = (CCacheItem_WuQi<K, T>)defront.Value;
                T t = item.t_value;
                if (false != t.IsMe(adapter, al))
                {
                    //更新元素的访问数和最后访问时间
                    item.hits++;
                    item.d_lastaccesstime = DateTime.Now;
                    result.Add(t);
                }
            }
            return result;
        }

        public int SynchronousAllObject(List<CCacheItem_WuQi<K,T>> litem, ref ICacheStorage_WuQi<K,T> container)
        {
            container.Clear();
            foreach (CCacheItem_WuQi<K, T> item in litem)
            {
                if(!container.Contains(item.key))
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
            foreach (CCacheItem_WuQi<K, T> item in listitem)
            {
                if (!container.Contains(item.key))
                    container.Add(item.key, item);
            }
            return listitem.Count;
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
