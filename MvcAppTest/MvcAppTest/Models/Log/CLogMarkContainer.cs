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
    public class CLogMarkContainer_WuQi : ObjectContainer_WuQi<CLogMark_WuQi>, IHandleMark_WuQi
    {
        public string str_conn = null;//数据库连接字符串
        public CLogMarkContainer_WuQi(string conn, ICacheStorage_WuQi ics, ICacheDependency_WuQi icd)
            : base(ics, icd)
        {
            this.str_conn = conn;
        }
        #region operatordb
        protected override void ClearDB()
        {
            if (null == this.str_conn)
                return;
            System.Data.SqlClient.SqlConnection conn = null;
            lock (this)
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
                    throw e;
                }
                finally
                {
                    if (null != conn)
                        conn.Close();
                }

            }
        }
        protected override bool DeleteDB(object k, CLogMark_WuQi t)
        {
            if (null == this.str_conn)
                return false;
            System.Data.SqlClient.SqlConnection conn = null;
            try
            {
                lock (this)
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
                throw e;
            }
            finally
            {
                if (null != conn)
                    conn.Close();
            }

            return true;
        }

        protected override bool InsertDB(object k, CLogMark_WuQi t)
        {
            if (null == this.str_conn)
                return false;
            System.Data.SqlClient.SqlConnection conn = null;
            try
            {
                lock (this)
                {
                    conn = new System.Data.SqlClient.SqlConnection(this.str_conn);
                    conn.Open();

                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("insert into logmarkinfo(markid,markdesc,markowner,markstate,marktime) values(@markid,@markdesc,@markowner,@markstate,@marktime);", conn);
                    cmd.Parameters.AddWithValue("@markid", t.i_Guid);
                    cmd.Parameters.AddWithValue("@markdesc", t.s_Desc);
                    cmd.Parameters.AddWithValue("@markowner", t.s_Owner);
                    cmd.Parameters.AddWithValue("@markstate", t.i_IsOpen);
                    cmd.Parameters.AddWithValue("@marktime", t.d_CreateTime.ToString().Trim());
                    if (0 == cmd.ExecuteNonQuery())
                        return false;
                }

            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                if (null != conn)
                    conn.Close();
            }

            return true;
        }
        protected override bool UpdateDB(object k, CLogMark_WuQi t)
        {
            if (null == this.str_conn)
                return false;
            System.Data.SqlClient.SqlConnection conn = null;
            try
            {
                lock (this)
                {
                    conn = new System.Data.SqlClient.SqlConnection(this.str_conn);
                    conn.Open();

                    System.Data.SqlClient.SqlCommand cmd =
                        new System.Data.SqlClient.SqlCommand("update logmarkinfo set markdesc=@markdesc,markowner=@markowner,markstate=@markstate,marktime=@marktime  where markid = @markid;", conn);
                    cmd.Parameters.AddWithValue("@markdesc", t.s_Desc);
                    cmd.Parameters.AddWithValue("@markowner", t.s_Owner);
                    cmd.Parameters.AddWithValue("@markstate", t.i_IsOpen);
                    cmd.Parameters.AddWithValue("@marktime", t.d_CreateTime.ToString().Trim());

                    cmd.Parameters.AddWithValue("@msgid", t.i_Guid);

                    if (0 == cmd.ExecuteNonQuery())
                        return false;
                }

            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                if (null != conn)
                    conn.Close();
            }

            return true;
        }
        public override System.Collections.Generic.Dictionary<object, CLogMark_WuQi> SynchronousDB()
        {
            if (null == this.str_conn)
                return null;
            Dictionary<object, CLogMark_WuQi> dict = new Dictionary<object, CLogMark_WuQi>();
            System.Data.SqlClient.SqlConnection conn = null;
            System.Data.SqlClient.SqlDataReader reader = null;
            try
            {
                lock (this)
                {
                    uint uguid = 0;
                    conn = new System.Data.SqlClient.SqlConnection(this.str_conn);
                    conn.Open();
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM logmarkinfo", conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        uguid = (uint)reader["markid"];
                        uint id = uguid;
                        string sdesc = reader["markdesc"].ToString();
                        string sowner = reader["markowner"].ToString();
                        int istate = int.Parse(reader["markstate"].ToString());
                        string logtime = reader["msgtime"].ToString();
                        CLogMark_WuQi record = new CLogMark_WuQi(id, sdesc, sowner, istate, DateTime.Parse(logtime));
                        dict.Add(id, record);
                    }
                    CLogMark_WuQi.SetMyGuid((object)uguid);
                }

            }
            catch (System.Exception e)
            {
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
