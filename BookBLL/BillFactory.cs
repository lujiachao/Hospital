using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;

namespace BookBLL
{
    public class BillFactory
    {
        DalFactory dalFactory = new DalFactory();
        public DataTable BillGetAccount()
        {
            DataTable dt = dalFactory.DalGetAccount();
            return dt;
        }
        public DataTable BillGetFactory(string Code)
        {
            DataTable dt = dalFactory.DalGetFactory(Code);
            return dt;
        }
        public bool BillCheckCode(string CODE)
        {
            bool code = dalFactory.DalCheckCode(CODE);
            return code;
        }
        public bool insertFactory(string code, string name, string shortname, string region, string legal_person, string linkman, string telephone, string address, string zipcode, string email, string licence, string Date, string state, string remark, string acc_id)
        {
            bool Bool = dalFactory.insertFactory(code, name, shortname,region,legal_person,linkman,telephone,address,zipcode,email,licence,Date,state,remark,acc_id);
            return Bool;
        }
        public bool updateFactory(string code, string name, string shortname, string region, string legal_person, string linkman, string telephone, string address, string zipcode, string email, string licence, string Date, string state, string remark)
        {
            bool Bool = dalFactory.updateFactory(code, name, shortname, region, legal_person, linkman, telephone, address, zipcode, email, licence, Date, state, remark);
            return Bool;
        }
        public bool deleteFactory(string code)
        {
            bool Bool = dalFactory.deleteFactory(code);
            return Bool;
        }
    }

}
