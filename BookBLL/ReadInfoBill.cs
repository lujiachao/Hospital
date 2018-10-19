using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;

namespace BookBLL
{
    
    public class ReadInfoBill
    {
        ReaderInfoDAL td = new ReaderInfoDAL();
        ServiceReference1.Service1Client TClient = new BookBLL.ServiceReference1.Service1Client();
        public bool checkInformation(string Code, string Password)
        { 
             if(TClient.CheckInformation(Code,Password))
             {
                return true;
             }
             else
             {
                return false;
             }
        }
    }
}
