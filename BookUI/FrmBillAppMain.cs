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
using Tool;
using DevExpress.XtraGrid;

namespace BookUI
{
    public partial class FrmBillAppMain : Form
    {
        public UIState UIState_BillAppMain;//界面状态
        private DataTable _dtMaterailSeqList;
        decimal _sto_ID;//当前登录仓库
        decimal _nameCode;//当前操作人员
        BillImageComBox billImageComBox = new BillImageComBox();
        BillPriceMain billPriceMain = new BillPriceMain();
        BillAppMain billAppMain = new BillAppMain();
        public FrmBillAppMain(decimal sto_ID, string nameCode)
        {
            InitializeComponent();
            _sto_ID = sto_ID;
            _nameCode = Convert.ToDecimal(nameCode);
        }
        // load函数
        private void FrmBillAppMain_Load(object sender, EventArgs e)
        {
            this.FuzhuAccount(this.imageComboBoxEditSearchAccountType);
            this.FuzhuAccount(this.imageComboBoxEdit1);
            this.FuzhuStorage(this.imageComboBoxEditAppStorage);
            this.UIState_BillAppMain = UIState.Default;
            ShowUIState();
        }
         //菜单栏按钮
        public void ShowUIState()
        {
            if (this.UIState_BillAppMain == UIState.Default)
            {
                barButtonItem1.Enabled = true;
                DataRow dr = gridViewBillAppList.GetFocusedDataRow();
                if (imageComboBoxEditSearchState.EditValue != "999" && dr != null)
                {
                    barButtonItem2.Enabled = true;
                    barButtonItem3.Enabled = true;
                }
                else
                {
                    barButtonItem2.Enabled = false;
                    barButtonItem3.Enabled = false;
                }
                barButtonItem4.Enabled = false;
                barButtonItem5.Enabled = false;
                barButtonItem7.Enabled = true;
                barButtonItem10.Enabled = false;
                barButtonItem11.Enabled = false;
                imageComboBoxEditSearchAccountType.Properties.ReadOnly = false;
                imageComboBoxEditSearchState.Properties.ReadOnly = false;
                dateEditInTimeStart.Properties.ReadOnly = false;
                dateEditInTimeEnd.Properties.ReadOnly = false;
                gridControlBillAppList.Enabled = true;
                gridControl1.Enabled = true;
                gridControlPopupMaterial.Visible = false;
                gridControlPopupStock.Visible = false;
                barButtonItem6.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else if (this.UIState_BillAppMain == UIState.Add)
            {
                barButtonItem1.Enabled = false;
                barButtonItem2.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem4.Enabled = true;
                barButtonItem5.Enabled = true;
                barButtonItem7.Enabled = false;
                barButtonItem10.Enabled = false;
                barButtonItem11.Enabled = false;
                imageComboBoxEditSearchAccountType.Properties.ReadOnly = true;
                imageComboBoxEditSearchState.Properties.ReadOnly = true;
                dateEditInTimeStart.Properties.ReadOnly = true;
                dateEditInTimeEnd.Properties.ReadOnly = true;
                gridControlBillAppList.Enabled = false;
                gridControl1.Enabled = false;
            }
            else if (this.UIState_BillAppMain == UIState.Edit)
            {

                barButtonItem1.Enabled = false;
                barButtonItem2.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem4.Enabled = true;
                barButtonItem5.Enabled = true;
                barButtonItem7.Enabled = false;
                barButtonItem10.Enabled = true;
                barButtonItem11.Enabled = true;
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

        private void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            GetBillList();
        }
        //获取搜索条件和事件参数
        private void GetBillList()
        {
            decimal AccountID = 0;
            if (imageComboBoxEditSearchAccountType.EditValue != null)
            {
                AccountID = decimal.Parse(imageComboBoxEditSearchAccountType.EditValue.ToString());
            }
            DateTime? BegTime =  Convert.ToDateTime(dateEditInTimeStart.EditValue);
            DateTime? EndTime = System.DateTime.Now;;
            if (dateEditInTimeEnd.EditValue != null && dateEditInTimeEnd.Enabled)
            {
                EndTime = DateTime.Parse(dateEditInTimeEnd.EditValue.ToString());
            }
            string state = string.Empty;
            if (imageComboBoxEditSearchState.EditValue != null)
            {
                state = imageComboBoxEditSearchState.EditValue.ToString();
            }
            DataTable dt = billAppMain.GetBAppMainListShow(_sto_ID, AccountID, state, BegTime, EndTime);
            gridControlBillAppList.DataSource = dt;
            this.UIState_BillAppMain = UIState.Default;
            ShowUIState();
        }

        private void gridViewBillAppList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            getDetail();
        }
        public void getDetail()
        {
            DataRow dr = gridViewBillAppList.GetFocusedDataRow();
            if (dr != null)
            {
                decimal id = Convert.ToDecimal(dr["ID"]);
                string state = imageComboBoxEditSearchState.EditValue.ToString();
                if (state == "0")
                {
                    labelControlApproveState.Text = "审核状态：未审核";
                }
                else if (state == "5")
                {
                    labelControlApproveState.Text = "审核状态：已审核";
                }
                else
                {
                    labelControlApproveState.Text = "审核状态：整单退回";
                }
                labelControlNo.Text = "No："+dr["NO"].ToString();
                imageComboBoxEdit1.EditValue= imageComboBoxEditSearchAccountType.EditValue;
                imageComboBoxEditAppStorage.EditValue = dr["TARGET_ID"].ToString();
                textEditRemark.Text = dr["REMARK"].ToString();
                labelControlInputEmp.Text = "录入人：" + dr["INPUT_EMP"].ToString();
                labelControlInputTime.Text = "录入时间：" + dr["INPUT_TIME"].ToString();
                labelControlCheckEmp.Text = "审核人：" + dr["CHECK_EMP"].ToString();
                labelControlCheckTime.Text = "审核时间：" + dr["CHECK_TIME"].ToString();
                DataTable dt = billAppMain.GetBAppDetailListShow(id);
                gridControl1.DataSource = dt;
            }
            else
            {
                labelControlApproveState.Text = "审核状态：";
                labelControlNo.Text = "No：";
                imageComboBoxEditAppStorage.EditValue = -1;
                textEditRemark.Text = "";
                labelControlInputEmp.Text = "录入人：";
                labelControlInputTime.Text = "录入时间：";
                labelControlCheckEmp.Text = "审核人：";
                labelControlCheckTime.Text = "审核时间：";
            }
        }
        private void gridControlBillAppList_DataSourceChanged(object sender, EventArgs e)
        {
            getDetail();
        }
        //新增
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.UIState_BillAppMain = UIState.Add;
            ShowUIState();
        }
        //修改
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.UIState_BillAppMain = UIState.Edit;
            ShowUIState();
        }
        //删除
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            decimal id = Convert.ToDecimal(gridViewBillAppList.GetFocusedDataRow()["ID"]);
            if (gridView1.RowCount == 0)
            {
                if (billAppMain.deleteBappMain(id))
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
        //保存
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
             if(this.UIState_BillAppMain == UIState.Add)
             {
                 decimal sto_id = _sto_ID;
                 decimal target_id = Convert.ToDecimal(imageComboBoxEditAppStorage.EditValue);
                 decimal acc_id = Convert.ToDecimal(imageComboBoxEdit1.EditValue);
                 string RQ = DateTime.Now.ToString("yyyyMMdd");
                 decimal? ID = billAppMain.GetMaxID(RQ);
                 string remark = textEditRemark.EditValue.ToString();
                 if (billAppMain.insertBappMain(ID, sto_id, target_id, acc_id, _nameCode, remark))
                 {
                     MessageBoxUtil.ShowInformation("保存成功");
                     GetBillList();
                 }
                 else
                 {
                     MessageBoxUtil.ShowWarning("保存失败");
                 }
             }
             else if (this.UIState_BillAppMain == UIState.Edit)
             {
                 decimal ID = Convert.ToDecimal(gridViewBillAppList.GetFocusedDataRow()["ID"]);
                 for (int rowIndex = 0; rowIndex < this.gridView1.RowCount; rowIndex++)
                 {
                     decimal app_nums = Convert.ToDecimal(this.gridView1.GetRowCellValue(rowIndex, "APP_NUM"));
                     if (billAppMain.updateBappMain(ID, app_nums))
                     { 
                         
                     }
                     else
                     {
                         MessageBoxUtil.ShowWarning("修改申领明细失败");
                         return;
                     }
                     GetBillList();
                     MessageBoxUtil.ShowInformation("修改入库明细成功");
                 }
             }
        }
        //取消
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.UIState_BillAppMain = UIState.Default;
            ShowUIState();
        }
        //审核
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            decimal id = Convert.ToDecimal(gridViewBillAppList.GetFocusedDataRow()["ID"]);
            if (XtraMessageBox.Show("是否通过审核", "提示", MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                if (billAppMain.updateBappMain(id))
                {                    
                    MessageBoxUtil.ShowInformation("该申领单审核已通过");
                    GetBillList();
                }
                else
                {
                    MessageBoxUtil.ShowWarning("审核失败");
                }
            }
            else
            {
                MessageBoxUtil.ShowInformation("该申领单审核未通过");
            }
        }
        //加明细
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridView1.AddNewRow();
            string sto_ID = gridViewBillAppList.GetFocusedDataRow()["TARGET_ID"].ToString();
            this.LoadMaterialList(this.gridControlPopupMaterial, sto_ID, Convert.ToDecimal(this.imageComboBoxEditSearchAccountType.EditValue), true);
            gridControlPopupMaterial.Visible = true;
            barButtonItem6.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
        }
        //减明细
        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.GetFocusedRow() != null)
            {
                decimal ID = decimal.Parse(gridViewBillAppList.GetFocusedDataRow()["ID"].ToString());
                decimal seq = decimal.Parse(this.gridView1.GetFocusedDataRow()["SEQ"].ToString());
                if (billAppMain.deleteBappDetail(ID, seq))
                {
                    MessageBoxUtil.ShowInformation("删除明细成功");
                    getDetail();
                }
                else
                {
                    MessageBoxUtil.ShowWarning("删除明细失败");
                }
            }
        }
        //保存明细
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            decimal ID = decimal.Parse(gridViewBillAppList.GetFocusedDataRow()["ID"].ToString());
            decimal seq = gridView1.FocusedRowHandle + 1;
            decimal mat_id = Convert.ToDecimal(gridViewPopupMaterial.GetFocusedDataRow()["ID"]);
            if (gridView1.GetFocusedRowCellValue(gridColumnMaterialAppNum).Equals(DBNull.Value))
            {
                MessageBoxUtil.ShowWarning("入库数量不可为空");
                return;
            }
            decimal app_num = Convert.ToDecimal(gridView1.GetFocusedRowCellValue(gridColumnMaterialAppNum));
            decimal send_num = 0;
            decimal buy_num = 0;
            decimal mat_seq = Convert.ToDecimal(gridViewPopupStock.GetFocusedDataRow()["MAT_SEQ"]);
            string remark = gridView1.GetFocusedRowCellValue(gridColumnMaterialRemark).ToString();
            if (billAppMain.insertBappDetail(ID,seq,mat_id,app_num,send_num,remark,mat_seq))
            { 
                MessageBoxUtil.ShowInformation("保存明细成功");
                getDetail();
            }
            else
            {
                MessageBoxUtil.ShowWarning("保存明细失败");
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
            if (string.Empty.Equals(gridView1.GetFocusedRowCellDisplayText(gridColumnMaterialID)))
            {
                _dtMaterailSeqList = null;
            }
            else
            {
                decimal matId = decimal.Parse(gridView1.GetFocusedRowCellDisplayText(gridColumnMaterialID));
                decimal accId = decimal.Parse(imageComboBoxEditSearchAccountType.EditValue.ToString());
                _dtMaterailSeqList = billPriceMain.GetMaterialSeqList(accId, matId);
            }
            gridControlPopupStock.DataSource = _dtMaterailSeqList;
            gridControlPopupStock.Visible = true;
        }

        private void gridViewPopupMaterial_DoubleClick(object sender, EventArgs e)
        {
            gridControlPopupMaterial.Visible = false;
            DataRow dr = gridViewPopupMaterial.GetFocusedDataRow();
            if (dr != null)
            {
                DataRow dr2 = gridView1.GetFocusedDataRow();
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

        private void gridViewPopupStock_DoubleClick(object sender, EventArgs e)
        {
            gridControlPopupStock.Visible = false;
            DataRow dr = gridViewPopupStock.GetFocusedDataRow();
            if (dr != null)
            {
                DataRow dr2 = gridView1.GetFocusedDataRow();
                dr2["MAT_SEQ"] = dr["MAT_SEQ"];
            }
        }


    }
}
