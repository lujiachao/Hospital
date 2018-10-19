using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;

namespace BookBLL
{
    public class BillAccount
    {
        DalAccount dalAccount = new DalAccount();
        public DataTable BillGetAccount(string state)
        {
            DataTable dt = dalAccount.DalGetAccount(state);
            return dt;
        }
        public DataTable BillGetEmpID(string code)
        {
            DataTable dt = dalAccount.DalGetEmpID(code);
            return dt;
        }
        public bool BillCheckCode(string CODE)
        {
            bool code = dalAccount.DalCheckCode(CODE);
            return code;
        }
        public bool insertAccount(string code, string name, string Price_rule, string Manage_type, string Use_stock, string inputcode1, string inputcode2, string State, string remark, string acc_emp_id, string danju)
        {
            bool Bool = dalAccount.insertAccount(code, name, Price_rule, Manage_type, Use_stock, inputcode1, inputcode2, State, remark,acc_emp_id,danju);
            return Bool;
        }
        public string BillPY(string type, string mark)
        {
            string FirstValue = dalAccount.DalPY(type, mark);
           return FirstValue;
        }
        public bool deleteAccount(string code)
        {
            bool Bool = dalAccount.deleteAccount(code);
            return Bool; 
        }
        public bool updateAccount(string code, string name, string Price_rule, string Manage_type, string Use_stock, string inputcode1, string inputcode2, string State, string remark, string acc_emp_id, string danju)
        {
            bool Bool = dalAccount.updateAccount(code, name, Price_rule, Manage_type, Use_stock, inputcode1, inputcode2, State, remark, acc_emp_id, danju);
            return Bool;
        }
        public bool insertPubEmp(string code,string name)
        {
            bool Bool = dalAccount.insertPubEmp(code, name);
            return Bool;
        }
        public bool insertAccEmp(string Acc_id, string Emp_id, string role, string department)
        {
            bool Bool = dalAccount.insertAccEmp(Acc_id, Emp_id, role, department);
            return Bool;
        }
        public bool updatePubEmp(string code, string name)
        {
            bool Bool = dalAccount.updatePubEmp(code, name);
            return Bool;
        }
        public bool updateAccEmp(string Acc_id, string Emp_id, string role, string department)
        {
            bool Bool = dalAccount.updateAccEmp(Acc_id,Emp_id,role,department);
            return Bool;
        }
        public bool deletePubEmp(string code)
        {
            bool Bool = dalAccount.deletePubEmp(code);
            return Bool;
        }
        public bool deleteAccEmp(string Acc_id, string Emp_id)
        {
            bool Bool = dalAccount.deleteAccEmp(Acc_id, Emp_id);
            return Bool;
        }
    }
}
