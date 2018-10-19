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
using Tool;

namespace BookUI
{
    public partial class FrmMaterialInfo : Form
    {
        private UIState _uIState_FMaterialInfo { get; set; }
        BillMaterialInfo billMaterialInfo = new  BillMaterialInfo();
        BillImageComBox billImageComBox = new BillImageComBox();
        public FrmMaterialInfo()
        {
            InitializeComponent();
        }
        /// <summary>
        /// load函数
        /// </summary>
        private void FrmMaterialInfo_Load(object sender, EventArgs e)
        {
            this.FuzhuFactory(this.imageComboBoxFactory);
            this.FuzhuCatalog(this.imageComboBoxEditCatalog);
            GetMaterialInfo();
            //this.FuzhuStorage(this.imageComboBoxEditInStorage);
            //this.FuzhuStorage(this.imageComboBoxEditOutStorage);
            this._uIState_FMaterialInfo = UIState.browse;
            ShowUIState();

        }
        /// <summary>
        /// 获取初始化品种·数据
        /// </summary>
        public void GetMaterialInfo()
        {
            DataTable dt = billMaterialInfo.BillGetMaterial();
            gridControlMaterialInfo.DataSource = dt;
        }
        /// <summary>
        /// imagecombox数据绑定
        /// </summary>
        public void FuzhuCatalog(ImageComboBoxEdit imageComboBoxEdit)
        {
            string stateIn = string.Empty;
            imageComboBoxEdit.Properties.Items.Clear();
            DataTable dt = billImageComBox.BillImageComboxCatalog();
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
        /// imagecombox数据绑定
        /// </summary>
        public void FuzhuFactory(ImageComboBoxEdit imageComboBoxEdit)
        {
            string stateIn = string.Empty;
            imageComboBoxEdit.Properties.Items.Clear();
            DataTable dt = billImageComBox.BillImageComboxFactory();
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
        /// 焦点行改变事件
        /// </summary>
        private void gridViewMaterialInfo_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridViewMaterialInfo.GetFocusedDataRow();
            if (dr != null)
            {
                textEditCode.EditValue = dr["CODE"].ToString();
                textEditName.EditValue = dr["NAME"].ToString();
                textEditSpec.EditValue = dr["SPEC"].ToString();
                textEditPermitNo.EditValue = dr["PERMITNO"].ToString();
                imageComboBoxEditState.EditValue = dr["STATE"].ToString();
                imageComboBoxEditGrade.EditValue = dr["GRADE"].ToString();
                imageComboBoxEditPriceRule.EditValue = dr["PRICE_RULE"].ToString();
                imageComboBoxEditPriceLevel.EditValue = dr["PRICE_LEVEL"].ToString();
                textEditRetailPrice.EditValue = dr["RETAIL_PRICE"].ToString();
                textEditTradePrice.EditValue = dr["TRADE_PRICE"].ToString();
                textEditBuyingPrice.EditValue = dr["BUYING_PRICE"].ToString();
                textEditMaxPrice.EditValue = dr["MAX_PRICE"].ToString();
                textEditHighLevel.EditValue = dr["HIGH_LEVEL"].ToString();
                textEditLowLevel.EditValue = dr["LOW_LEVEL"].ToString();
                imageComboBoxFactory.EditValue = dr["FACTORY"].ToString();
                textEditUnit.EditValue = dr["UNIT"].ToString();
                textEditUnitRatio.EditValue = dr["RATIO"].ToString();
                textEditMidUnit.EditValue = dr["MID_UNIT"].ToString();
                textEditMidRatio.EditValue = dr["RATIO"].ToString();
                imageComboBoxEditCatalog.EditValue = dr["CAT_ID"].ToString();
            }
        }
        public void ShowUIState()
        {
            if (this._uIState_FMaterialInfo == UIState.browse)
            {
                barButtonItem1.Enabled = true;
                barButtonItem2.Enabled = true;
                barButtonItem3.Enabled = true;
                barButtonItem4.Enabled = false;
                barButtonItem5.Enabled = false;
                textEditCode.Properties.ReadOnly = true;
                textEditName.Properties.ReadOnly = true;
                textEditSpec.Properties.ReadOnly = true;
                textEditPermitNo.Properties.ReadOnly = true;
                imageComboBoxEditState.Properties.ReadOnly = true;
                imageComboBoxEditGrade.Properties.ReadOnly = true;
                imageComboBoxEditPriceRule.Properties.ReadOnly = true;
                imageComboBoxEditPriceLevel.Properties.ReadOnly = true;
                textEditRetailPrice.Properties.ReadOnly = true;
                textEditTradePrice.Properties.ReadOnly = true;
                textEditBuyingPrice.Properties.ReadOnly = true;
                textEditMaxPrice.Properties.ReadOnly = true;
                textEditHighLevel.Properties.ReadOnly = true;
                textEditLowLevel.Properties.ReadOnly = true;
                imageComboBoxFactory.Properties.ReadOnly = true;
                textEditUnit.Properties.ReadOnly = true;
                textEditUnitRatio.Properties.ReadOnly = true;
                textEditMidUnit.Properties.ReadOnly = true;
                textEditMidRatio.Properties.ReadOnly = true;
                textEditRegisterNo.Properties.ReadOnly = true;
                dateEditRegisterValYearMonth.Properties.ReadOnly = true;
                imageComboBoxEditCatalog.Properties.ReadOnly = true;
            }
            if (this._uIState_FMaterialInfo == UIState.Add)
            {
                barButtonItem1.Enabled = false;
                barButtonItem2.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem4.Enabled = true;
                barButtonItem5.Enabled = true;
                textEditCode.Properties.ReadOnly = false;
                textEditName.Properties.ReadOnly = false;
                textEditSpec.Properties.ReadOnly = false;
                textEditPermitNo.Properties.ReadOnly = false;
                imageComboBoxEditState.Properties.ReadOnly = false;
                imageComboBoxEditGrade.Properties.ReadOnly = false;
                imageComboBoxEditPriceRule.Properties.ReadOnly = false;
                imageComboBoxEditPriceLevel.Properties.ReadOnly = false;
                textEditRetailPrice.Properties.ReadOnly = false;
                textEditTradePrice.Properties.ReadOnly = false;
                textEditBuyingPrice.Properties.ReadOnly = false;
                textEditMaxPrice.Properties.ReadOnly = false;
                textEditHighLevel.Properties.ReadOnly = false;
                textEditLowLevel.Properties.ReadOnly = false;
                imageComboBoxFactory.Properties.ReadOnly = false;
                textEditUnit.Properties.ReadOnly = false;
                textEditUnitRatio.Properties.ReadOnly = false;
                textEditMidUnit.Properties.ReadOnly = false;
                textEditMidRatio.Properties.ReadOnly = false;
                textEditRegisterNo.Properties.ReadOnly = false;
                dateEditRegisterValYearMonth.Properties.ReadOnly = false;
                imageComboBoxEditCatalog.Properties.ReadOnly = false;
            }
            if (this._uIState_FMaterialInfo == UIState.Edit)
            {
                barButtonItem1.Enabled = false;
                barButtonItem2.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem4.Enabled = true;
                barButtonItem5.Enabled = true;
                textEditCode.Properties.ReadOnly = true;
                textEditName.Properties.ReadOnly = false;
                textEditSpec.Properties.ReadOnly = false;
                textEditPermitNo.Properties.ReadOnly = false;
                imageComboBoxEditState.Properties.ReadOnly = false;
                imageComboBoxEditGrade.Properties.ReadOnly = false;
                imageComboBoxEditPriceRule.Properties.ReadOnly = false;
                imageComboBoxEditPriceLevel.Properties.ReadOnly = false;
                textEditRetailPrice.Properties.ReadOnly = false;
                textEditTradePrice.Properties.ReadOnly = false;
                textEditBuyingPrice.Properties.ReadOnly = false;
                textEditMaxPrice.Properties.ReadOnly = false;
                textEditHighLevel.Properties.ReadOnly = false;
                textEditLowLevel.Properties.ReadOnly = false;
                imageComboBoxFactory.Properties.ReadOnly = false;
                textEditUnit.Properties.ReadOnly = false;
                textEditUnitRatio.Properties.ReadOnly = false;
                textEditMidUnit.Properties.ReadOnly = false;
                textEditMidRatio.Properties.ReadOnly = false;
                textEditRegisterNo.Properties.ReadOnly = false;
                dateEditRegisterValYearMonth.Properties.ReadOnly = false;
                imageComboBoxEditCatalog.Properties.ReadOnly = false;
            }
        }
        /// <summary>
        /// 新增事件
        /// </summary>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this._uIState_FMaterialInfo = UIState.Add;
            ShowUIState();
            textEditCode.EditValue = "";
            textEditName.EditValue = "";
            textEditSpec.EditValue = "";
            textEditPermitNo.EditValue = "";
            imageComboBoxEditState.EditValue = "";
            imageComboBoxEditGrade.EditValue = "";
            imageComboBoxEditPriceRule.EditValue = "";
            imageComboBoxEditPriceLevel.EditValue = "";
            textEditRetailPrice.EditValue = "";
            textEditTradePrice.EditValue = "";
            textEditBuyingPrice.EditValue = "";
            textEditMaxPrice.EditValue = "";
            textEditHighLevel.EditValue = "";
            textEditLowLevel.EditValue = "";
            imageComboBoxFactory.EditValue = "";
            textEditUnit.EditValue = "";
            textEditUnitRatio.EditValue = "";
            textEditMidUnit.EditValue = "";
            textEditMidRatio.EditValue = "";
            textEditRegisterNo.EditValue = "";
            dateEditRegisterValYearMonth.EditValue = "";
            imageComboBoxEditCatalog.EditValue = "";
        }
        /// <summary>
        /// 修改事件
        /// </summary>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this._uIState_FMaterialInfo = UIState.Edit;
            ShowUIState();
        }
        /// <summary>
        /// 删除事件
        /// </summary>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string code = textEditCode.EditValue.ToString();
            if (billMaterialInfo.deleteMaterial(code))
            {
                MessageBoxUtil.ShowInformation("删除新品种成功");
                this._uIState_FMaterialInfo = UIState.browse;
                ShowUIState();
                GetMaterialInfo();
            }
            else
            {
                MessageBoxUtil.ShowInformation("删除新品种失败，请重试");
            }
            
        }
        /// <summary>
        /// 保存事件
        /// </summary>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this._uIState_FMaterialInfo == UIState.Add || this._uIState_FMaterialInfo == UIState.Edit)
            {
                if (textEditCode.EditValue.ToString() == "" || textEditCode.EditValue == null)
                {
                    MessageBoxUtil.ShowWarning("代码不可为空");
                    textEditCode.Focus();
                    return;
                }
                if (imageComboBoxEditCatalog.SelectedIndex == -1)
                {
                    MessageBoxUtil.ShowWarning("通用目录不可为空");
                    textEditCode.Focus();
                    return;
                }
                if (textEditName.EditValue.ToString() == "" || textEditName.EditValue == null)
                {
                    MessageBoxUtil.ShowWarning("名称不可为空");
                    textEditCode.Focus();
                    return;
                }
                if (imageComboBoxEditGrade.SelectedIndex == -1)
                {
                    MessageBoxUtil.ShowWarning("级别不可为空");
                    textEditCode.Focus();
                    return;
                }
                if (textEditUnit.EditValue.ToString() == "" || textEditUnit.EditValue == null)
                {
                    MessageBoxUtil.ShowWarning("采购单位不可为空");
                    textEditCode.Focus();
                    return;
                }
                if (textEditUnitRatio.EditValue.ToString() == "" || textEditUnitRatio.EditValue == null)
                {
                    MessageBoxUtil.ShowWarning("比例不可为空");
                    textEditCode.Focus();
                    return;
                }
                if (imageComboBoxEditPriceRule.SelectedIndex == -1)
                {
                    MessageBoxUtil.ShowWarning("计价规则不可为空");
                    textEditCode.Focus();
                    return;
                }
                if (textEditRetailPrice.EditValue.ToString() == "" || textEditRetailPrice.EditValue == null)
                {
                    MessageBoxUtil.ShowWarning("零价不可为空");
                    textEditCode.Focus();
                    return;
                }
                if (textEditTradePrice.EditValue.ToString() == "" || textEditTradePrice.EditValue == null)
                {
                    MessageBoxUtil.ShowWarning("批价不可为空");
                    textEditCode.Focus();
                    return;
                }
                if (textEditMaxPrice.EditValue.ToString() == "" || textEditMaxPrice.EditValue == null)
                {
                    MessageBoxUtil.ShowWarning("最高零价不可为空");
                    textEditCode.Focus();
                    return;
                }
                if (textEditUnit.EditValue.ToString() == "" || textEditUnit.EditValue == null)
                {
                    MessageBoxUtil.ShowWarning("单位不可为空");
                    textEditCode.Focus();
                    return;
                }
                if (textEditBuyingPrice.EditValue.ToString() == "" || textEditBuyingPrice.EditValue == null)
                {
                    MessageBoxUtil.ShowWarning("计划进价不可为空");
                    textEditCode.Focus();
                    return;
                }
                if (imageComboBoxEditPriceLevel.SelectedIndex == -1)
                {
                    MessageBoxUtil.ShowWarning("计价级别不可为空");
                    textEditCode.Focus();
                    return;
                }
                if (imageComboBoxEditState.SelectedIndex == -1)
                {
                    MessageBoxUtil.ShowWarning("状态不可为空");
                    textEditCode.Focus();
                    return;
                }
            }
            string code = textEditCode.EditValue.ToString();
            string name = textEditName.EditValue.ToString();
            string spec = textEditSpec.EditValue.ToString();
            string permitNo = textEditPermitNo.EditValue.ToString();
            string state = imageComboBoxEditState.EditValue.ToString();
            string grade = imageComboBoxEditGrade.EditValue.ToString();
            string pricerule = imageComboBoxEditPriceRule.EditValue.ToString();
            string pricelevel = imageComboBoxEditPriceLevel.EditValue.ToString();
            string retailprice = textEditRetailPrice.EditValue.ToString();
            string tradeprice = textEditTradePrice.EditValue.ToString();
            string buyingprice = textEditBuyingPrice.EditValue.ToString();
            string maxprice = textEditMaxPrice.EditValue.ToString();
            string hignlevel = textEditHighLevel.EditValue.ToString();
            string lowlevel = textEditLowLevel.EditValue.ToString();
            string factory = imageComboBoxFactory.EditValue.ToString();
            string unit = textEditUnit.EditValue.ToString();
            string unitratio = textEditUnitRatio.EditValue.ToString();
            string midunit = textEditMidUnit.EditValue.ToString();
            string midratio = textEditMidRatio.EditValue.ToString();
            //string registerNo = textEditRegisterNo.EditValue.ToString();
            //string registervalyearmonth = dateEditRegisterValYearMonth.EditValue.ToString();
            string catalog = imageComboBoxEditCatalog.EditValue.ToString();
            if (this._uIState_FMaterialInfo == UIState.Add)
            {
                if (billMaterialInfo.insertMaterialInfo(catalog, code, name, spec, factory, grade, permitNo, unit, unitratio, pricerule, retailprice, tradeprice, maxprice, buyingprice, pricelevel, hignlevel, lowlevel, state, midunit))
                {
                    MessageBoxUtil.ShowInformation("新增新品种成功");
                    this._uIState_FMaterialInfo = UIState.browse;
                    ShowUIState();
                    GetMaterialInfo();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("新增新品种失败，请重试");
                }
            }
            if (this._uIState_FMaterialInfo == UIState.Edit)
            {
                if (billMaterialInfo.updateMaterialInfo(catalog, code, name, spec, factory, grade, permitNo, unit, unitratio, pricerule, retailprice, tradeprice, maxprice, buyingprice, pricelevel, hignlevel, lowlevel, state, midunit))
                {
                    MessageBoxUtil.ShowInformation("修改新品种成功");
                    this._uIState_FMaterialInfo = UIState.browse;
                    ShowUIState();
                    GetMaterialInfo();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("修改新品种失败，请重试");
                }
            }
        }
        /// <summary>
        /// 取消事件
        /// </summary>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GetMaterialInfo();
            this._uIState_FMaterialInfo = UIState.browse;
            ShowUIState();
        }

        private void gridControlMaterialInfo_DataSourceChanged(object sender, EventArgs e)
        {
            DataRow dr = gridViewMaterialInfo.GetFocusedDataRow();
            textEditCode.EditValue = dr["CODE"].ToString();
            textEditName.EditValue = dr["NAME"].ToString();
            textEditSpec.EditValue = dr["SPEC"].ToString();
            textEditPermitNo.EditValue = dr["PERMITNO"].ToString();
            imageComboBoxEditState.EditValue = dr["STATE"].ToString();
            imageComboBoxEditGrade.EditValue = dr["GRADE"].ToString();
            imageComboBoxEditPriceRule.EditValue = dr["PRICE_RULE"].ToString();
            imageComboBoxEditPriceLevel.EditValue = dr["PRICE_LEVEL"].ToString();
            textEditRetailPrice.EditValue = dr["RETAIL_PRICE"].ToString();
            textEditTradePrice.EditValue = dr["TRADE_PRICE"].ToString();
            textEditBuyingPrice.EditValue = dr["BUYING_PRICE"].ToString();
            textEditMaxPrice.EditValue = dr["MAX_PRICE"].ToString();
            textEditHighLevel.EditValue = dr["HIGH_LEVEL"].ToString();
            textEditLowLevel.EditValue = dr["LOW_LEVEL"].ToString();
            imageComboBoxFactory.EditValue = dr["FACTORY"].ToString();
            textEditUnit.EditValue = dr["UNIT"].ToString();
            textEditUnitRatio.EditValue = dr["RATIO"].ToString();
            textEditMidUnit.EditValue = dr["MID_UNIT"].ToString();
            textEditMidRatio.EditValue = dr["RATIO"].ToString();
            imageComboBoxEditCatalog.EditValue = dr["CAT_ID"].ToString();
        }
        /// <summary>
        /// 判断code是否存在
        /// </summary>
        private void textEditCode_Leave(object sender, EventArgs e)
        {
            string code = textEditCode.EditValue.ToString();
            if (this._uIState_FMaterialInfo == UIState.Add)
            {
                if (code != null || code != "")
                {
                    bool Bool = billMaterialInfo.BillCheckCode(code);
                    if (Bool == true)
                    {
                        MessageBoxUtil.ShowWarning("该代码已存在，    请重新输入");
                        textEditCode.EditValue = null;
                        textEditCode.Focus();
                    }
                }
            }
        }
    }
}
