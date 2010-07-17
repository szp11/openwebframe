using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml;
using MvcAppTest.Models.Exception;

namespace MvcAppTest.Helper.corelevel.Exception
{
    public class CExceptionDecoraterBase_WuQi
    {
        private System.Exception se = null;
        public System.Exception SE
        {
            get { return this.se; }
            set { this.se = value; }
        }
        private CExceptionInfo_WuQi obj_info = null;
        public CExceptionInfo_WuQi ExceptionInfo{
            get { return this.obj_info; }
        }
        public CExceptionDecoraterBase_WuQi(System.Exception e,int id, DateTime t,string fullname,string basename,string source,string trace,string msg)
        {
            this.se = e;
            if(null == obj_info)
            {
                this.obj_info = new CExceptionInfo_WuQi(id,t, fullname, basename, source, trace, msg);
            }
            else
            {
                this.obj_info.i_id = id;
                this.obj_info.d_time = t; this.obj_info.s_fullname = fullname;
                this.obj_info.s_basefullname = basename; this.obj_info.s_source = source;
                this.obj_info.s_trace = trace; this.obj_info.s_msg = msg;

            }
        }

        public static CExceptionInfo_WuQi FromXML(XmlElement xe)
        {
            return null;
        }

    }
}
