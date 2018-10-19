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
using KingT.MMIS.Common;
using Tool;
using DevExpress.XtraGrid;

namespace BookUI
{
    public partial class FrmBillPriceMain : Form
    {
        BillPriceMain billPriceMain = new BillPriceMain();
        BillImageComBox billImageComBox = new BillImageComBox();
        //界面状态
        private UIState _UIState_BillPriceMain { get; set; }
        private DataTable _dtMaterailSeqList;
        public FrmBillPriceMain()
        { 
            InitializeComponent();
        }
        //加载调价单据列表
        public void LoadBillMainList()
        {
            decimal? storageID = 001;
            decimal? accountID = decimal.Parse(imageComboBoxEditSearchAccountType.EditValue.ToString());
            string state = imageComboBoxEditSearchState.EditValue.ToString();
            string docNo;
            if (textEditSearchDocNo.EditValue == null)
            {
                docNo = string.Empty;
            }
            else
            {
                docNo = textEditSearchDocNo.EditValue.ToString();
            }
            if (state == "999")
            {
                if (dateEditInTimeStart.EditValue == null || dateEditInTimeEnd.EditValue == null)
                {
                    MessageBoxUtil.ShowWarning("查询正式单据，日期不可为空！");
                    return;
                }
                DateTime? begTime = Convert.ToDateTime(dateEditInTimeStart.EditValue.ToString());
                DateTime? endTime = Convert.ToDateTime(dateEditInTimeEnd.EditValue.ToString());
                DataTable dt = billPriceMain.GetCheckedBillPriceInList(storageID, accountID, docNo, begTime, endTime);
                gridControlBillPriceList.DataSource = dt;
            }
            else
            {
                DataTable dt = billPriceMain.GetUnCheckedBillPriceInList(storageID,accountID,docNo,state);
                gridControlBillPriceList.DataSource = dt;
            }
        }
        //检索事件
        private void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            LoadBillMainList();
            LoadBPriceMain();
            this._UIState_BillPriceMain = UIState.Default;
            ShowUIState();
        }
        //Load函数
        private void FrmBillPriceMain_Load(object sender, EventArgs e)
        {
            this.FuzhuAccount(this.imageComboBoxEditSearchAccountType);
            this._UIState_BillPriceMain = UIState.Default;
            ShowUIState();
            barButtonItem1.Enabled = false;
            barButtonItem2.Enabled = false;
            barButtonItem3.Enabled = false;
            barButtonItem7.Enabled = false;
            barButtonItem8.Enabled = false;
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
        //加载调价单
        public void LoadBPriceMain()
        {
            DataRow dr = gridViewBillPriceList.GetFocusedDataRow();
            if (dr != null)
            {
                decimal billID = decimal.Parse(gridViewBillPriceList.GetFocusedDataRow()["ID"].ToString()); ;
                string state = imageComboBoxEditSearchState.EditValue.ToString();
                if (state == "999")  //正式单据
                {
                    labelControlApproveState.Visible = false;
                    labelControlNo.Text = "NO：" + gridViewBillPriceList.GetFocusedDataRow()["NO"].ToString();
                    textEditAccountType.Text = imageComboBoxEditSearchAccountType.Text;
                    textEditRemark.EditValue = gridViewBillPriceList.GetFocusedDataRow()["REMARK"].ToString();
                    labelControlInputEmp.Text = "录入人：" + gridViewBillPriceList.GetFocusedDataRow()["INPUT_EMP"].ToString();
                    labelControlInputTime.Text = "录入时间：" + gridViewBillPriceList.GetFocusedDataRow()["INPUT_TIME"].ToString();
                    textEditDocNo.EditValue = gridViewBillPriceList.GetFocusedDataRow()["DOCNO"].ToString();
                    if (gridViewBillPriceList.GetFocusedDataRow()["CHECK_EMP"] != null && imageComboBoxEditSearchState.EditValue.ToString() == "999" )
                    {
                        labelControlCheckEmp.Text = "审核人：" + gridViewBillPriceList.GetFocusedDataRow()["CHECK_EMP"].ToString();
                    }
                    else
                    {
                        labelControlCheckEmp.Text = "审核人：";
                    }
                    if (gridViewBillPriceList.GetFocusedDataRow()["CHECK_TIME"] != null && imageComboBoxEditSearchState.EditValue.ToString() == "999")
                    {
                        labelControlCheckTime.Text = "审核时间：" + gridViewBillPriceList.GetFocusedDataRow()["CHECK_TIME"].ToString();
                    }
                    else
                    {
                        labelControlCheckTime.Text = "审核时间：";
                    }
                    DataTable dtBillDetail = billPriceMain.GetCheckedBillPriceDetailListShow(billID, "1");
                    DataTableUtil.RemoveDBNull(ref dtBillDetail);
                    gridControlBillPriceDetail.DataSource = dtBillDetail;
                }
                else
                {
                    labelControlApproveState.Visible = true;
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
                    labelControlNo.Text = "NO：" + gridViewBillPriceList.GetFocusedDataRow()["NO"].ToString();
                    textEditDocNo.EditValue = gridViewBillPriceList.GetFocusedDataRow()["DOCNO"].ToString();
                    textEditAccountType.Text = imageComboBoxEditSearchAccountType.Text;
                    textEditRemark.EditValue = gridViewBillPriceList.GetFocusedDataRow()["REMARK"].ToString();
                    labelControlInputEmp.Text = "录入人：" + gridViewBillPriceList.GetFocusedDataRow()["INPUT_EMP"].ToString();
                    labelControlInputTime.Text = "录入时间：" + gridViewBillPriceList.GetFocusedDataRow()["INPUT_TIME"].ToString();
                    DataTable dtBillDetail = billPriceMain.GetUnCheckedBillPriceDetailListShow(billID, "1");
                    DataTableUtil.RemoveDBNull(ref dtBillDetail);
                    gridControlBillPriceDetail.DataSource = dtBillDetail;
                }
            }
            else
            {
                labelControlApproveState.Text = "审批情况：";
                labelControlNo.Text = "NO：";
                textEditDocNo.EditValue = null;
                textEditRemark.EditValue = null;
                labelControlInputEmp.Text = "录入人：";
                labelControlInputTime.Text = "录入时间：";
                gridControlBillPriceDetail.DataSource = null;
            }
        }
        //主单焦点改变事件
        private void gridViewBillPriceList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            LoadBPriceMain();
        }
        //清除单据信息
        public void ClearMainInfo()
        {
            labelControlApproveState.Visible = false;
            labelControlNo.Text = "NO：";
            labelControlInputEmp.Text = "录入人：";
            labelControlInputTime.Text = "录入时间：";
            labelControlCheckEmp.Text = "审核人：";
            labelControlCheckTime.Text = "审核时间：";
            gridControlBillPriceDetail.DataSource = null;
            textEditAccountType.Text = "";
        }
        /// <summary>
        /// 界面状态设置
        /// </summary>
        public void ShowUIState()
        {
            if (_UIState_BillPriceMain == UIState.Default || _UIState_BillPriceMain == UIState.Save || _UIState_BillPriceMain == UIState.Cancel)
            {
                barButtonItem4.Enabled = false;
                barButtonItem5.Enabled = false;
                if (gridViewBillPriceList.RowCount > 0)
                {
                    //不需要审批的单据在未审核状态下新增，修改，删除按钮可以操作
                    //需要审批的单据在审批未提交，审批不通过的状态下新增，修改，删除按钮可以操作
                    //其它情况下新增，修改，删除按钮不能操作
                    if (imageComboBoxEditSearchState.EditValue.ToString() == "999")   //已审核，主单数据
                    { 
                        barButtonItem1.Enabled = false;
                        barButtonItem2.Enabled = false;
                        barButtonItem3.Enabled = false;
                        barButtonItem7.Enabled = false;
                        barButtonItem8.Enabled = false;
                    }
                    else if (imageComboBoxEditSearchState.EditValue.ToString() == "0")  //无需审批，临时单
                    {
                        barButtonItem1.Enabled = true;
                        barButtonItem2.Enabled = true;
                        barButtonItem3.Enabled = true;
                        barButtonItem7.Enabled = false;
                        barButtonItem8.Enabled = true;
                    }
                    else if (imageComboBoxEditSearchState.EditValue.ToString() == "1")  //审批未提交，临时单
                    {
                        barButtonItem1.Enabled = true;
                        barButtonItem2.Enabled = true;
                        barButtonItem3.Enabled = true;
                        barButtonItem7.Enabled = true;
                        barButtonItem8.Enabled = false;
                    }
                    else if (imageComboBoxEditSearchState.EditValue.ToString() == "2")  //提交审批，临时单
                    {
                        barButtonItem1.Enabled = false;
                        barButtonItem2.Enabled = false;
                        barButtonItem3.Enabled = false;
                        barButtonItem7.Enabled = false;
                        barButtonItem8.Enabled = false;
                    }
                    else if (imageComboBoxEditSearchState.EditValue.ToString() == "3")  //审批通过，临时单
                    {
                        barButtonItem1.Enabled = false;
                        barButtonItem2.Enabled = false;
                        barButtonItem3.Enabled = false;
                        barButtonItem7.Enabled = false;
                        barButtonItem8.Enabled = true;
                    }
                    else if (imageComboBoxEditSearchState.EditValue.ToString() == "4")  //审批未通过，临时单
                    {
                        barButtonItem1.Enabled = true;
                        barButtonItem2.Enabled = true;
                        barButtonItem3.Enabled = true;
                        barButtonItem7.Enabled = true;
                        barButtonItem8.Enabled = false;
                    }
                }
            }
            else if (_UIState_BillPriceMain == UIState.Add || _UIState_BillPriceMain == UIState.Edit)
            {
                barButtonItem1.Enabled = false;
                barButtonItem2.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem4.Enabled = true;
                barButtonItem5.Enabled = true;
                barButtonItem7.Enabled = false;
                barButtonItem6.Enabled = false;
                barButtonItem8.Enabled = false;
            }
            if (_UIState_BillPriceMain == UIState.Add)
            { 
                //界面未销毁，需要对控件清空处理
                ClearMainInfo();
                DataTable dtBAppDetail = DevUtil.GetGridControlBindingDataTable(gridViewBillPriceDetail);
                for (int i = 0; i < 9; i++)
                {
                    DataRow dr = dtBAppDetail.NewRow();
                    dtBAppDetail.Rows.Add(dr);
                    gridControlBillPriceDetail.DataSource = dtBAppDetail;
                    gridViewBillPriceDetail.FocusedRowHandle = 0;
                    this.LoadMaterialList(this.gridControlPopupMaterial, "001", Convert.ToDecimal(this.imageComboBoxEditSearchAccountType.EditValue), true);
                    gridControlPopupMaterial.Visible = true;
                }

            }
            else if (_UIState_BillPriceMain == UIState.Edit)
            {
                gridViewBillPriceDetail.AddNewRow();
                gridViewBillPriceDetail.FocusedRowHandle = 0;
                this.LoadMaterialList(this.gridControlPopupMaterial, "001", Convert.ToDecimal(this.imageComboBoxEditSearchAccountType.EditValue), true);
            }
            gridViewBillPriceDetail.OptionsBehavior.Editable = true;
            if (gridControlBillPriceList.DataSource == null)
            {
                barButtonItem1.Enabled = false;
                barButtonItem2.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem8.Enabled = false;
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
        //新增事件

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ClearMainInfo();
            gridColumnStockNum.OptionsColumn.AllowEdit = true;
            gridColumnStockNum.OptionsColumn.AllowFocus = true;
            gridColumnMaterialNewRetailPrice.OptionsColumn.AllowEdit = true;
            gridColumnMaterialNewRetailPrice.OptionsColumn.AllowFocus = true;
            this._UIState_BillPriceMain = UIState.Add;
            ShowUIState();
        }
        //修改事件
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridColumnStockNum.OptionsColumn.AllowEdit = true;
            gridColumnStockNum.OptionsColumn.AllowFocus = true;
            gridColumnMaterialNewRetailPrice.Visible = true;
            gridColumnMaterialNewRetailPrice.VisibleIndex = 6;
            gridColumnMaterialNewRetailPrice.OptionsColumn.AllowEdit = true;
            gridColumnMaterialNewRetailPrice.OptionsColumn.AllowFocus = true;
            this._UIState_BillPriceMain = UIState.Edit;
            ShowUIState();
        }
        //删除事件
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            decimal ID = decimal.Parse(gridViewBillPriceList.GetFocusedDataRow()["ID"].ToString());
            decimal count = gridViewBillPriceDetail.FocusedRowHandle;
            decimal Seq = count + 1;
            if (billPriceMain.deleteBpriceTempDetail(ID,Seq))
            {
                MessageBoxUtil.ShowInformation("删除明细成功！");
                LoadBillMainList();
                LoadBPriceMain();
            }
            else
            {
                MessageBoxUtil.ShowError("删除明细失败！");
            }
        }
        //保存事件
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            decimal ID = decimal.Parse(gridViewBillPriceList.GetFocusedDataRow()["ID"].ToString());
            decimal? SEQ = billPriceMain.MaxSeq(ID) + 1;
            DataRow dr = gridViewPopupMaterial.GetFocusedDataRow();
            DataRow dr1 = gridViewPopupMatSeq.GetFocusedDataRow();
            if (dr != null && dr1 != null)
            {
                decimal MAT_ID = Convert.ToDecimal(dr["ID"]);
                decimal MAT_SEQ = Convert.ToDecimal(dr1["MAT_SEQ"]);
                decimal NUMS = Convert.ToDecimal(gridViewBillPriceDetail.GetFocusedRowCellValue(gridColumnStockNum));
                decimal RETAIL_PRICE_O = Convert.ToDecimal(dr["RETAIL_PRICE"]);
                decimal RETAIL_PRICE_N = Convert.ToDecimal(gridViewBillPriceDetail.GetFocusedRowCellValue(gridColumnMaterialNewRetailPrice));
                decimal TRADE_PRICE_O = Convert.ToDecimal(dr["TRADE_PRICE"]);
                decimal TRADE_PRICE_N = Convert.ToDecimal(gridViewBillPriceDetail.GetFocusedRowCellValue(gridColumnMaterialNewRetailPrice));
                decimal MAX_PRICE_O = Convert.ToDecimal(dr["MAX_PRICE"]);
                decimal MAX_PRICE_N = Convert.ToDecimal(gridViewBillPriceDetail.GetFocusedRowCellValue(gridColumnMaterialNewRetailPrice));
                string BATCH_NO = dr1["BATCH_NO"].ToString();
                DateTime EXPIRY_DATE = Convert.ToDateTime(dr1["EXPIRY_DATE"]);
                string REMARK = gridViewBillPriceDetail.GetFocusedRowCellValue(gridColumnMaterialRemark).ToString();
                if (this._UIState_BillPriceMain == UIState.Add)
                {
                    if (this.billPriceMain.insertBpriceTempDetail(ID, SEQ, MAT_ID, MAT_SEQ, NUMS, RETAIL_PRICE_O, RETAIL_PRICE_N, TRADE_PRICE_O, TRADE_PRICE_N, MAX_PRICE_O, MAX_PRICE_N, BATCH_NO, EXPIRY_DATE, REMARK))
                    {
                        MessageBoxUtil.ShowInformation("新增调价明细成功");
                        gridColumnStockNum.OptionsColumn.AllowEdit = false;
                        gridColumnStockNum.OptionsColumn.AllowFocus = false;
                        gridColumnMaterialNewRetailPrice.Visible = false;
                        LoadBillMainList();
                        LoadBPriceMain();
                        this._UIState_BillPriceMain = UIState.Default;
                        ShowUIState();
                    }
                    else
                    {
                        MessageBoxUtil.ShowInformation("新增调价明细失败");
                    }
                }
            }
            if (this._UIState_BillPriceMain == UIState.Edit)
            {
                decimal count = gridViewBillPriceDetail.FocusedRowHandle;
                decimal Seq = count + 1;
                decimal RETAIL_PRICE_N = Convert.ToDecimal(gridViewBillPriceDetail.GetFocusedRowCellValue(gridColumnMaterialNewRetailPrice));
                decimal NUMS = Convert.ToDecimal(gridViewBillPriceDetail.GetFocusedRowCellValue(gridColumnStockNum));
                string REMARK = gridViewBillPriceDetail.GetFocusedRowCellValue(gridColumnMaterialRemark).ToString();
                if (this.billPriceMain.updateBpriceTempDetail(ID, Seq, NUMS, RETAIL_PRICE_N, REMARK))
                {
                    MessageBoxUtil.ShowInformation("修改调价明细成功");
                    gridColumnStockNum.OptionsColumn.AllowEdit = false;
                    gridColumnStockNum.OptionsColumn.AllowFocus = false;
                    gridColumnMaterialNewRetailPrice.Visible = false;
                    LoadBillMainList();
                    LoadBPriceMain();
                    this._UIState_BillPriceMain = UIState.Default;
                    ShowUIState();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("修改调价明细失败");
                }
            }
        }
        //取消事件
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadBillMainList();
            LoadBPriceMain();
            gridControlPopupMaterial.Visible = false;
            gridControlPopupMatSeq.Visible = false;
            gridColumnStockNum.OptionsColumn.AllowEdit = false;
            gridColumnStockNum.OptionsColumn.AllowFocus = false;
            gridColumnMaterialNewRetailPrice.Visible = false;
            this._UIState_BillPriceMain = UIState.Default;
            ShowUIState();
        }
        //全选
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
        //提交审批
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
        //审核单据
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridViewBillPriceList.GetFocusedDataRow() == null)
            {
                return;
            }
            decimal ID = decimal.Parse(gridViewBillPriceList.GetFocusedDataRow()["ID"].ToString());
            if (XtraMessageBox.Show("确定要审核此单据?", "提示", MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                if (billPriceMain.CheckList(ID))
                {
                    MessageBoxUtil.ShowInformation("单据审核成功");
                    LoadBillMainList();
                    LoadBPriceMain();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("单据审核失败");
                }
            }
        }
        //加载批次明细
        public void LoadMaterialSeqList()
        {
            if (imageComboBoxEditSearchAccountType.EditValue == null)
                return;
            if (string.Empty.Equals(gridViewBillPriceDetail.GetFocusedRowCellDisplayText(gridColumnMaterialID)))
            {
                _dtMaterailSeqList = null;
            }
            else
            {
                decimal matId = decimal.Parse(gridViewBillPriceDetail.GetFocusedRowCellDisplayText(gridColumnMaterialID));
                decimal accId = decimal.Parse(imageComboBoxEditSearchAccountType.EditValue.ToString());
                _dtMaterailSeqList = billPriceMain.GetMaterialSeqList(accId,matId);
            }
            gridControlPopupMatSeq.DataSource = _dtMaterailSeqList;
            gridControlPopupMatSeq.Visible = true;
        }
        //自动填充相关信息
        decimal value;
        private void gridViewBillPriceDetail_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            
            decimal value1;
            if (e.Column == gridColumnStockNum)
            {
                value = Convert.ToDecimal(gridViewBillPriceDetail.GetFocusedRowCellValue(gridColumnStockNum));
            }
            if (e.Column == gridColumnMaterialNewRetailPrice)
            {
                value1 = Convert.ToDecimal(gridViewBillPriceDetail.GetFocusedRowCellValue(gridColumnMaterialNewRetailPrice));
                DataRow dr = gridViewPopupMaterial.GetFocusedDataRow();
                if (dr != null)
                {
                    decimal retail_price =  Convert.ToDecimal(dr["RETAIL_PRICE"]);
                    decimal ratio_buying =  Convert.ToDecimal(dr["RATIO_BUYING"]);
                    object total = (value1 - retail_price) * value / ratio_buying;
                    DataRow dr1 = gridViewBillPriceDetail.GetFocusedDataRow();
                    dr1["TOTAL"] = total;
                }
            }
        }
        //新增双击确认事件,获取物资信息
        private void gridViewPopupMaterial_DoubleClick(object sender, EventArgs e)
        {
            gridControlPopupMaterial.Visible = false;
            DataRow dr = gridViewPopupMaterial.GetFocusedDataRow();
            if (dr != null)
            {
                DataRow dr2 = gridViewBillPriceDetail.GetFocusedDataRow();
                dr2["Name"] = dr["NAME"];
                dr2["SPEC"] = dr["SPEC"];
                dr2["UNIT"] = dr["UNIT"];
                dr2["FACTORY_NAME"] = dr["FACTORY_NAME"];
                dr2["MAT_ID"] = dr["ID"];
                LoadMaterialSeqList();
            }
        }
        //新增双击确认事件,获取批次信息
        private void gridViewPopupMatSeq_DoubleClick(object sender, EventArgs e)
        {
            gridControlPopupMatSeq.Visible = false;
            DataRow dr = gridViewPopupMatSeq.GetFocusedDataRow();
            if (dr != null)
            {
                DataRow dr2 = gridViewBillPriceDetail.GetFocusedDataRow();
                dr2["MAT_SEQ"] = dr["MAT_SEQ"];
                dr2["BATCH_NO"] = dr["BATCH_NO"];
                dr2["EXPIRY_DATE"] = dr["EXPIRY_DATE"];
                gridColumnMaterialNewRetailPrice.Visible = true;
                gridColumnMaterialNewRetailPrice.VisibleIndex = 6;
            }
        }
    }
}
