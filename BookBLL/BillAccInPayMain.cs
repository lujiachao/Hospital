using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;
using System.Collections;

namespace BookBLL
{
    public class BillAccInPayMain
    {
        DalBillAccInPayMain dalAccInPayMain = new DalBillAccInPayMain();
        public DataTable GetUnPayCompanyList(decimal? sto_ID, decimal? acc_ID, decimal? binTypeID,  string payState, DateTime? dateTimeStart, DateTime? dateTimeEnd)
        {
            DataTable dt = dalAccInPayMain.GetUnPayCompanyList(sto_ID, acc_ID, binTypeID, payState, dateTimeStart, dateTimeEnd);
            return dt;
        }
        public DataTable GetPayBillMainList(decimal sto_ID, decimal acc_ID, decimal companyID, string payState, DateTime? dateTimeStart, DateTime? dateTimeEnd)
        {
            DataTable dt = dalAccInPayMain.GetPayBillMainList(sto_ID, acc_ID, companyID, payState, dateTimeStart, dateTimeEnd);
            return dt;
        }
        public string Dalbalance(decimal acc_ID)
        {
            string balance = dalAccInPayMain.Dalbalance(acc_ID);
            return balance;
        }
        public bool SubmitPayInfo(decimal ID, decimal EMP)
        {
            bool Bool = dalAccInPayMain.SubmitPayInfo(ID, EMP);
            return Bool;
        }
        public bool FinishPayInfo(decimal ID, decimal EMP, decimal BUYING_PRICE_TOTAL, decimal ACC_ID)
        {
            bool Bool = dalAccInPayMain.FinishPayInfo(ID, EMP, BUYING_PRICE_TOTAL, ACC_ID);
            return Bool;
        }
    }
}
