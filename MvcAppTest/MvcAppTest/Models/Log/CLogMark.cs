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
using System.Collections;
using System.Collections.Generic;
using MvcAppTest.Helper.Cache;

namespace MvcAppTest.Models.Log
{
    public class CLogMark_WuQi : IObjectAdapter_WuQi
    {
        //与SetMyGuid配合生成uint型唯一标识符
        //之所以使用uint数据类型的唯一标识符是为了优化数据库查询速度
        private static bool b_initguid = false;
        private static uint ui_guid = 0;
        public uint i_Guid = 0;
        public string s_Owner;
        public string s_Desc;
        public int i_IsOpen = 0;
        public DateTime d_CreateTime;

        private CLogMark_WuQi(string sdesc, string sowner, int iopen, DateTime dtime)
        {
            this.i_Guid = CLogMark_WuQi.ui_guid++;
            this.s_Owner = sowner;
            this.s_Desc = sdesc;
            this.i_IsOpen = 0;
            this.d_CreateTime = DateTime.Now;
        }
        public CLogMark_WuQi(uint id, string sdesc, string sowner, int iopen, DateTime dtime)
        {
            this.i_Guid = id;
            this.s_Owner = sowner;
            this.s_Desc = sdesc;
            this.i_IsOpen = iopen;
            this.d_CreateTime = DateTime.Now;
        }
        public CLogMark_WuQi GetNewMark(string sdesc, string sowner, int iopen, DateTime dtime)
        {
            if (false == CLogMark_WuQi.b_initguid)
                return null;
            return new CLogMark_WuQi(sdesc, sowner, iopen, dtime);
        }
        public object GetMyGuid()
        {
            return this.i_Guid;
        }
        static public void SetMyGuid(object objvalue)
        {
            if (false != CLogMark_WuQi.b_initguid)
                return;
            CLogMark_WuQi.ui_guid = (uint)objvalue;
            CLogMark_WuQi.b_initguid = true;
        }
        public bool IsMe(int adapter, ArrayList al)
        {
            bool bresult = false;
            switch (adapter)
            {
                case 0:
                    bresult = true;
                    break;
                default:
                    break;
            }
            return bresult;
        }
    }
}
