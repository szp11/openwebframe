using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAppTest.Helper.Cache
{
    /// <summary>
    /// 容器内的缓存区
    /// </summary>
    public interface ICacheStorage_WuQi : System.Collections.IEnumerable
    {
        /// <summary>
        /// 获取设置一个存储项
        /// </summary>
        /// <param name="key">存储项的健值</param>
        object this[object key] { get; set; }


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
        void Add(object key, object obj);

        /// <summary>
        /// 获取存储项
        /// </summary>
        /// <param name="key">存储项的健值</param>
        /// <returns>存储的对象，如果存储中没有命中，则返回<c>null</c></returns>
        object Get(object key);


        /// <summary>
        /// 设置存储项
        /// </summary>
        /// <param name="key">存储项的健值</param>
        /// <param name="value">存储的对象</param>
        /// <remarks>
        /// 仅针对存在存储项，若不存在，则不进行任何操作
        /// </remarks>
        void Set(object key, object obj);


        /// <summary>
        /// 移除存储项
        /// </summary>
        /// <param name="key">存储项的健值</param>
        void Remove(object key);


        /// <summary>
        /// 判断存储器中是否包含指定健值的存储项
        /// </summary>
        /// <param name="key">存储项的健值</param>
        /// <returns>是/否</returns>
        bool Contains(object key);

        /// <summary>
        /// 清除此存储器中所有的项
        /// </summary>
        void Clear();

        /// <summary>
        /// 获得此存储器中所有项的健值
        /// </summary>
        /// <returns>健值列表（数组）</returns>
        object[] GetAllKeys();

        /// <summary>
        /// 获取此存储器中所有项的值
        /// </summary>
        /// <returns>存储项列表（数组）</returns>
        object[] GetAllValues();


    }
}
