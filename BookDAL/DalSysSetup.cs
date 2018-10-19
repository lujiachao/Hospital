using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace BookDAL
{
    
    public class DalSysSetup
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        public DataTable DalGetSysList()
        {
            string sql = @"SELECT PARENTID,CODE,NAME,OBJECT,PARENTID1,IS_FUNCTION_POINT,IS_INFORMATION,REMAKE,INPUT_TIME,SEQ FROM SYS_OBJECT WHERE MODULE_CODE = 'M01' OR MODULE_CODE = 'M02' ";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        public DataTable DalImageCombox()
        {
            string sql = @"SELECT NAME,TYPE FROM SYS_OBJECT WHERE MODULE_CODE = 'M01'";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        public bool DalCheckCode(string CODE)
        {
            string sql = @"SELECT COUNT(*) FROM SYS_OBJECT WHERE CODE = '"+CODE+"'";
            int? count = sqlDBHelper.GetCount(sql);
            if (count != null && count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string DalMaxParentID(string TYPE)
        {
            string sql = @"select max(parentid) from sys_object where type= '"+TYPE+"'";
            string FirstValue = sqlDBHelper.GetScalar(sql).ToString();
            return FirstValue;
        }
        public string DalMaxType()
        {
            string sql = @"select max(type) from sys_object ";
            string MaxValue = sqlDBHelper.GetScalar(sql).ToString();
            return MaxValue;
        }
        public bool UpdateSysObject(string code,string isUser, string edition, string name, string Object, string version, string date, string seq, string remake)
        {
              DateTime Datetime = Convert.ToDateTime(date);
//            string sql = @"UPDATE SYS_OBJECT
//                           SET  IS_FUNCTION_POINT = '" + isUser + "' ,  IS_INFORMATION = '" + edition + "',NAME = '" + name + "',OBJECT = '" + Object + "',PARENTID1 = '" + version + "',INPUT_TIME = '" + Date + "',SEQ = '" + seq + "',REMAKE = '" + remake + "' WHERE CODE = '" + code + "'";
//            int rows = SqlDBHelper.ExecuteSql(sql);
//            if (rows > 0)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
             StringBuilder strSql = new StringBuilder();
             strSql.Append("UPDATE SYS_OBJECT SET ");
             strSql.Append("IS_FUNCTION_POINT=:edition,");
             strSql.Append("IS_INFORMATION=:isUser,");
             strSql.Append("NAME=:name,");
             strSql.Append("OBJECT=:Object,");
             strSql.Append("PARENTID1=:version,");
             strSql.Append("INPUT_TIME=:Datetime,");
             strSql.Append("SEQ=:seq,");
             strSql.Append("REMAKE=:remake");
             strSql.Append(" where CODE=:code");
             OleDbParameter[] parameters = {
					new OleDbParameter(":edition", OleDbType.VarChar,50),
					new OleDbParameter(":isUser", OleDbType.VarChar,50),
					new OleDbParameter(":name", OleDbType.VarChar,50),
					new OleDbParameter(":Object", OleDbType.VarChar,50),
					new OleDbParameter(":version", OleDbType.VarChar,50),
					new OleDbParameter(":Datetime", OleDbType.Date),
					new OleDbParameter(":seq", OleDbType.VarChar,50),
					new OleDbParameter(":remake", OleDbType.VarChar,50),
					new OleDbParameter(":code", OleDbType.VarChar,50)};
             parameters[0].Value = edition;
             parameters[1].Value = isUser;
             parameters[2].Value = name;
             parameters[3].Value = Object;
             parameters[4].Value = version;
             parameters[5].Value = Datetime;
             parameters[6].Value = seq;
             parameters[7].Value = remake;
             parameters[8].Value = code;
             int rows = SqlDBHelper.ExecuteSql(strSql.ToString(), parameters);
             if (rows > 0)
             {
                 return true;
             }
             else
             {
                 return false;
             }
        }
        public bool insertSysObject(string code, string isUser, string edition, string name, string Object, string version, string seq, string remake, string MaxParentID)
        {
            string moduleCode = "M01";
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO SYS_OBJECT(");
            strSql.Append("CODE,IS_FUNCTION_POINT,IS_INFORMATION,NAME,OBJECT,PARENTID1,SEQ,REMAKE,MODULE_CODE,PARENTID)");
            strSql.Append(" values (");
            strSql.Append(":code,:edition,:isUser,:name,:Object,:version,:seq,:remake,:moduleCode,:MaxParentID)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":code", OleDbType.VarChar,50),
					new OleDbParameter(":edition", OleDbType.VarChar,50),
					new OleDbParameter(":isUser", OleDbType.VarChar,50),
					new OleDbParameter(":name", OleDbType.VarChar,50),
					new OleDbParameter(":Object", OleDbType.VarChar,50),
					new OleDbParameter(":version", OleDbType.VarChar,50),
					new OleDbParameter(":seq", OleDbType.VarChar,50),
					new OleDbParameter(":remake", OleDbType.VarChar,50),
                    new OleDbParameter(":moduleCode", OleDbType.VarChar,50),
                    new OleDbParameter(":MaxParentID", OleDbType.VarChar,10)};
            parameters[0].Value = code;
            parameters[1].Value = edition;
            parameters[2].Value = isUser;
            parameters[3].Value = name;
            parameters[4].Value = Object;
            parameters[5].Value = version;
            parameters[6].Value = seq;
            parameters[7].Value = remake;           
            parameters[8].Value = moduleCode;
            parameters[9].Value = MaxParentID;
            int rows = SqlDBHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeleteSysObject(string code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM SYS_OBJECT");
            strSql.Append(" WHERE CODE = :code");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":code", OleDbType.VarChar,50)};
            parameters[0].Value = code;
            int rows = SqlDBHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
