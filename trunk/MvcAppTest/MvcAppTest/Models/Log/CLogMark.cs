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
using MvcAppTest.Helper.corelevel.Cache;

namespace MvcAppTest.Models.Log
{
    public class CLogMark_WuQi : IObjectAdapter_WuQi<uint>
    {
        //与SetMyGuid配合生成uint型唯一标识符
        //之所以使用uint数据类型的唯一标识符是为了优化数据库查询速度
        private static bool b_initguid = false;
        private static uint ui_guid = 0;
        private static object obj_lock = new object();

        public uint i_Guid;
        public string s_Owner;
        public string s_Desc;
        private int i_IsOpen;//控制该标记的状态，决定是否激发信息，0-不激发
        private DateTime d_CreateTime;

        private CLogMarkInfo info = null;//存储mark数据的结构，作为MVC中的MODEL
        public int IsOpen
        {
        get{return i_IsOpen;}
        set{
            i_IsOpen = value;
            if (null != info)
                info.i_IsOpen = i_IsOpen;
            }
        }
        public DateTime CreateTime
        {
            get{return d_CreateTime;}
            set{
                d_CreateTime =value;
                if(null != info)
                    info.d_CreateTime=this.d_CreateTime;
            }
        }
        
        private CLogMark_WuQi(string sdesc, string sowner, int iopen, DateTime dtime)
        {
            lock(CLogMark_WuQi.obj_lock)
            {
                CLogMark_WuQi.ui_guid++;
                this.i_Guid = CLogMark_WuQi.ui_guid;
            }

            this.s_Owner = sowner;
            this.s_Desc = sdesc;
            this.i_IsOpen = 1;
            this.d_CreateTime = DateTime.Now;

            if(null == info)
            {
                info = new CLogMarkInfo();
                info.i_Guid = this.i_Guid;
                info.i_IsOpen = this.i_IsOpen;
                info.s_Desc = this.s_Desc;
                info.s_Owner = this.s_Owner;
                info.d_CreateTime = this.d_CreateTime;
            }
            else
            {
                info.i_Guid = this.i_Guid;
                info.i_IsOpen = this.i_IsOpen;
                info.s_Desc = this.s_Desc;
                info.s_Owner = this.s_Owner;
                info.d_CreateTime = this.d_CreateTime;

            }
        }
        public CLogMark_WuQi(uint id, string sdesc, string sowner, int iopen, DateTime dtime)
        {
            this.i_Guid = id;
            this.s_Owner = sowner;
            this.s_Desc = sdesc;
            this.i_IsOpen = iopen;
            this.d_CreateTime = DateTime.Now;
            if (null == info)
            {
                info = new CLogMarkInfo();
                info.i_Guid = this.i_Guid;
                info.i_IsOpen = this.i_IsOpen;
                info.s_Desc = this.s_Desc;
                info.s_Owner = this.s_Owner;
                info.d_CreateTime = this.d_CreateTime;
            }
            else
            {
                info.i_Guid = this.i_Guid;
                info.i_IsOpen = this.i_IsOpen;
                info.s_Desc = this.s_Desc;
                info.s_Owner = this.s_Owner;
                info.d_CreateTime = this.d_CreateTime;

            }
        }
        static public CLogMark_WuQi GetNewMark(string sdesc, string sowner, int iopen, DateTime dtime)
        {
            if (false == CLogMark_WuQi.b_initguid)
                return null;
            return new CLogMark_WuQi(sdesc, sowner, iopen, dtime);
        }
        public uint GetMyGuid()
        {
            return this.i_Guid;
        }
        static public void SetMyGuid(uint objvalue)
        {
            lock(CLogMark_WuQi.obj_lock)
            {
                if (false != CLogMark_WuQi.b_initguid)
                    return;
                CLogMark_WuQi.ui_guid = (uint)objvalue;
                CLogMark_WuQi.b_initguid = true;

            }
        }
        public bool IsMe(int adapter, Hashtable al)
        {
            bool bresult = false;
            switch (adapter)
            {
                case 0:
                    bresult = true;
                    break;
                case 1:
                    if (0 != al.Count)
                    {
                        foreach (string owner in al.Values)
                        {
                            if (0 == this.s_Owner.CompareTo(owner))
                            {
                                bresult = true;
                                break;
                            }
                        }
                    } break;

                case 2:
                    string smsg = (string)al["msg"];
                    string sowner = (string)al["owner"];
                    if((0==this.s_Desc.CompareTo(smsg))&&(0 == this.s_Owner.CompareTo(sowner)))
                    {
                        bresult =true;
                    }
                    break;
                default:
                    break;
            }
            return bresult;
        }

        public CLogMarkInfo GetMarkModle()
        {
            return info;
        }


    }
}
