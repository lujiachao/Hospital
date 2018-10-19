using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.OleDb;

namespace BookDAL
{
    
    public class DalAccount
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        public DataTable DalGetAccount(string state)
        {
            string sql = @"SELECT * FROM MMIS_ACCOUNT_TYPE WHERE STATE = '"+state+"'";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        public DataTable DalGetEmpID(string code)
        {
            string sql = @"SELECT B.NAME,A.EMP_ID,A.ROLE,A.DEPARTMENT FROM MMIS_ACC_EMP A,PUB_EMP B WHERE A.EMP_ID = B.CODE AND A.ACC_ID = '"+code+"'";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        public bool DalCheckCode(string CODE)
        {
            string sql = @"SELECT COUNT(*) FROM MMIS_ACCOUNT_TYPE WHERE CODE = '" + CODE + "'";
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
        public bool insertAccount(string code, string name, string Price_rule, string Manage_type, string Use_stock, string inputcode1, string inputcode2, string State, string remark, string acc_emp_id,string danju)
        {
            StringBuilder strSql = new StringBuilder();
            int? id = Convert.ToInt32(code);
            int? price_rule = Convert.ToInt32(Price_rule);
            char manage_type = Convert.ToChar(Manage_type);
            char use_stock = Convert.ToChar(Use_stock);
            char state = Convert.ToChar(State);
            strSql.Append("INSERT INTO MMIS_ACCOUNT_TYPE(");
            strSql.Append("ID,CODE,NAME,PRICE_RULE,MANAGE_TYPE,USE_STOCK,INPUTCODE1,INPUTCODE2,STATE,REMARK,ACC_EMP_ID,DANJU)");
            strSql.Append(" values (");
            strSql.Append(":id,:code,:name,:price_rule,:manage_type,:use_stock,:inputcode1,:inputcode2,:state,:remark,:acc_emp_id,:danju)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,5),
                    new OleDbParameter(":code", OleDbType.VarChar,5),
					new OleDbParameter(":name", OleDbType.VarChar,32),
					new OleDbParameter(":price_rule", OleDbType.Numeric,2),
					new OleDbParameter(":manage_type", OleDbType.Char,1),
					new OleDbParameter(":use_stock", OleDbType.Char,1),
					new OleDbParameter(":inputcode1", OleDbType.VarChar,8),
					new OleDbParameter(":inputcode2", OleDbType.VarChar,8),
					new OleDbParameter(":state", OleDbType.Char,1),
                    new OleDbParameter(":remark", OleDbType.VarChar,128),
                    new OleDbParameter(":acc_emp_id", OleDbType.VarChar,16),
                    new OleDbParameter(":danju", OleDbType.VarChar,10)};
            parameters[0].Value = id;
            parameters[1].Value = code;
            parameters[2].Value = name;
            parameters[3].Value = price_rule;
            parameters[4].Value = manage_type;
            parameters[5].Value = use_stock;
            parameters[6].Value = inputcode1;
            parameters[7].Value = inputcode2;
            parameters[8].Value = state;
            parameters[9].Value = remark;
            parameters[10].Value = acc_emp_id;
            parameters[11].Value = danju;
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
        public bool updateAccount(string code, string name, string Price_rule, string Manage_type, string Use_stock, string inputcode1, string inputcode2, string State, string remark, string acc_emp_id, string danju)
        {
            StringBuilder strSql = new StringBuilder();
            int? id = Convert.ToInt32(code);
            int? price_rule = Convert.ToInt32(Price_rule);
            char manage_type = Convert.ToChar(Manage_type);
            char use_stock = Convert.ToChar(Use_stock);
            char state = Convert.ToChar(State);
            strSql.Append("UPDATE MMIS_ACCOUNT_TYPE SET ");
            strSql.Append("NAME = :name,");
            strSql.Append("PRICE_RULE = :price_rule,");
            strSql.Append("MANAGE_TYPE = :manage_type,");
            strSql.Append("USE_STOCK = :use_stock,");
            strSql.Append("INPUTCODE1 = :inputcode1,");
            strSql.Append("INPUTCODE2 = :inputcode2,");
            strSql.Append("STATE = :state,");
            strSql.Append("REMARK = :remark,");
            strSql.Append("ACC_EMP_ID = :acc_emp_id,");
            strSql.Append("DANJU = :danju");
            strSql.Append(" WHERE CODE = :code");
            OleDbParameter[] parameters = {
					new OleDbParameter(":name", OleDbType.VarChar,32),
					new OleDbParameter(":price_rule", OleDbType.Numeric,2),
					new OleDbParameter(":manage_type", OleDbType.Char,1),
					new OleDbParameter(":use_stock", OleDbType.Char,1),
					new OleDbParameter(":inputcode1", OleDbType.VarChar,8),
					new OleDbParameter(":inputcode2", OleDbType.VarChar,8),
					new OleDbParameter(":state", OleDbType.Char,1),
                    new OleDbParameter(":remark", OleDbType.VarChar,128),
                    new OleDbParameter(":acc_emp_id", OleDbType.VarChar,16),
                    new OleDbParameter(":danju", OleDbType.VarChar,10),
                    new OleDbParameter(":code", OleDbType.VarChar,5),};
            parameters[0].Value = name;
            parameters[1].Value = price_rule;
            parameters[2].Value = manage_type;
            parameters[3].Value = use_stock;
            parameters[4].Value = inputcode1;
            parameters[5].Value = inputcode2;
            parameters[6].Value = state;
            parameters[7].Value = remark;
            parameters[8].Value = acc_emp_id;
            parameters[9].Value = danju;
            parameters[10].Value = code;
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
        public string DalPY(string type,string mark)
        {
            string sql = @"select  " + type + "  from SYSINPUTCODE where HZ= '" + mark + "'";
            string FirstValue = sqlDBHelper.GetScalar(sql).ToString();
            return FirstValue;
        }
        public bool deleteAccount(string code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM MMIS_ACCOUNT_TYPE");
            strSql.Append(" WHERE CODE = :code");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":code", OleDbType.VarChar,10)};
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
        public bool insertPubEmp(string code,string name)
        {
            int? id = Convert.ToInt32(code);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO PUB_EMP(");
            strSql.Append("ID,CODE,NAME,PASSWORD,POWER)");
            strSql.Append(" values (");
            strSql.Append(":id,:code,:name,'1','1')");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,5),
                    new OleDbParameter(":code", OleDbType.VarChar,5),
                    new OleDbParameter(":name", OleDbType.VarChar,10)};
            parameters[0].Value = id;
            parameters[1].Value = code;
            parameters[2].Value = name;
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
        public bool insertAccEmp(string Acc_id, string Emp_id,string role,string department)
        {
            int? acc_id = Convert.ToInt32(Acc_id);
            int? emp_id = Convert.ToInt32(Emp_id);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO MMIS_ACC_EMP(");
            strSql.Append("ACC_ID,EMP_ID,ROLE,DEPARTMENT)");
            strSql.Append(" values (");
            strSql.Append(":acc_id,:emp_id,:role,:department)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":acc_id", OleDbType.Numeric,5),
                    new OleDbParameter(":emp_id", OleDbType.Numeric,5),
                    new OleDbParameter(":role", OleDbType.VarChar,5),
                    new OleDbParameter(":department", OleDbType.VarChar,10)};
            parameters[0].Value = acc_id;
            parameters[1].Value = emp_id;
            parameters[2].Value = role;
            parameters[3].Value = department;
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
        public bool updateAccEmp(string Acc_id, string Emp_id, string role, string department)
        {
            int? acc_id = Convert.ToInt32(Acc_id);
            int? emp_id = Convert.ToInt32(Emp_id);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_ACC_EMP SET ");
            strSql.Append("ROLE = :role,");
            strSql.Append("DEPARTMENT = :department");
            strSql.Append(" WHERE ACC_ID = :acc_id");
            strSql.Append(" AND EMP_ID = :emp_id");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":role", OleDbType.VarChar,10),
                    new OleDbParameter(":department", OleDbType.VarChar,5),
                    new OleDbParameter(":acc_id", OleDbType.Numeric,5),
                    new OleDbParameter(":emp_id", OleDbType.Numeric,5)};
            parameters[0].Value = role;
            parameters[1].Value = department;
            parameters[2].Value = acc_id;
            parameters[3].Value = emp_id;
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
        public bool updatePubEmp(string code, string name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE PUB_EMP SET ");
            strSql.Append("NAME = :name");
            strSql.Append(" WHERE CODE = :code");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":name", OleDbType.VarChar,10),
                    new OleDbParameter(":code", OleDbType.VarChar,5)};
            parameters[0].Value = name;
            parameters[1].Value = code;
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
        public bool deletePubEmp(string code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM PUB_EMP");
            strSql.Append(" WHERE CODE = :code");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":code", OleDbType.VarChar,10)};
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
        public bool deleteAccEmp(string Acc_id,string Emp_id)
        {
            int? acc_id = Convert.ToInt32(Acc_id);
            int? emp_id = Convert.ToInt32(Emp_id);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM MMIS_ACC_EMP");
            strSql.Append(" WHERE ACC_ID = :acc_id");
            strSql.Append(" AND EMP_ID = :emp_id");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":acc_id", OleDbType.Numeric,10),
                    new OleDbParameter(":emp_id", OleDbType.Numeric,10)};
            parameters[0].Value = acc_id;
            parameters[1].Value = emp_id;
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
