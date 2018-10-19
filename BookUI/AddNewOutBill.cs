using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using BookUI.Util;
using BookBLL;

namespace BookUI
{
    public partial class AddNewOutBill : Form
    {
        decimal _Code;
        BillImageComBox billImageComBox = new BillImageComBox();
        BillOutMain billOutMain = new BillOutMain();
        public AddNewOutBill(decimal _nameCode)
        {
            InitializeComponent();
            _Code = _nameCode;
        }
        //账册改变，绑定对应的出库分类
        private void imageComboBoxEdit2_EditValueChanged(object sender, EventArgs e)
        {
            decimal acc_ID = decimal.Parse(imageComboBoxEdit2.EditValue.ToString());
            this.FuzhuBinType(this.imageComboBoxEdit3, acc_ID);
        }
        /// <summary>
        /// 出库分类绑定
        /// </summary>
        public void FuzhuBinType(ImageComboBoxEdit imageComboBoxEdit, decimal acc_ID)
        {
            string stateIn = string.Empty;
            imageComboBoxEdit.Properties.Items.Clear();
            DataTable dt = billImageComBox.DalImageComboxBoutType(acc_ID);
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
        //Load函数
        private void AddNewOutBill_Load(object sender, EventArgs e)
        {
            this.FuzhuStorage(this.imageComboBoxEdit1);
            this.FuzhuAccount(this.imageComboBoxEdit2);
            this.FuzhuName(this.imageComboBoxEdit6);
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
        //出库分类改变事件
        private void imageComboBoxEdit3_EditValueChanged(object sender, EventArgs e)
        {
            decimal id = Convert.ToDecimal(imageComboBoxEdit3.EditValue);
            char target = billOutMain.getTarget(id);
            if (target == '1')
            {
                imageComboBoxEdit4.EditValue = "1";
            }
            else
            {
                imageComboBoxEdit4.EditValue = "2";
            }
        }
        //出库方式改变事件
        private void imageComboBoxEdit4_EditValueChanged(object sender, EventArgs e)
        {
            this.FuzhuCommpany(this.imageComboBoxEdit5, imageComboBoxEdit4.EditValue.ToString());
        }
        /// <summary>
        /// 往来单位数据绑定
        /// </summary>
        public void FuzhuCommpany(ImageComboBoxEdit imageComboBoxEdit, string TargetType)
        {
            if (TargetType == "2")
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
            else if (TargetType == "1")
            {
                string stateIn = string.Empty;
                imageComboBoxEdit.Properties.Items.Clear();
                DataTable dt = billImageComBox.DalImageComboxStorageClass(Convert.ToDecimal(imageComboBoxEdit1.EditValue), Convert.ToDecimal(imageComboBoxEdit2.EditValue));
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
        }
        //生成按钮功能实现
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (textEdit2.EditValue == null || textEdit2.EditValue.ToString() == "")
            {
                MessageBoxUtil.ShowWarning("单号不可为空");
                textEdit2.Focus();
                return;
            }
            string RQ = DateTime.Now.ToString("yyyyMMdd");
            decimal? ID = billOutMain.GetMaxID(RQ);
            decimal sto_id = Convert.ToDecimal(imageComboBoxEdit1.EditValue);
            string sto_name = imageComboBoxEdit1.Text;
            decimal acc_id = Convert.ToDecimal(imageComboBoxEdit2.EditValue);
            string acc_name = imageComboBoxEdit2.Text;
            string no = textEdit2.EditValue.ToString();
            decimal outtype = Convert.ToDecimal(imageComboBoxEdit3.EditValue);
            string out_name = imageComboBoxEdit3.Text;
            decimal outClass = Convert.ToDecimal(imageComboBoxEdit4.EditValue);
            string outClassName;
            if (outClass == 1)
            {
                outClassName = "内部仓库";
            }
            else
            {
                outClassName = "外部单位";
            }
            string outClass_name = imageComboBoxEdit4.Text;
            decimal outTarget = Convert.ToDecimal(imageComboBoxEdit5.EditValue);
            string outTarget_name = imageComboBoxEdit5.Text;
            DateTime? inTime = Convert.ToDateTime(timeEdit1.EditValue);
            decimal App_emp = Convert.ToDecimal(imageComboBoxEdit6.EditValue);
            string AppempName = imageComboBoxEdit6.Text;
            DateTime? AppTime = Convert.ToDateTime(timeEdit2.EditValue);
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
            string input_emp_name = billOutMain.GetName(_Code.ToString());
            DateTime input_time = DateTime.Now;
            char state = '1';
            char acc_bill_state = '0';
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("ID", Type.GetType("System.Decimal"));
            DataColumn dc2 = new DataColumn("STO_ID", Type.GetType("System.String"));
            DataColumn dc3 = new DataColumn("ACC_ID", Type.GetType("System.String"));
            DataColumn dc4 = new DataColumn("NO", Type.GetType("System.String"));
            DataColumn dc5 = new DataColumn("CLASS", Type.GetType("System.String"));
            DataColumn dc6 = new DataColumn("Get_Out", Type.GetType("System.String"));
            DataColumn dc7 = new DataColumn("COM_ID", Type.GetType("System.String"));
            DataColumn dc8 = new DataColumn("INTIME", Type.GetType("System.DateTime"));
            DataColumn dc9 = new DataColumn("APP_EMP", Type.GetType("System.String"));
            DataColumn dc10 = new DataColumn("APP_TIME", Type.GetType("System.DateTime"));
            DataColumn dc11 = new DataColumn("INPUT_EMP", Type.GetType("System.String"));
            DataColumn dc12 = new DataColumn("INPUT_TIME", Type.GetType("System.DateTime"));
            DataColumn dc13 = new DataColumn("STATE", Type.GetType("System.String"));
            DataColumn dc14 = new DataColumn("REMARK", Type.GetType("System.String"));
            DataColumn dc15 = new DataColumn("ACC_BILL_STATE", Type.GetType("System.String"));
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
            DataRow dr = dt.NewRow();
            dr["ID"] = ID;
            dr["STO_ID"] = sto_name;
            dr["ACC_ID"] = acc_name;
            dr["NO"] = no;
            dr["CLASS"] = out_name;
            dr["GET_OUT"] = outClassName;
            dr["COM_ID"] = outTarget_name;
            dr["INTIME"] = inTime;
            dr["APP_EMP"] = AppempName;
            dr["APP_TIME"] = AppTime;
            dr["INPUT_EMP"] = input_emp_name;
            dr["INPUT_TIME"] = input_time;
            dr["STATE"] = "审批未提交";
            dr["REMARK"] = remark;
            dr["ACC_BILL_STATE"] = "会计未入库";
            dt.Rows.Add(dr);
            gridControl1.DataSource = dt;
        }

        private void imageComboBoxEdit5_EditValueChanged(object sender, EventArgs e)
        {
            if (imageComboBoxEdit5.Text == "清空")
            {
                imageComboBoxEdit5.EditValue = null;
            }
        }
        //刷新功能的实现
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            textEdit2.EditValue = null;
            imageComboBoxEdit5.SelectedIndex = -1;
            timeEdit1.EditValue = "";
            imageComboBoxEdit6.SelectedIndex = -1;
            timeEdit2.EditValue = "";
            textEdit3.EditValue = "";
        }
        //保存按钮的实现
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string RQ = DateTime.Now.ToString("yyyyMMdd");
            decimal? ID = billOutMain.GetMaxID(RQ);
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
                MessageBoxUtil.ShowWarning("出库分类不可为空");
                imageComboBoxEdit3.Focus();
                return;
            }
            decimal outtype = Convert.ToDecimal(imageComboBoxEdit3.EditValue);//出库分类
            if (imageComboBoxEdit4.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowWarning("出库方式不可为空");
                imageComboBoxEdit4.Focus();
                return;
            }
            decimal outClass = Convert.ToDecimal(imageComboBoxEdit4.EditValue);//出库方式
            if (imageComboBoxEdit5.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowWarning("出库目标不可为空");
                imageComboBoxEdit5.Focus();
                return;
            }
            decimal outTarget = Convert.ToDecimal(imageComboBoxEdit5.EditValue);//出库目标
            if (Convert.ToString(timeEdit1.EditValue) == "0001/1/1 0:00:00")
            {
                MessageBoxUtil.ShowWarning("入账时间不可为空");
                timeEdit1.Focus();
                return;
            }
            DateTime? inTime = Convert.ToDateTime(timeEdit1.EditValue);
            decimal App_emp = Convert.ToDecimal(imageComboBoxEdit6.EditValue);//申请人
            DateTime? AppTime = Convert.ToDateTime(timeEdit2.EditValue);//申请时间
            string remark;
            if (textEdit3.EditValue == null || textEdit3.EditValue.ToString() == "")
            {
                remark = "";
            }
            else
            {
                remark = textEdit3.EditValue.ToString();
            }
            decimal input_emp = Convert.ToDecimal(_Code);//录入人
            DateTime input_time = DateTime.Now;//录入时间
            char state = '1';
            char acc_bill_state = '0';
            if (billOutMain.insertBOutTempMain(ID, sto_id, acc_id, no, outtype, outTarget, inTime, App_emp, AppTime, input_emp, input_time,state,remark,acc_bill_state))
            {
                MessageBoxUtil.ShowInformation("新增出库主单成功");
            }
            else
            {
                MessageBoxUtil.ShowWarning("新增出库主单失败");
            }
        }
    }
}
