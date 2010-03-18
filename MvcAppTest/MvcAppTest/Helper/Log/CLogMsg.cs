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

namespace MvcAppTest.Helper.Log
{
    /// <summary>
    /// 抽象类；外部创建的需存储的信息
    /// </summary>
    public abstract class CLogMsg_WuQi
    {
        public string str_logmsg; //信息内容
        public string str_logtype;//信息类型
        public string str_logowner;//信息所有者
        public DateTime d_logtime;//信息创建的时间
        public CLogMsg_WuQi(string smsg, string sowner, string stype)
        {
            this.str_logmsg = smsg;
            this.str_logtype = stype;
            this.str_logowner = sowner;
            this.d_logtime = DateTime.Now;
        }
        public CLogMsg_WuQi(string smsg, string sowner, string stype, DateTime logtime)
        {
            this.str_logmsg = smsg;
            this.str_logtype = stype;
            this.str_logowner = sowner;
            this.d_logtime = logtime;
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
        /// <summary>
        /// 将log信息格式化输出
        /// </summary>
        /// <returns></returns>
        public virtual string GetString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("[");
            sb.Append(this.str_logtype);
            sb.Append("]");
            sb.Append(this.d_logtime.ToString());
            sb.Append(":");
            sb.Append(this.str_logmsg);
            sb.Append("\t");
            return sb.ToString();
        }

    }
    /// <summary>
    /// DEBUG 信息类
    /// </summary>
    public class CLogDebugMsg_WuQi : CLogMsg_WuQi
    {
        public const string LOGTYPE_DEBUG = "debug";
        public CLogDebugMsg_WuQi(string smsg, string sowner)
            : base(smsg, sowner, CLogDebugMsg_WuQi.LOGTYPE_DEBUG)
        {

        }
        public CLogDebugMsg_WuQi(string smsg, string sowner, DateTime logtime)
            : base(smsg, sowner, CLogDebugMsg_WuQi.LOGTYPE_DEBUG, logtime)
        {

        }

    }
    /// <summary>
    /// RUN 信息类
    /// </summary>
    public class CLogRunMsg_WuQi : CLogMsg_WuQi
    {
        public const string LOGTYPE_RUN = "run";
        public CLogRunMsg_WuQi(string smsg, string sowner)
            : base(smsg, sowner, CLogRunMsg_WuQi.LOGTYPE_RUN)
        {

        }

        public CLogRunMsg_WuQi(string smsg, string sowner, DateTime logtime)
            : base(smsg, sowner, CLogRunMsg_WuQi.LOGTYPE_RUN, logtime)
        {

        }

    }
    /// <summary>
    /// 信息类的类厂,分离类的创建于业务逻辑
    /// </summary>
    public class CLogMsgFactory
    {
        public CLogMsg_WuQi CreateMsg(string logtype, string logmsg, string logowner, DateTime logtime)
        {
            switch (logtype)
            {
                case CLogRunMsg_WuQi.LOGTYPE_RUN:
                    return (CLogMsg_WuQi)new CLogRunMsg_WuQi(logmsg, logowner, logtime);
                case CLogDebugMsg_WuQi.LOGTYPE_DEBUG:
                    return (CLogMsg_WuQi)new CLogDebugMsg_WuQi(logmsg, logowner, logtime);

                default:
                    return null;
            }
        }
    }
}
