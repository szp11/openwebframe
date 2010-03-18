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
        /// <param name="maximum">缓存上限</param>
        void UpdateCache(ref ICacheStorage_WuQi container);
    }
}
