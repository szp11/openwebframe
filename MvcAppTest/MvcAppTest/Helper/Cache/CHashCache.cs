/*
 * 开源WEB统一开发框架
 * 
 * 具有配置、日志、异常、缓存、权限控制等各业务通用的多层统一开发框架，为业务管理系统的开发提高了速度和效率。
 * 
 * */
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

namespace MvcAppTest.Helper.Cache
{
    public class CHashCache_WuQi : ICacheStorage_WuQi
    {
        private System.Collections.Hashtable obj_cache= null;
        public CHashCache_WuQi()
        {
            if (null == obj_cache)
                obj_cache = new System.Collections.Hashtable();
        }
        /// <summary>
        /// 获取设置一个存储项
        /// </summary>
        /// <param name="key">存储项的健值</param>
        public object this[object key] { get { return this.obj_cache[key]; } set { this.obj_cache[key] = value; } }


        /// <summary>
        /// 获取此存储器所存储项的个数
        /// </summary>
        public int Count { get { return this.obj_cache.Count; } }

        /// <summary>
        /// 添加一项到存储器中
        /// </summary>
        /// <param name="key">存储项的健值</param>
        /// <param name="value">存储的对象</param>
        /// <remarks>
        /// 如果存在相同的健值，则更新存储的对象
        /// </remarks>
        public void Add(object key, object obj)
        {
            this.obj_cache.Add(key, obj);
        }

        /// <summary>
        /// 获取存储项
        /// </summary>
        /// <param name="key">存储项的健值</param>
        /// <returns>存储的对象，如果存储中没有命中，则返回<c>null</c></returns>
        public object Get(object key)
        {
            return this.obj_cache[key];
        }


        /// <summary>
        /// 设置存储项
        /// </summary>
        /// <param name="key">存储项的健值</param>
        /// <param name="value">存储的对象</param>
        /// <remarks>
        /// 仅针对存在存储项，若不存在，则不进行任何操作
        /// </remarks>
        public void Set(object key, object obj)
        {
            this.obj_cache[key] = obj;
        }


        /// <summary>
        /// 移除存储项
        /// </summary>
        /// <param name="key">存储项的健值</param>
        public void Remove(object key)
        {
           this.obj_cache.Remove(key);
        }


        /// <summary>
        /// 判断存储器中是否包含指定健值的存储项
        /// </summary>
        /// <param name="key">存储项的健值</param>
        /// <returns>是/否</returns>
        public bool Contains(object key)
        {
            return this.obj_cache.Contains(key);
        }

        /// <summary>
        /// 清除此存储器中所有的项
        /// </summary>
        public void Clear()
        {
            this.obj_cache.Clear();
        }

        /// <summary>
        /// 获得此存储器中所有项的健值
        /// </summary>
        /// <returns>健值列表（数组）</returns>
        public object[] GetAllKeys()
        {
            object[] tmp = new object[this.obj_cache.Count];
            int i =0;
            foreach(System.Collections.DictionaryEntry d in obj_cache)
            {
                tmp[i] = d.Key; 
            }
            return tmp;
        }

        /// <summary>
        /// 获取此存储器中所有项的值
        /// </summary>
        /// <returns>存储项列表（数组）</returns>
        public  object[] GetAllValues()
        {
            object[] tmp = new object[this.obj_cache.Count];
            int i = 0;
            foreach (System.Collections.DictionaryEntry d in obj_cache)
            {
                tmp[i] = d.Value;
            }
            return tmp;
        }


        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.obj_cache.GetEnumerator();
        }

    }
}
