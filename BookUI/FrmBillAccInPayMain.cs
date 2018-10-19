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
using BookBLL;
using BookUI.Util;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace BookUI
{
    public partial class FrmBillAccInPayMain : Form
    {
        BillAccInPayMain billAccInPayMain = new BillAccInPayMain();
        BillImageComBox billImageComBox = new BillImageComBox();
        string EMP;
        public FrmBillAccInPayMain(string NameCode)
        {
            InitializeComponent();
            EMP = NameCode;
        }
        /// <summary>
        /// 获取设置当前 的付款状态
        /// </summary>
        private string payState;
        //Load 函数
        private void FrmBillAccInPayMain_Load(object sender, EventArgs e)
        {
            this.FuzhuAccount(this.imageComboBoxEditSearchAccountType);
            LoadPayState();
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
        //账册改变事件
        private void imageComboBoxEditSearchAccountType_EditValueChanged(object sender, EventArgs e)
        {
             decimal acc_ID = decimal.Parse(imageComboBoxEditSearchAccountType.EditValue.ToString());
             this.FuzhuBinType(this.imageComboBoxEditSearchBInType, acc_ID);
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
        /// 加载付款状态
        /// </summary>
        private void LoadPayState()
        {
            this.imageComboBoxEditPayState.Properties.Items.Clear();
            ImageComboBoxItemCollection itemColApprove =
                new ImageComboBoxItemCollection(this.imageComboBoxEditPayState.Properties);

            itemColApprove.Add(new ImageComboBoxItem("未付款", "0"));
            itemColApprove.Add(new ImageComboBoxItem("待付款", "1"));
            itemColApprove.Add(new ImageComboBoxItem("已付款", "2"));


            this.imageComboBoxEditPayState.Properties.Items.AddRange(itemColApprove);
            if (this.imageComboBoxEditPayState.Properties.Items.Count > 0)
            {
                this.imageComboBoxEditPayState.SelectedIndex = 0;
            }
        }
        //通过付款状态控制时间
        private void imageComboBoxEditPayState_SelectedValueChanged(object sender, EventArgs e)
        {
            this.dateEditInTimeStart.Enabled =
                this.dateEditInTimeEnd.Enabled = (this.imageComboBoxEditPayState.EditValue.ToString() == "2") ? true : false;
            this.dateEditInTimeStart.EditValue = null;
            this.dateEditInTimeEnd.EditValue = null;
        }
        //检索按钮点击事件
        private void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            this.RefreshFormData();
            this.textEditPayMoney.EditValue = billAccInPayMain.Dalbalance(decimal.Parse(imageComboBoxEditSearchAccountType.EditValue.ToString()));
        }
        /// <summary>
        /// 刷新数据
        /// 包括公司列表和主单列表
        /// </summary>
        private void RefreshFormData()
        {
            this.LoadCompanyList();
            LoadBillAccMain();
            this.ShowUIState();
        }
        /// <summary>
        /// 加载付款单位信息列表
        /// </summary>
        private void LoadCompanyList()
        {
            this.payState = this.imageComboBoxEditPayState.EditValue.ToString();
            decimal? sto_ID = 1;
            decimal? acc_ID = Convert.ToDecimal(this.imageComboBoxEditSearchAccountType.EditValue);
            decimal? binTypeID = Convert.ToDecimal(this.imageComboBoxEditSearchBInType.EditValue);
            DateTime? dateTimeStart = null;
            DateTime? dateTimeEnd = null;
            if (this.imageComboBoxEditPayState.EditValue.ToString() == "2")
            {
                dateTimeStart = Convert.ToDateTime(this.dateEditInTimeStart.EditValue);
                dateTimeEnd = Convert.ToDateTime(this.dateEditInTimeEnd.EditValue);
            }
            DataTable dt = billAccInPayMain.GetUnPayCompanyList(sto_ID, acc_ID, binTypeID, payState, dateTimeStart, dateTimeEnd);
            this.gridControlAccPay.DataSource = dt;
        }
        /// <summary>
        /// 加载主单列表信息
        /// </summary>
        private void LoadBillAccMain()
        {
            DataRow drCompany = this.gridViewAccPay.GetFocusedDataRow();
            this.textEditPayMoney.Text = string.Empty;
            if (drCompany != null)
            {
                this.textEditCompanyName.Text = drCompany["COMPANY_NAME"].ToString();
                this.textEditCompanyName.Tag = drCompany["COM_ID"].ToString();
                this.textEditLinkMan.Text = drCompany["LINKMEN"].ToString();
                this.textEditTelephone.Text = drCompany["TELEPHONE"].ToString();
                this.textEditBank.Text = drCompany["BANK"].ToString();
                this.textEditBankAccount.Text = drCompany["BANK_ACCOUNT"].ToString();
                DateTime? dateTimeStart = null;
                DateTime? dateTimeEnd = null;

                dateTimeStart = Convert.ToDateTime(this.dateEditInTimeStart.EditValue);
                dateTimeEnd = Convert.ToDateTime(this.dateEditInTimeEnd.EditValue);

                DataTable dtPayBillMain = billAccInPayMain.GetPayBillMainList(decimal.Parse(drCompany["STO_ID"].ToString()), decimal.Parse(drCompany["ACC_ID"].ToString()), decimal.Parse(drCompany["COM_ID"].ToString()), this.payState, dateTimeStart, dateTimeEnd);
                this.gridControlPayMain.DataSource = dtPayBillMain;
                // 未确认付款
                if (this.payState != "2")
                {
                    this.textEditPayMoney.Text = billAccInPayMain.Dalbalance(decimal.Parse(drCompany["ACC_ID"].ToString()));
                }
            }
            else
            {
                this.textEditCompanyName.Text = string.Empty;
                this.textEditCompanyName.Tag = string.Empty;
                this.textEditLinkMan.Text = string.Empty;
                this.textEditTelephone.Text = string.Empty;
                this.textEditBank.Text = string.Empty;
                this.textEditBankAccount.Text = string.Empty;

                this.gridControlPayMain.DataSource = null;
            }
        }
        /// <summary>
        /// 全选按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送源</param>
        /// <param name="e">包含了事件所需的参数</param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barButtonItem1.Caption == "全选(&A)")
            {
                for (int rowIndex = 0; rowIndex < this.gridViewPayMain.RowCount; rowIndex++)
                {
                    this.gridViewPayMain.SetRowCellValue(rowIndex, "SELECTED", 1);
                    barButtonItem1.Caption = "反选(&A)";
                }
            }
            else
            {
                for (int rowIndex = 0; rowIndex < this.gridViewPayMain.RowCount; rowIndex++)
                {
                    this.gridViewPayMain.SetRowCellValue(rowIndex, "SELECTED", 0);
                    barButtonItem1.Caption = "全选(&A)";
                }
            }
        }
        //左边列焦点改变事件
        private void gridViewAccPay_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            LoadBillAccMain();
        }
        /// <summary>
        /// 设置页面状态
        /// </summary>
        /// <remarks>付费处理页面根据检索的付费状态设置页面相关</remarks>
        public void ShowUIState()
        {
            //设置默认值
            this.barButtonItem2.Enabled = true;
            this.barButtonItem3.Enabled = true;
            this.barButtonItem1.Enabled = true;
            this.gridColumnMainSelect.Visible = true;
            this.gridColumnMainRate.Visible = true;
            this.gridColumnMainNo.Caption = "验收单号";
            this.gridColumnMainCompanyName.Caption = "供货单位";
            this.gridColumnMainBuyingPrice.Caption = "进货总额";
            this.gridColumnMainDifference.Caption = "进销差额";
            // 未付款
            if (this.imageComboBoxEditPayState.EditValue.ToString() == "0")
            {
                this.barButtonItem3.Enabled = false;
            }
            // 待付款
            else if (this.imageComboBoxEditPayState.EditValue.ToString() == "1")
            {
                this.barButtonItem2.Enabled = false;

                this.gridColumnMainNo.Caption = "待付款单号";
                this.gridColumnMainCompanyName.Caption = "待付款单位";
                this.gridColumnMainBuyingPrice.Caption = "待付款总额";
                this.gridColumnMainDifference.Caption = "差额";
            }
            // 已付款
            else if (this.imageComboBoxEditPayState.EditValue.ToString() == "2")
            {
                this.barButtonItem2.Enabled = false;
                this.barButtonItem3.Enabled = false;
                this.barButtonItem1.Enabled = false;

                this.gridColumnMainSelect.Visible = false;
                this.gridColumnMainRate.Visible = false;

                this.gridColumnMainNo.Caption = "付款单号";
                this.gridColumnMainCompanyName.Caption = "付款单位";
                this.gridColumnMainBuyingPrice.Caption = "付款总额";
                this.gridColumnMainDifference.Caption = "差额";
                if (dateEditInTimeEnd.EditValue == null || dateEditInTimeEnd.EditValue.ToString() == "0001/1/1 0:00:00")
                {
                    MessageBoxUtil.ShowError("查看已付款详细，时间不可为空。");
                }
            }
        }
        /// <summary>
        /// 提交付款按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送源</param>
        /// <param name="e">包含了事件所需的参数</param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            decimal emp = decimal.Parse(EMP);
            for (int rowIndex = 0; rowIndex < this.gridViewPayMain.RowCount; rowIndex++)
            {
                if (this.gridViewPayMain.GetRowCellValue(rowIndex, "SELECTED").Equals("1"))
                { 
                    decimal ID =  Convert.ToDecimal(this.gridViewPayMain.GetRowCellValue(rowIndex, "ID"));
                    if (billAccInPayMain.SubmitPayInfo(ID, emp))
                    {
                        MessageBoxUtil.ShowInformation("" + ID + "单提交付款成功");
                    }
                    else
                    {
                        MessageBoxUtil.ShowInformation("提交付款失败");
                    }
                }
            }
        }
        /// <summary>
        /// 付款按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送源</param>
        /// <param name="e">包含了事件所需的参数</param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            decimal emp = decimal.Parse(EMP);
            for (int rowIndex = 0; rowIndex < this.gridViewPayMain.RowCount; rowIndex++)
            {
                string sel = Convert.ToString(this.gridViewPayMain.GetRowCellValue(rowIndex, "SELECTED"));
                if (this.gridViewPayMain.GetRowCellValue(rowIndex, "SELECTED").Equals("1"))
                {
                    decimal ID = Convert.ToDecimal(this.gridViewPayMain.GetRowCellValue(rowIndex, "ID"));
                    decimal BUYING_PRICE_TOTAL = Convert.ToDecimal(this.gridViewPayMain.GetRowCellValue(rowIndex, "BUYING_PRICE_TOTAL"));
                    decimal ACC_ID = Convert.ToDecimal(imageComboBoxEditSearchAccountType.EditValue.ToString());
                    if (billAccInPayMain.FinishPayInfo(ID, emp, BUYING_PRICE_TOTAL,ACC_ID))
                    {
                        MessageBoxUtil.ShowInformation("" + ID + "单审核付款成功");
                    }
                    else
                    {
                        MessageBoxUtil.ShowInformation("" + ID + "单审核付款失败");
                    }
                }
            }
        }
    }
}
