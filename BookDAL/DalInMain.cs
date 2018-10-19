using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.OleDb;

namespace BookDAL
{

    public class DalInMain
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        /// <summary>
        /// 获取列表，分栏，图标模式下入库正式单据组合的主单和明细单信息
        /// </summary>
        /// <param name="storageID">仓库编号</param>
        /// <param name="accountID">账册类型编号</param>
        /// <param name="binTypeID">单据类型编号</param>
        /// <param name="begTime">入账开始时间</param>
        /// <param name="endTime">入账结束时间</param>
        /// <param name="invoiceState">发票状态</param>
        /// <param name="invoiceNo">发票号</param>
        /// <param name="companyID">供货单位</param>
        /// <returns></returns>
        public DataTable GetBillInTempMainListShow(decimal? storageID, decimal? accountID, decimal? binTypeID,
                                            DateTime? begTime, DateTime? endTime, string invoiceState, string invoiceNo, decimal? companyID,string state)
        {
            string whereClause = string.Empty;
            if (storageID != null)
            {
                whereClause += " AND A.STO_ID="+storageID+"";
            }
            if (accountID != null)
            {
                whereClause += " AND A.ACC_ID="+accountID+"";
            }
            if (binTypeID != null)
            {
                whereClause += " AND A.CLASS="+binTypeID+"";
            }
            if (begTime != null && begTime.ToString() != "0001/1/1 0:00:00")
            {
                decimal begID = decimal.Parse(begTime.Value.ToString("yyyyMMdd") + "00000");
                whereClause += " AND A.ID > "+begID+"";
            }
            if (endTime != null && endTime.ToString() != "0001/1/1 0:00:00")
            {
                decimal endID = decimal.Parse(endTime.Value.ToString("yyyyMMdd") + "99999");
                whereClause += " AND A.ID <= "+endID+"";
            }
            if (invoiceState != "4" && invoiceNo != string.Empty && invoiceNo != null )
            {
                whereClause += " AND A.INVOICE_STATE = '" + invoiceState+ "'";
            }
            if (invoiceNo != string.Empty && invoiceNo != null)
            {
                whereClause += " AND BIN_DETAIL.INVOICENO LIKE '%' || '"+invoiceNo+"' || '%'";
            }
            if (companyID != null && companyID != 0)
            {
                whereClause += " AND A.COM_ID="+companyID+"";
            }
            if (state != null)
            {
                whereClause += " AND A.STATE=" + state + "";
            }
            string selectBillInTempMainSql = string.Format(@"SELECT 
                                                            A.ID,
                                                            A.NO,
                                                            A.STO_ID,
                                                            A.ACC_ID,
                                                            A.BUY_EMP,
                                                            A.BUY_TIME,
                                                            A.INPUT_EMP,
                                                            A.INPUT_TIME,
                                                            A.INVOICE_STATE,
                                                            A.INTIME,
                                                            A.STATE,
                                                            C.NAME COMPANYNAME,
                                                            NVL(SUM(B.NUMS),0) TOTALNUMS,
                                                            NVL(SUM(B.NUMS*B.BUYING_PRICE),0) TOTALBUYINGPRICE,
                                                            NVL(SUM(B.NUMS*B.RETAIL_PRICE),0) TOTALRETAILPRICE,
                                                            A.REMARK REMARK
                                                          FROM MMIS_BIN_TEMP_MAIN A,MMIS_BIN_DETAIL B,MMIS_COMPANY C
                                                        WHERE A.ID=B.ID(+) AND A.COM_ID=C.ID AND A.ACC_BILL_STATE = '0' AND A.CHECK_EMP IS NULL {0}
                                                          GROUP BY A.ID,A.NO,A.STO_ID,A.ACC_ID,C.NAME,A.REMARK,A.BUY_EMP,A.BUY_TIME,A.INPUT_EMP,A.INPUT_TIME,A.INVOICE_STATE,A.INTIME,A.STATE", whereClause);
            DataTable dt = sqlDBHelper.GetTable(selectBillInTempMainSql);
            return dt;
        }
        public string GetName(string Code)
        {
            string sql = @"SELECT NAME FROM PUB_EMP WHERE CODE= '"+Code+"'";
            string name = sqlDBHelper.GetScalar(sql).ToString();
            return name;
        }
        /// <summary>
        /// 获取详细数据
        /// </summary>
        /// <param name="billInTempID">已实物验收的主单编号</param>
        /// <param name="sto_ID">仓库ID</param>
        /// <param name="acc_ID">账册ID</param>
        /// <returns>获取详细数据</returns>
        public DataTable GetBillAccInTempDetailListShow(decimal billInTempID)
        {
            string selectBillAccInTempDetailSql = string.Format(@"SELECT 
                                                '0' SELECTNO,
                                                A.SEQ,
                                                A.INVOICENO,
                                                A.MAT_ID,
                                                A.MAT_SEQ,
                                                C.NAME,
                                                C.SPEC,
                                                C.UNIT,
                                                C.RATIO AS RATIO_BUYING,
                                                C.FACTORY,
                                                D.RATIO AS RATIO_SELF,
                                                E.NAME FACTORYNAME,
                                                ROUND(A.NUMS/C.RATIO,2) AS NUMS,
                                                A.BUYING_PRICE,
                                                A.RETAIL_PRICE,
                                                A.TRADE_PRICE,
                                                ROUND(A.BUYING_PRICE,4) AS BUYING_PRICE_SHOW,
                                                ROUND(A.RETAIL_PRICE,4) AS RETAIL_PRICE_SHOW,
                                                ROUND(A.TRADE_PRICE,4) AS TRADE_PRICE_SHOW,
                                                ROUND(A.NUMS*A.BUYING_PRICE/C.RATIO,4) TOTAL_BUYING_PRICE,
                                                A.RETURN_REASON,
                                                A.BATCH_NO,
                                                A.EXPIRY_DATE,                                        
                                                A.RETURN_NUM,
                                                A.REMARK
                                                FROM MMIS_BIN_TEMP_DETAIL A,MMIS_BIN_TEMP_MAIN B,
                                                MMIS_MATERIAL C,MMIS_STORAGE_MATERIAL D,MMIS_FACTORY E 
                                                WHERE A.ID=B.ID(+)
                                                AND A.MAT_ID=C.ID(+)
                                                AND C.FACTORY=E.ID(+)
                                                AND D.STO_ID=B.STO_ID
                                                AND D.ACC_ID=B.ACC_ID
                                                AND D.MAT_ID=A.MAT_ID   
                                                AND A.ID=:id 
                                                ORDER BY SEQ");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,15)};
            parameters[0].Value = billInTempID;
            DataTable dt = sqlDBHelper.Query(selectBillAccInTempDetailSql, parameters);
            return dt;
        }
        //获取当日最大单据号
        public decimal? GetMaxID(string RQ)
        {
            string sql = @"select max(ID) from MMIS_BIN_TEMP_MAIN WHERE ID  LIKE '%' || '" + RQ + "' || '%'";
            object obj = sqlDBHelper.GetScalar(sql);
            decimal? maxID ;
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
        public bool insertBinTempMain(decimal? id, decimal sto_id, decimal acc_id, string no, decimal bInType, decimal com_id, char invoice_state, decimal nums, DateTime intime, decimal buy_emp, DateTime buy_time,decimal input_emp,DateTime input_time,char state,string remark,char acc_bill_state)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO MMIS_BIN_TEMP_MAIN(");
            strSql.Append("ID,STO_ID,ACC_ID,NO,CLASS,COM_ID,INVOICE_STATE,NUMS,INTIME,BUY_EMP,BUY_TIME,INPUT_EMP,INPUT_TIME,STATE,REMARK,ACC_BILL_STATE)");
            strSql.Append(" values (");
            strSql.Append(":id,:sto_id,:acc_id,:no,:class,:com_id,:invoice_state,:nums,:intime,:buy_emp,:buy_time,:input_emp,:input_time,:state,:remark,:acc_bill_state)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,13),
                    new OleDbParameter(":sto_id", OleDbType.Numeric,5),
					new OleDbParameter(":acc_id", OleDbType.Numeric,3),
					new OleDbParameter(":no", OleDbType.VarChar,16),
					new OleDbParameter(":class", OleDbType.Numeric,3),
					new OleDbParameter(":com_id", OleDbType.Numeric,5),
					new OleDbParameter(":invoice_state", OleDbType.Char,1),
					new OleDbParameter(":nums", OleDbType.Numeric,2),
					new OleDbParameter(":intime", OleDbType.Date),                    
                    new OleDbParameter(":buy_emp", OleDbType.Numeric,5),
                    new OleDbParameter(":buy_time", OleDbType.Date),
                    new OleDbParameter(":input_emp", OleDbType.Numeric,5),
                    new OleDbParameter(":input_time", OleDbType.Date),
                    new OleDbParameter(":state", OleDbType.Char,1),
                    new OleDbParameter(":remark", OleDbType.VarChar,128),
                    new OleDbParameter(":acc_bill_state", OleDbType.Char,1)};
            parameters[0].Value = id;
            parameters[1].Value = sto_id;
            parameters[2].Value = acc_id;
            parameters[3].Value = no;
            parameters[4].Value = bInType;
            parameters[5].Value = com_id;
            parameters[6].Value = invoice_state;
            parameters[7].Value = nums;
            parameters[8].Value = intime;
            parameters[9].Value = buy_emp;
            parameters[10].Value = buy_time;
            parameters[11].Value = input_emp;
            parameters[12].Value = input_time;
            parameters[13].Value = state;
            parameters[14].Value = remark;
            parameters[15].Value = acc_bill_state;
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
        public bool updateBinTempDetail(decimal ID, decimal SEQ, decimal NUMS, decimal BUYING_PRICE)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_BIN_TEMP_DETAIL SET ");
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
        public bool insertBinTempDetail(decimal id, decimal seq, string invoiceno, decimal mat_id, decimal mat_seq, decimal nums, decimal buying_price, decimal retail_price, decimal trade_price, decimal return_num, decimal return_reason, string batch_no, DateTime expipy_date, string remark)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO MMIS_BIN_TEMP_DETAIL(");
            strSql.Append("ID,SEQ,INVOICENO,MAT_ID,MAT_SEQ,NUMS,BUYING_PRICE,RETAIL_PRICE,TRADE_PRICE,RETURN_NUM,RETURN_REASON,BATCH_NO,EXPIRY_DATE,REMARK)");
            strSql.Append(" values (");
            strSql.Append(":id,:seq,:invoiceno,:mat_id,:mat_seq,:nums,:buying_price,:retail_price,:trade_price,:return_num,:return_reason,:batch_no,:expipy_date,:remark)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,13),
                    new OleDbParameter(":seq", OleDbType.Numeric,5),
					new OleDbParameter(":invoiceno", OleDbType.VarChar,16),
					new OleDbParameter(":mat_id", OleDbType.Numeric,8),
					new OleDbParameter(":mat_seq", OleDbType.Numeric,5),
					new OleDbParameter(":nums", OleDbType.Numeric,10),
					new OleDbParameter(":buying_price", OleDbType.Numeric,12),
					new OleDbParameter(":retail_price", OleDbType.Numeric,12),                    
                    new OleDbParameter(":trade_price", OleDbType.Numeric,12),
                    new OleDbParameter(":return_num", OleDbType.Numeric,10),
                    new OleDbParameter(":return_reason", OleDbType.Numeric,3),
                    new OleDbParameter(":batch_no", OleDbType.VarChar,32),
                    new OleDbParameter(":expipy_date", OleDbType.Date),
                    new OleDbParameter(":remark", OleDbType.VarChar,128)};
            parameters[0].Value = id;
            parameters[1].Value = seq;
            parameters[2].Value = invoiceno;
            parameters[3].Value = mat_id;
            parameters[4].Value = mat_seq;
            parameters[5].Value = nums;
            parameters[6].Value = buying_price;
            parameters[7].Value = retail_price;
            parameters[8].Value = trade_price;
            parameters[9].Value = return_num;
            parameters[10].Value = return_reason;
            parameters[11].Value = batch_no;
            parameters[12].Value = expipy_date;
            parameters[13].Value = remark;
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
        public bool deleteBinTempDetail(decimal id,decimal seq)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM MMIS_BIN_TEMP_DETAIL");
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
        public bool deleteBinTempMain(decimal id)
        {  
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM MMIS_BIN_TEMP_MAIN");
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
            strSql.Append("UPDATE MMIS_BIN_TEMP_MAIN SET");
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
        public bool examineState(decimal id,string state)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_BIN_TEMP_MAIN SET");
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
        public string catchName(string Code)
        {
            string sql = @"SELECT NAME FROM PUB_EMP WHERE CODE = "+Code+"";
            string name = sqlDBHelper.GetScalar(sql).ToString();
            return name;
        }
    }
}
