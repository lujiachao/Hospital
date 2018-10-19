using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BookDAL;

namespace BookBLL
{
    public class BillPriceMain
    {
        DalPriceMain dalPriceMain = new DalPriceMain();
        public DataTable GetCheckedBillPriceInList(decimal? storageID, decimal? accountID, string docNo, DateTime? begTime, DateTime? endTime)
        {
            DataTable dt = dalPriceMain.GetCheckedBillPriceInList(storageID,accountID,docNo,begTime,endTime);
            return dt;
        }
        public DataTable GetUnCheckedBillPriceInList(decimal? storageID, decimal? accountID, string docNo, string state)
        {
            DataTable dt = dalPriceMain.GetUnCheckedBillPriceInList(storageID, accountID, docNo, state);
            return dt;
        }
        public DataTable GetUnCheckedBillPriceDetailListShow(decimal billPriceTempID, string priceMode)
        {
            DataTable dt = dalPriceMain.GetUnCheckedBillPriceDetailListShow(billPriceTempID,priceMode);
            return dt;
        }
        public DataTable GetCheckedBillPriceDetailListShow(decimal billPriceID, string priceMode)
        {
            DataTable dt = dalPriceMain.GetCheckedBillPriceDetailListShow(billPriceID, priceMode);
            return dt;
        }
        public DataTable GetMaterialNameList(decimal accountID, string sto_ID, bool showMaterial, bool showMaxUnit)
        {
            DataTable dt = dalPriceMain.GetMaterialNameList(accountID, sto_ID,showMaterial,showMaxUnit);
            return dt;
        }
        public DataTable GetMaterialSeqList(decimal accountID, decimal materialID)
        {
            DataTable dt = dalPriceMain.GetMaterialSeqList(accountID, materialID);
            return dt;
        }
        public bool insertBpriceTempDetail(decimal ID, decimal? SEQ, decimal MAT_ID, decimal MAT_SEQ, decimal NUMS, decimal RETAIL_PRICE_O, decimal RETAIL_PRICE_N, decimal TRADE_PRICE_O, decimal TRADE_PRICE_N, decimal MAX_PRICE_O, decimal MAX_PRICE_N, string BATCH_NO, DateTime EXPIRY_DATE, string REMARK)
        {
            bool Bool = dalPriceMain.insertBpriceTempDetail(ID,SEQ,MAT_ID,MAT_SEQ,NUMS,RETAIL_PRICE_O,RETAIL_PRICE_N,TRADE_PRICE_O,TRADE_PRICE_N,MAX_PRICE_O,MAX_PRICE_N,BATCH_NO,EXPIRY_DATE,REMARK);
            return Bool;
        }
        public decimal? MaxSeq(decimal ID)
        {
            decimal? FirstValue = dalPriceMain.MaxSeq(ID);
            return FirstValue;
        }
        public bool updateBpriceTempDetail(decimal ID, decimal? SEQ, decimal NUMS, decimal RETAIL_PRICE_N, string REMARK)
        {
            bool Bool = dalPriceMain.updateBpriceTempDetail(ID, SEQ, NUMS, RETAIL_PRICE_N, REMARK);
            return Bool;
        }
        public bool deleteBpriceTempDetail(decimal ID, decimal? SEQ)
        {
            bool Bool = dalPriceMain.deleteBpriceTempDetail(ID, SEQ);
            return Bool;
        }
        public bool CheckList(decimal ID)
        {
            bool Bool = dalPriceMain.CheckList(ID);
            return Bool;
        }
    }
}
