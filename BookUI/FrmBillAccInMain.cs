using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BookBLL;
using DevExpress.XtraEditors.Controls;
using BookUI.Util;
using Tool;
using System.Text.RegularExpressions;

namespace BookUI
{
    public partial class FrmBillAccInMain : Form
    {
        public UIState UIState_BillAccInMain;//界面状态
        BillImageComBox billImageComBox = new BillImageComBox();
        BillAccInMain billAccInMain = new BillAccInMain();
        decimal EMP;
        public FrmBillAccInMain(string NameCode)
        {
            InitializeComponent();
            EMP = decimal.Parse(NameCode);
        }
        // load函数
        private void FrmBillAccInMain_Load(object sender, EventArgs e)
        {
            this.FuzhuAccount(this.imageComboBoxEditSearchAccountType);
            this.FuzhuAccount(this.imageComboBoxEditAccountType);
            this.LoadCheckAppState(this.imageComboBoxEditSearchState, false);
            this.LoadInvoiceState(this.imageComboBoxEditSearchInvoiceState, true);
            this.LoadInvoiceState(this.imageComboBoxEditInvoiceState, true);
            this.FuzhuCommpany(this.imageComboBoxEdit2);
            this.FuzhuCommpany(this.imageComboBoxEdit1);
            imageComboBoxEditAccountType.SelectedIndex = -1;
            LoadBuyEmpList();
            UIState_BillAccInMain = UIState.Default;
            ShowUIState();
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
        /// 加载审核审批状态信息
        /// </summary>
        /// <param name="imageComboBoxEditState">状态下拉框控件</param>
        /// <param name="isApproval">是否审批</param>
        protected void LoadCheckAppState(ImageComboBoxEdit imageComboBoxEditState, bool isApproval)
        {
            imageComboBoxEditState.Properties.Items.Clear();
            ImageComboBoxItemCollection itemColApprove =
                new ImageComboBoxItemCollection(imageComboBoxEditState.Properties);
            if (isApproval)
            {
                itemColApprove.Add(new ImageComboBoxItem("审批未提交", "1"));
                itemColApprove.Add(new ImageComboBoxItem("审批已提交", "2"));
                itemColApprove.Add(new ImageComboBoxItem("审批不通过", "4"));
                itemColApprove.Add(new ImageComboBoxItem("审批通过", "3"));
                itemColApprove.Add(new ImageComboBoxItem("已审核", "999"));
            }
            else
            {
                itemColApprove.Add(new ImageComboBoxItem("未审核", "0"));
                itemColApprove.Add(new ImageComboBoxItem("已验收", "999"));
            }

            imageComboBoxEditState.Properties.Items.AddRange(itemColApprove);
            if (imageComboBoxEditState.Properties.Items.Count > 0)
            {
                imageComboBoxEditState.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// 加载发票状态(包含全部项)
        /// </summary>
        /// <param name="imageComboBoxEditInvoiceState">发票状态下拉框控件</param>
        /// <param name="isAll">是否包含全部选项</param>
        protected void LoadInvoiceState(ImageComboBoxEdit imageComboBoxEditInvoiceState, bool isAll)
        {
            if (isAll)
            {
                imageComboBoxEditInvoiceState.Properties.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem("全部", 4, -1));
            }

            imageComboBoxEditInvoiceState.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("票到货到", 1, -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("货到票未到", 2, -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("票到货未到", 3, -1)});
            if (imageComboBoxEditInvoiceState.Properties.Items.Count > 0)
            {
                imageComboBoxEditInvoiceState.SelectedIndex = 0;
            }
            if (imageComboBoxEditInvoiceState.Name == "imageComboBoxEditInvoiceState")
            {
                imageComboBoxEditInvoiceState.SelectedIndex = -1;
            }
        }
        //加载单据列表
        public void LoadBillMainList()
        {
            string state = imageComboBoxEditSearchState.EditValue.ToString();
            decimal? accountID = Convert.ToDecimal(imageComboBoxEditSearchAccountType.EditValue);
            decimal? binTypeID = Convert.ToDecimal(imageComboBoxEditSearchBInType.EditValue);
            DateTime? begTime = Convert.ToDateTime(this.dateEditInTimeStart.EditValue);
            DateTime? endTime = Convert.ToDateTime(this.dateEditInTimeEnd.EditValue);
            string invoiceState = imageComboBoxEditSearchInvoiceState.EditValue.ToString();
            if (imageComboBoxEditSearchInvoiceState.EditValue.ToString() == "4")
            {
                invoiceState = string.Empty;
            }
            string invoiceNo = textEditInvoiceNo.Text;
            decimal? companyID =  Convert.ToDecimal(imageComboBoxEdit2.EditValue);
            if (state == "0")
            {  
                DataTable dt = billAccInMain.GetUnCheckedBillAccInList(1,accountID,binTypeID,begTime,endTime,invoiceState,invoiceNo,companyID);
                gridControlBillAccInList.DataSource = dt;
            }
            else if (state == "999")
            {
                DataTable dt = billAccInMain.GetCheckedBillAccInList(1, accountID, binTypeID, begTime, endTime, invoiceState, invoiceNo, companyID);
                gridControlBillAccInList.DataSource = dt;
            }
        }
        //检索按钮点击事件
        private void simpleButtonSearch_Click(object sender, EventArgs e)
        {
           LoadBillMainList();
           ShowUIState();
        }
        //清空公司列表
        private void imageComboBoxEdit2_EditValueChanged(object sender, EventArgs e)
        {
            int Row = imageComboBoxEdit2.SelectedIndex;
            if (imageComboBoxEdit2.SelectedIndex == 107)
            {
                imageComboBoxEdit2.EditValue = null;
            }
        }
        //主单值改变事件
        private void gridViewBillAccInList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridViewBillAccInList.GetFocusedDataRow();
            if (dr != null)
            {
                decimal billID = decimal.Parse(dr["ID"].ToString());
                LoadBillAccInMain(billID);
            }
            else
            {
                wipeData();
            }
        }
        //加载入库单
        public void LoadBillAccInMain(decimal billID)
        {
            DataRow dr = gridViewBillAccInList.GetFocusedDataRow();
            string State = dr["APPROVE"].ToString(); //审批状况
            string state = imageComboBoxEditSearchState.EditValue.ToString(); //验收情况
            if (State == "0")
            {
                labelControlApproveState.Text = "审批情况：无需审批";
            }
            else if (State == "3")
            {
                labelControlApproveState.Text = "审批情况：审批通过";
            }
            else
            {
                labelControlApproveState.Text = "状态不正确";
            }
            labelControlNo.Text = "NO：" + dr["NO"].ToString();
            imageComboBoxEditAccountType.EditValue = imageComboBoxEditSearchAccountType.EditValue;
            decimal acc_ID = Convert.ToDecimal(imageComboBoxEditAccountType.EditValue.ToString());
            this.FuzhuBinType(this.imageComboBoxEditBInType, acc_ID);
            lookUpEditEmp.EditValue = dr["BUY_EMP"].ToString();
            imageComboBoxEditInvoiceState.EditValue = imageComboBoxEditSearchInvoiceState.EditValue;
            imageComboBoxEdit1.EditValue = dr["SECTION_ID"].ToString();
            if (dr["BUY_TIME"] != null)
            {
                dateEditBuyTime.EditValue = dr["BUY_TIME"].ToString();
            }
            if (dr["INTIME"] != null)
            {
                dateEditInTime.EditValue = dr["INTIME"].ToString();
            }
            textEditRemark.EditValue = dr["REMARK"].ToString();
            labelControlInputEmp.Text = "录入人：" + dr["INPUT_EMP"].ToString(); 
            labelControlInputTime.Text = "录入时间：" + dr["INPUT_TIME"].ToString();
            if (dr["CHECK_EMP"] != null)
            {
                labelControlCheckEmp.Text = "审核人：" + dr["CHECK_EMP"].ToString();
            }
            if (dr["CHECK_TIME"] != null)
            {
                labelControlCheckTime.Text = "审核时间：" + dr["CHECK_TIME"].ToString();
            }
            DataTable dt = billAccInMain.GetBillAccInTempDetailListShow(billID,state);
            gridControl1.DataSource = dt;
        }
        //加载经手人
        private void LoadBuyEmpList()
        {
            DataTable employeeList = this.billAccInMain.GetEmployeeList();
            lookUpEditEmp.Properties.DisplayMember = "NAME";
            lookUpEditEmp.Properties.ValueMember = "CODE";
            lookUpEditEmp.Properties.DataSource = employeeList;
        }
        //主单绑定数据改变事件
        private void gridControlBillAccInList_DataSourceChanged(object sender, EventArgs e)
        {
            DataRow dr = gridViewBillAccInList.GetFocusedDataRow();
            if (dr != null)
            {
                decimal billID = decimal.Parse(dr["ID"].ToString());
                LoadBillAccInMain(billID);
            }
            else
            {
                wipeData();
            }
        }
        public void wipeData()
        {
            labelControlApproveState.Text = "审批情况：";
            imageComboBoxEditAccountType.SelectedIndex = -1;
            imageComboBoxEditBInType.SelectedIndex = -1;
            lookUpEditEmp.EditValue = null;
            imageComboBoxEditInvoiceState.SelectedIndex = -1;
            imageComboBoxEdit1.SelectedIndex = -1;
            textEditRemark.Text = "";
            dateEditBuyTime.EditValue = null;
            dateEditInTime.EditValue = null;
            labelControlInputEmp.Text = "录入人：";
            labelControlInputTime.Text = "录入时间：";
            labelControlCheckEmp.Text = "审核人：";
            labelControlCheckTime.Text = "审核时间：";
            gridControl1.DataSource = null;
        }
        //状态切换
        private void imageComboBoxEditSearchState_SelectedIndexChanged(object sender, EventArgs e)
        {
            //未审核状态下入账日期不可用，获取的单据是全部未验收的单据
            //已审核状态下入账日期可用，获取的单据是入账日期对应的单据
            if (imageComboBoxEditSearchState.EditValue.ToString() == "999")
            {
                dateEditInTimeStart.Enabled = true;
                dateEditInTimeEnd.Enabled = true;
            }
            else
            {
                dateEditInTimeStart.Enabled = false;
                dateEditInTimeEnd.Enabled = false;
                dateEditInTimeStart.EditValue = null;
                dateEditInTimeEnd.EditValue = null;
            }
        }
        //菜单栏按钮
        public void ShowUIState()
        {
            if (UIState_BillAccInMain == UIState.Default)
            {
                //菜单
                barButtonItemSave.Enabled = false;
                barButtonItemCancel.Enabled = false;
                if (gridViewBillAccInList.RowCount > 0)
                {
                    string State = imageComboBoxEditSearchState.EditValue.ToString();
                    if (State == "0")
                    {
                        barButtonItemEdit.Enabled = true;
                        barButtonItemAudit.Enabled = true;
                    }
                    else
                    {
                        barButtonItemEdit.Enabled = false;
                        barButtonItemAudit.Enabled = false;
                    }
                }
                else
                {
                    barButtonItemEdit.Enabled = false;
                    barButtonItemAudit.Enabled = false;
                }
                //设置gridControl中选择列可编辑，其它列无法编辑
                gridColumnSelectNo.OptionsColumn.AllowEdit = true;
                gridColumnInvoiceNo.OptionsColumn.AllowEdit = false;
                gridColumnMaterialRemark.OptionsColumn.AllowEdit = false;
                gridColumnMaterialBuyingPrice.OptionsColumn.AllowEdit = false;
                gridColumnMaterialBuyingPrice.OptionsColumn.AllowFocus = false;

                gridColumnMaterialNums.OptionsColumn.AllowEdit = false;
                gridColumnMaterialNums.OptionsColumn.AllowFocus = false;
            }
            else if (UIState_BillAccInMain == UIState.Edit)
            {
                barButtonItemSave.Enabled = true;
                barButtonItemCancel.Enabled = true;
                barButtonItemEdit.Enabled = false;
                barButtonItemAudit.Enabled = false;
                gridControlBillAccInList.Enabled = false;
                //设置gridControl中选择列可编辑，其它列无法编辑
                gridColumnSelectNo.OptionsColumn.AllowEdit = false;
                gridColumnInvoiceNo.OptionsColumn.AllowEdit = true;
                gridColumnInvoiceNo.OptionsColumn.AllowFocus = true;
                gridColumnMaterialRemark.OptionsColumn.AllowEdit = true;
                gridColumnMaterialRemark.OptionsColumn.AllowFocus = true;

                gridColumnMaterialNums.OptionsColumn.AllowEdit = true;
                gridColumnMaterialNums.OptionsColumn.AllowFocus = true;
                gridColumnMaterialBuyingPrice.OptionsColumn.AllowFocus = true;
                gridColumnMaterialBuyingPrice.OptionsColumn.AllowEdit = true;
            }
        }
        //修改按钮点击事件
        private void barButtonItemEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UIState_BillAccInMain = UIState.Edit;
            ShowUIState();
        }
        //保存按钮点击事件
        private void barButtonItemSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            for (int rowIndex = 0; rowIndex < this.gridView1.RowCount; rowIndex++)
            {
                string invoiceNo = gridView1.GetRowCellValue(rowIndex, "INVOICENO").ToString();
                decimal NUMS = Convert.ToDecimal(gridView1.GetRowCellValue(rowIndex, "NUMS").ToString());
                decimal buying_price = Convert.ToDecimal(gridView1.GetRowCellValue(rowIndex, "BUYING_PRICE_SHOW").ToString());
                decimal ID = Convert.ToDecimal(gridViewBillAccInList.GetFocusedDataRow()["ID"].ToString());
                decimal Seq = rowIndex + 1;
                if (billAccInMain.updateBinDetail(ID, Seq, invoiceNo, NUMS, buying_price))
                {
                    
                }
                else
                {
                    MessageBoxUtil.ShowInformation("修改失败！");
                    return;
                }
            }
            LoadBillMainList();
            UIState_BillAccInMain = UIState.Default;
            ShowUIState();
            MessageBoxUtil.ShowInformation("修改成功！");
            
        }
        //取消按钮点击事件
        private void barButtonItemCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadBillMainList();
            UIState_BillAccInMain = UIState.Default;
            ShowUIState();
        }

        private void barButtonItemAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            for (int rowIndex = 0; rowIndex < this.gridView1.RowCount; rowIndex++)
            {
                decimal ID = Convert.ToDecimal(gridViewBillAccInList.GetFocusedDataRow()["ID"].ToString());
                decimal check_emp = EMP;
                DateTime check_time = DateTime.Now;
                decimal NUMS = Convert.ToDecimal(this.gridView1.GetRowCellValue(rowIndex, "NUMS"));
                decimal Sto_ID = 1;
                decimal Acc_ID = Convert.ToDecimal(imageComboBoxEditSearchAccountType.EditValue);
                decimal Mat_ID = Convert.ToDecimal(this.gridView1.GetRowCellValue(rowIndex, "MAT_ID"));
                decimal Mat_Seq = Convert.ToDecimal(this.gridView1.GetRowCellValue(rowIndex, "MAT_SEQ"));
                if (billAccInMain.updateBinMain(ID, check_emp, check_time,NUMS,Sto_ID,Acc_ID,Mat_ID,Mat_Seq))
                {
                    
                }
                else
                {
                    MessageBoxUtil.ShowInformation("单据验收失败");
                }
            }
            MessageBoxUtil.ShowInformation("单据验收成功");
            UIState_BillAccInMain = UIState.Default;
            ShowUIState();
        }
        //按单位验收点击事件
        private void barButtonItemCompanyAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }
        //查看修改后的数据格式是否正确
        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
        }
        //正则表达式判断字符是否为纯数字
        public bool IsNumeric(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            return Regex.IsMatch(s, @"^[+-]?\d+(\.\d+)?$");
        }
        //全选
        private void checkEditAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int rowIndex = 0; rowIndex < this.gridView1.RowCount; rowIndex++)
            {
                if (checkEditAll.Checked == true)
                {
                    this.gridView1.SetRowCellValue(rowIndex, "SELECTNO", "1");
                }
                else
                {
                    this.gridView1.SetRowCellValue(rowIndex, "SELECTNO", "0");
                }
            }
        }
    }
}
