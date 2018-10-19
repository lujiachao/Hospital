using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.OleDb;

namespace BookDAL
{
    public class DalAccInMain
    {
        string _roundBits;  //小数保留位数
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        /// <summary>
        /// 获取已通过实物审核，未通过会计审核的入库临时单据
        /// </summary>
        /// <param name="whereClause"></param>
        /// <param name="storageID"></param>
        /// <param name="accountID"></param>
        /// <param name="binTypeID"></param>
        /// <param name="state"></param>
        /// <param name="begTime"></param>
        /// <param name="endTime"></param>
        /// <param name="invoiceState"></param>
        /// <param name="invoiceNo"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public DataTable GetUnCheckedBillAccInList(decimal? storageID, decimal? accountID, decimal? binTypeID,
        DateTime? begTime, DateTime? endTime, string invoiceState, string invoiceNo, decimal? companyID)
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
            if (invoiceState != string.Empty && invoiceState != null)
            {
                whereClause += " AND A.INVOICE_STATE = "+invoiceState+"";
            }
            if (invoiceNo != string.Empty && invoiceNo != null && invoiceNo != "4")
            {
                whereClause += " AND B.INVOICENO LIKE '%' || '"+invoiceNo+"' || '%'";
            }
            if (companyID != null && companyID != 0)
            {
                whereClause += " AND A.COM_ID="+companyID+"";
            }
            string selectBillInMainSql = string.Format(@"SELECT ID,STATE,NO,SECTION_NAME,SECTION_ID,REMARK,APPROVE,BUY_EMP,BUY_TIME,INTIME,INPUT_EMP,INPUT_TIME,CHECK_EMP,CHECK_TIME,
                                                        ROUND(NVL(SUM(NUMS/RATIO_SELF),0),2) TOTALNUMS,
                                                        ROUND(NVL(SUM(NUMS*BUYING_PRICE/RATIO_BUYING),0),2) TOTALBUYINGPRICE,
                                                        ROUND(NVL(SUM(NUMS*RETAIL_PRICE/RATIO_BUYING),0),2) TOTALRETAILPRICE
                                                        FROM (SELECT A.ID,
                                                        '未审核通过' STATE,
                                                        A.NO,
                                                        A.INPUT_EMP,
                                                        A.INPUT_TIME,
                                                        A.CHECK_EMP,
                                                        A.CHECK_TIME,
                                                        A.BUY_EMP,
                                                        A.BUY_TIME,
                                                        A.INTIME,
                                                        A.COM_ID AS SECTION_ID,
                                                        A.STATE AS APPROVE,
                                                        (CASE WHEN (SELECT SOURCE FROM MMIS_BIN_TYPE WHERE ID=A.CLASS)='1'
                                                        THEN (SELECT NAME FROM MMIS_STORAGE WHERE ID=A.COM_ID)
                                                        ELSE (SELECT NAME FROM MMIS_COMPANY WHERE ID=A.COM_ID) END) AS SECTION_NAME,
                                                        A.REMARK,
                                                        B.NUMS,
                                                        B.BUYING_PRICE,
                                                        B.RETAIL_PRICE,
                                                        C.RATIO AS RATIO_BUYING,
                                                        D.RATIO AS RATIO_SELF
                                                        FROM MMIS_BIN_TEMP_MAIN A,MMIS_BIN_TEMP_DETAIL B,MMIS_MATERIAL C,MMIS_STORAGE_MATERIAL D
                                                        WHERE A.ID=B.ID(+) 
                                                        AND B.MAT_ID=C.ID(+)
                                                        AND D.STO_ID=A.STO_ID
                                                        AND D.ACC_ID=A.ACC_ID
                                                        AND D.MAT_ID=B.MAT_ID 
                                                        AND (A.STATE = '0' OR A.STATE = '3')
                                                        AND A.ACC_BILL_STATE = '0'
                                                        {0})H 
                                                        GROUP BY ID,STATE,NO,SECTION_ID,SECTION_NAME,REMARK,APPROVE,BUY_EMP,BUY_TIME,INTIME,INPUT_EMP,INPUT_TIME,CHECK_EMP,CHECK_TIME", whereClause);
            DataTable dt = sqlDBHelper.GetTable(selectBillInMainSql);
            return dt;
        }
        /// 获取已通过实物验收，同时通过会计审核的入库临时单据
        /// </summary>
        /// <param name="whereClause"></param>
        /// <param name="storageID"></param>
        /// <param name="accountID"></param>
        /// <param name="binTypeID"></param>
        /// <param name="state"></param>
        /// <param name="begTime"></param>
        /// <param name="endTime"></param>
        /// <param name="invoiceState"></param>
        /// <param name="invoiceNo"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public DataTable GetCheckedBillAccInList(decimal? storageID, decimal? accountID, decimal? binTypeID,
        DateTime? begTime, DateTime? endTime, string invoiceState, string invoiceNo, decimal? companyID)
        {
            string whereClause = string.Empty;
            if (storageID != null)
            {
                whereClause += " AND A.STO_ID=" + storageID + "";
            }
            if (accountID != null)
            {
                whereClause += " AND A.ACC_ID=" + accountID + "";
            }
            if (binTypeID != null)
            {
                whereClause += " AND A.CLASS=" + binTypeID + "";
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
            if (invoiceState != string.Empty && invoiceState != null)
            {
                whereClause += " AND A.INVOICE_STATE = " + invoiceState + "";
            }
            if (invoiceNo != string.Empty && invoiceNo != null)
            {
                whereClause += " AND B.INVOICENO LIKE '%' || '" + invoiceNo + "' || '%'";
            }
            if (companyID != null && companyID != 0)
            {
                whereClause += " AND A.COM_ID=" + companyID + "";
            }
            string selectBillInMainSql = string.Format(@"SELECT ID,STATE,NO,SECTION_NAME,SECTION_ID,REMARK,APPROVE,BUY_EMP,BUY_TIME,INTIME,INPUT_EMP,INPUT_TIME,CHECK_EMP,CHECK_TIME,
                                                        ROUND(NVL(SUM(NUMS/RATIO_SELF),0),2) TOTALNUMS,
                                                        ROUND(NVL(SUM(NUMS*BUYING_PRICE/RATIO_BUYING),0),2) TOTALBUYINGPRICE,
                                                        ROUND(NVL(SUM(NUMS*RETAIL_PRICE/RATIO_BUYING),0),2) TOTALRETAILPRICE
                                                        FROM (SELECT A.ID,
                                                        '审核通过' STATE,
                                                        A.NO,
                                                        A.INPUT_EMP,
                                                        A.INPUT_TIME,
                                                        A.CHECK_EMP,
                                                        A.CHECK_TIME,
                                                        A.INTIME,
                                                        A.COM_ID AS SECTION_ID,
                                                        A.STATE AS APPROVE,
                                                        A.BUY_EMP,
                                                        A.BUY_TIME,
                                                        (CASE WHEN (SELECT SOURCE FROM MMIS_BIN_TYPE WHERE ID=A.CLASS)='1'
                                                        THEN (SELECT NAME FROM MMIS_STORAGE WHERE ID=A.COM_ID)
                                                        ELSE (SELECT NAME FROM MMIS_COMPANY WHERE ID=A.COM_ID) END) AS SECTION_NAME,
                                                        A.REMARK,
                                                        B.NUMS,
                                                        B.BUYING_PRICE,
                                                        B.RETAIL_PRICE,
                                                        C.RATIO AS RATIO_BUYING,
                                                        D.RATIO AS RATIO_SELF
                                                        FROM MMIS_BIN_TEMP_MAIN A,MMIS_BIN_TEMP_DETAIL B,MMIS_MATERIAL C,MMIS_STORAGE_MATERIAL D
                                                        WHERE A.ID=B.ID(+) 
                                                        AND B.MAT_ID=C.ID(+)
                                                        AND D.STO_ID=A.STO_ID
                                                        AND D.ACC_ID=A.ACC_ID
                                                        AND D.MAT_ID=B.MAT_ID 
                                                        AND (A.STATE = '0' OR A.STATE = '3')
                                                        AND A.ACC_BILL_STATE = '1'
                                                        AND A.CHECK_EMP IS NOT NULL
                                                        AND A.CHECK_TIME IS NOT NULL
                                                        {0})H 
                                                        GROUP BY ID,STATE,NO,SECTION_ID,SECTION_NAME,REMARK,APPROVE,BUY_EMP,BUY_TIME,INTIME,INPUT_EMP,INPUT_TIME,CHECK_EMP,CHECK_TIME", whereClause);
            DataTable dt = sqlDBHelper.GetTable(selectBillInMainSql);
            return dt;
        }
        /// <summary>
        /// 获取详细数据
        /// </summary>
        /// <param name="billInTempID">已实物验收的主单编号</param>
        /// <param name="sto_ID">仓库ID</param>
        /// <param name="acc_ID">账册ID</param>
        /// <returns>获取详细数据</returns>
        public DataTable GetBillAccInTempDetailListShow(decimal billInTempID, string state)
        {
            _roundBits = "4";
            string whereClause = string.Empty;
            //if (state != null)
            //{
            //    if (state == "999")
            //    {
            //        whereClause = " AND A.ACC_BILL_ID IS NOT NULL";
            //    }
            //    else if (state == "0")
            //    {
            //        whereClause = " AND A.ACC_BILL_ID IS NULL";
            //    }
            //}
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
                                                ROUND(A.NUMS*A.BUYING_PRICE/C.RATIO,{0}) TOTAL_BUYING_PRICE,
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
                                                {1} ORDER BY SEQ", _roundBits, whereClause);
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,5)};
            parameters[0].Value = billInTempID;
            DataTable dt = sqlDBHelper.Query(selectBillAccInTempDetailSql, parameters);
            return dt;
        }
        public DataTable GetEmployeeList()
        {
            string Sql = @"SELECT CODE,NAME FROM PUB_EMP";
            DataTable dt = sqlDBHelper.GetTable(Sql);
            return dt;
        }
        public bool updateBinDetail(decimal ID,decimal SEQ,string INVOICENO,decimal NUMS,decimal BUYING_PRICE)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_BIN_TEMP_DETAIL SET ");
            strSql.Append("INVOICENO = :invoiceno,");
            strSql.Append("NUMS = :nums,");
            strSql.Append("BUYING_PRICE = :buying_price");
            strSql.Append(" WHERE ID = :id");
            strSql.Append(" AND SEQ = :seq");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":invoiceno", OleDbType.VarChar,16),
                    new OleDbParameter(":nums", OleDbType.Numeric,5),
                    new OleDbParameter(":buying_price", OleDbType.Numeric,12),
                    new OleDbParameter(":id", OleDbType.Numeric,13),
                    new OleDbParameter(":seq", OleDbType.Numeric,5)};
            parameters[0].Value = INVOICENO;
            parameters[1].Value = NUMS;
            parameters[2].Value = BUYING_PRICE;
            parameters[3].Value = ID;
            parameters[4].Value = SEQ;
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
        public bool updateBinMain(decimal ID, decimal Check_Emp,DateTime Check_Time,decimal NUMS,decimal Sto_ID,decimal Acc_ID,decimal Mat_ID,decimal Mat_Seq)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_BIN_TEMP_MAIN SET ");
            strSql.Append("CHECK_EMP = :Check_Emp,");
            strSql.Append("CHECK_TIME = :Check_Time,");
            strSql.Append("ACC_BILL_STATE = '1' ");
            strSql.Append(" WHERE ID = :id");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":Check_Emp", OleDbType.Numeric,5),
                    new OleDbParameter(":Check_Time", OleDbType.Date),
                    new OleDbParameter(":id", OleDbType.Numeric,13)};
            parameters[0].Value = Check_Emp;
            parameters[1].Value = Check_Time;
            parameters[2].Value = ID;
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("UPDATE MMIS_STOCK SET ");
            strSql1.Append("CUR_NUM = CUR_NUM + :nums");
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
