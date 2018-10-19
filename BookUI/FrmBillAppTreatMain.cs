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
using KingT.MMIS.Common;

namespace BookUI
{
    public partial class FrmBillAppTreatMain : Form
    {
        decimal _sto_ID;//当前登录库
        decimal _nameCode;//当前登录用户
        BillImageComBox billImageComBox = new BillImageComBox();
        BillAppTreatMain billAppTreatMain = new BillAppTreatMain();
        public UIState UIState_BillAppTreatMain;//界面状态
        public FrmBillAppTreatMain(decimal sto_ID,string NameCode)
        {
            InitializeComponent();
            _sto_ID = sto_ID;
            _nameCode = decimal.Parse(NameCode);
        }

        private void FrmBillAppTreatMain_Load(object sender, EventArgs e)
        {
            this.FuzhuAccount(this.imageComboBoxEditAccountType);
            this.FuzhuAccount(this.imageComboBoxEditMainAccountType);
            LoadSearchState();
            this.UIState_BillAppTreatMain = UIState.Default;
            ShowUiState();
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
        #region 搜索功能

        /// <summary>
        /// 加载状态类型
        /// </summary>
        private void LoadSearchState()
        {
            this.imageComboBoxEditState.Properties.Items.Clear();
            ImageComboBoxItemCollection itemColApprove =
                new ImageComboBoxItemCollection(this.imageComboBoxEditState.Properties);
            itemColApprove.Add(new ImageComboBoxItem("未处理单据", "1"));
            itemColApprove.Add(new ImageComboBoxItem("出库确认", "3"));
            itemColApprove.Add(new ImageComboBoxItem("整单退回", "5"));
            this.imageComboBoxEditState.Properties.Items.AddRange(itemColApprove);
            if (this.imageComboBoxEditState.Properties.Items.Count > 0)
                this.imageComboBoxEditState.SelectedIndex = 0;
        }
        #endregion

        private void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            this.LoadBAppTreatMainList();
        }
        /// <summary>
        /// 根据条件加载申领单据主单列表
        /// </summary>
        private void LoadBAppTreatMainList()
        {
            decimal acc_id = Convert.ToDecimal(imageComboBoxEditAccountType.EditValue);
            string state = imageComboBoxEditState.EditValue.ToString();
            decimal sto_id = _sto_ID;
            DateTime? dtStart = null;
            DateTime? dtEnd = null;
            if (this.dateEditInTimeStart.Enabled && this.dateEditInTimeEnd.Enabled)
            {
                dtStart = Convert.ToDateTime(this.dateEditInTimeStart.EditValue);
                dtEnd = Convert.ToDateTime(this.dateEditInTimeEnd.EditValue);
            }
            DataTable dt = billAppTreatMain.GetBAppTreatMainListShow(acc_id,sto_id,state,dtStart,dtEnd);
            gridControl1.DataSource = dt;
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            getDetail();          
        }
        private void getDetail()
        {
            DataRow dr = gridView1.GetFocusedDataRow();
            if (dr != null)
            {
                decimal id = Convert.ToDecimal(dr["ID"]);
                DataTable dt = billAppTreatMain.GetBAppDetailListShow(id);
                DataTableUtil.RemoveDBNull(ref dt);
                gridControl2.DataSource = dt;
                labelControlNo.Text = "No：" + dr["NO"].ToString();
                imageComboBoxEditMainAccountType.EditValue = imageComboBoxEditAccountType.EditValue;
                labelControlTarget.Text = "申领仓库：" + dr["STO_NAME"].ToString();
                textEditRemark.Text = dr["REMARK"].ToString();
                labelControlInputEmp.Text = "申领人：" + dr["INPUT_NAME"].ToString();
                labelControlInputTime.Text = "申领时间：" + dr["INPUT_TIME"].ToString();
            }
            else
            {
                gridControl2.DataSource = null;
                labelControlNo.Text = "No：" ;
                imageComboBoxEditMainAccountType.EditValue = null;
                labelControlTarget.Text = "申领仓库：" ;
                textEditRemark.Text = string.Empty;
                labelControlInputEmp.Text = "申领人：" ;
                labelControlInputTime.Text = "申领时间：" ;
            }
        }
        private void gridControl1_DataSourceChanged(object sender, EventArgs e)
        {
            getDetail();
        }
        //修改
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.UIState_BillAppTreatMain = UIState.Edit;
            ShowUiState();
        }
        //保存
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (UIState_BillAppTreatMain == UIState.Edit)
            { 
                decimal id = Convert.ToDecimal(gridView1.GetFocusedDataRow()["ID"]);
                decimal seq = Convert.ToDecimal(gridView2.GetFocusedDataRow()["SEQ"]);
                decimal send_num = Convert.ToDecimal(this.gridView2.GetFocusedRowCellValue(gridColumnSendNum));
                string remark = this.gridView2.GetFocusedRowCellValue(gridColumnMaterialRemark).ToString();
                if (billAppTreatMain.updateBappMain(id, seq, send_num,remark))
                {
                    MessageBoxUtil.ShowInformation("修改申领数量成功");
                    getDetail();
                    UIState_BillAppTreatMain = UIState.Default;
                    ShowUiState();
                }
                else
                {
                    MessageBoxUtil.ShowWarning("修改申领数量失败");
                }
            }
        }
        //取消
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UIState_BillAppTreatMain = UIState.Default;
            ShowUiState();
        }
        //出库确认
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow dr = gridView1.GetFocusedDataRow();
            if(dr != null)
            {
                decimal id = Convert.ToDecimal(gridView1.GetFocusedDataRow()["ID"]);
                if (billAppTreatMain.BappOut(id))
                {
                    //MessageBoxUtil.ShowInformation("出库确认成功");
                }
                else
                {
                    MessageBoxUtil.ShowWarning("出库确认失败");
                    return;
                }
                string RQ = DateTime.Now.ToString("yyyyMMdd");
                decimal? ID = billAppTreatMain.GetMaxID(RQ);
                decimal sto_id = Convert.ToDecimal(dr["TARGET_ID"]);
                decimal acc_id = Convert.ToDecimal(dr["ACC_ID"]);
                string no = dr["NO"].ToString();
                decimal _class = 3;
                DateTime intime = DateTime.Now;
                decimal target_id = Convert.ToDecimal(dr["STO_ID"]);
                decimal app_emp = Convert.ToDecimal(dr["INPUT_EMP"]);
                DateTime app_time = Convert.ToDateTime(dr["INPUT_TIME"]);
                decimal input_emp = _nameCode;
                DateTime input_time = DateTime.Now;
                char state = '0';
                string remark = dr["REMARK"].ToString();
                char acc_bill_state = '0';
                if (billAppTreatMain.insertBOutTempMain(ID, sto_id, acc_id, no, _class, target_id, intime, app_emp, app_time, input_emp, input_time, state, remark, acc_bill_state))
                {

                }
                else
                {
                    MessageBoxUtil.ShowWarning("出库确认失败");
                    return;
                }
                for (int rowIndex = 0; rowIndex < this.gridView2.RowCount; rowIndex++)
                {
                    DataRow dr1 = gridView2.GetFocusedDataRow();
                    decimal seq = Convert.ToDecimal(this.gridView2.GetRowCellValue(rowIndex, "SEQ"));
                    decimal mat_id = Convert.ToDecimal(this.gridView2.GetRowCellValue(rowIndex, "MAT_ID"));
                    decimal mat_seq = Convert.ToDecimal(this.gridView2.GetRowCellValue(rowIndex, "MAT_SEQ"));
                    decimal nums = Convert.ToDecimal(this.gridView2.GetRowCellValue(rowIndex, "SEND_NUM"));
                    decimal buying_price = Convert.ToDecimal(this.gridView2.GetRowCellValue(rowIndex, "BUYING_PRICE"));
                    decimal retail_price = Convert.ToDecimal(this.gridView2.GetRowCellValue(rowIndex, "RETAIL_PRICE"));
                    decimal trade_price = Convert.ToDecimal(this.gridView2.GetRowCellValue(rowIndex, "TRADE_PRICE"));
                    string batch_no = "001";
                    string Remark = this.gridView2.GetRowCellValue(rowIndex, "REMARK").ToString();
                    if (billAppTreatMain.insertBOutTempDetail(ID, seq, mat_id, mat_seq, nums, buying_price, retail_price, trade_price, batch_no, remark))
                    {
                        MessageBoxUtil.ShowWarning("出库确认成功");
                    }
                    else
                    {
                        MessageBoxUtil.ShowWarning("出库确认失败");
                        return;
                    }
                }
            }
        }
        //整单退回
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            decimal id = Convert.ToDecimal(gridView1.GetFocusedDataRow()["ID"]);
            if (billAppTreatMain.updateBappState(id))
            {
                MessageBoxUtil.ShowInformation("整单退回成功");
                LoadBAppTreatMainList();
            }
            else
            {
                MessageBoxUtil.ShowWarning("整单退回失败");
            }
        }  
        public void ShowUiState()
        {
            if (UIState_BillAppTreatMain == UIState.Edit)
            {
                barButtonItem1.Enabled = false;
                barButtonItem2.Enabled = true;
                barButtonItem3.Enabled = true;
                barButtonItem9.Enabled = false;
                barButtonItem5.Enabled = false;
                gridColumnSendNum.OptionsColumn.AllowEdit = true;
                gridColumnSendNum.OptionsColumn.AllowFocus = true;
            }
            if (UIState_BillAppTreatMain == UIState.Default)
            {
                barButtonItem1.Enabled = true;
                barButtonItem2.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem9.Enabled = true;
                barButtonItem5.Enabled = true;
                gridColumnSendNum.OptionsColumn.AllowEdit = false;
                gridColumnSendNum.OptionsColumn.AllowFocus = false;
                if (imageComboBoxEditState.EditValue.ToString() == "3" || imageComboBoxEditState.EditValue.ToString() == "5")
                { 
                    barButtonItem9.Enabled = false;
                    barButtonItem5.Enabled = false;
                }
            }
        }
        //状态改变事件
        private void imageComboBoxEditState_EditValueChanged(object sender, EventArgs e)
        {
            string state = imageComboBoxEditState.EditValue.ToString();
            if (state == "1")
            {
                barButtonItem1.Enabled = true;
                barButtonItem9.Enabled = true;
                barButtonItem5.Enabled = true;
            }
            else if (state == "3")
            {
                barButtonItem1.Enabled = false;
                barButtonItem9.Enabled = false;
                barButtonItem5.Enabled = false;
            }
            else if (state == "5")
            {
                barButtonItem1.Enabled = false;
                barButtonItem9.Enabled = false;
                barButtonItem5.Enabled = false;
            }
        }

        
    }
}
