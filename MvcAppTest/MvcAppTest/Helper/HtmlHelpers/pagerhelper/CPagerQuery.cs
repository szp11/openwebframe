using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Xml.Linq;

namespace MvcAppTest.Helper.HtmlHelpers.pagerhelper
{
    public class CPagerQuery<TPager, TEntityList>
    {
        public CPagerQuery(TPager pager, TEntityList entityList)
        {
            this.Pager = pager;
            this.EntityList = entityList;
        }
        public TPager Pager { get; set; }
        public TEntityList EntityList { get; set; } 
    }
}
