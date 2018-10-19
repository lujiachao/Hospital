using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.OleDb;

namespace BookDAL
{
    public class DalStorage
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        public DataTable DalGetStorage()
        {
            string sql = @"SELECT * FROM MMIS_STORAGE order by code";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        public DataTable DalAccount(string code)
        {
            string sql = string.Format(@"SELECT  A.CODE,A.NAME,(CASE WHEN A.STATE = '0' THEN '在用' ELSE '停用' END) STATE,
                           (CASE WHEN A.PRICE_RULE = 1 THEN '统一计价' ELSE '不统一计价' END) PRICE_RULE,
                            A.INPUTCODE1,A.INPUTCODE2
                            FROM MMIS_ACCOUNT_TYPE A,MMIS_STORAGE_RELATION B 
                            WHERE A.CODE = B.ACC_ID AND B.STO_ID = {0}
                            UNION 
                            SELECT  A.CODE,A.NAME,(CASE WHEN A.STATE = '0' THEN '在用' ELSE '停用' END) STATE,
                            (CASE WHEN A.PRICE_RULE = 1 THEN '统一计价' ELSE '不统一计价' END) PRICE_RULE,
                            A.INPUTCODE1,A.INPUTCODE2
                            FROM MMIS_ACCOUNT_TYPE A,MMIS_STORAGE_RELATION B 
                            WHERE A.CODE = B.ACC_ID AND B.TO_ID = {0}",code);
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        //获取当前账册下该仓库的上级库
        public DataTable DalTopStorage(string to_id, string acc_id)
        {
            string sql = string.Format(@"SELECT A.CODE,A.NAME,
                                         (CASE WHEN A.GRADE = '1' THEN '总库' WHEN A.GRADE = '2' THEN '流通库' ELSE '在用库' END) AS GRADE,
                                         (CASE WHEN A.TYPE = '1' THEN '独立仓库' WHEN A.TYPE = '2' THEN '科室仓库' WHEN A.TYPE = '3' THEN '病区仓库' WHEN A.TYPE = '4' THEN '个人仓库' WHEN A.TYPE = '5' THEN '供应仓库' ELSE '制剂仓库' END) AS TYPE,
                                         A.TOLINK,
                                         (CASE WHEN A.STATE = '0' THEN '停用' ELSE '在用' END) AS STATE
                                         FROM MMIS_STORAGE A,MMIS_STORAGE_RELATION B
                                         WHERE B.ACC_ID = {0}
                                         AND B.TO_ID = {1}
                                         AND A.CODE = B.STO_ID",acc_id,to_id);
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        //获取当前账册下该仓库的下级库
        public DataTable DalDownStorage(string sto_id, string acc_id)
        {
            string sql = string.Format(@"SELECT A.CODE,A.NAME,
                                         (CASE WHEN A.GRADE = '1' THEN '总库' WHEN A.GRADE = '2' THEN '流通库' ELSE '在用库' END) AS GRADE,
                                         (CASE WHEN A.TYPE = '1' THEN '独立仓库' WHEN A.TYPE = '2' THEN '科室仓库' WHEN A.TYPE = '3' THEN '病区仓库' WHEN A.TYPE = '4' THEN '个人仓库' WHEN A.TYPE = '5' THEN '供应仓库' ELSE '制剂仓库' END) AS TYPE,
                                         A.TOLINK,
                                         (CASE WHEN A.STATE = '0' THEN '停用' ELSE '在用' END) AS STATE
                                         FROM MMIS_STORAGE A,MMIS_STORAGE_RELATION B
                                         WHERE B.ACC_ID = {0}
                                         AND B.STO_ID = {1}
                                         AND A.CODE = B.TO_ID", acc_id, sto_id);
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        public DataTable DalEmp(string sto_id)
        {
            string sql = string.Format(@"SELECT B.CODE,B.NAME
                                         FROM MMIS_STORAGE_EMP A,PUB_EMP B
                                         WHERE A.EMP_ID = B.CODE
                                         AND A.STO_ID = {0}",sto_id);
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        public bool insertStorage(string code,string name,string grade,string type,string tolink,string inputcode1,string inputcode2,string state,string remark)
        {
            StringBuilder strSql = new StringBuilder();
            char Grade = Convert.ToChar(grade);
            char Type = Convert.ToChar(type);
            char State = Convert.ToChar(state);
            strSql.Append("INSERT INTO MMIS_STORAGE(");
            strSql.Append("CODE,NAME,GRADE,TYPE,TOLINK,INPUTCODE1,INPUTCODE2,STATE,REMARK)");
            strSql.Append(" values (");
            strSql.Append(":code,:name,:grade,:type,:tolink,:inputcode1,:inputcode2,:state,:remark)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":code", OleDbType.VarChar,5),
					new OleDbParameter(":name", OleDbType.VarChar,32),
					new OleDbParameter(":grade", OleDbType.Char,2),
					new OleDbParameter(":type", OleDbType.Char,1),
					new OleDbParameter(":tolink", OleDbType.VarChar,16),
					new OleDbParameter(":inputcode1", OleDbType.VarChar,8),
					new OleDbParameter(":inputcode2", OleDbType.VarChar,8),
					new OleDbParameter(":state", OleDbType.Char,1),
                    new OleDbParameter(":remark", OleDbType.VarChar,128)};
            parameters[0].Value = code;
            parameters[1].Value = name;
            parameters[2].Value = Grade;
            parameters[3].Value = Type;
            parameters[4].Value = tolink;
            parameters[5].Value = inputcode1;
            parameters[6].Value = inputcode2;
            parameters[7].Value = State;
            parameters[8].Value = remark;
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
        public bool DalCheckCode(string CODE)
        {
            string sql = @"SELECT COUNT(*) FROM MMIS_STORAGE WHERE CODE = '" + CODE + "'";
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
        public bool updateStorage(string code, string name, string grade, string type, string tolink, string inputcode1, string inputcode2, string state, string remark)
        {
            StringBuilder strSql = new StringBuilder();
            char Grade = Convert.ToChar(grade);
            char Type = Convert.ToChar(type);
            char State = Convert.ToChar(state);
            strSql.Append("UPDATE MMIS_STORAGE SET ");
            strSql.Append("NAME = :name,");
            strSql.Append("GRADE = :grade,");
            strSql.Append("TYPE = :type,");
            strSql.Append("TOLINK = :tolink,");
            strSql.Append("INPUTCODE1 = :inputcode1,");
            strSql.Append("INPUTCODE2 = :inputcode2,");
            strSql.Append("STATE = :state,");
            strSql.Append("REMARK = :remark");
            strSql.Append(" WHERE CODE = :code");
            OleDbParameter[] parameters = {
					new OleDbParameter(":name", OleDbType.VarChar,32),
					new OleDbParameter(":grade", OleDbType.Char,2),
					new OleDbParameter(":type", OleDbType.Char,1),
					new OleDbParameter(":tolink", OleDbType.VarChar,16),
					new OleDbParameter(":inputcode1", OleDbType.VarChar,8),
					new OleDbParameter(":inputcode2", OleDbType.VarChar,8),
					new OleDbParameter(":state", OleDbType.Char,1),
                    new OleDbParameter(":remark", OleDbType.VarChar,128),
                    new OleDbParameter(":code", OleDbType.VarChar,5)};
            parameters[0].Value = name;
            parameters[1].Value = Grade;
            parameters[2].Value = Type;
            parameters[3].Value = tolink;
            parameters[4].Value = inputcode1;
            parameters[5].Value = inputcode2;
            parameters[6].Value = State;
            parameters[7].Value = remark;
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
        public bool deleteStorage(string code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM MMIS_STORAGE");
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
        public bool deleteStorageEmp(string sto_id, string emp_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM MMIS_STORAGE_EMP");
            strSql.Append(" WHERE STO_ID = :sto_id");
            strSql.Append(" AND EMP_ID = :emp_id");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":sto_id", OleDbType.VarChar,10),
                    new OleDbParameter(":emp_id", OleDbType.VarChar,10)};
            parameters[0].Value = sto_id;
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
        public bool insertStorageEmp(string sto_id, string emp_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO MMIS_STORAGE_EMP(");
            strSql.Append(" STO_ID,EMP_ID)");
            strSql.Append(" values (");
            strSql.Append(":sto_id,:emp_id)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":sto_id", OleDbType.VarChar,10),
                    new OleDbParameter(":emp_id", OleDbType.VarChar,10)};
            parameters[0].Value = sto_id;
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
        public bool DalRelation(string sto_id,string to_id,string acc_id)
        {
            string sql = @"SELECT COUNT(*) FROM MMIS_STORAGE_RELATION WHERE STO_ID = '" + sto_id + "' AND TO_ID = '"+to_id+"' AND ACC_ID = '"+acc_id+"'";
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
        public bool insertRelation(string sto_id, string to_id, string acc_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO MMIS_STORAGE_RELATION(");

            strSql.Append(" STO_ID,TO_ID,ACC_ID)");
            strSql.Append(" values (");
            strSql.Append(":sto_id,:to_id,:acc_id)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":sto_id", OleDbType.VarChar,10),
                    new OleDbParameter(":to_id", OleDbType.VarChar,10),
                    new OleDbParameter(":acc_id", OleDbType.VarChar,10)};
            parameters[0].Value = sto_id;
            parameters[1].Value = to_id;
            parameters[2].Value = acc_id;
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
        public bool deleteRelation(string sto_id, string to_id, string acc_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM MMIS_STORAGE_RELATION");
            strSql.Append(" WHERE STO_ID = :sto_id");
            strSql.Append(" AND TO_ID = :to_id");
            strSql.Append(" AND ACC_ID = :acc_id");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":sto_id", OleDbType.VarChar,10),
                    new OleDbParameter(":to_id", OleDbType.VarChar,10),
                    new OleDbParameter(":acc_id", OleDbType.VarChar,10)};
            parameters[0].Value = sto_id;
            parameters[1].Value = to_id;
            parameters[2].Value = acc_id;
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
