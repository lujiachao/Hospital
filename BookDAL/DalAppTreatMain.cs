using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.OleDb;

namespace BookDAL
{
    public class DalAppTreatMain
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        public DataTable GetBAppTreatMainListShow(decimal accountID,
                                                           decimal storageID,
                                                           string state,
                                                           DateTime? begTime,
                                                           DateTime? endTime)
        {
            string whereClause = string.Empty;
            if (storageID != null)
            {
                whereClause += " AND A.TARGET_ID = "+storageID+"";
            }
            if (accountID != null)
            {
                whereClause += " AND A.ACC_ID = "+accountID+"";
            }
            if (!string.IsNullOrEmpty(state))
            { 
                whereClause += " AND A.STATE ="+state+"";            
            }
            if (begTime != null && begTime.ToString() != "0001/1/1 0:00:00")
            {
                string beg = begTime.Value.ToString("yyyy/MM/dd");
                whereClause += " AND A.INPUT_TIME >= TO_DATE('"+beg+"','yyyy/MM/dd')";
            }
            if (endTime != null && endTime.ToString() != "0001/1/1 0:00:00")
            {
                string end = endTime.Value.ToString("yyyy/MM/dd");
                whereClause += " AND A.INPUT_TIME <= TO_DATE('" + end + "','yyyy/MM/dd')";
            }
            string selectUnCheckedBAppTreatMainSql = string.Format
                 (@"SELECT A.ID,A.NO,B.NAME AS STO_NAME,D.NAME AS INPUT_NAME,
               A.CHECK_TIME,
                A.ACC_ID,C.NAME AS ACC_NAME,A.REMARK,A.STO_ID,A.TARGET_ID,
                A.INPUT_EMP,A.INPUT_TIME,A.CHECK_EMP,A.STATE, A.SEND_EMP 
                FROM MMIS_BAPP_MAIN A,MMIS_STORAGE B,MMIS_ACCOUNT_TYPE C,PUB_EMP D
                WHERE A.STO_ID=B.ID(+) 
                AND A.INPUT_EMP=D.ID(+)
                AND A.ACC_ID=C.ID(+)
                {0} ", whereClause);
            DataTable dt = sqlDBHelper.GetTable(selectUnCheckedBAppTreatMainSql);
            return dt; 
        }
        /// <summary>
        /// 根据申领编号获取科室申领未申领处理的明细单列表(目前该服务不支持通用目录申领)
        /// </summary>
        /// <param name="bAppID">申领编号</param>
        /// <returns>申领明细单列表</returns>
        public DataTable GetBAppDetailListShow(decimal bAppID)
        {
            string selectUnCheckedBAppDetailListSql = @"SELECT
                                                           A.SEQ,
                                                           A.CAT_ID,
                                                           A.MAT_ID,
                                                           A.MAT_SEQ,
                                                           C.NAME,
                                                           C.SPEC,
                                                           E.NAME FACTORY_NAME,
                                                           ROUND(A.APP_NUM/D.RATIO,2) AS APP_NUM,
                                                           C.BUYING_PRICE,
                                                           C.RETAIL_PRICE,
                                                            C.TRADE_PRICE,
                                                            ROUND(C.BUYING_PRICE*D.RATIO/C.RATIO,4) AS BUYING_PRICE_SHOW,
                                                            ROUND(C.RETAIL_PRICE*D.RATIO/C.RATIO,4) AS RETAIL_PRICE_SHOW, 
                                                            ROUND(A.APP_NUM*C.BUYING_PRICE/C.RATIO,4) AS BUYING_PRICE_TOTAL,                                             
                                                            ROUND(A.APP_NUM*C.RETAIL_PRICE/C.RATIO,4) AS RETAIL_PRICE_TOTAL,                                                         
                                                            A.REMARK,
                                                            A.RETURN_REASON,
                                                            D.UNIT,
                                                            A.SEND_NUM,
                                                            D.RATIO AS RATIO_SELF,
                                                            (SELECT RATIO FROM MMIS_STORAGE_MATERIAL WHERE STO_ID=B.TARGET_ID
                                                            AND ACC_ID=B.ACC_ID AND MAT_ID=A.MAT_ID) AS RATIO_TARGET,
                                                            C.RATIO AS RATIO_BUYING,
                                                            (SELECT ROUND(SUM(CUR_NUM-LOCK_NUM)/D.RATIO,2) FROM MMIS_STOCK WHERE STO_ID=B.TARGET_ID AND ACC_ID=B.ACC_ID AND MAT_ID=A.MAT_ID) AS STOCKS
                                                        FROM MMIS_BAPP_DETAIL A,MMIS_BAPP_MAIN B,
                                                             MMIS_MATERIAL C,MMIS_STORAGE_MATERIAL D,
                                                            MMIS_FACTORY E
                                                        WHERE A.ID=B.ID(+)
                                                        AND A.MAT_ID=C.ID(+)
                                                        AND C.FACTORY=E.ID(+)
                                                        AND D.STO_ID=B.STO_ID
                                                        AND D.ACC_ID=B.ACC_ID
                                                        AND D.MAT_ID=A.MAT_ID
                                                        AND A.ID=:bAppID 
                                                        AND C.STATE='1'
                                                        ORDER BY A.SEQ";
            OleDbParameter[] parameters = {
                    new OleDbParameter(":bAppID", OleDbType.Numeric,13)};
            parameters[0].Value = bAppID;
            DataTable dt = sqlDBHelper.Query(selectUnCheckedBAppDetailListSql, parameters);
            return dt;
        }
        public bool updateBappMain(decimal bAppID,decimal seq,decimal send_num,string remark)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_BAPP_DETAIL SET");
            strSql.Append(" SEND_NUM = :send_num,");
            strSql.Append(" REMARK = :remark");
            strSql.Append(" WHERE ID = :ID");
            strSql.Append(" AND SEQ = :seq");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":send_num", OleDbType.Numeric,13),
                    new OleDbParameter(":remark", OleDbType.VarChar,128),
                    new OleDbParameter(":ID", OleDbType.Numeric,13),
                    new OleDbParameter(":seq", OleDbType.Numeric,5)};
            parameters[0].Value = send_num;
            parameters[1].Value = remark;
            parameters[2].Value = bAppID;
            parameters[3].Value = seq;
           
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
        public bool updateBappState(decimal bAppID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_BAPP_MAIN SET");
            strSql.Append(" STATE = '5'");
            strSql.Append(" WHERE ID = :ID");
            OleDbParameter[] parameters ={
                        new OleDbParameter(":ID", OleDbType.Numeric,13)
                                         };
            parameters[0].Value = bAppID;

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
        public bool BappOut(decimal bAppID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_BAPP_MAIN SET");
            strSql.Append(" STATE = '3'");
            strSql.Append(" WHERE ID = :ID");
            OleDbParameter[] parameters ={
                        new OleDbParameter(":ID", OleDbType.Numeric,13)
                                         };
            parameters[0].Value = bAppID;
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
        //获取当日最大单据号
        public decimal? GetMaxID(string RQ)
        {
            string sql = @"select max(ID) from MMIS_BOUT_TEMP_MAIN WHERE ID  LIKE '%' || '" + RQ + "' || '%'";
            object obj = sqlDBHelper.GetScalar(sql);
            decimal? maxID;
            if (obj.Equals(DBNull.Value))
            {
                maxID = 0;
            }
            else
            {
                maxID = Convert.ToDecimal(obj);
            }
            decimal? rq;
            if (maxID == 0)
            {
                rq = Convert.ToDecimal(RQ + "00001");
            }
            else
            {
                rq = maxID + 1;
            }
            return rq;
        }
        public bool insertBOutTempMain(decimal? id, decimal sto_id, decimal acc_id, string no, decimal outtype, decimal outTarget, DateTime? intime, decimal app_emp, DateTime? app_time, decimal input_emp, DateTime input_time, char state, string remark, char acc_bill_state)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO MMIS_BOUT_TEMP_MAIN(");
            strSql.Append("ID,STO_ID,ACC_ID,NO,CLASS,INTIME,TARGET_ID,APP_EMP,APP_TIME,INPUT_EMP,INPUT_TIME,STATE,REMARK,ACC_BILL_STATE)");
            strSql.Append(" values (");
            strSql.Append(":id,:sto_id,:acc_id,:no,:class,:intime,:target_id,:app_emp,:app_time,:input_emp,:input_time,:state,:remark,:acc_bill_state)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,13),
                    new OleDbParameter(":sto_id", OleDbType.Numeric,5),
					new OleDbParameter(":acc_id", OleDbType.Numeric,3),
					new OleDbParameter(":no", OleDbType.VarChar,16),
					new OleDbParameter(":class", OleDbType.Numeric,3),
					new OleDbParameter(":intime", OleDbType.Date), 
                    new OleDbParameter(":target_id",OleDbType.Numeric,5),
                    new OleDbParameter(":app_emp", OleDbType.Numeric,5),
                    new OleDbParameter(":app_time", OleDbType.Date),
                    new OleDbParameter(":input_emp", OleDbType.Numeric,5),
                    new OleDbParameter(":input_time", OleDbType.Date),
                    new OleDbParameter(":state", OleDbType.Char,1),
                    new OleDbParameter(":remark", OleDbType.VarChar,128),
                    new OleDbParameter(":acc_bill_state", OleDbType.Char,2)};
            parameters[0].Value = id;
            parameters[1].Value = sto_id;
            parameters[2].Value = acc_id;
            parameters[3].Value = no;
            parameters[4].Value = outtype;
            parameters[5].Value = intime;
            parameters[6].Value = outTarget;
            parameters[7].Value = app_emp;
            parameters[8].Value = app_time;
            parameters[9].Value = input_emp;
            parameters[10].Value = input_time;
            parameters[11].Value = state;
            parameters[12].Value = remark;
            parameters[13].Value = acc_bill_state;
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
        public bool insertBOutTempDetail(decimal? id, decimal seq,decimal mat_id,decimal mat_seq,decimal nums,decimal buying_price,decimal retail_price,decimal trade_price,string batch_no,string remark)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO MMIS_BOUT_TEMP_DETAIL(");
            strSql.Append("ID,SEQ,MAT_ID,MAT_SEQ,NUMS,BUYING_PRICE,RETAIL_PRICE,TRADE_PRICE,BATCH_NO,REMARK)");
            strSql.Append(" values (");
            strSql.Append(":id,:seq,:mat_id,:mat_seq,:nums,:buying_price,:retail_price,:trade_price,:batch_no,:remark)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,13),
                    new OleDbParameter(":seq", OleDbType.Numeric,5),
					new OleDbParameter(":mat_id", OleDbType.Numeric,8),
					new OleDbParameter(":mat_seq", OleDbType.Numeric,5),
					new OleDbParameter(":nums", OleDbType.Numeric,10),
					new OleDbParameter(":buying_price", OleDbType.Numeric,12), 
                    new OleDbParameter(":retail_price",OleDbType.Numeric,12),
                    new OleDbParameter(":trade_price", OleDbType.Numeric,12),
                    new OleDbParameter(":batch_no", OleDbType.VarChar,32),
                    new OleDbParameter(":remark", OleDbType.VarChar,128)};
            parameters[0].Value = id;
            parameters[1].Value = seq;
            parameters[2].Value = mat_id;
            parameters[3].Value = mat_seq;
            parameters[4].Value = nums;
            parameters[5].Value = buying_price;
            parameters[6].Value = retail_price;
            parameters[7].Value = trade_price;
            parameters[8].Value = batch_no;
            parameters[9].Value = remark;
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
