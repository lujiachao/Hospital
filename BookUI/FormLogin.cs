using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BookBLL;
using DevExpress.XtraEditors;
using BookUI.Util;

namespace BookUI
{
    public partial class FormLogin : DevExpress.XtraEditors.XtraForm
    {
        ReadInfoBill td = new ReadInfoBill();
        string code;
        public FormLogin()
        {
            InitializeComponent();
            
        }
        private void btnLog_Click(object sender, EventArgs e)
        {      
            string password;
            code = txtId.Text;
            password = txtPassword.Text;
            submit(code, password);
        }
        public void submit(string Code,string Password)
        {
            if (Code == "")
            {
                MessageBoxUtil.ShowWarning("账号不能为空");                
                txtId.Text = "";
                txtId.Focus();
                return;
            }
            if (Password == "")
            {
                MessageBoxUtil.ShowWarning("密码不能为空");
                txtPassword.Text = "";
                txtPassword.Focus();
                return;
            }
            int i ;
            if (int.TryParse(Code,out i) == false)
            {
                MessageBoxUtil.ShowWarning("账号只能是纯数字");
                return;
            }
            if (int.TryParse(Password, out i) == false)
            {
                MessageBoxUtil.ShowWarning("密码只能是纯数字");
                return;
            }
            if (td.checkInformation(Code, Password) == false)
            {
                MessageBoxUtil.ShowWarning("密码错误，请重试");
                txtPassword.Text = "";
                txtPassword.Focus();
            }
            else
            {
                this.Visible = false;
                new FormMain(Code).ShowDialog();
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
