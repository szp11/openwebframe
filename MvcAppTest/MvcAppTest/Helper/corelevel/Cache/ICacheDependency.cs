using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAppTest.Helper.Cache
{
    /// <summary>
    /// 缓存过期策略
    /// </summary>
    public interface ICacheDependency_WuQi<K, T> where T : IObjectAdapter_WuQi<K>
    {
        int SynchronousAllObject(List<CCacheItem_WuQi<K, T>> litem, ref ICacheStorage_WuQi<K, T> container);
        bool Insert(K key, CCacheItem_WuQi<K, T> item, ref ICacheStorage_WuQi<K, T> container);
        int Insert(List<CCacheItem_WuQi<K, T>> listitem, ref ICacheStorage_WuQi<K, T> container);
        bool Delete(K key, ref ICacheStorage_WuQi<K, T> container);
        int Delete(List<CCacheItem_WuQi<K, T>> listitem, ref ICacheStorage_WuQi<K, T> container);
        void Clear(ref ICacheStorage_WuQi<K, T> container);
        List<T> Search(ref ICacheStorage_WuQi<K, T> container,int adapter,Hashtable al,out int getall);
        T SelectSingleObject(ref ICacheStorage_WuQi<K, T> container, K key,out int getall);
    }
}
