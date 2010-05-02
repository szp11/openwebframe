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
using System.Collections.Generic;
namespace MvcAppTest.Helper.Cache
{
    public class CCacheDependencyNull_WuQi:ICacheDependency_WuQi
    {
        public int SynchronousAllObject(List<CCacheItem_WuQi> litem, ref ICacheStorage_WuQi container)
        {
            container.Clear();
            foreach (CCacheItem_WuQi item in litem)
            {
                container.Add(item.key, item);
            }
            return container.Count;
        }
        public bool Insert(object k, CCacheItem_WuQi item, ref ICacheStorage_WuQi container)
       {
           container.Add(k, item);
           return true;
       }
        public int Insert( List<CCacheItem_WuQi> listitem,ref ICacheStorage_WuQi container)
        {
            foreach(CCacheItem_WuQi item in listitem)
            {
                container.Add(item.key, item);
            }
            return listitem.Count;
        }
        public bool Delete(object k, ref ICacheStorage_WuQi container)
       {
           container.Remove(k);
           return true;
       }
        public int Delete(List<CCacheItem_WuQi> listitem, ref ICacheStorage_WuQi container)
        {
            foreach (CCacheItem_WuQi item in listitem)
            {
                container.Remove(item.key);
            }
            return listitem.Count;
        }

       public void Clear(ref ICacheStorage_WuQi container)
       {
           container.Clear();
       }

    }
}
