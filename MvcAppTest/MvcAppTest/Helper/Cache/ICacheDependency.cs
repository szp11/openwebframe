using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAppTest.Helper.Cache
{
    /// <summary>
    /// 缓存过期策略
    /// </summary>
    public interface ICacheDependency_WuQi
    {
        int SynchronousAllObject(List<CCacheItem_WuQi> litem, ref ICacheStorage_WuQi container);
       bool Insert(object key, CCacheItem_WuQi item, ref ICacheStorage_WuQi container);
       int Insert(List<CCacheItem_WuQi> listitem,ref ICacheStorage_WuQi container);
       bool Delete(object key, ref ICacheStorage_WuQi container);
       int Delete(List<CCacheItem_WuQi> listitem, ref ICacheStorage_WuQi container);
       void Clear(ref ICacheStorage_WuQi container);
    }
}
