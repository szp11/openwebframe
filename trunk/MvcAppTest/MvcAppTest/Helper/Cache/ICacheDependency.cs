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
        /// <summary>
        /// 更新容器策略
        /// </summary>
        /// <param name="container">缓存区</param>
       bool Insert(object k, CCacheItem_WuQi item, ref ICacheStorage_WuQi container);
       bool Delete(object k, ref ICacheStorage_WuQi container);
       void Clear(ref ICacheStorage_WuQi container);
    }
}
