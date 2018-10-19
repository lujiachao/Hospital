using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;

namespace BookBLL
{
    public class BillOutMain
    {
        DalOutMain dalOutMain = new DalOutMain();
        public char getTarget(decimal ID)
        {
            char target = dalOutMain.getTarget(ID);
            return target;
        }
        public DataTable GetCheckedBillOutMainListOutCommShow(decimal storageID, decimal? accountID, decimal? bOutTypeID, DateTime? begTime, DateTime? endTime, string matState, decimal? targetID, char TargetType, char DISPATCHING)
        {
            DataTable dt = dalOutMain.GetCheckedBillOutMainListOutCommShow(storageID,accountID,bOutTypeID,begTime,endTime,matState,targetID,TargetType,DISPATCHING);
            return dt;
        }
        public DataTable GetCheckedBillOutDetailListShow(decimal bOutDetailID)
        {
            DataTable dt = dalOutMain.GetCheckedBillOutDetailListShow(bOutDetailID);
            return dt;
        }
        public decimal? GetMaxID(string RQ)
        {
            decimal? ID = dalOutMain.GetMaxID(RQ);
            return ID;
        }
        public string GetName(string Code)
        {
            string name = dalOutMain.GetName(Code);
            return name;
        }
        public bool insertBOutTempMain(decimal? id, decimal sto_id, decimal acc_id, string no, decimal outtype, decimal outTarget, DateTime? intime, decimal app_emp, DateTime? app_time, decimal input_emp, DateTime input_time, char state, string remark, char acc_bill_state)
        {
            bool Bool = dalOutMain.insertBOutTempMain(id,sto_id,acc_id,no,outtype,outTarget,intime,app_emp,app_time,input_emp,input_time,state,remark,acc_bill_state);
            return Bool;
        }
        public bool insertBoutTempDetail(decimal id, decimal seq, decimal mat_id, decimal mat_seq, decimal nums, decimal buying_price, decimal retail_price, decimal trade_price, string batch_no, DateTime expipy_date, decimal return_reason, string remark)
        {
            bool Bool = dalOutMain.insertBoutTempDetail(id,seq,mat_id,mat_seq,nums,buying_price,retail_price,trade_price,batch_no,expipy_date,return_reason,remark);
            return Bool;
        }
        public bool deleteBoutTempDetail(decimal id, decimal seq)
        {
            bool Bool = dalOutMain.deleteBoutTempDetail(id, seq);
            return Bool;
        }
        public bool updateBoutTempDetail(decimal ID, decimal SEQ, decimal NUMS, decimal BUYING_PRICE)
        {
            bool Bool = dalOutMain.updateBoutTempDetail(ID,SEQ,NUMS,BUYING_PRICE);
            return Bool;
        }
        public bool deleteBoutTempMain(decimal id)
        {
            bool Bool = dalOutMain.deleteBoutTempMain(id);
            return Bool;
        }
        public bool reviewState(decimal id)
        {
            bool Bool = dalOutMain.reviewState(id);
            return Bool;
        }
        public bool examineState(decimal id, string state)
        {
            bool Bool = dalOutMain.examineState(id,state);
            return Bool;
        }
    }
}
