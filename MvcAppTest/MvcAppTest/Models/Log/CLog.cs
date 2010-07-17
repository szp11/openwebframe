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
using MvcAppTest.Helper.corelevel.Cache;

namespace MvcAppTest.Models.Log
{
    public enum LOGSTATE
    {
        LogDebug = 0x1, LogRun = 0x2,
    }
    /// <summary>
    /// 存贮LOG信息的单件类。可以根据需要改变主库的实现
    /// </summary>
    public class CLog_WuQi
    {
        static public LOGSTATE logstate_all =LOGSTATE.LogDebug|LOGSTATE.LogRun;
        static public LOGSTATE logstate_now =LOGSTATE.LogDebug;
        static private bool b_init = false;//是否已经初始化
        static private CLog_WuQi c_log = null;
         private IHandleMsg_WuQi obj_logdb = null;//主信息库引用
         private IHandleMark_WuQi obj_markdb = null;//标记库引用
        /// <summary>
        /// 该静态函数用于初始化，必须在使用其他功能前提前调用。使用依赖注入
        /// </summary>
        /// <param name="maindb">主库的实现</param>
         public void InitLog(IHandleMsg_WuQi msgdb,IHandleMark_WuQi markdb)
        {
            if (false == CLog_WuQi.b_init)
            {
                if (null == obj_logdb)
                    obj_logdb = msgdb;
                if (null == obj_markdb)
                    obj_markdb = markdb;
//                 obj_logdb.SynchronousAllRecord();
                obj_markdb.SynchronousAllRecord();

                CLog_WuQi.b_init = true;
            }
        }

        static public CLog_WuQi GetLog()
        {
            if (null == CLog_WuQi.c_log)
            {
                CLog_WuQi.c_log = new CLog_WuQi();
            }
            return CLog_WuQi.c_log;
        }
        /// <summary>
        /// 私有构造函数
        /// </summary>
        /// <param name="msgdb"></param>
        /// <param name="markdb"></param>
        private CLog_WuQi()
        {
            CLog_WuQi.b_init = false;
        }

        /// <summary>
        /// 放置信息
        /// </summary>
        /// <param name="smsg">消息内容，要求唯一</param>
        /// <param name="stype">消息类型，目前约定两类分别用字符串“debug”“run”表示</param>
        /// <param name="sowner">消息所有者</param>
        public void TriggerLogMsg(string smsg, string stype,string sowner)
        {
            if (false == CLog_WuQi.b_init)
            {
                return;
            }
            obj_markdb.TriggerLogMsg(smsg, stype, sowner);
        }
        /// <summary>
        /// 获得某个人的所有信息
        /// </summary>
        /// <param name="msgowner">信息所有者</param>
        /// <returns></returns>
        public List<CLogMsg_WuQi> GetAllMsg(string msgowner)
        {            
            return obj_logdb.ReadAllRecord(1, msgowner);            
        }

        public int DeleteMsgs(List<uint> ids)
        {
            return obj_logdb.DeleteMsg(3, ids);
        }

        public List<CLogMark_WuQi> GetAllMarks(string markowner)
        {

            return obj_markdb.GetAllRecords(1,markowner);
        }
        /// <summary>
        /// 改变MARK标记的激活状态
        /// </summary>
        /// <param name="id">MARK标示id</param>
        public void ChangeMarkState(int id)
        {
            this.obj_markdb.ChangeMarkState(id);
        }


    }
}
