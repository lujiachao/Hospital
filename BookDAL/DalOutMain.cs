using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.OleDb;

namespace BookDAL
{
    public class DalOutMain
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        public char getTarget(decimal ID)
        {
            string sql = @"SELECT TARGET FROM MMIS_BOUT_TYPE WHERE ID = "+ID+"";
            char target = Convert.ToChar(sqlDBHelper.GetScalar(sql));
            return target;
        }
        //主单查询
        public DataTable GetCheckedBillOutMainListOutCommShow(decimal storageID, decimal? accountID, decimal? bOutTypeID, DateTime? begTime, DateTime? endTime, string matState, decimal? targetID, char TargetType, char DISPATCHING)
        {
            string whereClause = string.Empty;
            if (storageID != 0)
            {
                whereClause += " AND A.STO_ID="+storageID+"";
            }
            if (accountID != null)
            {
                whereClause += " AND A.ACC_ID="+accountID+"";
            }
            if (bOutTypeID != null)
            {
                whereClause += " AND A.CLASS="+bOutTypeID+"";
            }
            if (matState != null && matState != string.Empty)
            {
                whereClause += " AND A.STATE = "+matState+"";
            }
            if (targetID != null && targetID != 0)
            {
                whereClause += " AND A.TARGET_ID="+targetID+"";
            }
            if (TargetType != null)
            {
                whereClause += " AND C.TARGET = "+TargetType+"";
            }
            if (DISPATCHING != null)
            {
                whereClause += " AND C.DISPATCHING = "+DISPATCHING+"";
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
            string selectBillOutTempMainSql = string.Format(@"SELECT DISTINCT A.ID,A.STO_ID,A.ACC_ID,A.NO,C.NAME,A.STATE，A.INTIME,A.APP_EMP,A.APP_TIME,A.REMARK,A.INPUT_EMP,A.INPUT_TIME,
                                                              (CASE WHEN C.TARGET = '2' THEN
                                                              (SELECT SHORT_NAME FROM MMIS_COMPANY WHERE ID = A.TARGET_ID) 
                                                              ELSE (SELECT NAME FROM MMIS_STORAGE WHERE ID = A.TARGET_ID) END) AS TARGET
                                                              FROM MMIS_BOUT_TEMP_MAIN A,MMIS_BOUT_TYPE C
                                                              WHERE  A.CLASS = C.ID AND ACC_BILL_STATE = '0'
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
        public string GetName(string Code)
        {
            string sql = @"SELECT NAME FROM PUB_EMP WHERE CODE= '" + Code + "'";
            string name = sqlDBHelper.GetScalar(sql).ToString();
            return name;
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
        public bool insertBoutTempDetail(decimal id, decimal seq,decimal mat_id, decimal mat_seq, decimal nums, decimal buying_price, decimal retail_price, decimal trade_price, string batch_no, DateTime expipy_date,decimal return_reason, string remark)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO MMIS_BOUT_TEMP_DETAIL(");
            strSql.Append("ID,SEQ,MAT_ID,MAT_SEQ,NUMS,BUYING_PRICE,RETAIL_PRICE,TRADE_PRICE,BATCH_NO,EXPIRY_DATE,RETURN_REASON,REMARK)");
            strSql.Append(" values (");
            strSql.Append(":id,:seq,:mat_id,:mat_seq,:nums,:buying_price,:retail_price,:trade_price,:batch_no,:expipy_date,:return_reason,:remark)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,13),
                    new OleDbParameter(":seq", OleDbType.Numeric,5),
					new OleDbParameter(":mat_id", OleDbType.Numeric,8),
					new OleDbParameter(":mat_seq", OleDbType.Numeric,5),
					new OleDbParameter(":nums", OleDbType.Numeric,10),
					new OleDbParameter(":buying_price", OleDbType.Numeric,12),
					new OleDbParameter(":retail_price", OleDbType.Numeric,12),                    
                    new OleDbParameter(":trade_price", OleDbType.Numeric,12),
                    new OleDbParameter(":batch_no", OleDbType.VarChar,32),
                    new OleDbParameter(":expipy_date", OleDbType.Date),
                    new OleDbParameter(":return_reason", OleDbType.Numeric,3),
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
            parameters[9].Value = expipy_date;
            parameters[10].Value = return_reason;
            parameters[11].Value = remark;
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
        public bool deleteBoutTempDetail(decimal id, decimal seq)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM MMIS_BOUT_TEMP_DETAIL");
            strSql.Append(" WHERE ID = :id");
            strSql.Append(" AND SEQ = :seq");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,13),
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
        //临时单修改明细
        public bool updateBoutTempDetail(decimal ID, decimal SEQ, decimal NUMS, decimal BUYING_PRICE)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_BOUT_TEMP_DETAIL SET ");
            strSql.Append("NUMS = :nums,");
            strSql.Append("BUYING_PRICE = :buying_price");
            strSql.Append(" WHERE ID = :id");
            strSql.Append(" AND SEQ = :seq");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":nums", OleDbType.Numeric,12),
                    new OleDbParameter(":buying_price", OleDbType.Numeric,16),
                    new OleDbParameter(":id", OleDbType.Numeric,13),
                    new OleDbParameter(":seq", OleDbType.Numeric,5)};
            parameters[0].Value = NUMS;
            parameters[1].Value = BUYING_PRICE;
            parameters[2].Value = ID;
            parameters[3].Value = SEQ;
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
        public bool deleteBoutTempMain(decimal id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM MMIS_BOUT_TEMP_MAIN");
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
        //提交审批
        public bool reviewState(decimal id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_BOUT_TEMP_MAIN SET");
            strSql.Append(" STATE = '2'");
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
        //审核
        public bool examineState(decimal id, string state)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_BOUT_TEMP_MAIN SET");
            strSql.Append(" STATE = :state");
            strSql.Append(" WHERE ID = :id");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":state", OleDbType.Numeric,13),
                    new OleDbParameter(":id", OleDbType.Numeric,13)};
            parameters[0].Value = state;
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
    }
}
