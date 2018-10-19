using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.OleDb;

namespace BookDAL
{
    public class DalPriceMain
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        /// <summary>
       /// 获取通过实物审核的调价正式单据
       /// </summary>
       /// <param name="storageID"></param>
       /// <param name="accountID"></param>
       /// <param name="docNo"></param>
       /// <param name="begTime"></param>
       /// <param name="endTime"></param>
       /// <returns></returns>
        public DataTable GetCheckedBillPriceInList(decimal? storageID, decimal? accountID, string docNo, DateTime? begTime, DateTime? endTime)
        {
            string whereClause = string.Empty;
            decimal? begID = decimal.Parse(begTime.Value.ToString("yyyyMMdd") + "00000");
            decimal? endID = decimal.Parse(endTime.Value.ToString("yyyyMMdd") + "00000");
            if (accountID != null)
            {
                whereClause += " AND ACC_ID="+accountID+"";
            }
            if (docNo != string.Empty && docNo != null)
            {
                whereClause += " AND DOC_NO LIKE '%' || "+docNo+" || '%'";
            }
            if (begTime != null)
            {
                whereClause += " AND ID > "+begID+"";
            }
            if (endTime != null)
            {
                whereClause += " AND ID <= "+endID+"";
            }
            string sql = string.Format(@"SELECT  ID,STO_ID,ACC_ID,
                                                 '审核通过' STATE,
                                                 NO,
                                                 DOC_NO DOCNO,
                                                 (SELECT NAME FROM PUB_EMP WHERE ID = INPUT_EMP) INPUT_EMP,
                                                 (SELECT NAME FROM PUB_EMP WHERE ID = CHECK_EMP) CHECK_EMP,
                                                 INPUT_TIME,
                                                 CHECK_TIME,
                                                 REMARK
                                                 FROM MMIS_BPRICE_MAIN
                                                 WHERE STO_ID= " + storageID+" {0}", whereClause);
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        //获取未通过审核的调价临时单据
        public DataTable GetUnCheckedBillPriceInList(decimal? storageID, decimal? accountID, string docNo, string state)
        {
            string whereClause = string.Empty;
            char State = Convert.ToChar(state);
            if (accountID != null)
            {
                whereClause += " AND ACC_ID=" + accountID + "";
            }
            if (docNo != string.Empty && docNo != null)
            {
                whereClause += " AND DOC_NO LIKE '%' || " + docNo + " || '%'";
            }
            if (state != string.Empty && state != null)
            {
                whereClause += " AND STATE = " + State + "";
            }
            string sql = string.Format(@"SELECT ID,
                                                                    STATE,
                                                                    NO,
                                                                    DOC_NO DOCNO,
                                                                    (SELECT NAME FROM PUB_EMP WHERE ID = INPUT_EMP) INPUT_EMP,
                                                                    INPUT_TIME,
                                                                    REMARK
                                                               FROM MMIS_BPRICE_TEMP_MAIN
                                                              WHERE STO_ID= " + storageID + " AND CHECK_EMP IS NULL {0}", whereClause);
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        /// <summary>
        /// 获取调价临时单据组合的明细单信息
        /// </summary>
        /// <param name="billPriceTempID">主单编号</param>
        /// <param name="priceMode">调价模式：1调零售价，2调批发价，3调最高零售价(即国零价)</param>
        /// <returns></returns>
        public DataTable GetUnCheckedBillPriceDetailListShow(decimal billPriceTempID, string priceMode)
        {
            string tempSQL;
            if ("1".Equals(priceMode))
            {
                tempSQL = @"ROUND((A.RETAIL_PRICE_N - A.RETAIL_PRICE_O) * A.NUMS /C.RATIO,2) TOTAL, ";
            }
            else if ("2".Equals(priceMode))
            {
                tempSQL = @"ROUND((A.TRADE_PRICE_N - A.TRADE_PRICE_O) * A.NUMS /C.RATIO,2) TOTAL, ";
            }
            else
            {
                tempSQL = @"ROUND((A.MAX_PRICE_N - A.MAX_PRICE_O) * A.NUMS /C.RATIO,2) TOTAL, ";
            }
            string selectBillPriceTempDetailSql = @"SELECT 
                                                            A.MAT_ID,
                                                            C.NAME,
                                                            C.SPEC,
                                                            D.UNIT,
                                                            (SELECT NAME FROM MMIS_FACTORY WHERE ID=C.FACTORY) AS FACTORY_NAME,
                                                            A.MAT_SEQ,
                                                            ROUND(A.NUMS/D.RATIO,2) AS NUMS,
                                                            A.RETAIL_PRICE_O,
                                                            A.RETAIL_PRICE_N,
                                                            A.TRADE_PRICE_O,
                                                            A.TRADE_PRICE_N,
                                                            A.MAX_PRICE_O,
                                                            A.MAX_PRICE_N, 
                                                            ROUND(A.RETAIL_PRICE_O*D.RATIO/C.RATIO,2) AS RETAIL_PRICE_O_SHOW,
                                                            ROUND(A.RETAIL_PRICE_N*D.RATIO/C.RATIO,2) AS RETAIL_PRICE_N_SHOW,
                                                            ROUND(A.TRADE_PRICE_O*D.RATIO/C.RATIO,2) AS TRADE_PRICE_O_SHOW,
                                                            ROUND(A.TRADE_PRICE_N*D.RATIO/C.RATIO,2) AS TRADE_PRICE_N_SHOW,
                                                            ROUND(A.MAX_PRICE_O*D.RATIO/C.RATIO,2) AS MAX_PRICE_O_SHOW,
                                                            ROUND(A.MAX_PRICE_N*D.RATIO/C.RATIO,2) AS MAX_PRICE_N_SHOW,"
                                                            + tempSQL +
                                                            @"A.BATCH_NO,
                                                            A.EXPIRY_DATE,
                                                            A.REMARK,
                                                            C.RATIO AS RATIO_BUYING,
                                                            D.RATIO AS RATIO_SELF
                                                            FROM MMIS_BPRICE_TEMP_DETAIL A,MMIS_BPRICE_TEMP_MAIN B,
                                                                 MMIS_MATERIAL C,MMIS_STORAGE_MATERIAL D
                                                            WHERE A.ID=:id 
                                                            AND A.ID=B.ID(+)
                                                            AND A.MAT_ID=C.ID(+)
                                                            AND D.STO_ID=B.STO_ID
                                                            AND D.ACC_ID=B.ACC_ID
                                                            AND D.MAT_ID=A.MAT_ID
                                                            ORDER BY A.SEQ";
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,13)};
            parameters[0].Value = billPriceTempID;
            DataTable dt = sqlDBHelper.Query(selectBillPriceTempDetailSql,parameters);
            return dt;
        }
         /// <summary>
        /// 获取调价正式单据组合的明细单信息
        /// </summary>
        /// <param name="billPriceID">主单编号</param>
        /// <param name="priceMode">调价模式：1调零售价，2调批发价，3调最高零售价(即国零价)</param>
        /// <returns></returns>
        public DataTable GetCheckedBillPriceDetailListShow(decimal billPriceID, string priceMode)
        {
            string tempSQL;
            if ("1".Equals(priceMode))
            {
                tempSQL = @"ROUND((A.RETAIL_PRICE_N - A.RETAIL_PRICE_O) * A.NUMS/C.RATIO,2) TOTAL, ";
            }
            else if ("2".Equals(priceMode))
            {
                tempSQL = @"ROUND((A.TRADE_PRICE_N - A.TRADE_PRICE_O) * A.NUMS/C.RATIO,2) TOTAL, ";
            }
            else
            {
                tempSQL = @"ROUND((A.MAX_PRICE_N - A.MAX_PRICE_O) * A.NUMS/C.RATIO,2) TOTAL, ";
            }
            string selectBillPriceDetailSql = @"SELECT 
                                                A.MAT_ID,
                                                C.NAME,
                                                C.SPEC,
                                                D.UNIT,
                                                (SELECT NAME FROM MMIS_FACTORY WHERE ID=C.FACTORY) AS FACTORY_NAME,
                                                A.MAT_SEQ,
                                                ROUND(A.NUMS/D.RATIO,2) NUMS,
                                                A.RETAIL_PRICE_O,
                                                A.RETAIL_PRICE_N,
                                                A.TRADE_PRICE_O,
                                                A.TRADE_PRICE_N,
                                                A.MAX_PRICE_O,
                                                A.MAX_PRICE_N, 
                                                ROUND(A.RETAIL_PRICE_O*D.RATIO/C.RATIO,2) AS RETAIL_PRICE_O_SHOW,
                                                ROUND(A.RETAIL_PRICE_N*D.RATIO/C.RATIO,2) AS RETAIL_PRICE_N_SHOW,
                                                ROUND(A.TRADE_PRICE_O*D.RATIO/C.RATIO,2) AS TRADE_PRICE_O_SHOW,
                                                ROUND(A.TRADE_PRICE_N*D.RATIO/C.RATIO,2) AS TRADE_PRICE_N_SHOW,
                                                ROUND(A.MAX_PRICE_O*D.RATIO/C.RATIO,2) AS MAX_PRICE_O_SHOW,
                                                ROUND(A.MAX_PRICE_N*D.RATIO/C.RATIO,2) AS MAX_PRICE_N_SHOW,"
                                                 + tempSQL +
                                                @"A.BATCH_NO,
                                                A.EXPIRY_DATE,
                                                A.REMARK,
                                                C.RATIO AS RATIO_BUYING,
                                                D.RATIO AS RATIO_SELF 
                                                FROM MMIS_BPRICE_DETAIL A,MMIS_BPRICE_MAIN B,
                                                     MMIS_MATERIAL C,MMIS_STORAGE_MATERIAL D
                                                WHERE A.ID=:id 
                                                AND A.ID=B.ID(+)
                                                AND A.MAT_ID=C.ID(+)
                                                AND D.STO_ID=B.STO_ID
                                                AND D.ACC_ID=B.ACC_ID
                                                AND D.MAT_ID=A.MAT_ID
                                                ORDER BY A.SEQ";
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,13)};
            parameters[0].Value = billPriceID;
            DataTable dt = sqlDBHelper.Query(selectBillPriceDetailSql, parameters);
            return dt;
        }
        /// <summary>
        /// 通过账册编号获取账册下所有品种明细和关联的品种名称
        /// (使用：1、入库处理界面获取品种明细列表。2、发起申领界面获取品种明细列表。3、调价处理界面获取品种明细列表)
        /// </summary>
        /// <param name="accountID">账册标号</param>
        /// <param name="sto_ID">仓库ID</param>
        /// <param name="showMaterial">是否根据物资明细进行检索 false：根据库存进行检索</param>
        /// <param name="showMaxUnit">true 根据采购单位显示 false 根据该仓库管理单位显示</param>
        /// <returns>账册下所有品种明细和关联的品种名称</returns>
        public DataTable GetMaterialNameList(decimal accountID, string sto_ID, bool showMaterial, bool showMaxUnit)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT C.ID,C.CODE,C.NAME,B.NAME AS CATNAME,C.SPEC,C.FACTORY,C.BRAND,C.GRADE,C.PERMITNO,C.BUYING_PRICE,C.RETAIL_PRICE,C.TRADE_PRICE,C.MAX_PRICE,");
            strSql.Append("C.RATIO AS RATIO_BUYING,E.NAME FACTORY_NAME,D.NAME ALIASNAME,D.INPUTCODE1,D.INPUTCODE2,F.UNIT,F.RATIO RATIO_SELF, C.BUYING_COMPANY COM_ID,");
            strSql.Append("NVL((SELECT ROUND(SUM(CUR_NUM-LOCK_NUM)/F.RATIO,4) FROM MMIS_STOCK WHERE STO_ID="+sto_ID+" AND ACC_ID="+accountID+" AND MAT_ID=C.ID GROUP BY MAT_ID),0) AS STOCKS,");
            strSql.Append("ROUND(C.BUYING_PRICE*F.RATIO/C.RATIO,4) BUYING_PRICE_SHOW,ROUND(C.RETAIL_PRICE*F.RATIO/C.RATIO,4) RETAIL_PRICE_SHOW,ROUND(C.TRADE_PRICE*F.RATIO/C.RATIO,4) TRADE_PRICE_SHOW,ROUND(C.MAX_PRICE*F.RATIO/C.RATIO,4) MAX_PRICE_SHOW");
            strSql.Append(" FROM MMIS_ACC_MAT_CLASS A,MMIS_CATALOG B,MMIS_MATERIAL C,MMIS_MATERIAL_NAME D,MMIS_FACTORY E,MMIS_STORAGE_MATERIAL F ");
            strSql.Append(" WHERE  A.ACC_ID=" + accountID + " AND A.CLASS_ID=B.TYPE AND B.ID=C.CAT_ID AND C.ID=D.ID AND C.FACTORY=E.ID(+)  AND C.ID=F.MAT_ID AND F.STO_ID=" + sto_ID + " AND F.ACC_ID=" + accountID + " AND B.STATE='1' AND C.STATE='1' ORDER BY STOCKS DESC");
            DataTable dt = sqlDBHelper.GetTable(strSql.ToString());
            return dt;           
        }
        /// <summary>
        /// 通过账册编号及物资识别号获取物资批次列表
        /// </summary>
        /// <param name="accountID">账册编号</param>
        /// <param name="materialID">物资识别号</param>
        /// <returns></returns>
        public DataTable GetMaterialSeqList(decimal accountID, decimal materialID)
        {
            string selectMaterialListSql = @"SELECT 
                                              MAT_SEQ,
                                              SUM(CUR_NUM) NUM,
                                              RETAIL_PRICE,
                                              TRADE_PRICE,
                                              BATCH_NO,
                                              EXPIRY_DATE
                                            FROM MMIS_STOCK
                                            WHERE ACC_ID=:acc_id AND MAT_ID=:mat_id 
                                            GROUP BY MAT_SEQ, RETAIL_PRICE, TRADE_PRICE, BATCH_NO, EXPIRY_DATE 
                                            ORDER BY MAT_SEQ";
            OleDbParameter[] parameters = {
                    new OleDbParameter(":acc_id", OleDbType.Numeric,13),
                    new OleDbParameter(":mat_id", OleDbType.Numeric,13)};
            parameters[0].Value = accountID;
            parameters[1].Value = materialID;
            DataTable dt = sqlDBHelper.Query(selectMaterialListSql, parameters);
            return dt;
        }
        //临时单新增明细
        public bool insertBpriceTempDetail(decimal ID, decimal? SEQ, decimal MAT_ID, decimal MAT_SEQ, decimal NUMS, decimal RETAIL_PRICE_O, decimal RETAIL_PRICE_N, decimal TRADE_PRICE_O, decimal TRADE_PRICE_N, decimal MAX_PRICE_O, decimal MAX_PRICE_N, string BATCH_NO, DateTime EXPIRY_DATE, string REMARK)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO MMIS_BPRICE_TEMP_DETAIL(");
            strSql.Append("ID,SEQ,MAT_ID,MAT_SEQ,NUMS,RETAIL_PRICE_O,RETAIL_PRICE_N,TRADE_PRICE_O,TRADE_PRICE_N,MAX_PRICE_O,MAX_PRICE_N,BATCH_NO,EXPIRY_DATE,REMARK)");
            strSql.Append(" values (");
            strSql.Append(":id,:seq,:mat_id,:mat_seq,:nums,:retail_price_o,:retail_price_n,:trade_price_o,:trade_price_n,:max_price_o,:max_price_n,:batch_no,:expipy_date,:remark)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,13),
                    new OleDbParameter(":seq", OleDbType.Numeric,5),
                    new OleDbParameter(":mat_id", OleDbType.Numeric,8),
                    new OleDbParameter(":mat_seq", OleDbType.Numeric,5),
                    new OleDbParameter(":nums", OleDbType.Numeric,12),
                    new OleDbParameter(":retail_price_o", OleDbType.Numeric,16),
                    new OleDbParameter(":retail_price_n", OleDbType.Numeric,16),
                    new OleDbParameter(":trade_price_o", OleDbType.Numeric,16),
                    new OleDbParameter(":trade_price_n", OleDbType.Numeric,16),
                    new OleDbParameter(":max_price_o", OleDbType.Numeric,16),
                    new OleDbParameter(":max_price_n", OleDbType.Numeric,16),
                    new OleDbParameter(":batch_no", OleDbType.VarChar,32),
                    new OleDbParameter(":expipy_date", OleDbType.Date),
                    new OleDbParameter(":remark", OleDbType.VarChar,128)};
            parameters[0].Value = ID;
            parameters[1].Value = SEQ;
            parameters[2].Value = MAT_ID;
            parameters[3].Value = MAT_SEQ;
            parameters[4].Value = NUMS;
            parameters[5].Value = RETAIL_PRICE_O;
            parameters[6].Value = RETAIL_PRICE_N;
            parameters[7].Value = TRADE_PRICE_O;
            parameters[8].Value = TRADE_PRICE_N;
            parameters[9].Value = MAX_PRICE_O;
            parameters[10].Value = MAX_PRICE_N;
            parameters[11].Value = BATCH_NO;
            parameters[12].Value = EXPIRY_DATE;
            parameters[13].Value = REMARK;
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
        public decimal? MaxSeq(decimal ID)
        {
            string sql = @"SELECT MAX(SEQ) FROM MMIS_BPRICE_TEMP_DETAIL WHERE ID = "+ID+"";
            string Value = sqlDBHelper.GetScalar(sql).ToString();
            decimal? FirstValue;
            if (Value == null || Value.Equals(string.Empty))
            {
                FirstValue = 0;
            }
            else
            {
                FirstValue = decimal.Parse(sqlDBHelper.GetScalar(sql).ToString());
            }
            return FirstValue;
        }
        //临时单修改明细
        public bool updateBpriceTempDetail(decimal ID, decimal? SEQ, decimal NUMS, decimal RETAIL_PRICE_N, string REMARK)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE MMIS_BPRICE_TEMP_DETAIL SET ");
            strSql.Append("NUMS = :nums,");
            strSql.Append("RETAIL_PRICE_N = :retail_price_n");
            strSql.Append(" WHERE ID = :id");
            strSql.Append(" AND SEQ = :seq");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":nums", OleDbType.Numeric,12),
                    new OleDbParameter(":retail_price_n", OleDbType.Numeric,16),
                    new OleDbParameter(":id", OleDbType.Numeric,13),
                    new OleDbParameter(":seq", OleDbType.Numeric,5)};
            parameters[0].Value = NUMS;
            parameters[1].Value = RETAIL_PRICE_N;
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
        //删除调价单明细信息
        public bool deleteBpriceTempDetail(decimal ID, decimal? SEQ)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM MMIS_BPRICE_TEMP_DETAIL");
            strSql.Append(" WHERE ID = :id");
            strSql.Append(" AND SEQ = :seq");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,13),
                    new OleDbParameter(":seq", OleDbType.Numeric,5)};
            parameters[0].Value = ID;
            parameters[1].Value = SEQ;
            int rows = SqlDBHelper.ExecuteSql(strSql.ToString(), parameters);
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("UPDATE MMIS_BPRICE_TEMP_DETAIL SET ");
            strSql1.Append("SEQ = SEQ - 1");
            strSql1.Append(" WHERE ID = :id");
            strSql1.Append(" AND SEQ > :seq");
            OleDbParameter[] parameters1 = {
                    new OleDbParameter(":id", OleDbType.Numeric,13),
                    new OleDbParameter(":seq", OleDbType.Numeric,5)};
            parameters1[0].Value = ID;
            parameters1[1].Value = SEQ;
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
         //审核单据
        public bool CheckList(decimal ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO MMIS_BPRICE_DETAIL");
            strSql.Append(" SELECT * FROM MMIS_BPRICE_TEMP_DETAIL");
            strSql.Append(" WHERE ID = :id");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,13)};
            parameters[0].Value = ID;
            int rows = SqlDBHelper.ExecuteSql(strSql.ToString(), parameters);

            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("INSERT INTO MMIS_BPRICE_MAIN ");
            strSql1.Append("SELECT * FROM MMIS_BPRICE_TEMP_MAIN");
            strSql1.Append(" WHERE ID = :id");
            OleDbParameter[] parameters1 = {
                    new OleDbParameter(":id", OleDbType.Numeric,13)};
            parameters1[0].Value = ID;
            int rows1 = SqlDBHelper.ExecuteSql(strSql1.ToString(), parameters1);

            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("UPDATE  MMIS_BPRICE_TEMP_MAIN SET ");
            strSql2.Append("STATE = '5'");
            strSql2.Append(" WHERE ID = :id");
            OleDbParameter[] parameters2 = {
                    new OleDbParameter(":id", OleDbType.Numeric,13)};
            parameters2[0].Value = ID;
            int rows2 = SqlDBHelper.ExecuteSql(strSql2.ToString(), parameters2);
            if (rows2 > 0)
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
