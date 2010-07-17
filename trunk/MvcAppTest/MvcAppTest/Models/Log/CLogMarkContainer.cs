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
using MvcAppTest.Helper.corelevel.Exception;
namespace MvcAppTest.Models.Log
{
    public class CLogMarkContainer_WuQi : ObjectContainer_WuQi<uint,CLogMark_WuQi>, IHandleMark_WuQi
    {
        public string str_conn = null;//数据库连接字符串
        public IHandleMsg_WuQi logmsgcontainer = null;
        public CLogMarkContainer_WuQi(string conn, ICacheStorage_WuQi<uint, CLogMark_WuQi> ics, ICacheDependency_WuQi<uint, CLogMark_WuQi> icd)
            : base(ics, icd)
        {
            this.str_conn = conn;
            //首先清空数据库及缓存
            Clear();
            //获得数据库中做大的主键值
            SetMaxGuid();

        }
#region handlemark

        public  int SynchronousAllRecord()
        {
            return base.SynchronousAllObject();
        }

        public List<CLogMark_WuQi> GetAllRecords(int adapter,string markowner)
        {
            Hashtable al = new Hashtable();
            al.Add("markowner", markowner);            
            return base.Search(adapter, al);
        }
        public void ChangeMarkState(int id)
        {
            CLogMark_WuQi mark = SelectSingleObject((uint)id);
            if (0 == mark.IsOpen)
                mark.IsOpen = 1;
            else
                mark.IsOpen = 0;
            //更新数据库
            Update((uint)id, mark);
        }
        public int DeleteMarks(List<uint> ids)
        {
            Hashtable al = new Hashtable();
            foreach (var item in ids)
            {
                al.Add(item.ToString(),item);
            }
            List<CLogMark_WuQi> lmarks = Search(1,al);
            //多线程是否会影响删除搜索到得结果集？
            return Delete(1, lmarks);
        }

        public void TriggerLogMsg(string smsg, string stype,string sowner)
        {
            //原来的设计思路涉及到两次查询，大大降低了性能.
            //不得不重新考虑设计。浪费了大量的时间！所以前期系统分析一定要做细！
            //
            if((CLog_WuQi.logstate_now & CLog_WuQi.logstate_all)==LOGSTATE.LogDebug)
            {
                Hashtable al = new Hashtable();
                al.Add("msg",smsg);
                al.Add("owner",sowner);
                List<CLogMark_WuQi> marks = Search(2, al);
                
                CLogMark_WuQi mark =null;
                CLogMsg_WuQi msg =null;
                if (0 == marks.Count)
                {
                    mark = CLogMark_WuQi.GetNewMark(smsg,sowner,1,DateTime.Now);
                    if(null != mark)
                    {
                        msg = CLogMsgFactory.CreateMsg(stype, smsg, sowner, mark.GetMyGuid().ToString());
                        logmsgcontainer.InsertMsg(msg);
                        Insert(mark.GetMyGuid(), mark);
                    }
                }
                else
                {
                    foreach (CLogMark_WuQi markitem in marks)
                    {
                        if(0 != markitem.IsOpen)
                        {
                            msg = CLogMsgFactory.CreateMsg(stype, smsg, sowner, markitem.GetMyGuid().ToString());
                            logmsgcontainer.InsertMsg(msg);

                        }
                    }
                }

            }else if ((CLog_WuQi.logstate_now & CLog_WuQi.logstate_all) == LOGSTATE.LogRun)
            {
                CLogMsg_WuQi msg = CLogMsgFactory.CreateMsg(stype, smsg, sowner, "0");
                logmsgcontainer.InsertMsg(msg);
            }
            
        }

#endregion
        #region operatordb
        private void SetMaxGuid()
        {
            if (null == this.str_conn)
                return;

            System.Data.SqlClient.SqlConnection conn = null;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
//                 lock (this)
                {
                    uint ui_guid = 0;
                    conn = new System.Data.SqlClient.SqlConnection(this.str_conn);
                    conn.Open();
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("SELECT Max(markid) FROM logmarkinfo", conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                            ui_guid = uint.Parse(reader[0].ToString());
                    }
                    CLogMark_WuQi.SetMyGuid(ui_guid);
                }

            }
            catch (System.Exception e)
            {
                CExceptionContainer_WuQi.ProcessException(e);
                throw e;
            }
            finally
            {
                if (null != reader)
                    reader.Close();
                if (null != conn)
                    conn.Close();

            }
            return;
        }

        protected override void ClearDB()
        {
            if (null == this.str_conn)
                return;
            System.Data.SqlClient.SqlConnection conn = null;
//             lock (this)
            {
                try
                {
                    conn = new System.Data.SqlClient.SqlConnection(this.str_conn);
                    conn.Open();
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("DELETE  FROM [logmarkinfo]", conn);
                    cmd.ExecuteNonQuery();
                }
                catch (System.Exception e)
                {
                    CExceptionContainer_WuQi.ProcessException(e);
                    throw e;
                }
                finally
                {
                    if (null != conn)
                        conn.Close();
                }

            }
        }
        protected override bool DeleteDB(uint k, CLogMark_WuQi t)
        {
            if (null == this.str_conn)
                return false;
            System.Data.SqlClient.SqlConnection conn = null;
            try
            {
//                 lock (this)
                {
                    conn = new System.Data.SqlClient.SqlConnection(this.str_conn);
                    conn.Open();
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("delete from logmarkinfo where markid = @markid;", conn);
                    cmd.Parameters.AddWithValue("@markid", t.i_Guid);
                    if (0 == cmd.ExecuteNonQuery())
                        return false;
                }

            }
            catch (System.Exception e)
            {
                CExceptionContainer_WuQi.ProcessException(e);
                throw e;
            }
            finally
            {
                if (null != conn)
                    conn.Close();
            }

            return true;
        }

        protected override bool InsertDB(uint k, CLogMark_WuQi t)
        {
            if (null == this.str_conn)
                return false;
            System.Data.SqlClient.SqlConnection conn = null;
            try
            {
//                 lock (this)
                {
                    conn = new System.Data.SqlClient.SqlConnection(this.str_conn);
                    conn.Open();

                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("insert into logmarkinfo(markid,markdesc,markowner,markstate,marktime) values(@markid,@markdesc,@markowner,@markstate,@marktime);", conn);
                    cmd.Parameters.AddWithValue("@markid", (Int64)t.i_Guid);
                    cmd.Parameters.AddWithValue("@markdesc", t.s_Desc);
                    cmd.Parameters.AddWithValue("@markowner", t.s_Owner);
                    cmd.Parameters.AddWithValue("@markstate", t.IsOpen);
                    cmd.Parameters.AddWithValue("@marktime", t.CreateTime.ToString().Trim());
                    if (0 == cmd.ExecuteNonQuery())
                        return false;
                }

            }
            catch (System.Exception e)
            {
                CExceptionContainer_WuQi.ProcessException(e);
                throw e;
            }
            finally
            {
                if (null != conn)
                    conn.Close();
            }

            return true;
        }
        protected override bool UpdateDB(uint k, CLogMark_WuQi t)
        {
            if (null == this.str_conn)
                return false;
            System.Data.SqlClient.SqlConnection conn = null;
            try
            {
//                 lock (this)
                {
                    conn = new System.Data.SqlClient.SqlConnection(this.str_conn);
                    conn.Open();

                    System.Data.SqlClient.SqlCommand cmd =
                        new System.Data.SqlClient.SqlCommand("update logmarkinfo set markdesc=@markdesc,markowner=@markowner,markstate=@markstate,marktime=@marktime  where markid = @markid;", conn);
                    cmd.Parameters.AddWithValue("@markdesc", t.s_Desc);
                    cmd.Parameters.AddWithValue("@markowner", t.s_Owner);
                    cmd.Parameters.AddWithValue("@markstate", t.IsOpen);
                    cmd.Parameters.AddWithValue("@marktime", t.CreateTime.ToString().Trim());

                    cmd.Parameters.AddWithValue("@markid", t.i_Guid.ToString());

                    if (0 == cmd.ExecuteNonQuery())
                        return false;
                }

            }
            catch (System.Exception e)
            {
                CExceptionContainer_WuQi.ProcessException(e);
                throw e;
            }
            finally
            {
                if (null != conn)
                    conn.Close();
            }

            return true;
        }
        public override System.Collections.Generic.Dictionary<uint, CLogMark_WuQi> SynchronousDB()
        {
            if (null == this.str_conn)
                return null;
            Dictionary<uint, CLogMark_WuQi> dict = new Dictionary<uint, CLogMark_WuQi>();
            System.Data.SqlClient.SqlConnection conn = null;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
//                 lock (this)
                {
                    uint uguid = 0;
                    conn = new System.Data.SqlClient.SqlConnection(this.str_conn);
                    conn.Open();
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM logmarkinfo", conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        uguid = uint.Parse(reader["markid"].ToString());
                        uint id = uguid;
                        string sdesc = reader["markdesc"].ToString();
                        string sowner = reader["markowner"].ToString();
                        int istate = int.Parse(reader["markstate"].ToString());
                        string logtime = reader["marktime"].ToString();
                        CLogMark_WuQi record = new CLogMark_WuQi(id, sdesc, sowner, istate, DateTime.Parse(logtime));
                        dict.Add(id, record);
                    }
                }

            }
            catch (System.Exception e)
            {
                CExceptionContainer_WuQi.ProcessException(e);
                throw e;
            }
            finally
            {
                if (null != reader)
                    reader.Close();
                if (null != conn)
                    conn.Close();

            }
            return dict;
        }
        #endregion operatoedb
    }
}
