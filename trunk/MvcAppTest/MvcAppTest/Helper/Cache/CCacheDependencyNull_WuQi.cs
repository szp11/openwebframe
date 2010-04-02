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
    public class CCacheDependencyNull_WuQi:ICacheDependency_WuQi
    {
        public bool Insert(object k, CCacheItem_WuQi item, ref ICacheStorage_WuQi container)
       {
           container.Add(k, item);
           return true;
       }
        public bool Delete(object k, ref ICacheStorage_WuQi container)
       {
           container.Remove(k);
           return true;
       }
       public void Clear(ref ICacheStorage_WuQi container)
       {
           container.Clear();
       }

    }
}
