using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.OleDb;

namespace BookDAL
{
    public class DalFactory
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        public DataTable DalGetAccount()
        {
            string sql = @"SELECT * FROM MMIS_ACCOUNT_TYPE order by code";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        public DataTable DalGetFactory(string Code)
        {
            string sql = @"SELECT * FROM MMIS_FACTORY WHERE ACC_ID = '"+Code+"' ORDER BY CODE";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        public bool DalCheckCode(string CODE)
        {
            string sql = @"SELECT COUNT(*) FROM MMIS_FACTORY WHERE CODE = '" + CODE + "'";
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
        public bool insertFactory(string code, string name, string shortname, string region, string legal_person, string linkman, string telephone, string address, string zipcode, string email, string licence, string Date, string state, string remark, string acc_id)
        {
            StringBuilder strSql = new StringBuilder();
            int id = Convert.ToInt32(code);
            DateTime date1;
            if (Date == "" || Date == null)
            {
                date1 = DateTime.Now;                
            }
            else
            {
                date1 = Convert.ToDateTime(Date);
            }
            if (state == "" || state == null)
            {
                state = "1";
            }
            strSql.Append("INSERT INTO MMIS_FACTORY(");
            strSql.Append("ID,CODE,NAME,SHORT_NAME,REGION,LEGAL_PERSON,LINKMEN,TELEPHONE,ADDRESS,ZIPCODE,EMAIL,LICENCE,EXPIRY_DATE,STATE,REMARK,ACC_ID)");
            strSql.Append(" values (");
            strSql.Append(":id,:code,:name,:shortname,:region,:legal_person,:linkman,:telephone,:adress,:zipcode,:email,:licence,:date1,:state,:remark,:acc_id)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,5),
                    new OleDbParameter(":code", OleDbType.VarChar,5),
                    new OleDbParameter(":name", OleDbType.VarChar,64),
                    new OleDbParameter(":shortname", OleDbType.VarChar,32),
                    new OleDbParameter(":region", OleDbType.VarChar,32),
                    new OleDbParameter(":legal_person", OleDbType.VarChar,16),
                    new OleDbParameter(":linkman", OleDbType.VarChar,16),
                    new OleDbParameter(":telephone", OleDbType.VarChar,16),
                    new OleDbParameter(":adress", OleDbType.VarChar,64),
                    new OleDbParameter(":zipcode", OleDbType.VarChar,8),
                    new OleDbParameter(":email", OleDbType.VarChar,64),
                    new OleDbParameter(":licence", OleDbType.VarChar,64),
                    new OleDbParameter(":date1", OleDbType.Date),
                    new OleDbParameter(":state", OleDbType.VarChar,1),
                    new OleDbParameter(":remark", OleDbType.VarChar,128),
                    new OleDbParameter(":acc_id", OleDbType.VarChar,5)};
            parameters[0].Value = id;
            parameters[1].Value = code;
            parameters[2].Value = name;
            parameters[3].Value = shortname;
            parameters[4].Value = region;
            parameters[5].Value = legal_person;
            parameters[6].Value = linkman;
            parameters[7].Value = telephone;
            parameters[8].Value = address;
            parameters[9].Value = zipcode;
            parameters[10].Value = email;
            parameters[11].Value = licence;
            parameters[12].Value = date1;
            parameters[13].Value = state;
            parameters[14].Value = remark;
            parameters[15].Value = acc_id;
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
        public bool updateFactory(string code, string name, string shortname, string region, string legal_person, string linkman, string telephone, string address, string zipcode, string email, string licence, string Date, string state, string remark)
        {
            DateTime date1 = Convert.ToDateTime(Date);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_FACTORY SET ");
            strSql.Append("NAME = :name,");
            strSql.Append("SHORT_NAME = :shortname,");
            strSql.Append("REGION = :region,");
            strSql.Append("LEGAL_PERSON = :legal_person,");
            strSql.Append("LINKMEN = :linkman,");
            strSql.Append("TELEPHONE = :telephone,");
            strSql.Append("ADDRESS = :address,");
            strSql.Append("ZIPCODE = :zipcode,");
            strSql.Append("EMAIL = :email,");
            strSql.Append("LICENCE = :licence,");
            strSql.Append("EXPIRY_DATE = :date1,");
            strSql.Append("STATE = :state,");
            strSql.Append("REMARK = :remark");
            strSql.Append(" WHERE CODE = :code");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":name", OleDbType.VarChar,64),
                    new OleDbParameter(":shortname", OleDbType.VarChar,32),
                    new OleDbParameter(":region", OleDbType.VarChar,32),
                    new OleDbParameter(":legal_person", OleDbType.VarChar,16),
                    new OleDbParameter(":linkman", OleDbType.VarChar,16),
                    new OleDbParameter(":telephone", OleDbType.VarChar,16),
                    new OleDbParameter(":adress", OleDbType.VarChar,64),
                    new OleDbParameter(":zipcode", OleDbType.VarChar,8),
                    new OleDbParameter(":email", OleDbType.VarChar,64),
                    new OleDbParameter(":licence", OleDbType.VarChar,64),
                    new OleDbParameter(":date1", OleDbType.Date),
                    new OleDbParameter(":state", OleDbType.VarChar,1),
                    new OleDbParameter(":remark", OleDbType.VarChar,128),
                    new OleDbParameter(":code", OleDbType.VarChar,5)};
            parameters[0].Value = name;
            parameters[1].Value = shortname;
            parameters[2].Value = region;
            parameters[3].Value = legal_person;
            parameters[4].Value = linkman;
            parameters[5].Value = telephone;
            parameters[6].Value = address;
            parameters[7].Value = zipcode;
            parameters[8].Value = email;
            parameters[9].Value = licence;
            parameters[10].Value = date1;
            parameters[11].Value = state;
            parameters[12].Value = remark;
            parameters[13].Value = code;
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
        public bool deleteFactory(string code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM MMIS_FACTORY");
            strSql.Append(" WHERE ID = :code");
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
    }
}
