using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;

namespace BookBLL
{
    public class BillReportOut
    {
        DalReportOut dalReportOut = new DalReportOut();
        public char getTarget(decimal ID)
        {
            char target = dalReportOut.getTarget(ID);
            return target;
        }
        //主单查询
        public DataTable GetCheckedBillOutMainListOutCommShow(decimal storageID, decimal? accountID, decimal? bOutTypeID, decimal? targetID, DateTime? startYearMonthDay, DateTime? endYearMonthDay, string searchState)
        {
            DataTable dt = dalReportOut.GetCheckedBillOutMainListOutCommShow(storageID,accountID,bOutTypeID,targetID,startYearMonthDay,endYearMonthDay,searchState);
            return dt;
        }
    }
}
