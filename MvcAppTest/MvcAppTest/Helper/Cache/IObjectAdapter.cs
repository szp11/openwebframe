using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAppTest.Helper.Cache
{
    /// <summary>
    /// 放入容器的对象必须实现的接口，把条件查找等逻辑从容器中剥离出来
    /// 
    /// </summary>
    public interface IObjectAdapter_WuQi<K>
    {
        /// <summary>
        /// 条件查找，相当于select里的where。
        /// </summary>
        /// <param name="adapter">0：默认，能从容器中得到所有的对象。其他数值自定义</param>
        /// <param name="al">需要的外部参数集</param>
        /// <returns>符合查找条件返回true</returns>
        bool IsMe(int adapter, Hashtable args);
        K GetMyGuid();
    }
}
