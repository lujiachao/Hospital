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
    public partial class FrmBillInMain : Form
    {
        public UIState UIState_BillInMain;//界面状态
        private DataTable _dtMaterailSeqList;
        BillInMain billInMain = new BillInMain();
        BillImageComBox billImageComBox = new BillImageComBox();
        BillPriceMain billPriceMain = new BillPriceMain();
        decimal _sto_ID;//获取登录仓库
        string Input_code;//获取登录人员代码
        int sMode = 0;//明细处理判断
        public FrmBillInMain(decimal Sto_id,string code)
        {
            InitializeComponent();
            _sto_ID = Sto_id;
            Input_code = code;
        }
        //LOAD函数
        private void FrmBillInMain_Load(object sender, EventArgs e)
        {
            this.FuzhuAccount(this.imageComboBoxEditSearchAccountType);
            this.LoadCheckAppState(this.imageComboBoxEditSearchState, true);
            this.LoadInvoiceState(this.imageComboBoxEditSearchInvoiceState, true);
            this.FuzhuCommpany(this.imageComboBoxEditCompany);
            //this.FuzhuCommpany(this.imageComboBoxCompany1);
            this.UIState_BillInMain = UIState.Default;
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
                itemColApprove.Add(new ImageComboBoxItem("不需要审批", "0"));
                itemColApprove.Add(new ImageComboBoxItem("审批未提交", "1"));
                itemColApprove.Add(new ImageComboBoxItem("审批已提交", "2"));
                itemColApprove.Add(new ImageComboBoxItem("审批不通过", "4"));
                itemColApprove.Add(new ImageComboBoxItem("审批通过", "3"));
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
        //检索按钮事件加载
        private void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            GetBillList();
            this.UIState_BillInMain = UIState.Default;
            ShowUIState();
        }
        //获取单据
        private void GetBillList()
        {
            decimal? storage_ID = _sto_ID;
            decimal? acc_ID = decimal.Parse(imageComboBoxEditSearchAccountType.EditValue.ToString());
            decimal? BInType = decimal.Parse(imageComboBoxEditSearchBInType.EditValue.ToString());
            string state = imageComboBoxEditSearchState.EditValue.ToString();
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
            string InvoiceState = imageComboBoxEditSearchInvoiceState.EditValue.ToString();
            string InvoiceNo = string.Empty;
            if (imageComboBoxEditCompany.EditValue != null)
            {
                InvoiceNo = textEditSearchInvoiceNo.EditValue.ToString();
            }
            decimal? CompanyID = Convert.ToDecimal(imageComboBoxEditCompany.EditValue);
            DataTable dt = billInMain.GetBillInTempMainListShow(storage_ID,acc_ID,BInType,InTimeState,EndTimeEnd,InvoiceState,InvoiceNo,CompanyID,state);
            gridControlBillInList.DataSource = dt;
        }
        //主单焦点改变事件
        private void gridViewBillInList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gridViewBillInList.GetFocusedDataRow() != null)
            {
                LoadBillInMain();
                ShowUIState();
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
            textEditAccountType.Text = "";
            labelControlBuyEmp.Text = "经手人：";
            dateEditBuyTime.EditValue = null;
            textEditInvoiceState.Text = "";
            textEditCompany.Text = "";
            dateEdit1.EditValue = null;
            textEditRemark.Text = "";
            labelControlInputEmp.Text = "录入人：";
            labelControlInputTime.Text = "录入时间：";
            gridControlBillInDetail.DataSource = null;
        }
        //加载入库单
        public void LoadBillInMain()
        {
            DataRow dr = gridViewBillInList.GetFocusedDataRow();
            if (dr != null)
            {
                decimal billID = decimal.Parse(gridViewBillInList.GetFocusedDataRow()["ID"].ToString());
                string state = gridViewBillInList.GetFocusedDataRow()["STATE"].ToString();
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
                labelControlNo.Text = "NO：" + gridViewBillInList.GetFocusedDataRow()["NO"].ToString();
                textEditAccountType.Text = imageComboBoxEditSearchAccountType.Text;
                textEditBInType.Text = imageComboBoxEditSearchBInType.Text;
                string buy_emp = gridViewBillInList.GetFocusedDataRow()["BUY_EMP"].ToString();
                string buy_name = billInMain.catchName(buy_emp);
                labelControlBuyEmp.Text = "经手人：" + buy_name;
                dateEditBuyTime.EditValue = gridViewBillInList.GetFocusedDataRow()["BUY_TIME"].ToString();
                if (gridViewBillInList.GetFocusedDataRow()["INVOICE_STATE"].ToString() == "1")
                {
                    textEditInvoiceState.Text = "票到货到";
                }
                else if (gridViewBillInList.GetFocusedDataRow()["INVOICE_STATE"].ToString() == "2")
                {
                    textEditInvoiceState.Text = "货到票未到";
                }
                else
                {
                    textEditInvoiceState.Text = "票到货未到";
                }
                textEditCompany.Text = gridViewBillInList.GetFocusedDataRow()["COMPANYNAME"].ToString();
                dateEdit1.EditValue = gridViewBillInList.GetFocusedDataRow()["INTIME"].ToString();
                textEditRemark.Text = gridViewBillInList.GetFocusedDataRow()["REMARK"].ToString();
                string name = billInMain.GetName(gridViewBillInList.GetFocusedDataRow()["INPUT_EMP"].ToString());
                labelControlInputEmp.Text = "录入人：" + name;
                labelControlInputTime.Text = "录入时间：" + gridViewBillInList.GetFocusedDataRow()["INPUT_TIME"].ToString();
                DataTable dt = billInMain.GetBillAccInTempDetailListShow(billID);
                gridControlBillInDetail.DataSource = dt;
            }
        }
        //主单数据绑定改变事件
        private void gridControlBillInList_DataSourceChanged(object sender, EventArgs e)
        {
            if (gridViewBillInList.GetFocusedDataRow() != null)
            {
                LoadBillInMain();
                ShowUIState();
            }
            else
            { 
                ClearMainInfo();
            }
        }
        //菜单栏按钮
        public void ShowUIState()
        {
            if (this.UIState_BillInMain == UIState.Default)
            { 
                barButtonItemSave.Enabled = false;
                barButtonItemCancel.Enabled = false;
                barButtonItemAdd.Enabled = true;
                barButtonItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barButtonItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                gridControlPopupMaterial.Visible = false;
                gridControlPopupStock.Visible = false;
                if (gridViewBillInList.RowCount > 0)
                {
                    string state = gridViewBillInList.GetFocusedDataRow()["STATE"].ToString();
                    if (state == "0")
                    {
                        barButtonItemEdit.Enabled = true;
                        barButtonItemDelete.Enabled = true;
                        barButtonItemSubmitApprove.Enabled = false;
                        barButtonItemAudit.Enabled = false;
                        barButtonItem5.Enabled = true;
                        barButtonItem2.Enabled = true;
                    }
                    else if (state == "1" || state == "4")
                    {
                        barButtonItemEdit.Enabled = true;
                        barButtonItemDelete.Enabled = true;
                        barButtonItemSubmitApprove.Enabled = true;
                        barButtonItemAudit.Enabled = false;
                        barButtonItem5.Enabled = true;
                        barButtonItem2.Enabled = true;
                    }
                    else if (state == "2")
                    {
                        barButtonItemEdit.Enabled = false;
                        barButtonItemDelete.Enabled = false;
                        barButtonItemSubmitApprove.Enabled = false;
                        barButtonItemAudit.Enabled = true;
                        barButtonItem5.Enabled = false;
                        barButtonItem2.Enabled = false;
                    }
                    else if (state == "3")
                    {
                        barButtonItemEdit.Enabled = false;
                        barButtonItemDelete.Enabled = false;
                        barButtonItemSubmitApprove.Enabled = false;
                        barButtonItemAudit.Enabled = false;
                        barButtonItem5.Enabled = false;
                        barButtonItem2.Enabled = false;
                    }
                }
                else
                {
                    barButtonItemEdit.Enabled = false;
                    barButtonItemDelete.Enabled = false;
                    barButtonItemSubmitApprove.Enabled = false;
                    barButtonItemAudit.Enabled = false;
                    barButtonItem5.Enabled = false;
                    barButtonItem2.Enabled = false;
                }
            }
            else if (this.UIState_BillInMain == UIState.Add || this.UIState_BillInMain == UIState.Edit)
            {
                barButtonItemAdd.Enabled = false;
                barButtonItemEdit.Enabled = false;
                barButtonItemDelete.Enabled = false;
                barButtonItemSave.Enabled = true;
                barButtonItemCancel.Enabled = true;
                barButtonItemSubmitApprove.Enabled = false;
                barButtonItemAudit.Enabled = false;
            }
        }
        //新增按钮的实现
        private void barButtonItemAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddNewBill addNewBill = new AddNewBill(Input_code);
            addNewBill.ShowDialog();
            this.UIState_BillInMain = UIState.Add;
            ShowUIState();
        }
        //修改按钮的实现
        private void barButtonItemEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.UIState_BillInMain = UIState.Edit;
            ShowUIState();
            barButtonItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            barButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
        }
        //删除按钮的实现
        private void barButtonItemDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridViewBillInList.GetFocusedRow() != null)
            {
                decimal ID = decimal.Parse(gridViewBillInList.GetFocusedDataRow()["ID"].ToString());
                if (gridViewBillInDetail.RowCount == 0)
                {
                    if (billInMain.deleteBinTempMain(ID))
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
        //保存按钮的实现
        private void barButtonItemSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            decimal ID = decimal.Parse(gridViewBillInList.GetFocusedDataRow()["ID"].ToString());
            if (this.UIState_BillInMain == UIState.Edit && sMode == 0)
            {
                for (int rowIndex = 0; rowIndex < this.gridViewBillInDetail.RowCount; rowIndex++)
                {
                    decimal Seq = Convert.ToDecimal(this.gridViewBillInDetail.GetRowCellValue(rowIndex, "SEQ"));
                    decimal NUMS = Convert.ToDecimal(this.gridViewBillInDetail.GetRowCellValue(rowIndex, "NUMS"));
                    decimal BUYING_PRICE_SHOW = Convert.ToDecimal(this.gridViewBillInDetail.GetRowCellValue(rowIndex, "BUYING_PRICE_SHOW"));
                    if (billInMain.updateBinTempDetail(ID, Seq, NUMS, BUYING_PRICE_SHOW))
                    {
                        
                    }
                    else
                    {
                        MessageBoxUtil.ShowWarning("修改入库明细失败");
                        return;
                    }
                }
                this.UIState_BillInMain = UIState.Default;
                ShowUIState();
                LoadBillInMain();
                MessageBoxUtil.ShowInformation("修改入库明细成功");
            }
        }
        //取消按钮的实现
        private void barButtonItemCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.UIState_BillInMain = UIState.Default;
            ShowUIState();
            barButtonItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            barButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            barButtonItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            sMode = 0;
        }
        //加明细按钮的实现
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }
        //加明细按钮的实现
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }
        //加明细按钮的实现
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sMode = 1;
            gridViewBillInDetail.AddNewRow();
            string sto_ID = _sto_ID.ToString();
            this.LoadMaterialList(this.gridControlPopupMaterial, sto_ID, Convert.ToDecimal(this.imageComboBoxEditSearchAccountType.EditValue), true);
            gridControlPopupMaterial.Visible = true;
            barButtonItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
        }     
        //减明细按钮的实现
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridViewBillInDetail.GetFocusedRow() != null)
            {
                //gridViewBillInDetail.DeleteRow(gridViewBillInDetail.FocusedRowHandle);
                //barButtonItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                decimal ID = decimal.Parse(gridViewBillInList.GetFocusedDataRow()["ID"].ToString());
                decimal seq = decimal.Parse(this.gridViewBillInDetail.GetFocusedDataRow()["SEQ"].ToString());
                if (billInMain.deleteBinTempDetail(ID, seq))
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
            if (string.Empty.Equals(gridViewBillInDetail.GetFocusedRowCellDisplayText(gridColumnMaterialID)))
            {
                _dtMaterailSeqList = null;
            }
            else
            {
                decimal matId = decimal.Parse(gridViewBillInDetail.GetFocusedRowCellDisplayText(gridColumnMaterialID));
                decimal accId = decimal.Parse(imageComboBoxEditSearchAccountType.EditValue.ToString());
                _dtMaterailSeqList = billPriceMain.GetMaterialSeqList(accId, matId);
            }
            gridControlPopupStock.DataSource = _dtMaterailSeqList;
            gridControlPopupStock.Visible = true;
        }
        //物资列表双击事件
        private void gridViewPopupMaterial_DoubleClick(object sender, EventArgs e)
        {
            gridControlPopupMaterial.Visible = false;
            DataRow dr = gridViewPopupMaterial.GetFocusedDataRow();
            if (dr != null)
            {
                DataRow dr2 = gridViewBillInDetail.GetFocusedDataRow();
                dr2["NAME"] = dr["NAME"];
                dr2["SPEC"] = dr["SPEC"];
                dr2["UNIT"] = dr["UNIT"];
                dr2["FACTORYNAME"] = dr["FACTORY_NAME"];
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
                DataRow dr2 = gridViewBillInDetail.GetFocusedDataRow();
                dr2["MAT_SEQ"] = dr["MAT_SEQ"];
                dr2["BATCH_NO"] = dr["BATCH_NO"];
                dr2["EXPIRY_DATE"] = dr["EXPIRY_DATE"];
            }
        }
        //保存明细按钮的实现
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (sMode == 1)
            {
                decimal ID = decimal.Parse(gridViewBillInList.GetFocusedDataRow()["ID"].ToString());
                decimal seq = gridViewBillInDetail.FocusedRowHandle + 1;
                string invoiceno = gridViewBillInDetail.GetFocusedRowCellValue(gridColumnInvoiceNo).ToString();
                decimal mat_id = Convert.ToDecimal(gridViewPopupMaterial.GetFocusedDataRow()["ID"]);
                decimal mat_seq = Convert.ToDecimal(gridViewPopupStock.GetFocusedDataRow()["MAT_SEQ"]);
                if (gridViewBillInDetail.GetFocusedRowCellValue(gridColumnMaterialNums).Equals(DBNull.Value))
                {
                    MessageBoxUtil.ShowWarning("入库数量不可为空");
                    return;
                }
                decimal nums = Convert.ToDecimal(gridViewBillInDetail.GetFocusedRowCellValue(gridColumnMaterialNums));
                decimal buying_price = Convert.ToDecimal(gridViewPopupMaterial.GetFocusedDataRow()["BUYING_PRICE"]);
                decimal retail_price = Convert.ToDecimal(gridViewPopupMaterial.GetFocusedDataRow()["RETAIL_PRICE"]);
                decimal trade_price = Convert.ToDecimal(gridViewPopupMaterial.GetFocusedDataRow()["TRADE_PRICE"]);
                decimal return_num = 0;
                decimal return_reason = 1;
                string batch_no = "001";
                DateTime expipy_date = Convert.ToDateTime(gridViewPopupStock.GetFocusedDataRow()["EXPIRY_DATE"]);
                string remark = gridViewBillInDetail.GetFocusedRowCellValue(gridColumnMaterialRemark).ToString();
                if (billInMain.insertBinTempDetail(ID, seq, invoiceno, mat_id, mat_seq, nums, buying_price, retail_price, trade_price, return_num, return_reason, batch_no, expipy_date, remark))
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
        //提交审批按钮的实现
        private void barButtonItemSubmitApprove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            decimal ID = decimal.Parse(gridViewBillInList.GetFocusedDataRow()["ID"].ToString());
            if (billInMain.reviewState(ID))
            {
                GetBillList();
                MessageBoxUtil.ShowInformation("提交审批成功");
            }
            else
            {
                MessageBoxUtil.ShowWarning("提交审批失败");
            }
        }
        //审核按钮的实现
        private void barButtonItemAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            decimal ID = decimal.Parse(gridViewBillInList.GetFocusedDataRow()["ID"].ToString());
            string state;
            if (XtraMessageBox.Show("是否通过审核", "提示", MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                state = "3";
                if (billInMain.examineState(ID, state))
                {
                    GetBillList();
                    MessageBoxUtil.ShowInformation("该单据已审核通过");
                }
                else
                {
                    MessageBoxUtil.ShowWarning("审核失败");
                }
            }
            else
            {
                state = "4";
                if (billInMain.examineState(ID, state))
                {
                    GetBillList();
                    MessageBoxUtil.ShowInformation("审核成功");
                }
                else
                {
                    MessageBoxUtil.ShowWarning("审核失败");
                }
            }
        } 
    }
}
