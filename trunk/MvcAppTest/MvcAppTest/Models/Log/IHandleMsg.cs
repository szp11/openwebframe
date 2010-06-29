using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcAppTest.Helper.Cache;

namespace MvcAppTest.Models.Log
{
    /// <summary>
    /// 主库需要实现的接口; 可以扩展自己的实现
    /// </summary>
    public interface IHandleMsg_WuQi
    {
        /// <summary>
        /// 添加LOG信息
        /// </summary>
        /// <param name="smsg">信息内容</param>
        /// <returns></returns>
        bool InsertMsg(CLogMsg_WuQi smsg);
        /// <summary>
        /// 按指定的条件删除信息
        /// </summary>
        /// <param name="adapter">删除条件</param>
        /// <param name="al">所需参数集</param>
        /// <returns>删除的个数</returns>
        int DeleteMsg(int adapter, List<uint> al);
        /// <summary>
        /// 同步数据库和缓存中的数据，LOG模块加载时使用
        /// </summary>
        /// <returns>加载数据的数量</returns>
        int SynchronousAllRecord();
        /// <summary>
        /// 获得所有的数据
        /// </summary>
        /// <returns>信息所有者</returns>
        List<CLogMsg_WuQi> ReadAllRecord(int adapter, string msgowner);
        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <param name="smsg"></param>
        /// <returns></returns>
        int WriteAllRecord(CLogMsg_WuQi[] smsg);
        /// <summary>
        /// 清除所有数据
        /// </summary>
        void ClearAllRecord();
    }
}
