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

namespace MvcAppTest.Models.Log
{
    public class CLogMsgInfo
    {
        public uint ui_id { get; set; }//唯一标识符
        public string str_mark { get; set; }//发出该信息的标记
        public string str_logmsg { get; set; } //信息内容
        public string str_logtype { get; set; }//信息类型
        public string str_logowner { get; set; }//信息所有者
        public DateTime d_logtime { get; set; }//信息创建的时间
    }
}
