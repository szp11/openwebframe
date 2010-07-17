using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MvcAppTest.Models.Log;
using MvcAppTest.Helper.HtmlHelpers.pagerhelper;
using MvcAppTest.Helper.corelevel.Exception;

namespace MvcAppTest.Controllers
{
    public class LogController : Controller
    {
        //
        // GET: /Log/

        public ActionResult  Index()
        {
            try
            {
               //throw new System.NullReferenceException();
            }
            catch (System.Exception e)
            {
                ViewData["msg"] =CExceptionContainer_WuQi.ProcessException(e);
                return View("Error");
            }
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SearchMarkInfo()
        {            
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchMarkInfo(string  owner)
        {
            if (null == owner)
                return View();
            CLog_WuQi log = CLog_WuQi.GetLog();
            if (null == log)
                return View();
            //≤‚ ‘log
            log.TriggerLogMsg("test-SearchMarkinfo once", "debug", "me");

            IList<CLogMarkInfo> result = new List<CLogMarkInfo>();
            List<CLogMark_WuQi> marks = log.GetAllMarks(owner);
            foreach (CLogMark_WuQi mark in marks)
            {
                result.Add(mark.GetMarkModle());
            }
            if (0 == result.Count)
                return View("Error");
            TempData["infos"] = result;
            return RedirectToAction("ListInfos");            
        }
        [AcceptVerbs(HttpVerbs.Get)]    
        public ActionResult ListInfos(int ? page)
        {
            IList<CLogMarkInfo> result = TempData["infos"] as IList<CLogMarkInfo>;
            CPagerInfo pager = new CPagerInfo();
            pager.RecordCount = result.Count;
            pager.PageSize = 10;
            pager.CurrentPageIndex = (page != null ? (int)page : 1);
            IEnumerable<CLogMarkInfo> info2 = result.Where<CLogMarkInfo>(id => id.i_Guid > (pager.CurrentPageIndex - 1) * pager.PageSize && id.i_Guid <= pager.CurrentPageIndex * pager.PageSize);
            CPagerQuery<CPagerInfo, IEnumerable<CLogMarkInfo>> query = new CPagerQuery<CPagerInfo, IEnumerable<CLogMarkInfo>>(pager, info2);
            TempData["infos"] = result;
            return View(query);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ManagerMark(FormCollection collection)
        {
            try
            {
                string sswitch = Request.Form["switch"];

                IList<CLogMarkInfo> result = TempData["infos"] as IList<CLogMarkInfo>;
                TempData["infos"] = result;

                string[] stringsplit = collection.GetValue("Guid").AttemptedValue.Split(',');
                List<uint> ls = new List<uint>();
                foreach (var item in stringsplit)
                {
                    if (0 == item.CompareTo("false") || 0 == item.CompareTo("true"))
                    {

                    }
                    else
                    {
                        ls.Add(uint.Parse(item));
                    }
                }
                if (ls.Count == 0)
                {
                    return RedirectToAction("ListInfos");
                }

                if (null != sswitch)
                {
                    CLog_WuQi log = CLog_WuQi.GetLog();
                    foreach (var item in ls)
                    {
                        log.ChangeMarkState((int)item);
                    }
                }

                return RedirectToAction("ListInfos");

            }
            catch (System.Exception e)
            {
                ViewData["msg"] = CExceptionContainer_WuQi.ProcessException(e);
                return View("Error");

            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchMsgInfo(string owner)
        {
            if (null == owner)
                return View();
            CLog_WuQi log = CLog_WuQi.GetLog();
            if (null == log)
                return View();

            IList<CLogMsgInfo> result = new List<CLogMsgInfo>();
            List<CLogMsg_WuQi> msgs = log.GetAllMsg(owner);
            foreach (CLogMsg_WuQi msg in msgs)
            {
                result.Add(msg.GetMsgModel());
            }
            if (0 == result.Count)
                return View("Error");
            TempData["msginfos"] = result;
            return RedirectToAction("ListMsgInfos");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ListMsgInfos(int? page)
        {
            IList<CLogMsgInfo> result = TempData["msginfos"] as IList<CLogMsgInfo>;
            CPagerInfo pager = new CPagerInfo();
            pager.RecordCount = result.Count;
            pager.PageSize = 10;
            pager.CurrentPageIndex = (page != null ? (int)page : 1);
            IEnumerable<CLogMsgInfo> info2 = result.Where<CLogMsgInfo>(id => id.ui_id > (pager.CurrentPageIndex - 1) * pager.PageSize && id.ui_id <= pager.CurrentPageIndex * pager.PageSize);
            CPagerQuery<CPagerInfo, IEnumerable<CLogMsgInfo>> query = new CPagerQuery<CPagerInfo, IEnumerable<CLogMsgInfo>>(pager, info2);
            TempData["msginfos"] = result;
            return View(query);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ManagerMsg(FormCollection collection)
        {
            try
            {
                string sdelete = Request.Form["delete"];

                IList<CLogMsgInfo> result = TempData["msginfos"] as IList<CLogMsgInfo>;
                List<CLogMsgInfo> deletinfos = new List<CLogMsgInfo>();

                string[] stringsplit = collection.GetValue("Guid").AttemptedValue.Split(',');
                List<uint> ls = new List<uint>();
                foreach (var item in stringsplit)
                {
                    if (0 == item.CompareTo("false") || 0 == item.CompareTo("true"))
                    {

                    }
                    else
                    {
                        ls.Add(uint.Parse(item));
                        foreach (CLogMsgInfo info in result)
                        {
                            if (info.ui_id == uint.Parse(item))
                                deletinfos.Add(info);
                        }
                    }
                }
                if (ls.Count == 0)
                {
                    TempData["msginfos"] = result;
                    return RedirectToAction("ListMsgInfos");
                }


                if (null != sdelete)
                {
                    CLog_WuQi log = CLog_WuQi.GetLog();
                    int count = log.DeleteMsgs(ls);
                    if (count > 0)
                    {
                        foreach (CLogMsgInfo item in deletinfos)
                        {
                            result.Remove(item);
                        }

                    }
                    TempData["msginfos"] = result;
                    return RedirectToAction("ListMsgInfos");
                }
                return RedirectToAction("ListMsgInfos");

            }
            catch (System.Exception e)
            {
                ViewData["msg"] = CExceptionContainer_WuQi.ProcessException(e);
                return View("Error");
            }
        }

    }
}
