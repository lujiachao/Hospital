using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Collections;
using System.Data.OleDb;

namespace BookDAL
{
    public class DalBillAccInPayMain
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        private string _roundBits;
        public DataTable GetUnPayCompanyList(decimal? sto_ID, decimal? acc_ID, decimal? binTypeID,  string payState, DateTime? dateTimeStart, DateTime? dateTimeEnd)
        {
            string whereClause = string.Empty;
            this._roundBits = "4";
            if (sto_ID != null)
            {
                whereClause += string.Format(" AND A.STO_ID="+sto_ID+"");
            }
            if (acc_ID != null)
            {
                whereClause += string.Format(" AND A.ACC_ID="+acc_ID+"");
            }
            if (binTypeID != null)
            {
                whereClause += string.Format(" AND A.CLASS="+binTypeID+"");
            }
            if (dateTimeStart != null && dateTimeStart.ToString() != "0001/1/1 0:00:00")
            {
                decimal begID = decimal.Parse(dateTimeStart.Value.ToString("yyyyMMdd") + "00000");
                whereClause += string.Format(" AND A.ID >= "+begID+"");
            }
            if (dateTimeEnd != null && dateTimeEnd.ToString() != "0001/1/1 0:00:00")
            {
                decimal endID = decimal.Parse(dateTimeEnd.Value.ToString("yyyyMMdd") + "99999");
                whereClause += string.Format(" AND A.ID <= "+endID+"");
            }
            string selectSql = string.Empty;
            if (payState == "0")
            {
                selectSql = string.Format(@"SELECT A.STO_ID,A.ACC_ID,A.COM_ID,A.CLASS,B.NAME AS COMPANY_NAME,B.TELEPHONE,B.LINKMEN,B.BANK,B.BANK_ACCOUNT,
                                    SUM((SELECT ROUND(SUM(C.NUMS*C.BUYING_PRICE/D.RATIO),{1}) FROM MMIS_BIN_ACC_DETAIL C,MMIS_MATERIAL D WHERE C.MAT_ID=D.ID(+) AND C.ID=A.ID )) AS PAY_MONEY
                                    FROM MMIS_BIN_ACC_MAIN A,MMIS_COMPANY B 
                                    WHERE A.COM_ID=B.ID(+)
                                    AND (PAY_FINISHED='0' OR PAY_FINISHED='1')
                                    AND EXISTS (SELECT ID FROM MMIS_BIN_ACC_DETAIL WHERE ID=A.ID AND PAY_BILL_ID IS NULL) 
                                    {0}
                                    GROUP BY A.STO_ID,A.ACC_ID,A.COM_ID,A.CLASS,B.NAME,B.TELEPHONE,B.LINKMEN,B.BANK,B.BANK_ACCOUNT", whereClause, _roundBits);
            }
            else
            {
                string nullChecked = (payState == "1") ? string.Empty : "NOT";
                selectSql = string.Format(@"SELECT A.STO_ID,A.ACC_ID,A.COM_ID,A.CLASS,B.NAME AS COMPANY_NAME,B.TELEPHONE,B.LINKMEN,B.BANK,B.BANK_ACCOUNT,
                                SUM((SELECT ROUND(SUM(C.NUMS*C.BUYING_PRICE/D.RATIO),{2}) 
                                FROM MMIS_BIN_ACC_DETAIL C,MMIS_MATERIAL D 
                                WHERE C.ID=A.ID AND C.MAT_ID = D.ID )) AS PAY_MONEY
                                FROM MMIS_BIN_ACC_PAY A,MMIS_COMPANY B
                                WHERE A.COM_ID=B.ID(+)
                                AND A.CHECK_EMP IS {1} NULL {0}
                                GROUP BY A.STO_ID,A.ACC_ID,A.COM_ID,A.CLASS,B.NAME,B.TELEPHONE,B.LINKMEN,B.BANK,B.BANK_ACCOUNT", whereClause, nullChecked, _roundBits);
            }
            DataTable dt = sqlDBHelper.GetTable(selectSql);
            return dt;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
        }
        /// <summary>
        /// 根据采购单位ID获取付款主单信息
        /// </summary>
        /// <param name="sto_ID">仓库ID</param>
        /// <param name="acc_ID">账册ID</param>
        /// <param name="companyID">采购单位</param>
        /// <param name="payState">付款状态</param>
        /// <returns>付款主单信息</returns>
        public DataTable GetPayBillMainList(decimal sto_ID, decimal acc_ID, decimal companyID, string payState, DateTime? dateTimeStart, DateTime? dateTimeEnd)
        {
            this._roundBits = "4";
            StringBuilder strSql = new StringBuilder();
            string selectSql = string.Empty;
            //查询未付款
            if (payState == "0")
            {
                // 查询条件：根据单位ID 查询MMIS_BIN_ACC_MAIN 的未付款单据或者部分待付款的单位
                strSql.Append("SELECT A.*,DECODE(DETAIL_COUNT-UNPAY_DETAIL_COUNT,0,0,2) AS PAY_STATE,'0' SELECTED FROM (");
                strSql.Append(" SELECT ID,NO,COMPANY_NAME,BUYING_PRICE_TOTAL,RETAIL_PRICE_TOTAL,(RETAIL_PRICE_TOTAL-BUYING_PRICE_TOTAL) AS TOTAL_DIFFERENCE,(CASE WHEN RETAIL_PRICE_TOTAL=0 THEN 0 ELSE ROUND(BUYING_PRICE_TOTAL/RETAIL_PRICE_TOTAL,2) END) AS RATE, UNPAY_DETAIL_COUNT,DETAIL_COUNT, '' AS CHECK_EMP, '' AS CHECK_TIME, '' CHECK_NAME FROM (");
                strSql.Append(" SELECT A.ID,NO,B.NAME AS COMPANY_NAME,(SELECT SUM(ROUND(C.NUMS*C.BUYING_PRICE/D.RATIO,4)) FROM MMIS_BIN_ACC_DETAIL C,MMIS_MATERIAL D WHERE C.MAT_ID=D.ID(+) AND C.ID=A.ID) AS BUYING_PRICE_TOTAL,(SELECT SUM(ROUND(C.NUMS*C.RETAIL_PRICE/D.RATIO,4)) FROM MMIS_BIN_ACC_DETAIL C,MMIS_MATERIAL D WHERE C.MAT_ID=D.ID(+) AND C.ID=A.ID) AS RETAIL_PRICE_TOTAL,(SELECT COUNT(*) FROM MMIS_BIN_ACC_DETAIL WHERE ID=A.ID AND PAY_BILL_ID IS NULL) AS UNPAY_DETAIL_COUNT,(SELECT COUNT(*) FROM MMIS_BIN_ACC_DETAIL WHERE ID=A.ID) AS DETAIL_COUNT FROM MMIS_BIN_ACC_MAIN A,MMIS_COMPANY B");
                strSql.Append(" WHERE A.COM_ID=B.ID(+) AND A.COM_ID=" + companyID + " AND A.STO_ID=" + sto_ID + " AND A.ACC_ID=" + acc_ID + ") WHERE UNPAY_DETAIL_COUNT!=0)A");
                DataTable dt = sqlDBHelper.GetTable(strSql.ToString());
                return dt;  
            }
            else
            {
                string nullPayState = (payState == "1") ? string.Empty : "NOT";

                //添加日期限制 yx 20170405
                string dataWhere = string.Empty;
                if (payState == "2")
                {
                    string dateTimeEnd1 = dateTimeEnd.ToString().Substring(0,9) + " 23:59:59";
                    DateTime? dateTimeEnd2 = Convert.ToDateTime(dateTimeEnd1);
                    dataWhere = " AND A.CHECK_TIME BETWEEN  '" + dateTimeStart + "' AND '" + dateTimeEnd2 + "' ";
                }
                selectSql = string.Format(@"SELECT ID,NO,COMPANY_NAME,BUYING_PRICE_TOTAL,RETAIL_PRICE_TOTAL,(RETAIL_PRICE_TOTAL-BUYING_PRICE_TOTAL) AS TOTAL_DIFFERENCE, ROUND(BUYING_PRICE_TOTAL/RETAIL_PRICE_TOTAL,2) AS RATE,0 AS PAY_STATE,CHECK_EMP AS CHECK_EMP, CHECK_TIME AS CHECK_TIME, CHECK_NAME AS CHECK_NAME,'0' SELECTED FROM (SELECT A.ID,NO,B.NAME AS COMPANY_NAME,(SELECT SUM(ROUND(C.NUMS*C.BUYING_PRICE/D.RATIO,{2})) FROM MMIS_BIN_ACC_DETAIL C,MMIS_MATERIAL D WHERE C.MAT_ID=D.ID(+) AND C.ID=A.ID) AS BUYING_PRICE_TOTAL,(SELECT SUM(ROUND(C.NUMS*C.RETAIL_PRICE/D.RATIO,{2})) FROM MMIS_BIN_ACC_DETAIL C,MMIS_MATERIAL D WHERE C.MAT_ID=D.ID(+) AND C.ID=A.ID) AS RETAIL_PRICE_TOTAL,A.CHECK_EMP AS CHECK_EMP, A.CHECK_TIME AS CHECK_TIME, C.NAME AS CHECK_NAME FROM MMIS_BIN_ACC_PAY A,MMIS_COMPANY B, PUB_EMP C WHERE A.COM_ID=B.ID(+) AND A.CHECK_EMP IS {0} NULL AND A.CHECK_EMP = C.ID(+) AND A.COM_ID=" + companyID + " AND A.STO_ID=" + sto_ID + " AND A.ACC_ID=" + acc_ID + " {1} )", nullPayState, dataWhere, _roundBits);
                DataTable dt = sqlDBHelper.GetTable(selectSql);
                return dt;  
            }
        }
        //获取当前账册余额
        public string  Dalbalance(decimal acc_ID)
        {
            string sql = @"select BALANCE FROM MMIS_ACCOUNT_BALANCE WHERE ID = "+acc_ID+"";
            object Balance = sqlDBHelper.GetScalar(sql);
            string balance = Balance.ToString();
            return balance;
        }
        /// <summary>
        /// 提交付款，生成待付款信息
        /// </summary>
        /// <param name="entityBInAccPay">付款信息</param>
        /// <param name="bInAccDetailList">入库会计明细单</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>提交付款是否成功</returns>
        public bool SubmitPayInfo(decimal ID,decimal EMP)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO MMIS_BIN_ACC_PAY");
            strSql.Append(" (ID,STO_ID,ACC_ID,NO,CLASS,COM_ID,SUBMIT_EMP,SUBMIT_TIME)");
            strSql.Append(" SELECT ID,STO_ID,ACC_ID,NO,CLASS,COM_ID,"+EMP+" SUBMIT_EMP,SYSDATE");
            strSql.Append(" FROM MMIS_BIN_ACC_MAIN WHERE ID = :id");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,13)};
            parameters[0].Value = ID;
            int rows = SqlDBHelper.ExecuteSql(strSql.ToString(), parameters);
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("DELETE FROM MMIS_BIN_ACC_MAIN");
            strSql1.Append(" WHERE ID = :id");
            int rows1 = SqlDBHelper.ExecuteSql(strSql1.ToString(), parameters);
            if (rows > 0)
            {
                if (rows1 > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        ///  确认付款操作
        /// </summary>
        /// <param name="listPayBillID">确认付款的付费单据ID List</param>
        /// <param name="checkEmp">审核人ID</param>
        /// <returns>确认付款是否成功</returns>
        public bool FinishPayInfo(decimal ID, decimal EMP, decimal BUYING_PRICE_TOTAL, decimal ACC_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_BIN_ACC_PAY SET");
            strSql.Append(" CHECK_EMP = :EMP,");
            strSql.Append(" CHECK_TIME = SYSDATE");
            strSql.Append(" WHERE ID = :ID");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":EMP", OleDbType.Numeric,13),
                    new OleDbParameter(":ID", OleDbType.Numeric,13)};
            parameters[0].Value = EMP;
            parameters[1].Value = ID;
            int rows = SqlDBHelper.ExecuteSql(strSql.ToString(), parameters);
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("UPDATE MMIS_ACCOUNT_BALANCE SET");
            strSql1.Append(" BALANCE = BALANCE - :BUYING_PRICE_TOTAL");
            strSql1.Append(" WHERE ID = :ACC_ID");
            OleDbParameter[] parameters1 = {
                                new OleDbParameter(":BUYING_PRICE_TOTAL", OleDbType.Numeric,13),
                                new OleDbParameter(":ACC_ID", OleDbType.Numeric,13)};
            parameters1[0].Value = BUYING_PRICE_TOTAL;
            parameters1[1].Value = ACC_ID;
            int rows1 = SqlDBHelper.ExecuteSql(strSql1.ToString(), parameters1);
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
