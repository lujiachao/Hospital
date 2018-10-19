using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.OleDb;

namespace BookDAL
{
    public class DalReportIn
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        /// <summary>
        /// 获取指定仓库账册入库信息
        /// </summary>
        /// <param name="sto_ID">仓库ID</param>
        /// <param name="acc_ID">账册ID</param>
        /// <param name="inTypeID">入库类型</param>
        /// <param name="invoiceState">发票状态</param>
        /// <param name="startYearMonthDay">起始时间(8位年月日)</param>
        /// <param name="endYearMonthDay">结束时间(8位年月日)</param>
        /// <returns>指定仓库账册入库信息</returns>
        public DataTable GetBillInStatList(decimal sto_ID, decimal acc_ID, decimal? inTypeID, string invoiceState, DateTime? startYearMonthDay, DateTime? endYearMonthDay, decimal? com_ID, string searchState)
        {
            string whereClause = string.Empty;
            if (inTypeID != null && inTypeID > 0)
            {
                whereClause += " AND A.CLASS = " + inTypeID + "";
            }
            if (com_ID != null && com_ID > 0)
            {
                whereClause += " AND A.COM_ID = " + com_ID + "";
            }
            if (!string.IsNullOrEmpty(invoiceState) && invoiceState != "4")
            {
                whereClause += " AND A.INVOICE_STATE = " + invoiceState + "";
            }
            if (searchState == "Account")
            {
                if (startYearMonthDay != null && startYearMonthDay.ToString() != "0001/1/1 0:00:00")
                {
                    decimal begID = decimal.Parse(startYearMonthDay.Value.ToString("yyyyMMdd") + "00000");
                    whereClause += " AND A.ID > " + begID + "";
                }
                if (endYearMonthDay != null && endYearMonthDay.ToString() != "0001/1/1 0:00:00")
                {
                    decimal endID = decimal.Parse(endYearMonthDay.Value.ToString("yyyyMMdd") + "99999");
                    whereClause += " AND A.ID <= " + endID + "";
                }
            }
            else if (searchState == "General")
            {
                if (startYearMonthDay != null && startYearMonthDay.ToString() != "0001/1/1 0:00:00")
                {
                    decimal begID = decimal.Parse(startYearMonthDay.Value.ToString("yyyyMMdd"));
                    whereClause += " AND TO_NUMBER(TO_CHAR(A.INTIME,'YYYYMMDD')) > " + begID + "";
                }
                if (endYearMonthDay != null && endYearMonthDay.ToString() != "0001/1/1 0:00:00")
                {
                    decimal endID = decimal.Parse(endYearMonthDay.Value.ToString("yyyyMMdd") + "99999");
                    whereClause += " AND TO_NUMBER(TO_CHAR(A.INTIME,'YYYYMMDD')) <= " + endID + "";
                }
            }
            else if (searchState == "Checked")
            {
                if (startYearMonthDay != null && startYearMonthDay.ToString() != "0001/1/1 0:00:00")
                {
                    decimal begID = decimal.Parse(startYearMonthDay.Value.ToString("yyyyMMdd"));
                    whereClause += " AND TO_NUMBER(TO_CHAR(A.CHECK_TIME,'YYYYMMDD')) > " + begID + "";
                }
                if (endYearMonthDay != null && endYearMonthDay.ToString() != "0001/1/1 0:00:00")
                {
                    decimal endID = decimal.Parse(endYearMonthDay.Value.ToString("yyyyMMdd") + "99999");
                    whereClause += " AND TO_NUMBER(TO_CHAR(A.CHECK_TIME,'YYYYMMDD')) <= " + endID + "";
                }
            }
            string selectSql = string.Format(@"SELECT A.CLASS,B.NAME AS TYPE_NAME,A.COM_ID,C.NAME AS COM_NAME,
                                            ROUND(SUM(D.NUMS*D.BUYING_PRICE/E.RATIO),4) AS BUYING_PRICE_TOTAL,
                                            ROUND(SUM(D.NUMS*D.RETAIL_PRICE/E.RATIO),4) AS RETAIL_PRICE_TOTAL,
                                            ROUND(SUM(D.NUMS*D.RETAIL_PRICE/E.RATIO)-SUM(D.NUMS*D.BUYING_PRICE/E.RATIO),2) AS BUYING_RETAIL_SUB,
                                            (CASE WHEN SUM(D.NUMS*D.RETAIL_PRICE/E.RATIO)=0 THEN 1
                                            ELSE ROUND(SUM(D.NUMS*D.BUYING_PRICE/E.RATIO)/SUM(D.NUMS*D.RETAIL_PRICE/E.RATIO),2) END) AS RATE
                                          FROM MMIS_BIN_TEMP_MAIN A,MMIS_BIN_TYPE B,MMIS_COMPANY C,MMIS_BIN_TEMP_DETAIL D,MMIS_MATERIAL E 
                                         WHERE A.CLASS=B.ID(+)
                                           AND A.COM_ID=C.ID(+)
                                           AND A.STO_ID=:sto_id 
                                           AND A.ACC_ID=:acc_ID
                                           AND A.ID=D.ID
                                           AND D.MAT_ID=E.ID
                                           AND B.SOURCE='2'
                                           AND A.CHECK_EMP IS NOT NULL
                                           AND A.ACC_BILL_STATE = '1'
                                           {0}
                                           GROUP BY A.CLASS,B.NAME,A.COM_ID,C.NAME
                                           ORDER BY A.CLASS", whereClause);
            OleDbParameter[] parameters = {
                    new OleDbParameter(":sto_id", OleDbType.Numeric,5),
                    new OleDbParameter("acc_ID",OleDbType.Numeric,5)};
            parameters[0].Value = sto_ID;
            parameters[1].Value = acc_ID;
            DataTable dt = sqlDBHelper.Query(selectSql, parameters);
            return dt;
        }
    }
}
