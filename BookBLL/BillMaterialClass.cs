using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;

namespace BookBLL
{
    public class BillMaterialClass
    {
        DalMaterialClass dalMaterialClass = new DalMaterialClass();
        public DataTable BillGetAccount()
        {
            DataTable dt = dalMaterialClass.DalGetAccount();
            return dt;
        }
        public DataTable BillGetMatClass(string acc_ID)
        {
            DataTable dt = dalMaterialClass.DalGetMatClass(acc_ID);
            return dt;
        }
        public bool BillCheckCode(string CODE)
        {
            bool code = dalMaterialClass.DalCheckCode(CODE);
            return code;
        }
        public bool BillInsertMatClass(string code, string name, string pid, string inputcode1, string inputcode2, string state, string remark, string manage_type, string acc_id)
        {
            bool Bool = dalMaterialClass.insertMatClass(code,name,pid,inputcode1,inputcode2,state,remark,manage_type,acc_id);
            return Bool;
        }
        public bool updateMatClass(string code, string name, string inputcode1, string inputcode2, string state, string remark, string manage_type)
        {
            bool Bool = dalMaterialClass.updateMatClass(code, name, inputcode1, inputcode2, state, remark, manage_type);
            return Bool;
        }
        public bool deleteMatClass(string code)
        {
            bool Bool = dalMaterialClass.deleteMatClass(code);
            return Bool;
        }
    }
}
