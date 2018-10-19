using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.OleDb;

namespace BookDAL
{
    public class DalReportOut
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        public char getTarget(decimal ID)
        {
            string sql = @"SELECT TARGET FROM MMIS_BOUT_TYPE WHERE ID = " + ID + "";
            char target = Convert.ToChar(sqlDBHelper.GetScalar(sql));
            return target;
        }
        //主单查询
        public DataTable GetCheckedBillOutMainListOutCommShow(decimal storageID, decimal? accountID, decimal? bOutTypeID, decimal? targetID, DateTime? startYearMonthDay, DateTime? endYearMonthDay, string searchState)
        {
            string whereClause = string.Empty;
            if (bOutTypeID != null && bOutTypeID !=0)//出库分类
            {
                whereClause += " AND A.CLASS=" + bOutTypeID + "";
            }
            if (targetID != null && targetID != 0)//出库目标
            {
                whereClause += " AND A.TARGET_ID=" + targetID + "";
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
            string selectSql = string.Format(@"SELECT BOUT_STAT.*,ROWNUM 
                                            FROM 
                                            (SELECT INPUT_EMP,INPUT_TIME,CLASS,TYPE_NAME,TARGET_ID,TARGET,TARGET_NAME,ROUND(SUM(BUYING_PRICE),4) AS BUYING_PRICE_TOTAL,SUM(RETAIL_PRICE) AS RETAIL_PRICE_TOTAL,SUM(RETAIL_PRICE-BUYING_PRICE) AS BUYING_RETAIL_SUB,COUNT(*) AS NUMS
                                            FROM 
                                            (SELECT A.CLASS,
                                            A.INPUT_EMP,
                                            A.INPUT_TIME,
                                            B.NAME AS TYPE_NAME,
                                            A.TARGET_ID,
                                            B.TARGET,
                                            (CASE WHEN B.TARGET='1' THEN (SELECT NAME FROM MMIS_STORAGE WHERE ID=A.TARGET_ID)
                                            ELSE (SELECT NAME FROM MMIS_COMPANY WHERE ID=A.TARGET_ID) END) AS TARGET_NAME,
                                            (SELECT SUM(ROUND(NUMS*MMIS_BOUT_TEMP_DETAIL.BUYING_PRICE/MMIS_MATERIAL.RATIO,4)) FROM MMIS_BOUT_TEMP_DETAIL,MMIS_MATERIAL WHERE MMIS_BOUT_TEMP_DETAIL.ID=A.ID AND MMIS_BOUT_TEMP_DETAIL.MAT_ID=MMIS_MATERIAL.ID) 
                                            AS BUYING_PRICE,
                                            (SELECT SUM(ROUND(NUMS*MMIS_BOUT_TEMP_DETAIL.RETAIL_PRICE/MMIS_MATERIAL.RATIO,4)) FROM MMIS_BOUT_TEMP_DETAIL,MMIS_MATERIAL WHERE MMIS_BOUT_TEMP_DETAIL.ID=A.ID AND MMIS_BOUT_TEMP_DETAIL.MAT_ID=MMIS_MATERIAL.ID)
                                            AS RETAIL_PRICE
                                            FROM MMIS_BOUT_TEMP_MAIN A,MMIS_BOUT_TYPE B
                                            WHERE A.CLASS=B.ID(+)
                                            AND A.STO_ID=:sto_ID 
                                            AND A.ACC_ID=:acc_ID
                                            {0})
                                            GROUP BY CLASS,TYPE_NAME,TARGET_ID,TARGET,TARGET_NAME,INPUT_EMP,INPUT_TIME) BOUT_STAT", whereClause);
            OleDbParameter[] parameters = {
                    new OleDbParameter(":sto_ID", OleDbType.Numeric,15),
                    new OleDbParameter(":acc_ID", OleDbType.Numeric,15)};
            parameters[0].Value = storageID;
            parameters[1].Value = accountID;
            DataTable dt = sqlDBHelper.Query(selectSql, parameters);
            return dt;
        }
    }
}
