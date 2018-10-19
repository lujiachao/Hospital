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
using DevExpress.XtraEditors.Controls;
using BookUI.Util;

namespace BookUI
{
    public partial class FrmBillAccOutMain : Form
    {
        decimal _sto_ID;//当前登录仓库
        decimal _nameCode;//当前操作人员
        BillAccOutMain billAccOutMain = new BillAccOutMain();
        BillImageComBox billImageComBox = new BillImageComBox();
        public FrmBillAccOutMain(decimal sto_ID, string nameCode)
        {
            InitializeComponent();
            _sto_ID = sto_ID;
            _nameCode = Convert.ToDecimal(nameCode);
        }
        //load函数
        private void FrmBillAccOutMain_Load(object sender, EventArgs e)
        {
            this.FuzhuAccount(this.imageComboBoxEditSearchAccountType);
            this.LoadCheckAppState(this.imageComboBoxEditSearchState, false);
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
            this.FuzhuBinType(this.imageComboBoxEditSearchBOutType, acc_ID);
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
                itemColApprove.Add(new ImageComboBoxItem("不需要审批", "0"));
                itemColApprove.Add(new ImageComboBoxItem("审批未提交", "1"));
                itemColApprove.Add(new ImageComboBoxItem("审批已提交", "2"));
                itemColApprove.Add(new ImageComboBoxItem("审批通过", "3"));
                itemColApprove.Add(new ImageComboBoxItem("审批不通过", "4"));
            }
            else
            {
                itemColApprove.Add(new ImageComboBoxItem("未审核", "0"));
                itemColApprove.Add(new ImageComboBoxItem("已验收", "1"));
            }

            imageComboBoxEditState.Properties.Items.AddRange(itemColApprove);
            if (imageComboBoxEditState.Properties.Items.Count > 0)
            {
                imageComboBoxEditState.SelectedIndex = 0;
            }
        }
        //出库分类改变事件
        private void imageComboBoxEditSearchBOutType_EditValueChanged(object sender, EventArgs e)
        {
            decimal id = Convert.ToDecimal(imageComboBoxEditSearchBOutType.EditValue);
            char target = this.billAccOutMain.getTarget(id);
            if (target == '1')
            {
                imageComboBoxEditSearchTargetType.EditValue = "1";
            }
            else
            {
                imageComboBoxEditSearchTargetType.EditValue = "2";
            }
        }
        //出库方式改变事件
        private void imageComboBoxEditSearchTargetType_EditValueChanged(object sender, EventArgs e)
        {
            this.FuzhuCommpany(this.imageComboBoxEdit1, imageComboBoxEditSearchTargetType.EditValue.ToString());
        }
        /// </summary>
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
                DataTable dt = billImageComBox.DalImageComboxStorageClass(_sto_ID, Convert.ToDecimal(imageComboBoxEditSearchAccountType.EditValue));
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
        //检索按钮的实现
        private void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            GetBillList(); 
        }
        //获取单据
        private void GetBillList()
        {
            decimal storageID = _sto_ID;
            decimal? accountID = Convert.ToDecimal(imageComboBoxEditSearchAccountType.EditValue);
            decimal? bOutTypeID = Convert.ToDecimal(imageComboBoxEditSearchBOutType.EditValue);
            string matState = imageComboBoxEditSearchState.EditValue.ToString();
            DateTime? InTimeState = null;
            if (dateEditInTimeStart.EditValue != null)
            {
                InTimeState = Convert.ToDateTime(dateEditInTimeStart.EditValue.ToString());
            }
            DateTime? EndTimeEnd = null;
            if (dateEditInTimeEnd.EditValue != null)
            {
                EndTimeEnd = Convert.ToDateTime(dateEditInTimeEnd.EditValue.ToString());
            }
            decimal? targetID = Convert.ToDecimal(imageComboBoxEdit1.EditValue);
            char TargetType = Convert.ToChar(imageComboBoxEditSearchTargetType.EditValue);
            char DISPATCHING = Convert.ToChar(imageComboBoxEditSearchMaterialState.EditValue);
            DataTable dt = billAccOutMain.GetCheckedBillOutMainListOutCommShow(storageID, accountID, bOutTypeID, InTimeState, EndTimeEnd, matState, targetID, TargetType, DISPATCHING);
            gridControlBillOutList.DataSource = dt;
        }
        //主单焦点改变事件
        private void gridViewBillOutList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            LoadBillInMain();
        }
        public void LoadBillInMain()
        {
            DataRow dr = gridViewBillOutList.GetFocusedDataRow();
            if (dr != null)
            {
                decimal billID = decimal.Parse(dr["ID"].ToString());
                string acc_bill_state = dr["ACC_BILL_STATE"].ToString();
                if (acc_bill_state == "0")
                {
                    labelControlApproveState.Text = "验收状态：未验收";
                }
                else if (acc_bill_state == "1")
                {
                    labelControlApproveState.Text = "验收状态：已验收";
                }
                labelControlNo.Text = "NO：" + dr["NO"].ToString();
                textEditAccoutType.Text = imageComboBoxEditSearchAccountType.Text;
                textEditBoutType.Text = imageComboBoxEditSearchBOutType.Text;
                string rq = dr["ID"].ToString().Substring(0, 8);
                dateEditInTime.EditValue = rq;
                textEditTarget.Text = dr["TARGET"].ToString();
                textEditTargetType.Text = imageComboBoxEditSearchMaterialState.Text;
                textEditRemark.Text = dr["REMARK"].ToString();
                labelControlInputEmp.Text = "录入人：" + dr["INPUT_EMP"].ToString();
                labelControlInputTime.Text = "录入时间：" + dr["INPUT_TIME"].ToString();
                if (acc_bill_state == "1")
                {
                    labelControlCheckEmp.Text = "审核人：" + dr["CHECK_EMP"].ToString();
                    labelControlCheckTime.Text = "审核时间：" + dr["CHECK_TIME"].ToString();
                }
                DataTable dt = billAccOutMain.GetCheckedBillOutDetailListShow(billID);
                gridControlBillOutDetail.DataSource = dt;
            }
            else
            {
                ClearMainInfo();
            }
        }
        //清除单据信息
        public void ClearMainInfo()
        {
            labelControlApproveState.Text = "验收状态：";
            labelControlNo.Text = "NO：";
            textEditAccoutType.Text = "";
            textEditBoutType.Text = "";
            dateEditInTime.EditValue = null;
            textEditRemark.Text = "";
            textEditTargetType.Text = "";
            textEditTarget.Text = "";
            labelControlInputEmp.Text = "录入人：";
            labelControlInputTime.Text = "录入时间：";
            labelControlInputEmp.Text = "审核人：";
            labelControlInputTime.Text = "审核时间：";
            gridControlBillOutDetail.DataSource = null;
        }
        //审核按钮的实现
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow dr = gridViewBillOutList.GetFocusedDataRow();
            if (dr != null && dr["ACC_BILL_STATE"].ToString() == "0")
            {
                decimal billID = decimal.Parse(dr["ID"].ToString());
                if (XtraMessageBox.Show("是否通过审核", "提示", MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    for (int rowIndex = 0; rowIndex < this.gridViewBillOutDetail.RowCount; rowIndex++)
                    {
                        decimal NUMS = Convert.ToDecimal(this.gridViewBillOutDetail.GetRowCellValue(rowIndex, "NUMS"));
                        decimal Sto_ID = _sto_ID;
                        decimal Acc_ID = Convert.ToDecimal(imageComboBoxEditSearchAccountType.EditValue);
                        decimal Mat_ID = Convert.ToDecimal(this.gridViewBillOutDetail.GetRowCellValue(rowIndex, "MAT_ID"));
                        decimal Mat_Seq = Convert.ToDecimal(this.gridViewBillOutDetail.GetRowCellValue(rowIndex, "MAT_SEQ"));
                        if (billAccOutMain.updateStock(NUMS, Sto_ID, Acc_ID, Mat_ID, Mat_Seq))
                        {

                        }
                    }
                    char acc_bill_state = '1';
                    if (billAccOutMain.updateBoutTempMain(billID, _nameCode, acc_bill_state))
                    {
                        GetBillList(); 
                        MessageBoxUtil.ShowInformation("完成审核");                      
                    }
                    else
                    {
                        MessageBoxUtil.ShowWarning("审核操作出错");
                    }             
                }
                else
                {
                    char acc_bill_state = '0';
                    if (billAccOutMain.updateBoutTempMain(billID, _nameCode, acc_bill_state))
                    {
                        GetBillList(); 
                        MessageBoxUtil.ShowInformation("完成审核");
                    }
                    else
                    {
                        MessageBoxUtil.ShowWarning("审核操作出错");
                    }
                }
            }
        }

        private void gridControlBillOutList_DataSourceChanged(object sender, EventArgs e)
        {
            LoadBillInMain();
        }
    }
}
