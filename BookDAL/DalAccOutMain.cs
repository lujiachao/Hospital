using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.OleDb;

namespace BookDAL
{
    public class DalAccOutMain
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        public char getTarget(decimal ID)
        {
            string sql = @"SELECT TARGET FROM MMIS_BOUT_TYPE WHERE ID = " + ID + "";
            char target = Convert.ToChar(sqlDBHelper.GetScalar(sql));
            return target;
        }
        //主单查询
        public DataTable GetCheckedBillOutMainListOutCommShow(decimal storageID, decimal? accountID, decimal? bOutTypeID, DateTime? begTime, DateTime? endTime, string matState, decimal? targetID, char TargetType, char DISPATCHING)
        {
            string whereClause = string.Empty;
            if (storageID != 0)
            {
                whereClause += " AND A.STO_ID=" + storageID + "";
            }
            if (accountID != null)
            {
                whereClause += " AND A.ACC_ID=" + accountID + "";
            }
            if (bOutTypeID != null)
            {
                whereClause += " AND A.CLASS=" + bOutTypeID + "";
            }
            if (matState != null && matState != string.Empty)
            {
                whereClause += " AND A.ACC_BILL_STATE = " + matState + "";
            }
            if (targetID != null && targetID != 0)
            {
                whereClause += " AND A.TARGET_ID=" + targetID + "";
            }
            if (TargetType != null)
            {
                whereClause += " AND C.TARGET = " + TargetType + "";
            }
            if (DISPATCHING != null)
            {
                whereClause += " AND C.DISPATCHING = " + DISPATCHING + "";
            }
            if (begTime != null && begTime.ToString() != "0001/1/1 0:00:00")
            {
                decimal begID = decimal.Parse(begTime.Value.ToString("yyyyMMdd") + "00000");
                whereClause += " AND A.ID > " + begID + "";
            }
            if (endTime != null && endTime.ToString() != "0001/1/1 0:00:00")
            {
                decimal endID = decimal.Parse(endTime.Value.ToString("yyyyMMdd") + "99999");
                whereClause += " AND A.ID <= " + endID + "";
            }
            string selectBillOutTempMainSql = string.Format(@"SELECT DISTINCT A.ID,A.STO_ID,A.ACC_ID,A.NO,C.NAME,A.STATE，A.INTIME,A.APP_EMP,A.APP_TIME,A.REMARK,A.INPUT_EMP,A.INPUT_TIME,A.ACC_BILL_STATE,A.CHECK_EMP,A.CHECK_TIME,
                                                              (CASE WHEN C.TARGET = '2' THEN
                                                              (SELECT SHORT_NAME FROM MMIS_COMPANY WHERE ID = A.TARGET_ID) 
                                                              ELSE (SELECT NAME FROM MMIS_STORAGE WHERE ID = A.TARGET_ID) END) AS TARGET
                                                              FROM MMIS_BOUT_TEMP_MAIN A,MMIS_BOUT_TYPE C
                                                              WHERE  A.CLASS = C.ID AND (A.STATE = '0' OR A.STATE ='3')
                                                              {0} ORDER BY A.ID DESC", whereClause);
            DataTable dt = sqlDBHelper.GetTable(selectBillOutTempMainSql);
            return dt;
        }
        //获取出库正式单据组合的明细单信息
        public DataTable GetCheckedBillOutDetailListShow(decimal bOutDetailID)
        {
            string selectBillOutDetailSql = string.Format(@"SELECT  
                                            A.SEQ,
                                            A.MAT_SEQ,
                                            A.MAT_ID MAT_ID,
                                            C.NAME,
                                            C.SPEC,
                                            C.FACTORY,
                                            E.NAME FACTORY_NAME,
                                            C.NAME || '/' || C.SPEC || '/' || E.NAME NAME_SPEC_FACTORY,
                                            ROUND(A.NUMS/D.RATIO,2) AS NUMS,
                                            A.BUYING_PRICE,
                                            A.RETAIL_PRICE,
                                            A.TRADE_PRICE,
                                            ROUND(A.BUYING_PRICE*D.RATIO/C.RATIO,4) AS BUYING_PRICE_SHOW,
                                            ROUND(A.RETAIL_PRICE*D.RATIO/C.RATIO,4) AS RETAIL_PRICE_SHOW,
                                            ROUND(A.TRADE_PRICE*D.RATIO/C.RATIO,4) AS TRADE_PRICE_SHOW,
                                            ROUND(A.NUMS*A.RETAIL_PRICE/C.RATIO,4) AS RETAIL_PRICE_TOTAL,
                                            ROUND(A.NUMS*A.BUYING_PRICE/C.RATIO,4) AS BUYING_PRICE_TOTAL,
                                            A.RETURN_REASON,
                                            A.BATCH_NO,
                                            A.EXPIRY_DATE,
                                            A.REMARK,
                                            D.UNIT,
                                            D.RATIO AS RATIO_SELF,
                                            (SELECT RATIO FROM MMIS_STORAGE_MATERIAL WHERE STO_ID=B.TARGET_ID
                                            AND ACC_ID=B.ACC_ID AND MAT_ID=A.MAT_ID) AS RATIO_TARGET,
                                            C.RATIO AS RATIO_BUYING
                                            FROM MMIS_BOUT_TEMP_DETAIL A,MMIS_BOUT_TEMP_MAIN B,MMIS_MATERIAl C,MMIS_STORAGE_MATERIAL D,MMIS_FACTORY E           
                                            WHERE A.ID=B.ID(+)
                                            AND A.MAT_ID=C.ID(+)
                                            AND C.FACTORY=E.ID(+)
                                            AND D.STO_ID=B.STO_ID
                                            AND D.ACC_ID=B.ACC_ID
                                            AND D.MAT_ID=A.MAT_ID
                                            AND A.ID=:id ORDER BY SEQ");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,15)};
            parameters[0].Value = bOutDetailID;
            DataTable dt = sqlDBHelper.Query(selectBillOutDetailSql, parameters);
            return dt;
        }
        public bool updateBoutTempMain(decimal bOutDetailID,decimal checkEmp,char acc_bill_state)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_BOUT_TEMP_MAIN SET ");
            strSql.Append("ACC_BILL_STATE = :acc_bill_state,");
            strSql.Append("CHECK_EMP = :checkEmp,");
            strSql.Append("CHECK_TIME = SYSDATE");
            strSql.Append(" WHERE ID = :id");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":acc_bill_state",OleDbType.Char,5),
                    new OleDbParameter(":checkEmp", OleDbType.Numeric,12),
                    new OleDbParameter(":id", OleDbType.Numeric,16)};
            parameters[0].Value = acc_bill_state;
            parameters[1].Value = checkEmp;
            parameters[2].Value = bOutDetailID;
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
        public bool updateStock(decimal NUMS, decimal Sto_ID, decimal Acc_ID, decimal Mat_ID, decimal Mat_Seq)
        {
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("UPDATE MMIS_STOCK SET ");
            strSql1.Append("CUR_NUM = CUR_NUM - :nums");
            strSql1.Append(" WHERE STO_ID = :sto_id");
            strSql1.Append(" AND ACC_ID = :acc_id");
            strSql1.Append(" AND MAT_ID = :mat_id");
            strSql1.Append(" AND MAT_SEQ = :mat_seq");
            OleDbParameter[] parameters1 = {
                    new OleDbParameter(":nums", OleDbType.Numeric,5),
                    new OleDbParameter(":sto_id", OleDbType.Numeric,5),
                    new OleDbParameter(":acc_id", OleDbType.Numeric,3),
                    new OleDbParameter(":mat_id", OleDbType.Numeric,8),
                    new OleDbParameter(":mat_seq", OleDbType.Numeric,5)};
            parameters1[0].Value = NUMS;
            parameters1[1].Value = Sto_ID;
            parameters1[2].Value = Acc_ID;
            parameters1[3].Value = Mat_ID;
            parameters1[4].Value = Mat_Seq;
            int rows1 = SqlDBHelper.ExecuteSql(strSql1.ToString(), parameters1);
            if (rows1 > 0)
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
