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

namespace MvcAppTest.Models.Exception
{
    /// <summary>
    /// 异常信息MODEL
    /// </summary>
    public class CExceptionInfo_WuQi
    {
        public int i_id { get; set; }//索引
        public DateTime d_time { get; set; }//触发时间
        public string s_fullname { get; set; }//全称
        public string s_basefullname { get; set; }//systemexception或applicationexception
        public string s_source { get; set; }//来源
        public string s_trace { get; set; }//足迹
        public string s_msg { get; set; }//信息

        public CExceptionInfo_WuQi(int id,DateTime t, string fullname, string basename, string source, string trace, string msg)
        {
            this.i_id = id;
            this.d_time = t; this.s_fullname = fullname;
            this.s_basefullname = basename; this.s_source = source;
            this.s_trace = trace; this.s_msg = msg;
        }

    }
}
