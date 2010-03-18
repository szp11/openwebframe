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
    /// 缓存长度策略主要是控制缓存的长度在允许范围内；以满足内存占用的限制
    /// </summary>
    public class CCacheDependencyLength_WuQi : ICacheDependency_WuQi
    {
        public void UpdateCache(ref ICacheStorage_WuQi container)
        { }

        private int i_objectcountmax = 1000;//容器内缓存对象的上限
        private bool b_containeroverflow = false;//容器是否已经溢出
        private double d_objectcuttime = 0.5;//容器上溢时，将减少容器内对象数量的倍率
        public CCacheDependencyLength_WuQi(int num,double cuttime)
        {
            this.i_objectcountmax = num;
            this.d_objectcuttime = cuttime;
        }

    }
}
