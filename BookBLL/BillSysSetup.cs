using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;

namespace BookBLL
{
    public class BillSysSetup
    {
        DalSysSetup dalSysSetup = new DalSysSetup();
        public DataTable BillGetSysList()
        {
            DataTable dt = new DataTable();
            dt = dalSysSetup.DalGetSysList();
            return dt;
        }
        public DataTable BillImageCombox()
        {
            DataTable dt = new DataTable();
            dt = dalSysSetup.DalImageCombox();
            return dt;
        }
        public bool BillCheckCode(string CODE)
        {
            bool code = dalSysSetup.DalCheckCode(CODE);
            return code;
        }
        public string BillMaxParentID(string TYPE)
        {
            string type = dalSysSetup.DalMaxParentID(TYPE);
            return type;
        }
        public bool UpdateSysObject(string code, string isUser, string edition, string name, string Object, string version, string date, string seq, string remake)
        {
            bool Bool = dalSysSetup.UpdateSysObject(code, isUser, edition, name, Object, version, date, seq, remake);
            return Bool;
        }
        public bool InsertSysObject(string code, string isUser, string edition, string name, string Object, string version, string seq, string remake, string MaxParentID)
        {
            bool Bool = dalSysSetup.insertSysObject(code, isUser, edition, name, Object, version, seq, remake, MaxParentID);
            return Bool;
        }
        public bool DeleteSysObject(string code)
        {
            bool Bool = dalSysSetup.DeleteSysObject(code);
            return Bool;
        }
        public string BillMaxType()
        {
            string MaxValue = dalSysSetup.DalMaxType();
            return MaxValue;
        }
    }
}
