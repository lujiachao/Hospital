using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BookBLL;
using Tool;
using BookUI.Util;

namespace BookUI
{
    public partial class FrmFactory : Form
    {
        
        private UIState _uIState_FFactory { get; set; } // 获取设置界面状态
        BillFactory billFactory = new BillFactory();
        public FrmFactory()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Load函数
        /// </summary>
        private void FrmFactory_Load(object sender, EventArgs e)
        {
            GetAccount();
            this._uIState_FFactory = UIState.browse;
            ShowUIState();
        }
        /// <summary>
        /// 获取初始化账册数据
        /// </summary>
        public void GetAccount()
        {
            DataTable dt = billFactory.BillGetAccount();
            gridControlAccountType.DataSource = dt;
        }
        /// <summary>
        ///获取厂家数据
        /// </summary>
        public void GetFactory(string Code)
        {
            DataTable dt = billFactory.BillGetFactory(Code);
            gridControlFactory.DataSource = dt;
        }
        /// <summary>
        ///账册主单焦点行改变事件
        /// </summary>
        private void gridViewAccountType_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridViewAccountType.GetFocusedDataRow();
            string code = dr["CODE"].ToString();
            GetFactory(code);
        }
        /// <summary>
        ///gridControlFactory数据绑定
        /// </summary>
        private void gridViewFactory_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridViewFactory.GetFocusedDataRow();
            if (dr != null)
            {
                textEditCode.EditValue = dr["CODE"].ToString();
                textEditName.EditValue = dr["NAME"].ToString();
                textEditSName.EditValue = dr["SHORT_NAME"].ToString();
                textEditRegion.EditValue = dr["REGION"].ToString();
                textEditLperson.EditValue = dr["LEGAL_PERSON"].ToString();
                textEditLman.EditValue = dr["LINKMEN"].ToString();
                textEditPhone.EditValue = dr["TELEPHONE"].ToString();
                textEditAddress.EditValue = dr["ADDRESS"].ToString();
                dateEditDate.EditValue = dr["EXPIRY_DATE"].ToString();
                textEditZipcode.EditValue = dr["EMAIL"].ToString();
                textEditLicelce.EditValue = dr["LICENCE"].ToString();
                comboBoxEditState.EditValue = dr["STATE"].ToString();
                textEditRemark.EditValue = dr["REMARK"].ToString();
            }
        }
        /// <summary>
        /// 绑定数据改变事件
        /// </summary>
        private void gridControlFactory_DataSourceChanged(object sender, EventArgs e)
        {
            DataRow dr = gridViewFactory.GetFocusedDataRow();
            textEditCode.EditValue = dr["CODE"].ToString();
            textEditName.EditValue = dr["NAME"].ToString();
            textEditSName.EditValue = dr["SHORT_NAME"].ToString();
            textEditRegion.EditValue = dr["REGION"].ToString();
            textEditLperson.EditValue = dr["LEGAL_PERSON"].ToString();
            textEditLman.EditValue = dr["LINKMEN"].ToString();
            textEditPhone.EditValue = dr["TELEPHONE"].ToString();
            textEditAddress.EditValue = dr["ADDRESS"].ToString();
            dateEditDate.EditValue = dr["EXPIRY_DATE"].ToString();
            textEditZipcode.EditValue = dr["EMAIL"].ToString();
            textEditLicelce.EditValue = dr["LICENCE"].ToString();
            comboBoxEditState.EditValue = dr["STATE"].ToString();
            textEditRemark.EditValue = dr["REMARK"].ToString();
        }
        /// <summary>
        /// 界面状态控制
        /// </summary>
        public void ShowUIState()
        {
            if (this._uIState_FFactory == UIState.browse)
            {
                barButtonItem1.Enabled = true;
                barButtonItem2.Enabled = true;
                barButtonItem3.Enabled = true;
                barButtonItem4.Enabled = false;
                barButtonItem5.Enabled = false;
                barButtonItem6.Enabled = true;
                textEditCode.Properties.ReadOnly = true;
                textEditName.Properties.ReadOnly = true;
                textEditSName.Properties.ReadOnly = true;
                textEditRegion.Properties.ReadOnly = true;
                textEditLperson.Properties.ReadOnly = true;
                textEditLman.Properties.ReadOnly = true;
                textEditPhone.Properties.ReadOnly = true;
                textEditAddress.Properties.ReadOnly = true;
                dateEditDate.Properties.ReadOnly = true;
                textEditZipcode.Properties.ReadOnly = true;
                textEditEmail.Properties.ReadOnly = true;
                textEditLicelce.Properties.ReadOnly = true;
                comboBoxEditState.Properties.ReadOnly = true;
                textEditRemark.Properties.ReadOnly = true;
            }
            if (this._uIState_FFactory == UIState.Add)
            {
                barButtonItem1.Enabled = false;
                barButtonItem2.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem4.Enabled = true;
                barButtonItem5.Enabled = true;
                barButtonItem6.Enabled = false;
                textEditCode.Properties.ReadOnly = false;
                textEditName.Properties.ReadOnly = false;
                textEditSName.Properties.ReadOnly = false;
                textEditRegion.Properties.ReadOnly = false;
                textEditLperson.Properties.ReadOnly = false;
                textEditLman.Properties.ReadOnly = false;
                textEditPhone.Properties.ReadOnly = false;
                textEditAddress.Properties.ReadOnly = false;
                dateEditDate.Properties.ReadOnly = false;
                textEditZipcode.Properties.ReadOnly = false;
                textEditEmail.Properties.ReadOnly = false;
                textEditLicelce.Properties.ReadOnly = false;
                comboBoxEditState.Properties.ReadOnly = false;
                textEditRemark.Properties.ReadOnly = false;
            }
            if (this._uIState_FFactory == UIState.Edit)
            {
                barButtonItem1.Enabled = false;
                barButtonItem2.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem4.Enabled = true;
                barButtonItem5.Enabled = true;
                barButtonItem6.Enabled = false;
                textEditCode.Properties.ReadOnly = true;
                textEditName.Properties.ReadOnly = false;
                textEditSName.Properties.ReadOnly = false;
                textEditRegion.Properties.ReadOnly = false;
                textEditLperson.Properties.ReadOnly = false;
                textEditLman.Properties.ReadOnly = false;
                textEditPhone.Properties.ReadOnly = false;
                textEditAddress.Properties.ReadOnly = false;
                dateEditDate.Properties.ReadOnly = false;
                textEditZipcode.Properties.ReadOnly = false;
                textEditEmail.Properties.ReadOnly = false;
                textEditLicelce.Properties.ReadOnly = false;
                comboBoxEditState.Properties.ReadOnly = false;
                textEditRemark.Properties.ReadOnly = false;
            }
        }
        /// <summary>
        /// 新增按钮的实现
        /// </summary>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this._uIState_FFactory = UIState.Add;
            ShowUIState();
            textEditCode.EditValue = "";
            textEditName.EditValue = "";
            textEditSName.EditValue = "";
            textEditRegion.EditValue = "";
            textEditLperson.EditValue = "";
            textEditLman.EditValue = "";
            textEditPhone.EditValue = "";
            textEditAddress.EditValue = "";
            dateEditDate.EditValue = "";
            textEditZipcode.EditValue = "";
            textEditEmail.EditValue = "";
            textEditLicelce.EditValue = "";
            comboBoxEditState.EditValue = "";
            textEditRemark.EditValue = "";
        }
        /// <summary>
        /// 修改按钮的实现
        /// </summary>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this._uIState_FFactory = UIState.Edit;
            ShowUIState();
        }
        /// <summary>
        /// 保存按钮的实现
        /// </summary>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow dr = gridViewAccountType.GetFocusedDataRow();
            string acc_id = dr["CODE"].ToString();
            string code = textEditCode.EditValue.ToString();
            string name = textEditName.EditValue.ToString();
            string sname = textEditSName.EditValue.ToString();
            string region = textEditRegion.EditValue.ToString();
            string lperson = textEditLperson.EditValue.ToString();
            string lman = textEditLman.EditValue.ToString();
            string phone = textEditPhone.EditValue.ToString();
            string address = textEditAddress.EditValue.ToString();
            string date = dateEditDate.EditValue.ToString();
            string zipcode = textEditZipcode.EditValue.ToString();
            string email = textEditEmail.EditValue.ToString();
            string lincelce = textEditLicelce.EditValue.ToString();
            string state = comboBoxEditState.EditValue.ToString();
            string remark = textEditRemark.EditValue.ToString();
            int i;
            if (textEditCode.EditValue.ToString() == "" || textEditCode.EditValue == null)
            {
                MessageBoxUtil.ShowWarning("代码不可为空 ！");
                textEditCode.Focus();
                return;
            }
            if (int.TryParse(code, out i) == false)
            {
                MessageBoxUtil.ShowWarning("代码只能是数字 ！");
                textEditCode.EditValue = "";
                textEditCode.Focus();
                return;
            }
            if (textEditName.EditValue.ToString() == "" || textEditName.EditValue == null)
            {
                MessageBoxUtil.ShowWarning("名称不可为空 ！");
                textEditName.Focus();
                return;
            }
            if (textEditSName.EditValue.ToString() == "" || textEditSName.EditValue == null)
            {
                MessageBoxUtil.ShowWarning("简称不可为空 ！");
                textEditSName.Focus();
                return;
            }
            if (this._uIState_FFactory == UIState.Add)
            {
                bool Bool = billFactory.BillCheckCode(code);
                if (Bool == true)
                {
                    MessageBoxUtil.ShowWarning("该代码已存在，    请重新输入");
                    textEditCode.EditValue = null;
                    textEditCode.Focus();
                    return;
                }
                if (billFactory.insertFactory(code, name, sname, region, lperson, lman, phone, address, zipcode, email, lincelce, date,state,remark,acc_id))
                {
                    MessageBoxUtil.ShowInformation("保存新厂商成功");
                    this._uIState_FFactory = UIState.browse;
                    ShowUIState();
                    GetAccount();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("保存新厂商失败，请重试");
                }
            }
            if (this._uIState_FFactory == UIState.Edit)
            {
                if (billFactory.updateFactory(code, name, sname, region, lperson, lman, phone, address, zipcode, email, lincelce, date, state, remark))
                {
                    MessageBoxUtil.ShowInformation("修改厂商信息成功");
                    this._uIState_FFactory = UIState.browse;
                    ShowUIState();
                    GetAccount();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("修改厂商信息失败，请重试");
                }
            }
        }
        /// <summary>
        /// 删除按钮的实现
        /// </summary>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string code = textEditCode.EditValue.ToString();
            if (billFactory.deleteFactory(code))
            {
                MessageBoxUtil.ShowInformation("成功删除厂商信息");
            }
            else
            {
                MessageBoxUtil.ShowInformation("删除厂商失败，请重试");
            }
        }
        /// <summary>
        /// 取消按钮的实现
        /// </summary>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridViewAccountType.FocusedRowHandle = -1;
            this._uIState_FFactory = UIState.browse;
            ShowUIState();
        }
    }
}
