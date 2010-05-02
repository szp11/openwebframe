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
using System.Collections;
using System.Collections.Generic;
using MvcAppTest.Helper.Cache;

namespace MvcAppTest.Models.Log
{
    /// <summary>
    /// 存贮LOG信息的单件类。可以根据需要改变主库的实现
    /// </summary>
    public class CLog_WuQi
    {

        static private bool b_init = false;//是否已经初始化
        static private CLog_WuQi c_log = null;
        static private IHandleMsg_WuQi obj_logdb = null;//主库引用
        /// <summary>
        /// 该静态函数用于初始化，必须在使用其他功能前提前调用。使用依赖注入
        /// </summary>
        /// <param name="maindb">主库的实现</param>
        static public void InitLog(IHandleMsg_WuQi maindb)
        {
            if (false == CLog_WuQi.b_init)
            {
                CLog_WuQi.c_log = new CLog_WuQi(maindb);
                obj_logdb.SynchronousAllRecord();
            }
        }

        static public CLog_WuQi GetLog()
        {

            if (false == CLog_WuQi.b_init)
            {
                return null;
            }
            else
            {
                return CLog_WuQi.c_log;
            }
        }
        /// <summary>
        /// 私有构造函数
        /// </summary>
        /// <param name="maindb"></param>
        private CLog_WuQi(IHandleMsg_WuQi maindb)
        {
            if (null == CLog_WuQi.obj_logdb)
                CLog_WuQi.obj_logdb = maindb;

            CLog_WuQi.b_init = true;
        }
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="cmsg">根据不同的log信息 实例化不同的继承类</param>
        public void AddMsg(CLogMsg_WuQi cmsg)
        {
            if (false == CLog_WuQi.b_init)
            {
                return;
            }

            obj_logdb.InsertMsg(cmsg);
            return;
        }
        /// <summary>
        /// 删除某个所有人的信息
        /// </summary>
        /// <param name="msgowner">信息所有者</param>
        /// <returns></returns>
        public int DeleteMsgOwner(string msgowner)
        {
            if (false == CLog_WuQi.b_init)
                return -1;
            ArrayList al = new ArrayList();
            al.Add(msgowner);
            return obj_logdb.DeleteMsg(1, al);
        }
        /// <summary>
        /// 获得某个人的所有信息
        /// </summary>
        /// <param name="msgowner">信息所有者</param>
        /// <returns></returns>
        public List<CLogMsg_WuQi> GetAllMsg(string msgowner)
        {
            List<CLogMsg_WuQi> loglist = new List<CLogMsg_WuQi>();
            ArrayList al = obj_logdb.ReadAllRecord(1, msgowner);
            if (al.Count == 0)
                return loglist;
            else
            {
                CLogMsg_WuQi[] slr = new CLogMsg_WuQi[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    slr[i] = (CLogMsg_WuQi)al[i];
                    loglist.Add(slr[i]);

                }

                return loglist;
            }
        }
    }
}
