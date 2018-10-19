using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;

namespace Tool
{
    /// <summary>
    /// 功能；SQL数据库访问类
    /// </summary>
    public class SqlDBHelper
    {
        static string connString = ConfigurationManager.ConnectionStrings["connString"].ToString();
        /// <summary>
        /// 查询获得首行首列的值，格式化SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object GetScalar(string sql)
        {
            object obj = null;
            try
            {
                OleDbConnection conn = new OleDbConnection(connString);
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                obj = cmd.ExecuteScalar();
                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return obj;
        }
        /// <summary>
        /// 增删改操作，返回受影响的行数，格式化SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int GetNonQuery(string sql)
        {
            int i = 0;
            try
            {
                OleDbConnection conn = new OleDbConnection(connString);
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                i = cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return i;
        }
        /// <summary>
        /// 查询操作，返回一个数据表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public  DataTable GetTable(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                OleDbConnection conn = new OleDbConnection(connString);
                conn.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);
                da.Fill(dt);
                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        /// <summary>
        /// 获得没有参数绑定表中的数据记录数
        /// </summary>
        /// <param name="sql">计算的数据条数</param>
        /// <param name="parameters">参数</param>
        /// <returns>null:出错;数字:获得数据记录数</returns>
        public int? GetCount(string sql)
        {
            int? count = null;
            object obj = null;
            try
            {
                OleDbConnection conn = new OleDbConnection(connString);
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                obj = cmd.ExecuteScalar();
                count = Convert.ToInt32(obj);
                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return count;
        }
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                using (OleDbCommand cmd = new OleDbCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params OleDbParameter[] cmdParms)
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.OleDb.OleDbException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }
        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, string cmdText, OleDbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (OleDbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
        #endregion
        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params OleDbParameter[] cmdParms)
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.OleDb.OleDbException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public  DataTable Query(string SQLString, params OleDbParameter[] cmdParms)
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                OleDbCommand cmd = new OleDbCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        da.Fill(dt);
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.OleDb.OleDbException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return dt;
                }
            }
        }
    }
}
