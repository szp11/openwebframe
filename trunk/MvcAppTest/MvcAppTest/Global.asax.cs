using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using MvcAppTest.Helper.corelevel.Cache;
using MvcAppTest.Models.Log;
using MvcAppTest.Helper.corelevel.Exception;

namespace MvcAppTest
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
            //启动顺序
            CExceptionContainer_WuQi.Init(Server.MapPath("."));
            string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString();
            CLogMarkContainer_WuQi markcontainers = new CLogMarkContainer_WuQi(connstring, new CHashCache_WuQi<uint, CLogMark_WuQi>(), new CCacheDependencyFull_WuQi<uint, CLogMark_WuQi>());
            CLogMsgContainer_WuQi msgcontainers = new CLogMsgContainer_WuQi(connstring, new CHashCache_WuQi<uint, CLogMsg_WuQi>(), new CCacheDependencyTime_WuQi<uint, CLogMsg_WuQi>());
            markcontainers.logmsgcontainer = msgcontainers;
            msgcontainers.RegisterTimeDependency(1);
            CLog_WuQi log = CLog_WuQi.GetLog();
            log.InitLog(msgcontainers, markcontainers);
            CLog_WuQi.logstate_now = LOGSTATE.LogDebug;

        }


    }
}