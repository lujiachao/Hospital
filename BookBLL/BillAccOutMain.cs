using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;

namespace BookBLL
{
    public class BillAccOutMain
    {
        DalAccOutMain dalAccOutMain = new DalAccOutMain();
        public char getTarget(decimal ID)
        {
            char target = dalAccOutMain.getTarget(ID);
            return target;
        }
        public DataTable GetCheckedBillOutMainListOutCommShow(decimal storageID, decimal? accountID, decimal? bOutTypeID, DateTime? begTime, DateTime? endTime, string matState, decimal? targetID, char TargetType, char DISPATCHING)
        {
            DataTable dt = dalAccOutMain.GetCheckedBillOutMainListOutCommShow(storageID, accountID, bOutTypeID, begTime, endTime, matState, targetID, TargetType, DISPATCHING);
            return dt;
        }
        public DataTable GetCheckedBillOutDetailListShow(decimal bOutDetailID)
        {
            DataTable dt = dalAccOutMain.GetCheckedBillOutDetailListShow(bOutDetailID);
            return dt;
        }
        public bool updateBoutTempMain(decimal bOutDetailID, decimal checkEmp,char acc_bill_state)
        {
            bool Bool = dalAccOutMain.updateBoutTempMain(bOutDetailID, checkEmp, acc_bill_state);
            return Bool;
        }
        public bool updateStock(decimal NUMS, decimal Sto_ID, decimal Acc_ID, decimal Mat_ID, decimal Mat_Seq)
        {
            bool Bool = dalAccOutMain.updateStock(NUMS,Sto_ID,Acc_ID,Mat_ID,Mat_Seq);
            return Bool;
        }
    }
}
