using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using MvcAppTest.Helper.HtmlHelpers.pagerhelper;

using MvcAppTest.Helper.corelevel.Exception;
using MvcAppTest.Models.Exception;
namespace MvcAppTest.Controllers
{
    public class ExceptionController : Controller
    {
        //
        // GET: /Exception/

        public ActionResult Index()
        {
            IList<CExceptionInfo_WuQi> elist = CExceptionContainer_WuQi.GetInfos();
            if(null != elist && elist.Count > 0)
            {
                TempData["infos"] = elist;
                return RedirectToAction("Exceptioninfos");
            }
            
            return View();
        }

        public ActionResult Exceptioninfos(int ? page)
        {
            IList<CExceptionInfo_WuQi> result = TempData["infos"] as IList<CExceptionInfo_WuQi>;
            CPagerInfo pager = new CPagerInfo();
            pager.RecordCount = result.Count;
            pager.PageSize = 10;
            pager.CurrentPageIndex = (page != null ? (int)page : 1);
            IEnumerable<CExceptionInfo_WuQi> info2 = result.Where<CExceptionInfo_WuQi>(id => id.i_id > (pager.CurrentPageIndex - 1) * pager.PageSize && id.i_id <= pager.CurrentPageIndex * pager.PageSize);
            CPagerQuery<CPagerInfo, IEnumerable<CExceptionInfo_WuQi>> query = new CPagerQuery<CPagerInfo, IEnumerable<CExceptionInfo_WuQi>>(pager, info2);
            TempData["infos"] = result;
            return View(query);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ManagerException(FormCollection collection)
        {
            string sdelete = Request.Form["delete"];
            if (null != sdelete)
            {
                CExceptionContainer_WuQi.ClearInfos();
                IList<CExceptionInfo_WuQi> elist = CExceptionContainer_WuQi.GetInfos();
                if (null != elist)
                {
                    TempData["infos"] = elist;
                    return RedirectToAction("Exceptioninfos");
                }
            }
            IList<CExceptionInfo_WuQi> result = TempData["infos"] as IList<CExceptionInfo_WuQi>;
            TempData["infos"] = result;

            string[] stringsplit = collection.GetValue("Guid").AttemptedValue.Split(',');
            List<int> ls = new List<int>();
            foreach (var item in stringsplit)
            {
                if (0 == item.CompareTo("false") || 0 == item.CompareTo("true"))
                {

                }
                else
                {
                    ls.Add(int.Parse(item));
                }
            }
            if (ls.Count == 0)
            {
                return RedirectToAction("Exceptioninfos");
            }



            return RedirectToAction("Exceptioninfos");
        }
    }
}
