using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;

namespace BookBLL
{
    public class BillAccInMain
    {
        DalAccInMain dalAccInMain = new DalAccInMain();
        public DataTable GetUnCheckedBillAccInList(decimal? storageID, decimal? accountID, decimal? binTypeID,
        DateTime? begTime, DateTime? endTime, string invoiceState, string invoiceNo, decimal? companyID)
        {
            DataTable dt = dalAccInMain.GetUnCheckedBillAccInList(storageID,accountID,binTypeID,begTime,endTime,invoiceState,invoiceNo,companyID);
            return dt;
        }
        public DataTable GetCheckedBillAccInList(decimal? storageID, decimal? accountID, decimal? binTypeID,
        DateTime? begTime, DateTime? endTime, string invoiceState, string invoiceNo, decimal? companyID)
        {
            DataTable dt = dalAccInMain.GetCheckedBillAccInList(storageID, accountID, binTypeID, begTime, endTime, invoiceState, invoiceNo, companyID);
            return dt;
        }
        public DataTable GetBillAccInTempDetailListShow(decimal billInTempID,string state)
        {
            DataTable dt = dalAccInMain.GetBillAccInTempDetailListShow(billInTempID,state);
            return dt;
        }
        public DataTable GetEmployeeList()
        {
            DataTable dt = dalAccInMain.GetEmployeeList();
            return dt;
        }
        public bool updateBinDetail(decimal ID, decimal SEQ, string INVOICENO, decimal NUMS, decimal BUYING_PRICE)
        {
            bool Bool = dalAccInMain.updateBinDetail(ID, SEQ, INVOICENO, NUMS, BUYING_PRICE);
            return Bool;
        }
        public bool updateBinMain(decimal ID, decimal Check_Emp, DateTime Check_Time, decimal NUMS, decimal Sto_ID, decimal Acc_ID, decimal Mat_ID, decimal Mat_Seq)
        {
            bool Bool = dalAccInMain.updateBinMain(ID, Check_Emp, Check_Time,NUMS,Sto_ID,Acc_ID,Mat_ID,Mat_Seq);
            return Bool;
        }
    }
}
