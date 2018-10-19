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
using Tool;
using DevExpress.XtraGrid;

namespace BookUI
{
    public partial class FrmBillOutMain : Form
    {
        decimal _sto_ID;//当前登录仓库
        decimal _nameCode;//当前操作人员
        private DataTable _dtMaterailSeqList;
        BillImageComBox billImageComBox = new BillImageComBox();
        BillOutMain billOutMain = new BillOutMain();
        public UIState UIState_BillOutMain;//界面状态
        int sMode = 0;//明细处理判断
        BillPriceMain billPriceMain = new BillPriceMain();
        public FrmBillOutMain(decimal sto_ID,string nameCode)
        {
            InitializeComponent();
            _sto_ID = sto_ID;
            _nameCode = Convert.ToDecimal(nameCode);
        }
        //load函数
        private void FrmBillOutMain_Load(object sender, EventArgs e)
        {
            this.FuzhuAccount(this.imageComboBoxEditSearchAccountType);
            this.LoadCheckAppState(this.imageComboBoxEditSearchState, true);
            this.UIState_BillOutMain = UIState.Default;
            ShowUIState();
            //this.FuzhuCommpany(this.imageComboBoxEditTargetName, imageComboBoxEditSearchTargetType.EditValue.ToString());
        }
        /// <summary>
        /// 往来单位数据绑定
        /// </summary>
        public void FuzhuCommpany(ImageComboBoxEdit imageComboBoxEdit,string TargetType)
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
                itemColApprove.Add(new ImageComboBoxItem("已验收", "999"));
            }

            imageComboBoxEditState.Properties.Items.AddRange(itemColApprove);
            if (imageComboBoxEditState.Properties.Items.Count > 0)
            {
                imageComboBoxEditState.SelectedIndex = 0;
            }
        }
        //账册改变事件
        private void imageComboBoxEditSearchAccountType_EditValueChanged(object sender, EventArgs e)
        {
            decimal acc_ID = decimal.Parse(imageComboBoxEditSearchAccountType.EditValue.ToString());
            this.FuzhuBinType(this.imageComboBoxEditSearchBOutType, acc_ID);
        }
        //出库方式改变事件
        private void imageComboBoxEditSearchTargetType_EditValueChanged(object sender, EventArgs e)
        {
            this.FuzhuCommpany(this.imageComboBoxEditTargetName, imageComboBoxEditSearchTargetType.EditValue.ToString());
        }
        //出库分类改变事件
        private void imageComboBoxEditSearchBOutType_EditValueChanged(object sender, EventArgs e)
        {
            decimal id = Convert.ToDecimal(imageComboBoxEditSearchBOutType.EditValue);
            char target = billOutMain.getTarget(id);
            if (target == '1')
            {
                imageComboBoxEditSearchTargetType.EditValue = "1";
            }
            else
            {
                imageComboBoxEditSearchTargetType.EditValue = "2";
            }
        }
        //清空出库目标
        private void imageComboBoxEditTargetName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (imageComboBoxEditTargetName.Text == "清空")
            {
                imageComboBoxEditTargetName.EditValue = null;
            }
        }
        //检索按钮的实现
        private void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            GetBillList();  
            this.UIState_BillOutMain = UIState.Default;
            ShowUIState();
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
            decimal? targetID = Convert.ToDecimal(imageComboBoxEditTargetName.EditValue);
            char TargetType = Convert.ToChar(imageComboBoxEditSearchTargetType.EditValue);
            char DISPATCHING = Convert.ToChar(imageComboBoxEditSearchMaterialState.EditValue);
            DataTable dt = billOutMain.GetCheckedBillOutMainListOutCommShow(storageID, accountID, bOutTypeID, InTimeState, EndTimeEnd, matState, targetID, TargetType, DISPATCHING);
            gridControlBillOutList.DataSource = dt;
        }
        //主单焦点改变事件
        private void gridViewBillOutList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            LoadBillInMain();
        }
        //获取明细信息
        public void LoadBillInMain()
        {
            DataRow dr = gridViewBillOutList.GetFocusedDataRow();
            if (dr != null)
            {
                decimal billID = decimal.Parse(dr["ID"].ToString());
                string state = dr["STATE"].ToString();
                if (state == "0")
                {
                    labelControlApproveState.Text = "审批情况：无需审批";
                }
                else if (state == "1")
                {
                    labelControlApproveState.Text = "审批情况：审批未提交";
                }
                else if (state == "2")
                {
                    labelControlApproveState.Text = "审批情况：审批已提交";
                }
                else if (state == "3")
                {
                    labelControlApproveState.Text = "审批情况：审批通过";
                }
                else if (state == "4")
                {
                    labelControlApproveState.Text = "审批情况：审批不通过";
                }
                labelControlNo.Text = "NO：" + dr["NO"].ToString();
                textEditAccoutType.Text = imageComboBoxEditSearchAccountType.Text;
                textEditBoutType.Text = imageComboBoxEditSearchBOutType.Text;
                dateEditInTime.EditValue = dr["INTIME"].ToString();
                textEdit1.EditValue = imageComboBoxEditSearchTargetType.Text;
                textEdit2.Text = dr["TARGET"].ToString();
                textEdit3.Text = dr["APP_EMP"].ToString();
                dateEditAppTime.EditValue = dr["APP_TIME"].ToString();
                textEditRemark.Text = dr["REMARK"].ToString();
                labelControlInputEmp.Text = "录入人：" + dr["INPUT_EMP"].ToString();
                labelControlInputTime.Text = "录入时间：" + dr["INPUT_TIME"].ToString();
                DataTable dt = billOutMain.GetCheckedBillOutDetailListShow(billID);
                gridControlBillOutDetail.DataSource =  dt;
            }
            else
            {
                ClearMainInfo();
            }
        }
        //清除单据信息
        public void ClearMainInfo()
        {
            labelControlApproveState.Text = "审批情况：";
            labelControlNo.Text = "NO：";
            textEditAccoutType.Text = "";
            textEditBoutType.Text = "";
            dateEditInTime.EditValue = null;
            textEdit1.Text = "";
            textEdit2.Text = "";
            textEdit3.Text = "";
            dateEditAppTime.EditValue = null;
            textEditRemark.Text = "";
            labelControlInputEmp.Text = "录入人：";
            labelControlInputTime.Text = "录入时间：";
            gridControlBillOutDetail.DataSource = null;
        }
        private void gridControlBillOutList_DataSourceChanged(object sender, EventArgs e)
        {
            LoadBillInMain();
        }
        //新增事件的实现
        private void barButtonItemAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddNewOutBill addNewOutBill = new AddNewOutBill(_nameCode);
            addNewOutBill.ShowDialog();
        }
        //修改按钮功能实现
        private void barButtonItemEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.UIState_BillOutMain = UIState.Edit;
            ShowUIState();
        }
        //删除按钮的功能实现
        private void barButtonItemDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridViewBillOutList.GetFocusedRow() != null)
            {
                decimal ID = decimal.Parse(gridViewBillOutList.GetFocusedDataRow()["ID"].ToString());
                if (gridViewBillOutDetail.RowCount == 0)
                {
                    if (billOutMain.deleteBoutTempMain(ID))
                    {
                        GetBillList();
                        MessageBoxUtil.ShowInformation("删除主单成功");
                    }
                    else
                    {
                        MessageBoxUtil.ShowWarning("删除主单失败");
                    }
                }
                else
                {
                    MessageBoxUtil.ShowWarning("必须先删除明细信息，才可以删除主单数据");
                }
            }
        }
        //保存按钮的功能实现
        private void barButtonItemSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            decimal ID = decimal.Parse(gridViewBillOutList.GetFocusedDataRow()["ID"].ToString());
            if (this.UIState_BillOutMain == UIState.Edit && sMode == 0)
            {
                for (int rowIndex = 0; rowIndex < this.gridViewBillOutDetail.RowCount; rowIndex++)
                {
                    decimal Seq = Convert.ToDecimal(this.gridViewBillOutDetail.GetRowCellValue(rowIndex, "SEQ"));
                    decimal NUMS = Convert.ToDecimal(this.gridViewBillOutDetail.GetRowCellValue(rowIndex, "NUMS"));
                    decimal BUYING_PRICE_SHOW = Convert.ToDecimal(this.gridViewBillOutDetail.GetRowCellValue(rowIndex, "BUYING_PRICE_SHOW"));
                    if (billOutMain.updateBoutTempDetail(ID, Seq, NUMS, BUYING_PRICE_SHOW))
                    {

                    }
                    else
                    {
                        MessageBoxUtil.ShowWarning("修改入库明细失败");
                        return;
                    }
                }
                this.UIState_BillOutMain = UIState.Default;
                ShowUIState();
                LoadBillInMain();
                MessageBoxUtil.ShowInformation("修改入库明细成功");
            }
        }
        //取消按钮的功能实现
        private void barButtonItemCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.UIState_BillOutMain = UIState.Default;
            ShowUIState();
            barButtonItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            barButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            barButtonItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            sMode = 0;
        }
        //提交审批
        private void barButtonItemSubmitApprove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            decimal ID = decimal.Parse(gridViewBillOutList.GetFocusedDataRow()["ID"].ToString());
            if (billOutMain.reviewState(ID))
            {
                GetBillList();
                MessageBoxUtil.ShowInformation("提交审批成功");
            }
            else
            {
                MessageBoxUtil.ShowWarning("提交审批失败");
            }
        }
        //审核
        private void barButtonItemAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            decimal ID = decimal.Parse(gridViewBillOutList.GetFocusedDataRow()["ID"].ToString());
            string state;
            if (XtraMessageBox.Show("是否通过审核", "提示", MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                state = "3";
                if (billOutMain.examineState(ID, state))
                {
                    GetBillList();
                    MessageBoxUtil.ShowInformation("该单据已通过审核");
                }
                else
                {
                    MessageBoxUtil.ShowWarning("审核失败");
                }
            }
            else
            {
                state = "4";
                if (billOutMain.examineState(ID, state))
                {
                    GetBillList();
                    MessageBoxUtil.ShowInformation("该单据已通过审核");
                }
                else
                {
                    MessageBoxUtil.ShowWarning("审核失败");
                }
            }
        }
        //加明细
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sMode = 1;
            gridViewBillOutDetail.AddNewRow();
            string sto_ID = _sto_ID.ToString();
            this.LoadMaterialList(this.gridControlPopupMaterial, sto_ID, Convert.ToDecimal(this.imageComboBoxEditSearchAccountType.EditValue), true);
            gridControlPopupMaterial.Visible = true;
            barButtonItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
        }
        /// <summary>
        /// 加载品种明细
        /// </summary>
        /// <param name="gridControlMaterial">品种明细列表</param>
        /// <param name="acc_ID">账册ID</param>
        /// <param name="showByMat">true 根据物资明细加载 false 根据库存进行加载</param>
        /// <remarks>仓库ID取自当前仓库</remarks>
        public void LoadMaterialList(GridControl gridControlMaterial, string sto_ID, decimal? acc_ID, bool showByMat)
        {
            LoadMaterialList(gridControlMaterial, sto_ID, acc_ID, showByMat, false);
        }
        /// <summary>
        /// 加载品种明细
        /// </summary>
        /// <param name="gridControlMaterial">品种明细列表</param>
        /// <param name="acc_ID">账册ID</param>
        /// <param name="showByMat">true 根据物资明细加载 false 根据库存进行加载</param>
        /// <param name="showMaxUnit">true 根据采购单位显示 false 根据该仓库管理单位显示</param>
        /// <remarks>仓库ID取自当前仓库</remarks>
        public void LoadMaterialList(GridControl gridControlMaterial, string sto_ID, decimal? acc_ID, bool showByMat, bool showMaxUnit)
        {
            if (acc_ID == null)
            {
                return;
            }

            DataTable dtMaterialNameList = billPriceMain.GetMaterialNameList(acc_ID.Value, sto_ID, showByMat, showMaxUnit);
            gridControlMaterial.DataSource = dtMaterialNameList;
        }
        //加载批次明细
        public void LoadMaterialSeqList()
        {
            if (imageComboBoxEditSearchAccountType.EditValue == null)
                return;
            if (string.Empty.Equals(gridViewBillOutDetail.GetFocusedRowCellDisplayText(gridColumnMaterialID)))
            {
                _dtMaterailSeqList = null;
            }
            else
            {
                decimal matId = decimal.Parse(gridViewBillOutDetail.GetFocusedRowCellDisplayText(gridColumnMaterialID));
                decimal accId = decimal.Parse(imageComboBoxEditSearchAccountType.EditValue.ToString());
                _dtMaterailSeqList = billPriceMain.GetMaterialSeqList(accId, matId);
            }
            gridControlPopupStock.DataSource = _dtMaterailSeqList;
            gridControlPopupStock.Visible = true;
        }
        //减明细
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridViewBillOutDetail.GetFocusedRow() != null)
            {
                decimal ID = decimal.Parse(gridViewBillOutList.GetFocusedDataRow()["ID"].ToString());
                decimal seq = decimal.Parse(this.gridViewBillOutDetail.GetFocusedDataRow()["SEQ"].ToString());
                if (billOutMain.deleteBoutTempDetail(ID, seq))
                { 
                    LoadBillInMain();
                    MessageBoxUtil.ShowInformation("删除明细成功");
                }
                else
                {
                    MessageBoxUtil.ShowWarning("删除明细失败");
                }
            }
        }
        //保存明细
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridViewBillOutList.GetFocusedDataRow() == null)
            {
                return;
            }
            if (gridViewBillOutDetail.GetFocusedDataRow() == null)
            {
                return;
            }
            if (gridViewPopupMaterial.GetFocusedDataRow() == null)
            {
                return;
            }
            if (gridViewPopupStock.GetFocusedDataRow() == null)
            {
                return;
            }
            if (sMode == 1)
            {
                decimal ID = decimal.Parse(gridViewBillOutList.GetFocusedDataRow()["ID"].ToString());
                decimal seq = gridViewBillOutDetail.FocusedRowHandle + 1;
                decimal mat_id = Convert.ToDecimal(gridViewPopupMaterial.GetFocusedDataRow()["ID"]);
                decimal mat_seq = Convert.ToDecimal(gridViewPopupStock.GetFocusedDataRow()["MAT_SEQ"]);
                if (gridViewBillOutDetail.GetFocusedRowCellValue(gridColumnMaterialNums).Equals(DBNull.Value))
                {
                    MessageBoxUtil.ShowWarning("入库数量不可为空");
                    return;
                }
                decimal nums = Convert.ToDecimal(gridViewBillOutDetail.GetFocusedRowCellValue(gridColumnMaterialNums));
                decimal buying_price = Convert.ToDecimal(gridViewPopupMaterial.GetFocusedDataRow()["BUYING_PRICE"]);
                decimal retail_price = Convert.ToDecimal(gridViewPopupMaterial.GetFocusedDataRow()["RETAIL_PRICE"]);
                decimal trade_price = Convert.ToDecimal(gridViewPopupMaterial.GetFocusedDataRow()["TRADE_PRICE"]);
                string batch_no = "001";
                DateTime expipy_date = Convert.ToDateTime(gridViewPopupStock.GetFocusedDataRow()["EXPIRY_DATE"]);
                decimal return_reason = 1;
                string remark = gridViewBillOutDetail.GetFocusedRowCellValue(gridColumnMaterialRemark).ToString();
                if (billOutMain.insertBoutTempDetail(ID,seq,mat_id,mat_seq,nums,buying_price,retail_price,trade_price,batch_no,expipy_date,return_reason,remark))
                {
                    sMode = 0;
                    LoadBillInMain();
                    MessageBoxUtil.ShowInformation("保存明细成功");
                }
                else
                {
                    MessageBoxUtil.ShowWarning("保存明细失败");
                }

            }
        }
        //菜单栏按钮
        public void ShowUIState()
        {
            if (this.UIState_BillOutMain == UIState.Default)
            {
                barButtonItemSave.Enabled = false;
                barButtonItemCancel.Enabled = false;
                barButtonItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barButtonItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                gridControlPopupMaterial.Visible = false;
                gridControlPopupStock.Visible = false;
                if (gridViewBillOutList.RowCount > 0)
                {
                    string state = gridViewBillOutList.GetFocusedDataRow()["STATE"].ToString();
                    if (state == "0")
                    {
                        barButtonItemEdit.Enabled = true;
                        barButtonItemDelete.Enabled = true;
                        barButtonItemSubmitApprove.Enabled = false;
                        barButtonItemAudit.Enabled = false;
                    }
                    else if (state == "1" || state == "4")
                    {
                        barButtonItemEdit.Enabled = true;
                        barButtonItemDelete.Enabled = true;
                        barButtonItemSubmitApprove.Enabled = true;
                        barButtonItemAudit.Enabled = false;
                    }
                    else if (state == "2")
                    {
                        barButtonItemEdit.Enabled = false;
                        barButtonItemDelete.Enabled = false;
                        barButtonItemSubmitApprove.Enabled = false;
                        barButtonItemAudit.Enabled = true;
                    }
                    else if (state == "3")
                    {
                        barButtonItemEdit.Enabled = false;
                        barButtonItemDelete.Enabled = false;
                        barButtonItemSubmitApprove.Enabled = false;
                        barButtonItemAudit.Enabled = false;
                    }
                }
                else
                {
                    barButtonItemEdit.Enabled = false;
                    barButtonItemDelete.Enabled = false;
                    barButtonItemSubmitApprove.Enabled = false;
                    barButtonItemAudit.Enabled = false;
                }
            }
            else if (this.UIState_BillOutMain == UIState.Edit)
            {
                barButtonItemAdd.Enabled = false;
                barButtonItemEdit.Enabled = false;
                barButtonItemDelete.Enabled = false;
                barButtonItemSubmitApprove.Enabled = false;
                barButtonItemSave.Enabled = true;
                barButtonItemCancel.Enabled = true;
                barButtonItemAudit.Enabled = false;
                barButtonItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                barButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
        }
        //物资列表双击事件
        private void gridViewPopupMaterial_DoubleClick(object sender, EventArgs e)
        {
            gridControlPopupMaterial.Visible = false;
            DataRow dr = gridViewPopupMaterial.GetFocusedDataRow();
            if (dr != null)
            {
                DataRow dr2 = gridViewBillOutDetail.GetFocusedDataRow();
                dr2["NAME"] = dr["NAME"];
                dr2["SPEC"] = dr["SPEC"];
                dr2["UNIT"] = dr["UNIT"];
                dr2["FACTORY_NAME"] = dr["FACTORY_NAME"];
                dr2["MAT_ID"] = dr["ID"];
                dr2["BUYING_PRICE_SHOW"] = dr["BUYING_PRICE_SHOW"];
                dr2["RETAIL_PRICE_SHOW"] = dr["RETAIL_PRICE_SHOW"];
                LoadMaterialSeqList();
            }
        }
        //新增双击确认事件,获取批次信息
        private void gridControlPopupStock_DoubleClick(object sender, EventArgs e)
        {
            gridControlPopupStock.Visible = false;
            DataRow dr = gridViewPopupStock.GetFocusedDataRow();
            if (dr != null)
            {
                DataRow dr2 = gridViewBillOutDetail.GetFocusedDataRow();
                dr2["MAT_SEQ"] = dr["MAT_SEQ"];
                dr2["BATCH_NO"] = dr["BATCH_NO"];
                dr2["EXPIRY_DATE"] = dr["EXPIRY_DATE"];
            }
        }
    }
}
