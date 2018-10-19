using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BookBLL;
using BookUI.Util;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;

namespace BookUI
{
    public partial class AddNewBill : Form
    {
        string _Code ;
        BillInMain billInMain = new BillInMain();
        BillImageComBox billImageComBox = new BillImageComBox();
        public AddNewBill(string Code)
        {
            InitializeComponent();
            _Code = Code;
        }
        /// <summary>
        /// 账册imagecombox数据绑定
        /// </summary>
        public void FuzhuStorage(ImageComboBoxEdit imageComboBoxEdit)
        {
            string stateIn = string.Empty;
            imageComboBoxEdit.Properties.Items.Clear();
            DataTable dt = billImageComBox.BillImageComboxStorage();
            foreach (DataRow dr in dt.Rows)
            {
                imageComboBoxEdit.Properties.Items.Add(new ImageComboBoxItem(dr["NAME"].ToString(),
                                                                                        dr["CODE"].ToString(),
                                                                                        -1));
            }
            if (imageComboBoxEdit.Properties.Items.Count > 0)
            {
                imageComboBoxEdit.SelectedIndex = 0;
            }
            else
            {
                MessageBoxUtil.ShowInformation("数据丢失");
            }
        }
        /// <summary>
        /// 账册imagecombox数据绑定
        /// </summary>
        public void FuzhuAccount(ImageComboBoxEdit imageComboBoxEdit)
        {
            string stateIn = string.Empty;
            imageComboBoxEdit.Properties.Items.Clear();
            DataTable dt = billImageComBox.BillImageComboxAccount();
            foreach (DataRow dr in dt.Rows)
            {
                imageComboBoxEdit.Properties.Items.Add(new ImageComboBoxItem(dr["NAME"].ToString(),
                                                                                        dr["CODE"].ToString(),
                                                                                        -1));
            }
            if (imageComboBoxEdit.Properties.Items.Count > 0)
            {
                imageComboBoxEdit.SelectedIndex = 0;
            }
            else
            {
                MessageBoxUtil.ShowInformation("数据丢失");
            }
        }
        private void AddNewBill_Load(object sender, EventArgs e)
        {
            this.FuzhuStorage(this.imageComboBoxEdit1);
            this.FuzhuAccount(this.imageComboBoxEdit2);
            this.FuzhuCommpany(this.imageComboBoxEdit4);
            this.FuzhuName(this.imageComboBoxEdit6);
        }
        private void imageComboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal acc_ID = Convert.ToDecimal(imageComboBoxEdit2.EditValue);
            this.FuzhuBinType(this.imageComboBoxEdit3, acc_ID);
        }
        private void imageComboBoxEdit2_EditValueChanged(object sender, EventArgs e)
        {
            //decimal acc_ID = Convert.ToDecimal(imageComboBoxEdit3.EditValue);
            //this.FuzhuBinType(this.imageComboBoxEdit3, acc_ID);
        }
        // <summary>
        /// 人员绑定
        /// </summary>
        public void FuzhuName(ImageComboBoxEdit imageComboBoxEdit)
        {
            string stateIn = string.Empty;
            imageComboBoxEdit.Properties.Items.Clear();
            DataTable dt = billImageComBox.BillImageCombox();
            foreach (DataRow dr in dt.Rows)
            {
                imageComboBoxEdit.Properties.Items.Add(new ImageComboBoxItem(dr["NAME"].ToString(),
                                                                                        dr["CODE"].ToString(),
                                                                                        -1));
            }
            if (imageComboBoxEdit.Properties.Items.Count > 0)
            {
                imageComboBoxEdit.SelectedIndex = -1;
            }
            else
            {
                MessageBoxUtil.ShowInformation("数据丢失");
            }
        }
        /// <summary>
        /// 入库分类绑定
        /// </summary>
        public void FuzhuBinType(ImageComboBoxEdit imageComboBoxEdit, decimal acc_ID)
        {
            string stateIn = string.Empty;
            imageComboBoxEdit.Properties.Items.Clear();
            DataTable dt = billImageComBox.DalImageComboxBinType(acc_ID);
            foreach (DataRow dr in dt.Rows)
            {
                imageComboBoxEdit.Properties.Items.Add(new ImageComboBoxItem(dr["NAME"].ToString(),
                                                                                        dr["ID"].ToString(),
                                                                                        -1));
            }
            if (imageComboBoxEdit.Properties.Items.Count > 0)
            {
                imageComboBoxEdit.SelectedIndex = 0;
            }
            else
            {
                MessageBoxUtil.ShowInformation("数据丢失");
            }
        }
        /// <summary>
        /// 往来单位数据绑定
        /// </summary>
        public void FuzhuCommpany(ImageComboBoxEdit imageComboBoxEdit)
        {
            string stateIn = string.Empty;
            imageComboBoxEdit.Properties.Items.Clear();
            DataTable dt = billImageComBox.BillImageComboxCompany1();
            dt.Rows.Add(0, "清空");
            foreach (DataRow dr in dt.Rows)
            {
                imageComboBoxEdit.Properties.Items.Add(new ImageComboBoxItem(dr["NAME"].ToString(),
                                                                                        dr["ID"].ToString(),
                                                                                        -1));
            }
            if (imageComboBoxEdit.Properties.Items.Count > 0)
            {
                imageComboBoxEdit.SelectedIndex = -1;
            }
            else
            {
                MessageBoxUtil.ShowInformation("数据丢失");
            }
        }
        //生成按钮功能实现
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            decimal sto_id = Convert.ToDecimal(imageComboBoxEdit1.EditValue);
            string sto_name = imageComboBoxEdit1.Text;
            decimal acc_id = Convert.ToDecimal(imageComboBoxEdit2.EditValue);
            string acc_name = imageComboBoxEdit2.Text;
            if (textEdit2.EditValue == null || textEdit2.EditValue.ToString() == "")
            {
                MessageBoxUtil.ShowWarning("单号不可为空");
                textEdit2.Focus();
                return;
            }
            string no = textEdit2.EditValue.ToString();
            decimal intype = Convert.ToDecimal(imageComboBoxEdit3.EditValue);
            string class_name = imageComboBoxEdit3.Text;
            decimal com_id = Convert.ToDecimal(imageComboBoxEdit4.EditValue);
            string com_name = imageComboBoxEdit4.Text;
            char incoice_state = Convert.ToChar(imageComboBoxEdit5.EditValue);
            string invoice_name = imageComboBoxEdit5.Text;
            decimal nums = Convert.ToDecimal(textEdit1.EditValue);
            DateTime inTime = Convert.ToDateTime(timeEdit1.EditValue);
            decimal buy_emp = Convert.ToDecimal(imageComboBoxEdit6.EditValue);
            string buy_emp_name = imageComboBoxEdit6.Text;
            DateTime buy_time = Convert.ToDateTime(timeEdit2.EditValue);
            string remark;
            if (textEdit3.EditValue == null || textEdit3.EditValue.ToString() == "")
            {
                remark = "";
            }
            else
            {
                remark = textEdit3.EditValue.ToString();
            }
            decimal input_emp = Convert.ToDecimal(_Code);
            string input_emp_name = billInMain.GetName(_Code);
            DateTime input_time = DateTime.Now;
            char state = '1';
            char acc_bill_state = '0';
            string RQ = DateTime.Now.ToString("yyyyMMdd");
            decimal? ID = billInMain.GetMaxID(RQ);
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("ID", Type.GetType("System.Decimal"));
            DataColumn dc2 = new DataColumn("STO_ID", Type.GetType("System.String"));
            DataColumn dc3 = new DataColumn("ACC_ID", Type.GetType("System.String"));
            DataColumn dc4 = new DataColumn("NO", Type.GetType("System.String"));
            DataColumn dc5 = new DataColumn("CLASS", Type.GetType("System.String"));
            DataColumn dc6 = new DataColumn("COM_ID", Type.GetType("System.String"));
            DataColumn dc7 = new DataColumn("INVOICE_STATE",Type.GetType("System.String"));
            DataColumn dc8 = new DataColumn("NUMS", Type.GetType("System.Decimal"));
            DataColumn dc9 = new DataColumn("INTIME",Type.GetType("System.DateTime"));
            DataColumn dc10 = new DataColumn("BUY_EMP", Type.GetType("System.String"));
            DataColumn dc11 = new DataColumn("BUY_TIME", Type.GetType("System.DateTime"));
            DataColumn dc12 = new DataColumn("INPUT_EMP", Type.GetType("System.String"));
            DataColumn dc13 = new DataColumn("INPUT_TIME", Type.GetType("System.DateTime"));
            DataColumn dc14 = new DataColumn("STATE", Type.GetType("System.String"));
            DataColumn dc15 = new DataColumn("REMARK", Type.GetType("System.String"));
            DataColumn dc16 = new DataColumn("ACC_BILL_STATE", Type.GetType("System.String"));
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            dt.Columns.Add(dc5);
            dt.Columns.Add(dc6);
            dt.Columns.Add(dc7);
            dt.Columns.Add(dc8);
            dt.Columns.Add(dc9);
            dt.Columns.Add(dc10);
            dt.Columns.Add(dc11);
            dt.Columns.Add(dc12);
            dt.Columns.Add(dc13);
            dt.Columns.Add(dc14);
            dt.Columns.Add(dc15);
            dt.Columns.Add(dc16);
            DataRow dr = dt.NewRow();
            dr["ID"] = ID;
            dr["STO_ID"] = sto_name;
            dr["ACC_ID"] = acc_name;
            dr["NO"] = no;
            dr["CLASS"] = class_name;
            dr["COM_ID"] = com_name;
            dr["INVOICE_STATE"] = invoice_name;
            dr["NUMS"] = nums;
            dr["INTIME"] = inTime;
            dr["BUY_EMP"] = buy_emp_name;
            dr["BUY_TIME"] = buy_time;
            dr["INPUT_EMP"] = input_emp_name;
            dr["INPUT_TIME"] = input_time;
            dr["STATE"] = "审批未提交";
            dr["REMARK"] = remark;
            dr["ACC_BILL_STATE"] = "会计未入库";
            dt.Rows.Add(dr);
            gridControl1.DataSource = dt;
        }
        //保存按钮的实现
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (imageComboBoxEdit1.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowWarning("入库仓库不可为空");
                imageComboBoxEdit1.Focus();
                return;
            }
            decimal sto_id = Convert.ToDecimal(imageComboBoxEdit1.EditValue);
            if (imageComboBoxEdit2.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowWarning("入库账册不可为空");
                imageComboBoxEdit2.Focus();
                return;
            }
            decimal acc_id = Convert.ToDecimal(imageComboBoxEdit2.EditValue);
            if (textEdit2.EditValue == null || textEdit2.EditValue.ToString() == "")
            {
                MessageBoxUtil.ShowWarning("单号不可为空");
                textEdit2.Focus();
                return;
            }
            string no = textEdit2.EditValue.ToString();
            if (imageComboBoxEdit3.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowWarning("入库分类不可为空");
                imageComboBoxEdit3.Focus();
                return;
            }
            decimal intype = Convert.ToDecimal(imageComboBoxEdit3.EditValue);
            if (imageComboBoxEdit4.SelectedIndex == -1 ||imageComboBoxEdit4.EditValue == null||imageComboBoxEdit4.EditValue.ToString() ==string.Empty)
            {
                MessageBoxUtil.ShowWarning("进货单位不可为空");
                imageComboBoxEdit4.Focus();
                return;
            }
            decimal com_id = Convert.ToDecimal(imageComboBoxEdit4.EditValue);
            if (imageComboBoxEdit5.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowWarning("发票状态不可为空");
                imageComboBoxEdit5.Focus();
                return;
            }
            char invoice_state = Convert.ToChar(imageComboBoxEdit5.EditValue);
            if (textEdit1.EditValue == null || textEdit1.EditValue.ToString() == string.Empty)
            {
                MessageBoxUtil.ShowWarning("单据数不可为空");
                textEdit1.Focus();
                return;
            }
            if (textEdit1.EditValue.ToString().Length > 2)
            {
                MessageBoxUtil.ShowWarning("单据数不可超过99");
                textEdit1.EditValue = null;
                textEdit1.Focus();
                return;
            }
            decimal nums = Convert.ToDecimal(textEdit1.EditValue);
            if (Convert.ToString(timeEdit1.EditValue) == "0001/1/1 0:00:00")
            {
                MessageBoxUtil.ShowWarning("入账时间不可为空");
                timeEdit1.Focus();
                return;
            }
            DateTime inTime = Convert.ToDateTime(timeEdit1.EditValue);
            decimal buy_emp = Convert.ToDecimal(imageComboBoxEdit6.EditValue);
            DateTime buy_time = Convert.ToDateTime(timeEdit2.EditValue);
            string remark;
            if (textEdit3.EditValue == null || textEdit3.EditValue.ToString() == "")
            {
                remark = "";
            }
            else
            {
                remark = textEdit3.EditValue.ToString();
            }
            decimal input_emp = Convert.ToDecimal(_Code);
            DateTime input_time = DateTime.Now;
            char state = '1';
            char acc_bill_state = '0';
            string RQ = DateTime.Now.ToString("yyyyMMdd");
            decimal? ID = billInMain.GetMaxID(RQ);
            if (billInMain.insertBinTempMain(ID, sto_id, acc_id, no, intype, com_id, invoice_state, nums, inTime, buy_emp, buy_time, input_emp, input_time, state, remark, acc_bill_state))
            {
                MessageBoxUtil.ShowInformation("新增入库主单成功");
            }
            else
            {
                MessageBoxUtil.ShowWarning("新增入库主单失败");
            }
        }

        private void imageComboBoxEdit4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (imageComboBoxEdit4.Text == "清空")
            {
                imageComboBoxEdit4.EditValue = null;
            }
        }
        //刷新按钮的实现
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            textEdit2.Text = "";
            imageComboBoxEdit3.SelectedIndex = -1;
            imageComboBoxEdit4.SelectedIndex = -1;
            imageComboBoxEdit5.SelectedIndex = -1;
            textEdit1.Text = "";
            timeEdit1.EditValue = null;
            imageComboBoxEdit6.SelectedIndex = -1;
            timeEdit2.EditValue = null;
            textEdit3.Text = "";
        }
    }
}
