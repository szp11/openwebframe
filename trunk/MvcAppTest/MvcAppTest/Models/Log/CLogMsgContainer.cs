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
    /// <summary>
    /// 继承了对象容器类，用来缓存log信息。继承ILogDB_WuQi接口，来实现主库
    /// </summary>
    internal class CLogMsgContainer_WuQi : ObjectContainer_WuQi<CLogMsg_WuQi>, IHandleMsg_WuQi
    {
        private string str_conn = null;
        public CLogMsgContainer_WuQi(string conn, ICacheStorage_WuQi ics, ICacheDependency_WuQi icd)
            : base(ics, icd)
        {
            this.str_conn = conn;
        }
        //实现ILogDB_WuQi接口
        #region handlemsg
        public bool InsertMsg(CLogMsg_WuQi smsg)
        {
            return base.Insert(smsg.ui_id, smsg);
        }
        private bool Delete(CLogMsg_WuQi smsg)
        {
            return base.Delete(smsg.ui_id, smsg);
        }
        public int DeleteMsg(int adapter, ArrayList al)
        {
            ArrayList msgs = base.Search(adapter, al);
            foreach (CLogMsg_WuQi slr in msgs)
            {
                this.Delete(slr);
            }
            return msgs.Count;
        }
        public int SynchronousAllRecord()
        {
            return base.SynchronousAllObject();
        }
        public ArrayList ReadAllRecord(int adapter, string msgowner)
        {
            ArrayList al = new ArrayList();
            al.Add(msgowner);
            return base.Search(adapter, al);
        }
        public int WriteAllRecord(CLogMsg_WuQi[] smsg)
        {
            return 0;
        }
        public void ClearAllRecord()
        {
            base.Clear();
        }

        #endregion

        //实现基类的 virtual函数,完成对数据库的操作
        #region operatordb

        public override Dictionary<object, CLogMsg_WuQi> SynchronousDB()
        {
            if (null == this.str_conn)
                return null;

            Dictionary<object, CLogMsg_WuQi> dict = new Dictionary<object, CLogMsg_WuQi>();

            System.Data.SqlClient.SqlConnection conn = null;
            System.Data.SqlClient.SqlDataReader reader = null;

            try
            {
                lock (this)
                {
                    uint ui_guid = 0;
                    conn = new System.Data.SqlClient.SqlConnection(this.str_conn);
                    conn.Open();
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM logmsginfo", conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ui_guid = (uint)reader["msgid"];
                        uint id = ui_guid;
                        string smark = reader["msgmark"].ToString();
                        string stype = reader["msgtype"].ToString();
                        string smsg = reader["logmsg"].ToString();
                        string sowner = reader["msgowner"].ToString();
                        string logtime = reader["msgtime"].ToString();
                        CLogMsg_WuQi record = CLogMsgFactory.CreateMsg(id, smark, stype, smsg, sowner, DateTime.Parse(logtime));
                        dict.Add(id, record);
                    }

                    CLogMsg_WuQi.SetMyGuid(ui_guid);

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

        protected override bool InsertDB(object k, CLogMsg_WuQi t)
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

                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("insert into logmsginfo(msgid,msgmark,msgtype,logmsg,msgowner,msgtime) values(@logid,@logmark,@logtype,@logmsg,@logowner,@logtime);", conn);
                    cmd.Parameters.AddWithValue("@logid", t.ui_id);
                    cmd.Parameters.AddWithValue("@logmark", t.str_mark);
                    cmd.Parameters.AddWithValue("@logtype", t.str_logtype);
                    cmd.Parameters.AddWithValue("@logmsg", t.str_logmsg);
                    cmd.Parameters.AddWithValue("@logowner", t.str_logowner);
                    cmd.Parameters.AddWithValue("@logtime", t.d_logtime.ToString().Trim());
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

        protected override bool DeleteDB(object k, CLogMsg_WuQi t)
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

                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("delete from logmsginfo where msgid = @msgid;", conn);

                    cmd.Parameters.AddWithValue("@msgid", t.ui_id);
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
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("DELETE  FROM [logmsginfo]", conn);
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

        protected override bool UpdateDB(object k, CLogMsg_WuQi t)
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

                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("update logmsginfo set msgmark=@logmark, msgtype=@logtype,logmsg=@logmsg,msgowner=@logowner,msgtime=@logtime where msgid = @msgid;", conn);
                    cmd.Parameters.AddWithValue("@logmark", t.str_mark);
                    cmd.Parameters.AddWithValue("@logtype", t.str_logtype);
                    cmd.Parameters.AddWithValue("@logmsg", t.str_logmsg);
                    cmd.Parameters.AddWithValue("@logowner", t.str_logowner);
                    cmd.Parameters.AddWithValue("@logtime", t.d_logtime.ToString().Trim());

                    cmd.Parameters.AddWithValue("@msgid", t.ui_id);
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

        #endregion
    }

}
