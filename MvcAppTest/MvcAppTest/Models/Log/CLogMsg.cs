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

    /// <summary>
    /// 抽象类；外部创建的需存储的信息
    /// </summary>
    public abstract class CLogMsg_WuQi : IObjectAdapter_WuQi<uint>
    {
        //与SetMyGuid配合生成uint型唯一标识符
        //之所以使用uint数据类型的唯一标识符是为了优化数据库查询速度
        protected static bool b_initguid = false;
        protected static uint ui_guid = 0;
        private static object obj_lock = new object();

        public uint ui_id;//唯一标识符
        public string str_mark;//发出该信息的标记
        public string str_logmsg; //信息内容
        public string str_logtype;//信息类型
        public string str_logowner;//信息所有者
        public DateTime d_logtime;//信息创建的时间

        public CLogMsgInfo msginfo = null;
        public CLogMsg_WuQi(string smsg, string sowner, string stype, string smark)
        {
            lock(CLogMsg_WuQi.obj_lock)
            {
                CLogMsg_WuQi.ui_guid++;
                this.ui_id = CLogMsg_WuQi.ui_guid;
            }
            
            this.str_mark = smark;
            this.str_logmsg = smsg;
            this.str_logtype = stype;
            this.str_logowner = sowner;
            this.d_logtime = DateTime.Now;
            if (null == msginfo)
                msginfo = new CLogMsgInfo();
            msginfo.ui_id = this.ui_id;
            msginfo.str_mark = this.str_mark;
            msginfo.str_logmsg = this.str_logmsg;
            msginfo.str_logtype = this.str_logtype;
            msginfo.str_logowner = this.str_logowner;
            msginfo.d_logtime = this.d_logtime;
        }
        public CLogMsg_WuQi(uint sid, string smark, string stype, string smsg, string sowner, DateTime logtime)
        {
            this.ui_id = sid;
            this.str_mark = smark;
            this.str_logmsg = smsg;
            this.str_logtype = stype;
            this.str_logowner = sowner;
            this.d_logtime = logtime;

            if (null == msginfo)
                msginfo = new CLogMsgInfo();
            msginfo.ui_id = this.ui_id;
            msginfo.str_mark = this.str_mark;
            msginfo.str_logmsg = this.str_logmsg;
            msginfo.str_logtype = this.str_logtype;
            msginfo.str_logowner = this.str_logowner;
            msginfo.d_logtime = this.d_logtime;
        }
        public string GetOwner()
        {
            return this.str_logowner;
        }
        public string GetMsg()
        {
            return this.str_logmsg;
        }
        public string GetLogType()
        {
            return this.str_logtype;
        }

        public DateTime GetCreateTime()
        {
            return this.d_logtime;
        }
        public uint GetMyGuid()
        {
            return this.ui_id;
        }
        static public void SetMyGuid(uint objvalue)
        {
            lock(CLogMsg_WuQi.obj_lock)
            {
                if (false != CLogMsg_WuQi.b_initguid)
                    return;
                CLogMsg_WuQi.ui_guid = objvalue;
                CLogMsg_WuQi.b_initguid = true;

            }
        }
        public virtual bool IsMe(int adapter, Hashtable al)
        {
            bool bresult = false;
            switch (adapter)
            {
                case 0:
                    bresult = true;
                    break;
                case 1://信息所有者
                    if (0 != al.Count)
                    {                        
                        foreach (string owner in al.Values)
                        {
                            if (0 == this.str_logowner.CompareTo(owner))
                            {
                                bresult = true;
                                break;
                            }
                        }
                    }
                    break;
                case 2:
                    if(0 != al.Count)
                    {
                        foreach ( uint item in al.Values)
                        {
                            if(this.ui_id == item)
                            {
                                bresult = true; break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            return bresult;
        }
        /// <summary>
        /// 将log信息格式化输出
        /// </summary>
        /// <returns></returns>
        public virtual string GetString()
        {
            return String.Format("[{0}]{1}:{2}", this.str_logtype, this.d_logtime.ToString(), this.str_logmsg);
        }

        public CLogMsgInfo GetMsgModel()
        {
            return this.msginfo;
        }
    }
    /// <summary>
    /// DEBUG 信息类
    /// </summary>
    public class CLogDebugMsg_WuQi : CLogMsg_WuQi
    {
        public const string LOGTYPE_DEBUG = "debug";
        private CLogDebugMsg_WuQi(string smsg, string sowner, string smark)
            : base(smsg, sowner, CLogDebugMsg_WuQi.LOGTYPE_DEBUG, smark)
        {

        }
        public static CLogMsg_WuQi GetNewMsg(string smsg, string sowner, string smark)
        {
            if (false == CLogMsg_WuQi.b_initguid)
                return null;
            return new CLogDebugMsg_WuQi(smsg, sowner, smark);
        }
        public CLogDebugMsg_WuQi(uint id, string smark, string smsg, string sowner, DateTime logtime)
            : base(id, smark, CLogDebugMsg_WuQi.LOGTYPE_DEBUG, smsg, sowner, logtime)
        {

        }

    }
    /// <summary>
    /// RUN 信息类
    /// </summary>
    public class CLogRunMsg_WuQi : CLogMsg_WuQi
    {
        public const string LOGTYPE_RUN = "run";
        private CLogRunMsg_WuQi(string smsg, string sowner, string smark)
            : base(smsg, sowner, CLogRunMsg_WuQi.LOGTYPE_RUN, smark)
        {

        }
        public static CLogMsg_WuQi GetNewMsg(string smsg, string sowner, string smark)
        {
            if (false == CLogMsg_WuQi.b_initguid)
                return null;
            return new CLogRunMsg_WuQi(smsg, sowner, smark);
        }
        public CLogRunMsg_WuQi(uint id, string smark, string smsg, string sowner, DateTime logtime)
            : base(id, smark, CLogRunMsg_WuQi.LOGTYPE_RUN, smsg, sowner, logtime)
        {

        }

    }
    /// <summary>
    /// 信息类的静态类厂,分离类的创建于业务逻辑
    /// </summary>
    public static class CLogMsgFactory
    {
        static public CLogMsg_WuQi CreateMsg(string stype, string smsg, string sowner, string smark)
        {
            switch (stype)
            {
                case CLogRunMsg_WuQi.LOGTYPE_RUN:
                    return CLogRunMsg_WuQi.GetNewMsg(smsg, sowner, smark);
                case CLogDebugMsg_WuQi.LOGTYPE_DEBUG:
                    return CLogDebugMsg_WuQi.GetNewMsg(smsg, sowner, smark);
                default:
                    return null;
            }
        }

        static public CLogMsg_WuQi CreateMsg(uint id, string smark, string stype, string smsg, string sowner, DateTime logtime)
        {
            switch (stype)
            {
                case CLogRunMsg_WuQi.LOGTYPE_RUN:
                    return (CLogMsg_WuQi)new CLogRunMsg_WuQi(id, smark, smsg, sowner, logtime);
                case CLogDebugMsg_WuQi.LOGTYPE_DEBUG:
                    return (CLogMsg_WuQi)new CLogDebugMsg_WuQi(id, smark, smsg, sowner, logtime);
                default:
                    return null;
            }
        }
    }
}
