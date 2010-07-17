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
    /// <summary>
    /// 继承了对象容器类，用来缓存log信息。继承ILogDB_WuQi接口，来实现主库
    /// </summary>
    internal class CLogMsgContainer_WuQi : ObjectContainer_WuQi<uint,CLogMsg_WuQi>, IHandleMsg_WuQi
    {
        private string str_conn = null;
        public CLogMsgContainer_WuQi(string conn, ICacheStorage_WuQi<uint, CLogMsg_WuQi> ics, ICacheDependency_WuQi<uint, CLogMsg_WuQi> icd)
            : base(ics, icd)
        {
            this.str_conn = conn;
            //首先清空数据库及缓存
            Clear();
            //获得数据库中做大的主键值
            SetMaxGuid();
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
        public int DeleteMsg(int adapter, List<uint> al)
        {
            if(3 == adapter)
            {
                List<CLogMsg_WuQi> msgs = new List<CLogMsg_WuQi>();
                foreach (var item in al)
                {
                    msgs.Add(base.SelectSingleObject(item));
                }

                foreach (CLogMsg_WuQi slr in msgs)
                {
                    this.Delete(slr);
                }
                return msgs.Count;
            }

            return 0;
        }
        public int SynchronousAllRecord()
        {
            return base.SynchronousAllObject();
        }
        public List<CLogMsg_WuQi> ReadAllRecord(int adapter, string msgowner)
        {
            Hashtable al = new Hashtable();
            al.Add("owner",msgowner);
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
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("SELECT Max(msgid) FROM logmsginfo", conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if(!reader.IsDBNull(0))
                            ui_guid = uint.Parse(reader[0].ToString());
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
            return;
        }
        protected override List<CLogMsg_WuQi> SearchDB(int adapter, Hashtable paraset)
        {
            if (null == this.str_conn)
                return null;

            List<CLogMsg_WuQi> dict = new List<CLogMsg_WuQi>();

            System.Data.SqlClient.SqlConnection conn = null;
            System.Data.SqlClient.SqlDataReader reader = null;

            try
            {
//                 lock (this)
                {
                    uint ui_guid = 0;
                    conn = new System.Data.SqlClient.SqlConnection(this.str_conn);
                    conn.Open();
                    System.Data.SqlClient.SqlCommand cmd = null;
                    //根据不同查询条件搜索数据库
                    if(0 == adapter)
                    {
                        cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM logmsginfo", conn);
                    }else if ( 1 ==adapter)
                    {
                        cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM logmsginfo where msgowner =@owner", conn);
                        cmd.Parameters.AddWithValue("@owner", (string)paraset["owner"]);
                    }
                    
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ui_guid = uint.Parse(reader["msgid"].ToString());
                        uint id = ui_guid;
                        string smark = reader["msgmark"].ToString();
                        string stype = reader["msgtype"].ToString();
                        string smsg = reader["logmsg"].ToString();
                        string sowner = reader["msgowner"].ToString();
                        string logtime = reader["msgtime"].ToString();
                        CLogMsg_WuQi record = CLogMsgFactory.CreateMsg(id, smark, stype, smsg, sowner, DateTime.Parse(logtime));
                        dict.Add(record);
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
        public override Dictionary<uint, CLogMsg_WuQi> SynchronousDB()
        {
            if (null == this.str_conn)
                return null;

            Dictionary<uint, CLogMsg_WuQi> dict = new Dictionary<uint, CLogMsg_WuQi>();

            System.Data.SqlClient.SqlConnection conn = null;
            System.Data.SqlClient.SqlDataReader reader = null;

            try
            {
//                 lock (this)
                {
                    uint ui_guid = 0;
                    conn = new System.Data.SqlClient.SqlConnection(this.str_conn);
                    conn.Open();
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM logmsginfo", conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ui_guid = uint.Parse(reader["msgid"].ToString());
                        uint id = ui_guid;
                        string smark = reader["msgmark"].ToString();
                        string stype = reader["msgtype"].ToString();
                        string smsg = reader["logmsg"].ToString();
                        string sowner = reader["msgowner"].ToString();
                        string logtime = reader["msgtime"].ToString();
                        CLogMsg_WuQi record = CLogMsgFactory.CreateMsg(id, smark, stype, smsg, sowner, DateTime.Parse(logtime));
                        dict.Add(id, record);
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

        protected override bool InsertDB(uint k, CLogMsg_WuQi t)
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

                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("insert into logmsginfo(msgid,msgmark,msgtype,logmsg,msgowner,msgtime) values(@logid,@logmark,@logtype,@logmsg,@logowner,@logtime);", conn);
                    cmd.Parameters.AddWithValue("@logid", (Int64)t.ui_id);
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

        protected override bool DeleteDB(uint k, CLogMsg_WuQi t)
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

                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("delete from logmsginfo where msgid = @msgid;", conn);

                    cmd.Parameters.AddWithValue("@msgid", (Int64)t.ui_id);
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
//             lock (this)
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

        protected override bool UpdateDB(uint k, CLogMsg_WuQi t)
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
