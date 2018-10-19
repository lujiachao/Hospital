using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;

namespace BookBLL
{
    
    public class BillInMain
    {
        DalInMain dalInMain = new DalInMain();
        public DataTable GetBillInTempMainListShow(decimal? storageID, decimal? accountID, decimal? binTypeID,
                                            DateTime? begTime, DateTime? endTime, string invoiceState, string invoiceNo, decimal? companyID,string state)
        {
            DataTable dt = dalInMain.GetBillInTempMainListShow(storageID,accountID,binTypeID,begTime,endTime,invoiceState,invoiceNo,companyID,state);
            return dt;
        }
        public string GetName(string Code)
        {
            string name = dalInMain.GetName(Code);
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
            DataTable dt = dalInMain.GetBillAccInTempDetailListShow(billInTempID);
            return dt;
        }
        public decimal? GetMaxID(string RQ)
        {
            decimal? ID = dalInMain.GetMaxID(RQ);
            return ID;
        }
        public bool insertBinTempMain(decimal? id, decimal sto_id, decimal acc_id, string no, decimal bInType, decimal com_id, char invoice_state, decimal nums, DateTime intime, decimal buy_emp, DateTime buy_time, decimal input_emp, DateTime input_time, char state, string remark, char acc_bill_state)
        {
            bool Bool = dalInMain.insertBinTempMain(id,sto_id,acc_id,no,bInType,com_id,invoice_state,nums,intime,buy_emp,buy_time,input_emp,input_time,state,remark,acc_bill_state);
            return Bool;
        }
        public bool updateBinTempDetail(decimal ID, decimal SEQ, decimal NUMS, decimal BUYING_PRICE)
        {
            bool Bool = dalInMain.updateBinTempDetail(ID, SEQ, NUMS, BUYING_PRICE);
            return Bool;
        }
        public bool insertBinTempDetail(decimal id, decimal seq, string invoiceno, decimal mat_id, decimal mat_seq, decimal nums, decimal buying_price, decimal retail_price, decimal trade_price, decimal return_num, decimal return_reason, string batch_no, DateTime expipy_date, string remark)
        {
            bool Bool = dalInMain.insertBinTempDetail(id,seq,invoiceno,mat_id,mat_seq,nums,buying_price,retail_price,trade_price,return_num,return_reason,batch_no,expipy_date,remark);
            return Bool;
        }
        public bool deleteBinTempDetail(decimal id, decimal seq)
        {
            bool Bool = dalInMain.deleteBinTempDetail(id,seq);
            return Bool;
        }
        public bool deleteBinTempMain(decimal id)
        {
            bool Bool = dalInMain.deleteBinTempMain(id);
            return Bool;
        }
        public bool reviewState(decimal id)
        {
            bool Bool = dalInMain.reviewState(id);
            return Bool;
        }
        public bool examineState(decimal id, string state)
        {
            bool Bool = dalInMain.examineState(id,state);
            return Bool;
        }
        public string catchName(string Code)
        {
            string name = dalInMain.catchName(Code);
            return name;
        }
    }
}
