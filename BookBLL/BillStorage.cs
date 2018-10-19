using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;

namespace BookBLL
{   
    public class BillStorage
    {
        DalStorage dalStorage = new DalStorage();
        public DataTable BillGetStorage()
        {
            DataTable dt = dalStorage.DalGetStorage();
            return dt;
        }
        public DataTable BillAccount(string code)
        {
            DataTable dt = dalStorage.DalAccount(code);
            return dt;
        }
        //上级库
        public DataTable BillTopStorage(string to_id, string acc_id)
        {
            DataTable dt = dalStorage.DalTopStorage(to_id,acc_id);
            return dt;
        }
        //下级库
        public DataTable BillDownStorage(string sto_id, string acc_id)
        {
            DataTable dt = dalStorage.DalDownStorage(sto_id, acc_id);
            return dt;
        }
        //人员绑定
        public DataTable BillEmp(string sto_id)
        {
            DataTable dt = dalStorage.DalEmp(sto_id);
            return dt;
        }
        public bool insertStorage(string code, string name, string grade, string type, string tolink, string inputcode1, string inputcode2, string state, string remark)
        {
            bool Bool = dalStorage.insertStorage(code, name, grade, type, tolink, inputcode1, inputcode2, state, remark);
            return Bool;
        }
        public bool BillCheckCode(string CODE)
        {
            bool code = dalStorage.DalCheckCode(CODE);
            return code;
        }
        public bool updateStorage(string code, string name, string grade, string type, string tolink, string inputcode1, string inputcode2, string state, string remark)
        {
            bool Bool = dalStorage.updateStorage(code, name, grade, type, tolink, inputcode1, inputcode2, state, remark);
            return Bool;
        }
        public bool deleteStorage(string code)
        {
            bool Bool = dalStorage.deleteStorage(code);
            return Bool;
        }
        public bool deleteStorageEmp(string sto_id,string emp_id)
        {
            bool Bool = dalStorage.deleteStorageEmp(sto_id,emp_id);
            return Bool;
        }
        public bool insertStorageEmp(string sto_id, string emp_id)
        {
            bool Bool = dalStorage.insertStorageEmp(sto_id, emp_id);
            return Bool;
        }
        public bool BillRelation(string sto_id, string to_id, string acc_id)
        {
            bool Bool = dalStorage.DalRelation(sto_id, to_id, acc_id);
            return Bool;
        }
        public bool insertRelation(string sto_id, string to_id, string acc_id)
        {
            bool Bool = dalStorage.insertRelation(sto_id, to_id, acc_id);
            return Bool;
        }
        public bool deleteRelation(string sto_id, string to_id, string acc_id)
        {
            bool Bool = dalStorage.deleteRelation(sto_id, to_id, acc_id);
            return Bool;
        }
    }

}
