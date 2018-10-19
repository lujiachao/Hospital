using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.OleDb;

namespace BookDAL
{

    public class DalMaterialClass
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        public DataTable DalGetAccount()
        {
            string sql = @"SELECT * FROM MMIS_ACCOUNT_TYPE WHERE STATE = 1 order by code";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        public DataTable DalGetMatClass(string acc_ID)
        {
            string sql = string.Empty;
            if (acc_ID != null)
            {
                sql = @"
                      SELECT MAT_CLASS.*
                         FROM MMIS_ACC_MAT_CLASS ACC_MAT_CLASS,MMIS_MAT_CLASS MAT_CLASS
                      WHERE ACC_MAT_CLASS.CLASS_ID = MAT_CLASS.ID
                           AND (MAT_CLASS.STATE = '0' OR MAT_CLASS.STATE = '1')
                           AND ACC_MAT_CLASS.ACC_ID =  '" + acc_ID + "'";
            }
            else
            {
                sql = @"
                          SELECT MAT_CLASS.*
                            FROM MMIS_MAT_CLASS MAT_CLASS 
                           WHERE (MAT_CLASS.STATE = '0' OR MAT_CLASS.STATE='1') ";
            }
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        public bool DalCheckCode(string CODE)
        {
            string sql = @"SELECT COUNT(*) FROM MMIS_MAT_CLASS WHERE CODE = '" + CODE + "'";
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
        public bool insertMatClass(string code, string name, string pid, string inputcode1, string inputcode2, string state, string remark,string manage_type,string acc_id)
        {
            StringBuilder strSql = new StringBuilder();
            int id = Convert.ToInt32(code);
            int Acc_id = Convert.ToInt32(acc_id);
            int Pid = Convert.ToInt32(pid);
            char State = Convert.ToChar(state);
            strSql.Append("INSERT INTO MMIS_MAT_CLASS(");
            strSql.Append("ID,CODE,NAME,PID,INPUTCODE1,INPUTCODE2,STATE,REMARK,MANAGE_TYPE)");
            strSql.Append(" values (");
            strSql.Append(":id,:code,:name,:pid,:inputcode1,:inputcode2,:state,:remark,:manage_type)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,3),
                    new OleDbParameter(":code", OleDbType.VarChar,8),
					new OleDbParameter(":name", OleDbType.VarChar,32),
					new OleDbParameter(":pid", OleDbType.Numeric,3),
					new OleDbParameter(":inputcode1", OleDbType.VarChar,8),
					new OleDbParameter(":inputcode2", OleDbType.VarChar,8),
					new OleDbParameter(":state", OleDbType.Char,1),
                    new OleDbParameter(":remark", OleDbType.VarChar,128),
                    new OleDbParameter(":manage_type", OleDbType.VarChar,128)};
            parameters[0].Value = id;
            parameters[1].Value = code;
            parameters[2].Value = name;
            parameters[3].Value = Pid;
            parameters[4].Value = inputcode1;
            parameters[5].Value = inputcode2;
            parameters[6].Value = State;
            parameters[7].Value = remark;
            parameters[8].Value = manage_type;
            int rows = SqlDBHelper.ExecuteSql(strSql.ToString(), parameters);
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("INSERT INTO MMIS_ACC_MAT_CLASS(");
            strSql1.Append("ACC_ID,CLASS_ID)");
            strSql1.Append(" values (");
            strSql1.Append(":Acc_id,:id)");
            OleDbParameter[] parameters1 = {
                    new OleDbParameter(":Acc_id", OleDbType.Numeric,3),
                    new OleDbParameter(":id", OleDbType.Numeric,3)};
            parameters1[0].Value = Acc_id;
            parameters1[1].Value = id;
            int rows1 = SqlDBHelper.ExecuteSql(strSql1.ToString(), parameters1);
            if (rows > 0 && rows1 > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool updateMatClass(string code, string name, string inputcode1, string inputcode2, string state, string remark, string manage_type)
        {
            StringBuilder strSql = new StringBuilder();
            int id = Convert.ToInt32(code);
            char State = Convert.ToChar(state);
            strSql.Append("UPDATE MMIS_MAT_CLASS SET ");
            strSql.Append("NAME = :name,");
            strSql.Append("INPUTCODE1 = :inputcode1,");
            strSql.Append("INPUTCODE2 = :inputcode2,");
            strSql.Append("STATE = :state,");
            strSql.Append("REMARK = :remark,");
            strSql.Append("MANAGE_TYPE = :manage_type");
            strSql.Append(" WHERE CODE = :code");
            OleDbParameter[] parameters = {                  
					new OleDbParameter(":name", OleDbType.VarChar,32),
					new OleDbParameter(":inputcode1", OleDbType.VarChar,8),
					new OleDbParameter(":inputcode2", OleDbType.VarChar,8),
					new OleDbParameter(":state", OleDbType.Char,1),
                    new OleDbParameter(":remark", OleDbType.VarChar,128),
                    new OleDbParameter(":manage_type", OleDbType.VarChar,128),
                    new OleDbParameter(":code", OleDbType.VarChar,8)};
            parameters[0].Value = name;
            parameters[1].Value = inputcode1;
            parameters[2].Value = inputcode2;
            parameters[3].Value = State;
            parameters[4].Value = remark;
            parameters[5].Value = manage_type;
            parameters[6].Value = code;
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
        public bool deleteMatClass(string code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM MMIS_MAT_CLASS");
            strSql.Append(" WHERE CODE = :code");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":code", OleDbType.VarChar,10)};
            parameters[0].Value = code;
            int rows = SqlDBHelper.ExecuteSql(strSql.ToString(), parameters);
            
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("DELETE FROM MMIS_ACC_MAT_CLASS");
            strSql1.Append(" WHERE CLASS_ID = :code");
            OleDbParameter[] parameters1 = {
                 new OleDbParameter(":code", OleDbType.VarChar,10)};
            parameters1[0].Value = code;
            int rows1 = SqlDBHelper.ExecuteSql(strSql1.ToString(), parameters1);
            if (rows > 0 && rows1 >0)
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
