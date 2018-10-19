using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;

namespace BookBLL
{
    public class BillAppMain
    {
        DalAppMain dalAppMain = new DalAppMain();
        public DataTable GetBAppMainListShow(decimal storageID, decimal accountID, string state,
                                                    DateTime? begTime, DateTime? endTime)
        {
            DataTable dt = dalAppMain.GetBAppMainListShow(storageID,accountID,state,begTime,endTime);
            return dt;
        }
        public DataTable GetBAppDetailListShow(decimal bAppID)
        {
            DataTable dt = dalAppMain.GetBAppDetailListShow(bAppID);
            return dt;
        }
        public decimal? GetMaxID(string RQ)
        {
            decimal? ID = dalAppMain.GetMaxID(RQ);
            return ID;
        }
        public bool insertBappMain(decimal? id, decimal sto_id, decimal target_id, decimal acc_id, decimal input_emp, string remark)
        {
            bool Bool = dalAppMain.insertBappMain(id,sto_id,target_id,acc_id,input_emp,remark);
            return Bool;
        }
        public bool updateBappMain(decimal id, decimal app_num)
        {
            bool Bool = dalAppMain.updateBappMain(id, app_num);
            return Bool;
        }
        public bool deleteBappMain(decimal id)
        {
            bool Bool = dalAppMain.deleteBappMain(id);
            return Bool;
        }
        public bool insertBappDetail(decimal id, decimal seq, decimal mat_id, decimal app_num, decimal send_num, string remark, decimal mat_seq)
        {
            bool Bool = dalAppMain.insertBappDetail(id,seq,mat_id,app_num,send_num,remark,mat_seq);
            return Bool;
        }
        public bool deleteBappDetail(decimal id, decimal seq)
        {
            bool Bool = dalAppMain.deleteBappDetail(id,seq);
            return Bool;
        }
        public bool updateBappMain(decimal id)
        {
            bool Bool = dalAppMain.updateBappMain(id);
            return Bool;
        }
    }
}
