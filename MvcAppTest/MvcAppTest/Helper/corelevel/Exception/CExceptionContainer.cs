using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;
using System.Text;
using System.IO;
using MvcAppTest.Models.Exception;

namespace MvcAppTest.Helper.corelevel.Exception
{
    /// <summary>
    /// 异常信息容器
    /// </summary>
    public class CExceptionContainer_WuQi
    {
        private static int msg_id = 1;//异常信息索引
        private static bool b_initguid = false;
        private static object obj_lock = new object();

        private static List<CExceptionInfo_WuQi> l_exception = null;
        private static string s_xmlfilepath;//存储异常的XML文件

        private static Dictionary<int ,string> dictionary_msg=null;//用户提示信息字典
        public static IList<CExceptionInfo_WuQi> GetInfos()
        {
            lock (CExceptionContainer_WuQi.obj_lock)
            {
                if (null == CExceptionContainer_WuQi.l_exception)
                    return null;
                //得到当前异常信息的快照
                List<CExceptionInfo_WuQi> tempE = new List<CExceptionInfo_WuQi>();
                foreach (CExceptionInfo_WuQi item in CExceptionContainer_WuQi.l_exception)
                {
                    tempE.Add(new CExceptionInfo_WuQi(item.i_id, item.d_time, item.s_fullname, item.s_basefullname, item.s_source, item.s_trace, item.s_msg));
                }

                return tempE;

            }

        }
        public static int ClearInfos()
        {
            lock(CExceptionContainer_WuQi.obj_lock)
            {
                int result = CExceptionContainer_WuQi.l_exception.Count;
                CExceptionContainer_WuQi.l_exception.Clear();
                return result;

            }
        }
        public static bool Init(string spath)
        {
            lock(CExceptionContainer_WuQi.obj_lock)
            {
                if (false != CExceptionContainer_WuQi.b_initguid)
                    return true;
                CExceptionContainer_WuQi.s_xmlfilepath = spath+"\\exceptionstore.xml";
                //删除以前的异常信息记录，也可考虑更名保存
                if (File.Exists(CExceptionContainer_WuQi.s_xmlfilepath))
                {
                    File.Delete(CExceptionContainer_WuQi.s_xmlfilepath);
                }
                //新建异常信息存储文件
                XmlDocument xmldoc = new XmlDocument();
                XmlDeclaration declaration = xmldoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmldoc.AppendChild(declaration);
                XmlNode rootnode = xmldoc.CreateNode(XmlNodeType.Element, "exceptions", null);
                xmldoc.AppendChild(rootnode);

                xmldoc.Save(CExceptionContainer_WuQi.s_xmlfilepath);

                CExceptionContainer_WuQi.l_exception = new List<CExceptionInfo_WuQi>();
                //存储发生异常时向用户提供的信息，约定从 1 开始。
                CExceptionContainer_WuQi.dictionary_msg = new Dictionary<int, string>();
                CExceptionContainer_WuQi.dictionary_msg.Add(0, "异常管理系统发生故障！请与系统管理人员联系。");
                CExceptionContainer_WuQi.dictionary_msg.Add(1, "对不起，系统发生故障！请与系统管理人员联系。");

                CExceptionContainer_WuQi.b_initguid = true;
                return CExceptionContainer_WuQi.b_initguid;
            }
        }
        /// <summary>
        /// 处理异常方法
        /// </summary>
        /// <param name="e">接收到得异常，此函数一般接收系统异常，对于应用程序异常可以提前设置catch捕获</param>
        /// <returns>用户提示信息，友好一点啊</returns>
        public static string ProcessException(System.Exception e)
        {
            lock(CExceptionContainer_WuQi.obj_lock)
            {
                if (null == e || false == CExceptionContainer_WuQi.b_initguid)
                    return CExceptionContainer_WuQi.dictionary_msg[1];

                Type t = e.GetType();
                string sbase = null;
                System.Exception issystem = null;
                Type tt = t;
                //判断是否为系统异常
                while (tt != null)
                {
                    if (tt.Equals((new System.SystemException()).GetType()))
                    {
                        issystem = e;
                    }
                    tt = tt.BaseType;
                }
                if (null == issystem)
                {
                    sbase = "applicationexception";
                }
                else
                {
                    sbase = "systemexception";
                }
                string sname = t.FullName;
                string source = e.Source;
                string trace = e.StackTrace;
                string smsg = e.Message;
                DateTime dtime = DateTime.Now;

                if(0 == sbase.CompareTo("systemexception"))
                {
                    CExceptionDecoraterBase_WuQi decorater = new CExceptionDecoraterBase_WuQi(e, CExceptionContainer_WuQi.msg_id++, dtime, sname, sbase, source, trace, smsg);
                    CExceptionContainer_WuQi.l_exception.Add(decorater.ExceptionInfo);
                    int result = CExceptionContainer_WuQi.InsertSystemToXml(decorater);
                    return CExceptionContainer_WuQi.dictionary_msg[result];
   
                }                
                if(0 == sbase.CompareTo("applicationexception"))
                {
                    //应用程序异常暂时不考虑
                }

                return CExceptionContainer_WuQi.dictionary_msg[1];

            }

        }
        /// <summary>
        /// 将系统异常信息记录到xml文件中
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        private static int InsertSystemToXml(CExceptionDecoraterBase_WuQi de)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement xe = null;
            try
            {
                doc.Load(CExceptionContainer_WuQi.s_xmlfilepath);
                XmlNode root = doc.SelectSingleNode("exceptions");
                if (null == root)
                    return 0;
                xe = doc.CreateElement("exception");
                xe.SetAttribute("type", "systemexception");

                XmlElement xetime = doc.CreateElement("time");
                xetime.InnerText = de.ExceptionInfo.d_time.ToString().Trim();
                xe.AppendChild(xetime);

                XmlElement xefullname = doc.CreateElement("fullname");
                xefullname.InnerText = de.ExceptionInfo.s_fullname;
                xe.AppendChild(xefullname);

                XmlElement xebasename = doc.CreateElement("basename");
                xebasename.InnerText = de.ExceptionInfo.s_basefullname;
                xe.AppendChild(xebasename);

                XmlElement xesource = doc.CreateElement("source");
                xesource.InnerText = de.ExceptionInfo.s_source;
                xe.AppendChild(xesource);

                XmlElement xetrace = doc.CreateElement("trace");
                xetrace.InnerText = de.ExceptionInfo.s_trace;
                xe.AppendChild(xetrace);

                XmlElement xemsg = doc.CreateElement("msg");
                xemsg.InnerText = de.ExceptionInfo.s_msg;
                xe.AppendChild(xemsg);

                root.AppendChild(xe);
                doc.Save(CExceptionContainer_WuQi.s_xmlfilepath);

            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                
            }
            return 1;
        }
    }
}
