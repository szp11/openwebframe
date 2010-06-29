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

namespace MvcAppTest.Helper.HtmlHelpers.pagerhelper
{
    public class CPagerInfo
    {
        public int RecordCount { get; set; }

        public int CurrentPageIndex { get; set; }

        public int PageSize { get; set; }

    }
}
