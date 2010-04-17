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

namespace MvcAppTest.Helper.Log
{
    /// <summary>
    /// 继承了对象容器类，用来缓存log信息。继承ILogDB_WuQi接口，来实现主库
    /// </summary>
    internal class CLogMsgContainer_WuQi : ObjectContainer_WuQi<SLogRecord_WuQi>, IHandleMsg_WuQi
    {
        private string str_conn = null;
        public CLogMsgContainer_WuQi(string conn, ICacheStorage_WuQi ics, ICacheDependency_WuQi icd)
            : base(ics,icd)
        {
            this.str_conn = conn;
        }
        //实现ILogDB_WuQi接口
        #region
        public bool InsertMsg(CLogMsg_WuQi smsg)
        {
            SLogRecord_WuQi slr = new SLogRecord_WuQi(smsg);
            return base.Insert(slr.str_guid, slr);
        }
        private bool Delete(SLogRecord_WuQi smsg)
        {
            return base.Delete(smsg.str_guid, smsg);
        }
        public int DeleteMsg(int adapter, ArrayList al)
        {
            ArrayList msgs = base.Search(adapter, al);
            foreach (SLogRecord_WuQi slr in msgs)
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
        #region

        public override Dictionary<object, SLogRecord_WuQi> SynchronousDB()
        {
            Dictionary<object, SLogRecord_WuQi> dict = new Dictionary<object, SLogRecord_WuQi>();

            System.Data.SqlClient.SqlConnection conn = null;
            System.Data.SqlClient.SqlDataReader reader = null;

            try
            {
                lock (this)
                {
                    conn = new System.Data.SqlClient.SqlConnection(this.str_conn);
                    conn.Open();
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM loginfo", conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string logid = reader["logid"].ToString();
                        string logtype = reader["typeid"].ToString();
                        string logmsg = reader["logmsg"].ToString();
                        string logowner = reader["logowner"].ToString();
                        string logtime = reader["logtime"].ToString();
                        SLogRecord_WuQi record = new SLogRecord_WuQi(logid, logtype, logmsg, logowner, DateTime.Parse(logtime));
                        dict.Add(logid, record);
                    }

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

        protected override bool InsertDB(object k, SLogRecord_WuQi t)
        {
            System.Data.SqlClient.SqlConnection conn = null;
            try
            {
                lock (this)
                {
                    conn = new System.Data.SqlClient.SqlConnection(this.str_conn);
                    conn.Open();

                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("insert into loginfo(logid,typeid, logmsg,logowner,logtime) values(@logid,@logtype,@logmsg,@logowner,@logtime);", conn);
                    cmd.Parameters.AddWithValue("@logid", t.str_guid);
                    cmd.Parameters.AddWithValue("@logtype", t.obj_Msg.str_logtype);
                    cmd.Parameters.AddWithValue("@logmsg", t.obj_Msg.str_logmsg);
                    cmd.Parameters.AddWithValue("@logowner", t.obj_Msg.str_logowner);
                    cmd.Parameters.AddWithValue("@logtime", t.obj_Msg.d_logtime.ToString().Trim());
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

        protected override bool DeleteDB(object k, SLogRecord_WuQi t)
        {
            System.Data.SqlClient.SqlConnection conn = null;
            try
            {
                lock (this)
                {
                    conn = new System.Data.SqlClient.SqlConnection(this.str_conn);
                    conn.Open();

                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("delete from loginfo where logid = @logid;", conn);

                    cmd.Parameters.AddWithValue("@logid", t.str_guid);
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
            System.Data.SqlClient.SqlConnection conn = null;
            lock (this)
            {
                try
                {
                    conn = new System.Data.SqlClient.SqlConnection(this.str_conn);
                    conn.Open();
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("DELETE  FROM [loginfo]", conn);
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

        protected override bool UpdateDB(object k, SLogRecord_WuQi t)
        {
            return base.UpdateDB(k, t);
        }

        #endregion
    }

}
