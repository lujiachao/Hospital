using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.OleDb;

namespace BookDAL
{
    public class DalCatalogInfo
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        /// <summary>
        /// 获取物资分类、通用目录、物资明细树形表
        /// </summary>
        /// <param name="acc_ID">账册ID</param>
        /// <param name="levelState">物资树的级别 1：至物资分类，2：至通用目录 3至物资明细</param>
        /// <returns>物资分类、通用目录、物资明细树形表</returns>
        public DataTable GetMaterialTreeListInfo(decimal acc_ID, int levelState)
        {
            string selectSql = string.Empty;
            DataTable dt;
            if (acc_ID <= 0)
            {
                string levelstring = string.Empty;
                if (levelState == 2)
                {
                    //去掉 WHERE STATE = '1'
                    levelstring = @"UNION ALL SELECT ID+100000 ID,NAME,ID TAGID,TYPE PID,0 IMAGEINDEX
                                FROM KTMMIS.MMIS_CATALOG   ";
                }
                selectSql = string.Format(@"SELECT 0 ID,'全部' NAME,0 TAGID,-1 PID,0 IMAGEINDEX 
                                FROM DUAL 
                                UNION ALL SELECT ID, NAME,ID TAGID,PID,0 IMAGEINDEX 
                                FROM KTMMIS.MMIS_MAT_CLASS WHERE STATE='1' {0}", levelstring);
                dt = sqlDBHelper.GetTable(selectSql);
            }
            else
            {
                string levelstring = string.Empty;
                if (levelState == 2)
                {
                    //去掉 MMIS_CATALOG.STATE='1' AND
                    levelstring = @" UNION ALL SELECT MMIS_CATALOG.ID+100000 ID,MMIS_CATALOG.NAME,MMIS_CATALOG.ID TAGID,TYPE PID,0 IMAGEINDEX,STATE,2 LEVELSTATE
                                FROM MMIS_CATALOG WHERE  MMIS_CATALOG.TYPE IN (SELECT CLASS_ID FROM MMIS_ACC_MAT_CLASS WHERE ACC_ID=" + acc_ID + ")";
                }
                selectSql = string.Format(@"SELECT 0 ID,'全部' NAME,0 TAGID,-1 PID,0 IMAGEINDEX,'1' STATE,0 LEVELSTATE
                                FROM DUAL 
                                UNION ALL SELECT ID, NAME,ID TAGID,PID,0 IMAGEINDEX,'1' STATE,1 LEVELSTATE 
                                FROM MMIS_MAT_CLASS WHERE STATE='1' AND ID IN (SELECT CLASS_ID FROM MMIS_ACC_MAT_CLASS WHERE ACC_ID=" + acc_ID + ") {0} ORDER BY 1", levelstring);
                dt = sqlDBHelper.GetTable(selectSql);
            }
            return dt;
        }
        /// <summary>
        /// 通过通用目录号和在用状态获取对应物资列表
        /// </summary>
        /// <param name="catalogID"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public DataTable GetMaterialList(decimal catalogID)
        {
            string selectSql = string.Empty;
            DataTable dt;
            selectSql = @"SELECT * FROM MMIS_MATERIAL WHERE CAT_ID= " + catalogID + " ";
            dt = sqlDBHelper.GetTable(selectSql);
            return dt;
        }
        public bool DalCheckCode(string CODE)
        {
            string sql = @"SELECT COUNT(*) FROM MMIS_CATALOG WHERE CODE = '" + CODE + "'";
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
        public bool insertCatalog(string code, string name, string Account,string Type,string Price_rule,string App_mode,string inputcode1,string inputcode2,string State,string Cansell)
        {
            StringBuilder strSql = new StringBuilder();
            int id = Convert.ToInt32(code);
            int account = Convert.ToInt32(Account);
            int type = Convert.ToInt32(Type);
            int price_rule = Convert.ToInt32(Price_rule);
            char app_mode = Convert.ToChar(App_mode);
            char state = Convert.ToChar(State);
            char cansell = Convert.ToChar(Cansell);
            strSql.Append("INSERT INTO MMIS_CATALOG(");
            strSql.Append("ID,CODE,NAME,ACCOUNT,TYPE,PRICE_RULE,APP_MODE,INPUTCODE1,INPUTCODE2,STATE,CANSELL)");
            strSql.Append(" values (");
            strSql.Append(":id,:code,:name,:account,:type,:price_rule,:app_mode,:inputcode1,:inputcode2,:state,:cansell)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,5),
                    new OleDbParameter(":code", OleDbType.VarChar,16),
                    new OleDbParameter(":name", OleDbType.VarChar,64),
                    new OleDbParameter(":account", OleDbType.Numeric,5),
                    new OleDbParameter(":type", OleDbType.Numeric,5),
                    new OleDbParameter(":price_rule", OleDbType.Numeric,2),
                    new OleDbParameter(":app_mode", OleDbType.Char,1),
                    new OleDbParameter(":inputcode1", OleDbType.VarChar,8),
                    new OleDbParameter(":inputcode2", OleDbType.VarChar,8),
                    new OleDbParameter(":state", OleDbType.Char,1),
                    new OleDbParameter(":cansell", OleDbType.Char,1)};
            parameters[0].Value = id;
            parameters[1].Value = code;
            parameters[2].Value = name;
            parameters[3].Value = account;
            parameters[4].Value = type;
            parameters[5].Value = price_rule;
            parameters[6].Value = app_mode;
            parameters[7].Value = inputcode1;
            parameters[8].Value = inputcode2;
            parameters[9].Value = state;
            parameters[10].Value = cansell;
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
        public bool updateCatalog(string code, string name, string Account, string Type, string Price_rule, string App_mode, string inputcode1, string inputcode2, string State, string Cansell)
        {
            StringBuilder strSql = new StringBuilder();
            int id = Convert.ToInt32(code) - 100000;
            int account = Convert.ToInt32(Account);
            int type = Convert.ToInt32(Type);
            int price_rule = Convert.ToInt32(Price_rule);
            char app_mode = Convert.ToChar(App_mode);
            char state = Convert.ToChar(State);
            char cansell = Convert.ToChar(Cansell);
            strSql.Append("UPDATE MMIS_CATALOG SET ");
            strSql.Append("NAME = :name,");
            strSql.Append("ACCOUNT = :account,");
            strSql.Append("TYPE = :type,");
            strSql.Append("PRICE_RULE = :price_rule,");
            strSql.Append("APP_MODE = :app_mode,");
            strSql.Append("INPUTCODE1 = :inputcode1,");
            strSql.Append("INPUTCODE2 = :inputcode2,");
            strSql.Append("STATE = :state,");
            strSql.Append("CANSELL = :cansell");
            strSql.Append(" WHERE CODE = :id");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":name", OleDbType.VarChar,64),
                    new OleDbParameter(":account", OleDbType.Numeric,5),
                    new OleDbParameter(":type", OleDbType.Numeric,5),
                    new OleDbParameter(":price_rule", OleDbType.Numeric,2),
                    new OleDbParameter(":app_mode", OleDbType.Char,1),
                    new OleDbParameter(":inputcode1", OleDbType.VarChar,8),
                    new OleDbParameter(":inputcode2", OleDbType.VarChar,8),
                    new OleDbParameter(":state", OleDbType.Char,1),
                    new OleDbParameter(":cansell", OleDbType.Char,1),
                    new OleDbParameter(":id", OleDbType.Numeric,5)};
            parameters[0].Value = name;
            parameters[1].Value = account;
            parameters[2].Value = type;
            parameters[3].Value = price_rule;
            parameters[4].Value = app_mode;
            parameters[5].Value = inputcode1;
            parameters[6].Value = inputcode2;
            parameters[7].Value = state;
            parameters[8].Value = cansell;
            parameters[9].Value = id;
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
        public bool deleteCatalog(string code)
        {
            StringBuilder strSql = new StringBuilder();
            int id = Convert.ToInt32(code) - 100000;
            strSql.Append("DELETE FROM MMIS_CATALOG");
            strSql.Append(" WHERE ID = :id");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,5)};
            parameters[0].Value = id;
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
