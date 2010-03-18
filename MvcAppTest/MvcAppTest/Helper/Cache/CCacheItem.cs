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
    /// <summary>
    /// 缓存的对象单元;目的为配合过期策略
    /// </summary>
    public class CCacheItem_WuQi
    {
        public object key;//键值
        public object t_value;//值
        public int hits;//访问次数
        public DateTime d_lastaccesstime;//最后一次访问时间

        public CCacheItem_WuQi(object k, object t)
        {
            this.key = k; this.t_value = t; this.hits = 0; this.d_lastaccesstime = DateTime.Now;
        }
    }

}
