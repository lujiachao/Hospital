using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BookDAL;

namespace BookBLL
{

    public class BillAppTreatMain
    {
        DalAppTreatMain dalAppTreatMain = new DalAppTreatMain();
        public DataTable GetBAppTreatMainListShow(decimal accountID,
                                                                   decimal storageID,
                                                                   string state,
                                                                   DateTime? begTime,
                                                                   DateTime? endTime)
        {
            DataTable dt = dalAppTreatMain.GetBAppTreatMainListShow(accountID,storageID,state,begTime,endTime);
            return dt;
        }
        public DataTable GetBAppDetailListShow(decimal bAppID)
        {
            DataTable dt = dalAppTreatMain.GetBAppDetailListShow(bAppID);
            return dt;
        }
        public bool updateBappMain(decimal bAppID, decimal seq, decimal send_num,string remark)
        {
            bool Bool = dalAppTreatMain.updateBappMain(bAppID,seq,send_num,remark);
            return Bool;
        }
        public bool updateBappState(decimal bAppID)
        {
            bool Bool = dalAppTreatMain.updateBappState(bAppID);
            return Bool;
        }
        public bool BappOut(decimal bAppID)
        {
            bool Bool = dalAppTreatMain.BappOut(bAppID);
            return Bool;
        }
        public decimal? GetMaxID(string RQ)
        {
            decimal? ID = dalAppTreatMain.GetMaxID(RQ);
            return ID;
        }
        public bool insertBOutTempMain(decimal? id, decimal sto_id, decimal acc_id, string no, decimal outtype, decimal outTarget, DateTime? intime, decimal app_emp, DateTime? app_time, decimal input_emp, DateTime input_time, char state, string remark, char acc_bill_state)
        {
            bool Bool = dalAppTreatMain.insertBOutTempMain(id, sto_id, acc_id, no, outtype, outTarget, intime, app_emp, app_time, input_emp, input_time, state, remark, acc_bill_state);
            return Bool;
        }
        public bool insertBOutTempDetail(decimal? id, decimal seq, decimal mat_id, decimal mat_seq, decimal nums, decimal buying_price, decimal retail_price, decimal trade_price, string batch_no, string remark)
        {
            bool Bool = dalAppTreatMain.insertBOutTempDetail(id,seq,mat_id,mat_seq,nums,buying_price,retail_price,trade_price,batch_no,remark);
            return Bool;
        }
    }
}
