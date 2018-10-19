using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;

namespace BookBLL
{
    public class BillReportIn
    {
        DalReportIn dalReportIn = new DalReportIn();
        public DataTable GetBillInStatList(decimal sto_ID, decimal acc_ID, decimal? inTypeID, string invoiceState, DateTime? startYearMonthDay, DateTime? endYearMonthDay, decimal? com_ID, string searchState)
        {
            DataTable dt = dalReportIn.GetBillInStatList(sto_ID, acc_ID, inTypeID, invoiceState, startYearMonthDay, endYearMonthDay, com_ID, searchState);
            return dt;
        }
    }
}
