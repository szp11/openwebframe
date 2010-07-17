using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;

namespace MvcAppTest.Helper.corelevel.Cache
{
    public class CCacheDependencyNull_WuQi<K, T> : ICacheDependency_WuQi<K, T> where T : IObjectAdapter_WuQi<K>
    {

        public T SelectSingleObject(ref ICacheStorage_WuQi<K, T> container, K k, out int getall)
        {
            getall = 0;//该策略无缓存，需要再到数据库中查询了。
            return default(T);
        }

        public List<T> Search(ref ICacheStorage_WuQi<K, T> container, int adapter, Hashtable al,out int getall)
        {
            getall =0;//该策略无缓存，需要再到数据库中查询。
            List<T> result = new List<T>();
            return result;
        }

        public int SynchronousAllObject(List<CCacheItem_WuQi<K,T>> litem, ref ICacheStorage_WuQi<K,T> container)
        {
            container.Clear();
            return container.Count;
        }
        public bool Insert(K k, CCacheItem_WuQi<K, T> item, ref ICacheStorage_WuQi<K, T> container)
       {
           return true;
       }
        public int Insert(List<CCacheItem_WuQi<K, T>> listitem, ref ICacheStorage_WuQi<K, T> container)
        {
            return listitem.Count;
        }
        public bool Delete(K k, ref ICacheStorage_WuQi<K, T> container)
       {
           return true;
       }
        public int Delete(List<CCacheItem_WuQi<K, T>> listitem, ref ICacheStorage_WuQi<K, T> container)
        {
            return listitem.Count;
        }

        public void Clear(ref ICacheStorage_WuQi<K, T> container)
       {
           return;
       }

    }
}
