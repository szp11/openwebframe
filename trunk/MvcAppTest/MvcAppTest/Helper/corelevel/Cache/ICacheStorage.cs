using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAppTest.Helper.corelevel.Cache
{
    /// <summary>
    /// 容器内的缓存区
    /// 泛型缓冲区能够减少值类型数据的装箱、拆箱操作，从而优化速度
    /// </summary>
    public interface ICacheStorage_WuQi<K, T> //: IEnumerable<KeyValuePair<K, T>>
    {
        /// <summary>
        /// 获取设置一个存储项
        /// </summary>
        /// <param name="key">存储项的健值</param>
        CCacheItem_WuQi<K, T> this[K key] { get; set; }


        /// <summary>
        /// 获取此存储器所存储项的个数
        /// </summary>
      int Count { get; }

        /// <summary>
        /// 添加一项到存储器中
        /// </summary>
        /// <param name="key">存储项的健值</param>
        /// <param name="value">存储的对象</param>
        /// <remarks>
        /// 如果存在相同的健值，则更新存储的对象
        /// </remarks>
      void Add(K key, CCacheItem_WuQi<K, T> obj);

        /// <summary>
        /// 获取存储项
        /// </summary>
        /// <param name="key">存储项的健值</param>
        /// <returns>存储的对象，如果存储中没有命中，则返回<c>null</c></returns>
      CCacheItem_WuQi<K, T> Get(K key);


        /// <summary>
        /// 设置存储项
        /// </summary>
        /// <param name="key">存储项的健值</param>
        /// <param name="value">存储的对象</param>
        /// <remarks>
        /// 仅针对存在存储项，若不存在，则不进行任何操作
        /// </remarks>
      void Set(K key, CCacheItem_WuQi<K, T> obj);


        /// <summary>
        /// 移除存储项
        /// </summary>
        /// <param name="key">存储项的健值</param>
        void Remove(K key);


        /// <summary>
        /// 判断存储器中是否包含指定健值的存储项
        /// </summary>
        /// <param name="key">存储项的健值</param>
        /// <returns>是/否</returns>
        bool Contains(K key);

        /// <summary>
        /// 清除此存储器中所有的项
        /// </summary>
        void Clear();

        /// <summary>
        /// 获得此存储器中所有项的健值
        /// </summary>
        /// <returns>健值列表（数组）</returns>
        K[] GetAllKeys();

        /// <summary>
        /// 获取此存储器中所有项的值
        /// </summary>
        /// <returns>存储项列表（数组）</returns>
        CCacheItem_WuQi<K, T>[] GetAllValues();

        Dictionary<K, CCacheItem_WuQi<K, T>>.Enumerator GetEnumerator();
    }
}
