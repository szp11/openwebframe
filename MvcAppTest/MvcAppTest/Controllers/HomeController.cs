using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcAppTest.Helper;
using MvcAppTest.Helper.Cache;
using MvcAppTest.Helper.Log;

namespace MvcAppTest.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult  Index()
        {
//             string providername = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ProviderName;
//             string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString();
//             CLog_WuQi.InitLog(new CLogMsgContainer_WuQi(connstring, new CHashCache_WuQi(), new CCacheDependencyLength_WuQi(100)));
//             CLog_WuQi slw = CLog_WuQi.GetLog();
//             if(null != slw)
//             {
//                 slw.DeleteMsgOwner("1");
//             }
//             string t = DateTime.Now.ToString().Trim();
//             ViewData["Message"] = t + "  " +DateTime.Parse(t).ToString() ;
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
