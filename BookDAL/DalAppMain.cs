using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.OleDb;

namespace BookDAL
{
    public class DalAppMain
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        public DataTable GetBAppMainListShow(decimal storageID, decimal accountID, string state,
                                                    DateTime? begTime, DateTime? endTime)
        {
            string whereClause = string.Empty;
            if (storageID != null)
            {
                whereClause += " AND A.STO_ID = "+storageID+"";
            }
            if (accountID != null)
            {
                whereClause += " AND A.ACC_ID = "+accountID+"";
            }
            if (!string.IsNullOrEmpty(state))
            {
                whereClause += " AND A.STATE = "+state+""; 
            }
            if (begTime != null)
            {
                string begID = begTime.Value.ToString("yyyyMMdd") + "00000";
                whereClause += " AND A.ID >= "+begID+"";
            }
            if (endTime != null)
            {
                string endID = endTime.Value.ToString("yyyyMMdd") + "99999";
                whereClause += " AND A.ID <= "+endID+"";
            }
            string selectCheckedBAppMainListSql = string.Format(@"SELECT 
                                                                    A.ID, 
                                                                    A.NO, 
                                                                    B.NAME ACCOUNT_TYPE_NAME,
                                                                    A.ACC_ID,
                                                                    A.TARGET_ID,
                                                                    A.REMARK,
                                                                    A.INPUT_EMP,
                                                                    A.INPUT_TIME,
                                                                    A.CHECK_EMP,
                                                                    A.CHECK_TIME
                                                                   FROM MMIS_BAPP_MAIN A,MMIS_ACCOUNT_TYPE B 
                                                                   WHERE A.ACC_ID = B.ID {0}", whereClause);
            DataTable dt = sqlDBHelper.GetTable(selectCheckedBAppMainListSql);
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
                                                            ROUND(C.BUYING_PRICE*D.RATIO/C.RATIO,4) AS BUYING_PRICE_SHOW,
                                                            ROUND(C.RETAIL_PRICE*D.RATIO/C.RATIO,4) AS RETAIL_PRICE_SHOW, 
                                                            ROUND(A.APP_NUM*C.BUYING_PRICE/C.RATIO,4) AS BUYING_PRICE_TOTAL,                                             
                                                            ROUND(A.APP_NUM*C.RETAIL_PRICE/C.RATIO,4) AS RETAIL_PRICE_TOTAL,                                                         
                                                            A.REMARK,
                                                            A.RETURN_REASON,
                                                            D.UNIT,
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
        //获取当日最大单据号
        public decimal? GetMaxID(string RQ)
        {
            string sql = @"select max(ID) from MMIS_BAPP_MAIN WHERE ID  LIKE '%' || '" + RQ + "' || '%'";
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
        public bool insertBappMain(decimal? id, decimal sto_id, decimal target_id, decimal acc_id,decimal input_emp,string remark)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO MMIS_BAPP_MAIN(");
            strSql.Append("ID,STO_ID,TARGET_ID,ACC_ID,NO,INPUT_EMP,INPUT_TIME,STATE,REMARK)");
            strSql.Append(" values (");
            strSql.Append(":id,:sto_id,:tarcet_id,:acc_id,:no,:input_emp,:input_time,:state,:remark)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,13),
                    new OleDbParameter(":sto_id", OleDbType.Numeric,5),
                    new OleDbParameter(":target_id",OleDbType.Numeric,5),
					new OleDbParameter(":acc_id", OleDbType.Numeric,3),
					new OleDbParameter(":no", OleDbType.VarChar,16),
                    new OleDbParameter(":input_emp", OleDbType.Numeric,5),
                    new OleDbParameter(":input_time", OleDbType.Date),
                    new OleDbParameter(":state", OleDbType.Char,1),
                    new OleDbParameter(":remark", OleDbType.VarChar,128)};
            parameters[0].Value = id;
            parameters[1].Value = sto_id;
            parameters[2].Value = target_id;
            parameters[3].Value = acc_id;
            parameters[4].Value = id;
            parameters[5].Value = input_emp;
            parameters[6].Value = System.DateTime.Now;
            parameters[7].Value = '0';
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
        public bool updateBappMain(decimal id,decimal app_num)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_BAPP_DETAIL SET");
            strSql.Append(" APP_NUM = :app_num");
            strSql.Append(" WHERE ID = :ID");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":app_num",OleDbType.Numeric,3),
                    new OleDbParameter(":ID", OleDbType.Numeric,13)};
            parameters[0].Value = app_num;
            parameters[1].Value = id;
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
        public bool deleteBappMain(decimal id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM MMIS_BAPP_MAIN");
            strSql.Append(" WHERE ID = :id");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,13)};
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
        public bool insertBappDetail(decimal id,decimal seq,decimal mat_id,decimal app_num,decimal send_num,string remark,decimal mat_seq)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO MMIS_BAPP_DETAIL(");
            strSql.Append("ID,SEQ,MAT_ID,APP_NUM,SEND_NUM,REMARK,MAT_SEQ)");
            strSql.Append(" values (");
            strSql.Append(":id,:seq,:mat_id,:app_num,:send_num,:remark,:mat_seq)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id",OleDbType.Numeric,13),
                    new OleDbParameter(":seq", OleDbType.Numeric,5),
                    new OleDbParameter(":mat_id",OleDbType.Numeric,5),
                    new OleDbParameter(":app_num",OleDbType.Numeric,10),
                    new OleDbParameter(":send_num",OleDbType.Numeric,10),
                    new OleDbParameter(":remark",OleDbType.VarChar,128),
                    new OleDbParameter(":mat_seq",OleDbType.Numeric,5)
                                          };
            parameters[0].Value = id;
            parameters[1].Value = seq;
            parameters[2].Value = mat_id;
            parameters[3].Value = app_num;
            parameters[4].Value = send_num;
            parameters[5].Value = remark;
            parameters[6].Value = mat_seq;
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
        public bool deleteBappDetail(decimal id, decimal seq)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM MMIS_BAPP_DETAIL");
            strSql.Append(" WHERE ID = :id");
            strSql.Append(" AND SEQ = :seq");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id",OleDbType.Numeric,13),
                    new OleDbParameter(":seq", OleDbType.Numeric,5)};
            parameters[0].Value = id;
            parameters[1].Value = seq;
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
        public bool updateBappMain(decimal id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_BAPP_MAIN SET ");
            strSql.Append("STATE = '1' ");
            strSql.Append(" WHERE ID = :id");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id",OleDbType.Numeric,13)};
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
