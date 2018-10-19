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
using DevExpress.XtraTreeList.Nodes;
using Tool;

namespace BookUI
{
    public partial class FrmCatalogInfo : Form
    {
        BillCatalogInfo billCatalogInfo = new BillCatalogInfo();
        BillImageComBox billImageComBox = new BillImageComBox();
        BillAccount billAccount = new BillAccount();
        /// <summary>
        /// 获取设置界面状态
        /// </summary>
        private UIState _uIState_FCatalogInfo { get; set; }
        public FrmCatalogInfo()
        {
            InitializeComponent();
        }
        /// <summary>
        /// load 事件
        /// </summary>
        private void FrmCatalogInfo_Load(object sender, EventArgs e)
        {
            this.FuzhuAccount(this.imageComboBoxEditAccount);
            this.MaterialClass(this.imageComboBoxEdit1);
            BindDefaultData();
            this._uIState_FCatalogInfo = UIState.browse;
            this.ShowUIState();
        }
        /// <summary>
        /// treeList1 数据绑定
        /// </summary>
        public void BindDefaultData()
        {
            DataTable dt = billCatalogInfo.GetMaterialTreeListInfo(Convert.ToDecimal(this.imageComboBoxEditAccount.EditValue), 2);
            this.treeList1.DataSource = dt;
            dt.DefaultView.RowFilter = "STATE=" + this.radioGroupMaterial.EditValue + " OR LEVELSTATE <> 2";
        }
        /// <summary>
        /// 分类imagecombox数据绑定
        /// </summary>
        public void MaterialClass(ImageComboBoxEdit imageComboBoxEdit)
        {
            string stateIn = string.Empty;
            imageComboBoxEdit.Properties.Items.Clear();
            DataTable dt = billImageComBox.BillImageComboxMatClass();
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
        /// 账册下拉框编辑值改变
        /// </summary>
        /// <param name="sender">事件发送源</param>
        /// <param name="e">包含了事件所需的参数</param>
        private void imageComboBoxEditAccount_EditValueChanged(object sender, EventArgs e)
        {
            this.BindDefaultData();
            this.treeList1.CollapseAll();
        }
        /// <summary>
        /// 可用性选择变更
        /// </summary>
        /// <param name="sender">事件发送源</param>
        /// <param name="e">包含了事件所需的参数</param>
        private void radioGroupMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtMaterial = (DataTable)(this.treeList1.DataSource);
            dtMaterial.DefaultView.RowFilter = "STATE=" + this.radioGroupMaterial.EditValue + " OR LEVELSTATE <> 2";
        }
        /// <summary>
        /// 物资树焦点改变事件
        /// </summary>
        /// <param name="sender">事件发送源</param>
        /// <param name="e">包含了事件所需的参数</param>
        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            TreeListNode _FocusedNode = treeList1.FocusedNode;
            if (e.Node != null)
            {
                decimal catID = Convert.ToDecimal(e.Node.GetValue("TAGID"));
                this.LoadMaterialList(catID);
                if (_FocusedNode != null)
                {

                    textEditCode.Text = _FocusedNode["ID"].ToString();
                    textEditName.Text = _FocusedNode["NAME"].ToString();
                }
            }
        }
        /// <summary>
        /// 加载品种明细列表
        /// </summary>
        /// <param name="cat_ID">通用目录ID</param>
        private void LoadMaterialList(decimal cat_ID)
        {
            DataTable dt = billCatalogInfo.GetMaterialList(cat_ID);
            gridControlMaterial.DataSource = dt;
        }
        /// <summary>
        /// 新增目录按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送源</param>
        /// <param name="e">包含了事件所需的参数</param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this._uIState_FCatalogInfo = UIState.Add;
            this.ShowUIState();
            textEditCode.EditValue = "";
            textEditName.EditValue = "";
            imageComboBoxEditAppMode.EditValue = "";
            imageComboBoxEdit1.EditValue = "";
            textEditInputCode1.EditValue = "";
            textEditInputCode2.EditValue = "";
            imageComboBoxEditPriceRule.EditValue = "";
            imageComboBoxEditState.EditValue = "";
            checkEditCansell.Checked = false;
        }
        /// <summary>
        /// 修改目录按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送源</param>
        /// <param name="e">包含了事件所需的参数</param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this._uIState_FCatalogInfo = UIState.Edit;
            this.ShowUIState();
        }
        /// <summary>
        /// 删除目录按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送源</param>
        /// <param name="e">包含了事件所需的参数</param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string code = textEditCode.EditValue.ToString();
            if (billCatalogInfo.deleteCatalog(code))
            {
                MessageBoxUtil.ShowQuestion("删除成功！");
                BindDefaultData();
            }
            else
            {
                MessageBoxUtil.ShowError("删除失败！");
            }
        }
        /// <summary>
        /// 保存目录按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送源</param>
        /// <param name="e">包含了事件所需的参数</param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (textEditCode.EditValue.ToString() == "" || textEditCode.EditValue == null)
            {
                MessageBoxUtil.ShowWarning("代码不可为空");
                textEditCode.Focus();
                return;
            }
            if (textEditName.EditValue.ToString() == "" || textEditName.EditValue == null)
            {
                MessageBoxUtil.ShowWarning("名称不可为空");
                textEditName.Focus();
                return;
            }
            if (imageComboBoxEdit1.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowWarning("分类不可为空");
                textEditName.Focus();
                return;
            }
            if (imageComboBoxEditPriceRule.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowWarning("计价规则不可为空");
                textEditName.Focus();
                return;
            }
            if (imageComboBoxEditAppMode.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowWarning("申领方式不可为空");
                textEditName.Focus();
                return;
            }
            if (imageComboBoxEditState.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowWarning("状态不可为空");
                textEditName.Focus();
                return;
            }
            string code = textEditCode.EditValue.ToString();
            string name = textEditName.EditValue.ToString();
            string Account = imageComboBoxEditAccount.EditValue.ToString();
            string type = imageComboBoxEdit1.EditValue.ToString();
            string Price_rule = imageComboBoxEditPriceRule.EditValue.ToString();
            string App_mode = imageComboBoxEditAppMode.EditValue.ToString();
            string inputcode1 = textEditInputCode1.EditValue.ToString();
            string inputcode2 = textEditInputCode2.EditValue.ToString();
            string state = imageComboBoxEditState.EditValue.ToString();
            string cansell;
            if (checkEditCansell.Checked == true)
            {
                cansell = "1";
            }
            else
            {
                cansell = "0";
            }
            if (this._uIState_FCatalogInfo == UIState.Add)
            {
                if (billCatalogInfo.insertCatalog(code,name,Account,type,Price_rule,App_mode,inputcode1,inputcode2,state,cansell))
                {
                    MessageBoxUtil.ShowInformation("保存新目录成功");
                    this._uIState_FCatalogInfo = UIState.browse;
                    ShowUIState();
                    BindDefaultData();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("保存新目录失败，请重试");
                }
            }
            if (this._uIState_FCatalogInfo == UIState.Edit)
            {
                if (billCatalogInfo.updateCatalog(code, name, Account, type, Price_rule, App_mode, inputcode1, inputcode2, state, cansell))
                {
                    MessageBoxUtil.ShowInformation("修改新目录成功");
                    this._uIState_FCatalogInfo = UIState.browse;
                    ShowUIState();
                    BindDefaultData();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("修改新目录失败，请重试");
                }
            }
        }
        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送源</param>
        /// <param name="e">包含了事件所需的参数</param>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this._uIState_FCatalogInfo = UIState.browse;
            this.ShowUIState();
        }
        /// <summary>
        /// 界面状态设置
        /// </summary>
        public void ShowUIState()
        {
            if (this._uIState_FCatalogInfo == UIState.browse)
            {
                barButtonItem1.Enabled = true;
                barButtonItem2.Enabled = true;
                barButtonItem3.Enabled = true;
                barButtonItem4.Enabled = false;
                barButtonItem5.Enabled = false;
                imageComboBoxEditAccount.Properties.ReadOnly = false;
                radioGroupMaterial.Properties.ReadOnly = false;
                textEditCode.Properties.ReadOnly = true;
                textEditName.Properties.ReadOnly = true;
                imageComboBoxEditAppMode.Properties.ReadOnly = true;
                imageComboBoxEdit1.Properties.ReadOnly = true;
                textEditInputCode1.Properties.ReadOnly = true;
                textEditInputCode2.Properties.ReadOnly = true;
                imageComboBoxEditPriceRule.Properties.ReadOnly = true;
                imageComboBoxEditState.Properties.ReadOnly = true;
                checkEditCansell.Properties.ReadOnly = true;
            }
            if (this._uIState_FCatalogInfo == UIState.Add)
            {
                barButtonItem1.Enabled = false;
                barButtonItem2.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem4.Enabled = true;
                barButtonItem5.Enabled = true;
                imageComboBoxEditAccount.Properties.ReadOnly = true;
                radioGroupMaterial.Properties.ReadOnly = true;
                textEditCode.Properties.ReadOnly = false;
                textEditName.Properties.ReadOnly = false;
                imageComboBoxEditAppMode.Properties.ReadOnly = false;
                imageComboBoxEdit1.Properties.ReadOnly = false;
                textEditInputCode1.Properties.ReadOnly = false;
                textEditInputCode2.Properties.ReadOnly = false;
                imageComboBoxEditPriceRule.Properties.ReadOnly = false;
                imageComboBoxEditState.Properties.ReadOnly = false;
                checkEditCansell.Properties.ReadOnly = false;
            }
            if (this._uIState_FCatalogInfo == UIState.Edit)
            {
                barButtonItem1.Enabled = false;
                barButtonItem2.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem4.Enabled = true;
                barButtonItem5.Enabled = true;
                imageComboBoxEditAccount.Properties.ReadOnly = true;
                radioGroupMaterial.Properties.ReadOnly = true;
                textEditCode.Properties.ReadOnly = true;
                textEditName.Properties.ReadOnly = false;
                imageComboBoxEditAppMode.Properties.ReadOnly = false;
                imageComboBoxEdit1.Properties.ReadOnly = false;
                textEditInputCode1.Properties.ReadOnly = false;
                textEditInputCode2.Properties.ReadOnly = false;
                imageComboBoxEditPriceRule.Properties.ReadOnly = false;
                imageComboBoxEditState.Properties.ReadOnly = false;
                checkEditCansell.Properties.ReadOnly = false;                   
            }
        }
        /// <summary>
        /// 代码不能重复
        /// </summary>
        private void textEditCode_Leave(object sender, EventArgs e)
        {
            if (this._uIState_FCatalogInfo == UIState.Add)
            {
                string code = textEditCode.EditValue.ToString();
                int i;
                bool Bool = billCatalogInfo.BillCheckCode(code);
                if (Bool == true)
                {
                    MessageBoxUtil.ShowWarning("该代码已存在，    请重新输入");
                    textEditCode.EditValue = null;
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
            }
        }
        /// <summary>
        /// 自动获取拼音码和五笔码
        /// </summary>
        string InputCode1;
        string InputCode2;
        private void textEditName_Leave(object sender, EventArgs e)
        {
            if (this._uIState_FCatalogInfo == UIState.Add || this._uIState_FCatalogInfo == UIState.Edit)
            {
                string name = textEditName.EditValue.ToString();
                string InputCode1 = "";
                string InputCode2 = "";
                for (int j = 0; j < name.Length; j++)
                {
                    string mark = name.Substring(j, 1);
                    string py = "PY";
                    string wb = "WB";
                    string PY = billAccount.BillPY(py, mark);
                    string WB = billAccount.BillPY(wb, mark);
                    string WB1 = WB.Substring(0, 1);
                    InputCode1 = InputCode1 + PY;
                    InputCode2 += WB1;
                }
                textEditInputCode1.EditValue = InputCode1;
                textEditInputCode2.EditValue = InputCode2;
            }
        }
    }
}
