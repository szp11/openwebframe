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
using MvcAppTest.Helper.Cache;

namespace MvcAppTest.Helper.Log
{
    /// <summary>
    /// 库中存储的log信息;外部信息CLogMsg_WuQi的代理；
    /// 必须实现IObjectAdapter_WuQi接口,来完成select 操作；
    /// 另外包含了存储到容器中所必须的 key 值
    /// </summary>
    internal class SLogRecord_WuQi : IObjectAdapter_WuQi
    {
        public string str_guid = null;
        public CLogMsg_WuQi obj_Msg = null;
        public SLogRecord_WuQi(CLogMsg_WuQi msg)
        {
            this.str_guid = System.Guid.NewGuid().ToString();
            this.obj_Msg = msg;
        }
        public SLogRecord_WuQi(string logid, string logtype, string logmsg, string logowner, DateTime logtime)
        {
            this.str_guid = logid;
            this.obj_Msg = new CLogMsgFactory().CreateMsg(logtype, logmsg, logowner, logtime);

        }
        public CLogMsg_WuQi ChangeToLogMsg()
        {
            return this.obj_Msg;
        }
        /// <summary>
        /// 确定自我。继承自IObjectAdapter_WuQi。用来在对象容器中查找到自己
        /// </summary>
        /// <param name="adapter">判断条件的序号，自己来确定</param>
        /// <param name="al">判断时需要的参数</param>
        /// <returns></returns>
        public bool IsMe(int adapter, System.Collections.ArrayList al)
        {
            bool result = false;
            switch (adapter)
            {
                case 0://总是被选中
                    result = true;
                    break;
                case 1://信息所有者
                    if (0 != al.Count)
                    {
                        foreach (string owner in al)
                        {
                            if (this.obj_Msg.str_logowner == owner)
                            {
                                result = true;
                                break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            return result;
        }


    }

}
