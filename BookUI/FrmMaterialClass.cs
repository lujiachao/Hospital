using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BookBLL;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraEditors.Controls;
using Tool;
using BookUI.Util;

namespace BookUI
{
    public partial class FrmMaterialClass : Form
    {
        BillMaterialClass billMaterialClass = new BillMaterialClass();
        BillAccount billAccount = new BillAccount();
        /// <summary>
        /// 获取设置界面状态
        /// </summary>
        private UIState _uIState_FMaterialClass { get; set; }
        public FrmMaterialClass()
        {
            InitializeComponent();
        }
        /// <summary>
        /// load函数
        /// </summary>
        private void FrmMaterialClass_Load(object sender, EventArgs e)
        {
            DataTable dt = billMaterialClass.BillGetAccount();
            gridControlAccountType.DataSource = dt;
            GetDate();
            DataTable dtManageType = new DataTable();
            dtManageType.Columns.Add("manage_type_value");
            dtManageType.Columns.Add("manage_type_text");
            dtManageType.Rows.Add("1", "材料");
            dtManageType.Rows.Add("2", "普通资产");
            dtManageType.Rows.Add("3", "设备");
            dtManageType.Rows.Add("4", "药品");
            dtManageType.Rows.Add("5", "消毒包");
            foreach (DataRow dr in dtManageType.Rows)
            {
                repositoryItemImageComboBoxManageType.Items.Add(new ImageComboBoxItem(dr["manage_type_text"].ToString(),
                                                                                          dr["manage_type_value"].ToString(),
                                                                                          -1));
            }
            this._uIState_FMaterialClass = UIState.browse;
            ShowUIState();
        }
        /// <summary>
        /// 分类数据绑定
        /// </summary>
        public void GetDate()
        {
            DataRow dr = gridViewAccountType.GetFocusedDataRow();
            string acc_ID = dr["CODE"].ToString();
            DataTable dtMatClassList = billMaterialClass.BillGetMatClass(acc_ID);
            DataRow drMatClass = dtMatClassList.NewRow();
            drMatClass["ID"] = 0;
            drMatClass["CODE"] = "";
            drMatClass["NAME"] = "物资分类";
            drMatClass["PID"] = -1;
            drMatClass["MANAGE_TYPE"] = "-1";
            drMatClass["STATE"] = "1";
            dtMatClassList.Rows.Add(drMatClass);
            treeListMatClass.DataSource = dtMatClassList;
            treeListMatClass.ExpandAll();//展开所有节点  
        }
        /// <summary>
        /// 账册表行改变事件
        /// </summary>
        private void gridViewAccountType_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GetDate();
        }
        /// <summary>
        /// 分类树点击事件
        /// </summary>
        private void treeListMatClass_Click(object sender, EventArgs e)
        {
            TreeListNode _FocusedNode = treeListMatClass.FocusedNode;
            // 焦点定位
            if (_FocusedNode == null)
            {
                textEditCode.Text = "";
                textEditName.Text = "";
                imageComboBoxEditManageType.SelectedIndex = -1;
                imageComboBoxEditState.SelectedIndex = -1;
                labelControlParentNode.Text = "";
                textEditInputCode1.Text = "";
                textEditInputCode2.Text = "";
                textEditRemark.Text = "";
            }
            else
            {
                textEditCode.Text = _FocusedNode["CODE"].ToString();
                textEditName.Text = _FocusedNode["NAME"].ToString();
                imageComboBoxEditManageType.EditValue = _FocusedNode["MANAGE_TYPE"].ToString();
                imageComboBoxEditState.EditValue = Int32.Parse(_FocusedNode["STATE"].ToString());
                if (_FocusedNode.ParentNode == null)
                {
                    labelControlParentNode.Text = "";
                }
                else
                {
                    labelControlParentNode.Text = _FocusedNode.ParentNode["NAME"].ToString();
                }
                textEditInputCode1.Text = _FocusedNode["INPUTCODE1"].ToString();
                textEditInputCode2.Text = _FocusedNode["INPUTCODE2"].ToString();
                textEditRemark.Text = _FocusedNode["REMARK"].ToString();

            }
        }
        /// <summary>
        /// 界面状态设置
        /// </summary>
        public void ShowUIState()
        {
            if (this._uIState_FMaterialClass == UIState.browse)
            {
                barButtonItemAdd.Enabled = true;
                barButtonItemEdit.Enabled = true;
                barButtonItemDelete.Enabled = true;
                barButtonItemSave.Enabled = false;
                barButtonItemCancle.Enabled = false;
                textEditCode.Properties.ReadOnly = true;
                textEditName.Properties.ReadOnly = true;
                imageComboBoxEditManageType.Properties.ReadOnly = true;
                imageComboBoxEditState.Properties.ReadOnly = true;
                textEditInputCode1.Properties.ReadOnly = true;
                textEditInputCode2.Properties.ReadOnly = true;
                textEditRemark.Properties.ReadOnly = true;
            }
            if (this._uIState_FMaterialClass == UIState.Add)
            {
                barButtonItemAdd.Enabled = false;
                barButtonItemEdit.Enabled = false;
                barButtonItemDelete.Enabled = false;
                barButtonItemSave.Enabled = true;
                barButtonItemCancle.Enabled = true;
                textEditCode.Properties.ReadOnly = false;
                textEditName.Properties.ReadOnly = false;
                imageComboBoxEditManageType.Properties.ReadOnly = false;
                imageComboBoxEditState.Properties.ReadOnly = false;
                textEditInputCode1.Properties.ReadOnly = false;
                textEditInputCode2.Properties.ReadOnly = false;
                textEditRemark.Properties.ReadOnly = false;
                textEditCode.Text = "";
                textEditName.Text = "";
                imageComboBoxEditManageType.SelectedIndex = -1;
                imageComboBoxEditState.SelectedIndex = -1;
                textEditInputCode1.Text = "";
                textEditInputCode2.Text = "";
                textEditRemark.Text = "";
            }
            if (this._uIState_FMaterialClass == UIState.Edit)
            {
                barButtonItemAdd.Enabled = false;
                barButtonItemEdit.Enabled = false;
                barButtonItemDelete.Enabled = false;
                barButtonItemSave.Enabled = true;
                barButtonItemCancle.Enabled = true;
                textEditCode.Properties.ReadOnly = true;
                textEditName.Properties.ReadOnly = false;
                imageComboBoxEditManageType.Properties.ReadOnly = false;
                imageComboBoxEditState.Properties.ReadOnly = false;
                textEditInputCode1.Properties.ReadOnly = false;
                textEditInputCode2.Properties.ReadOnly = false;
                textEditRemark.Properties.ReadOnly = false;
            }
        }
        /// <summary>
        /// 新增按钮的实现
        /// </summary>
        private void barButtonItemAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this._uIState_FMaterialClass = UIState.Add;
            ShowUIState();
        }
        /// <summary>
        /// 修改按钮的实现
        /// </summary>
        private void barButtonItemEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this._uIState_FMaterialClass = UIState.Edit;
            ShowUIState();
        }
        /// <summary>
        /// 删除按钮的实现
        /// </summary>
        private void barButtonItemDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string code = textEditCode.EditValue.ToString();
            if (code != "" || textEditCode.EditValue != null)
            {
                if (billMaterialClass.deleteMatClass(code))
                {
                    MessageBoxUtil.ShowInformation("删除成功！");
                    GetDate();
                }
                else
                {
                    MessageBoxUtil.ShowError("删除失败！");
                }
            }
        }
        /// <summary>
        /// 保存按钮的实现
        /// </summary>
        private void barButtonItemSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
            if (imageComboBoxEditState.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowWarning("状态不可为空");
                imageComboBoxEditState.Focus();
                return;
            }
            if (imageComboBoxEditManageType.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowWarning("管理属性不可为空");
                imageComboBoxEditManageType.Focus();
                return;
            }
            string code = textEditCode.EditValue.ToString();
            string name = textEditName.EditValue.ToString();
            string managetype = imageComboBoxEditManageType.EditValue.ToString();
            string state = imageComboBoxEditState.EditValue.ToString();
            string inputcode1 = textEditInputCode1.EditValue.ToString();
            string inputcode2 = textEditInputCode2.EditValue.ToString();
            string remark = textEditRemark.EditValue.ToString();
            TreeListNode _FocusedNode = treeListMatClass.FocusedNode;
            string PID;
            if (_FocusedNode["CODE"].ToString() == "" || _FocusedNode["CODE"] == null)
            {
                PID = "0";               
            }
            else
            {
                PID = _FocusedNode["CODE"].ToString();
            }
            DataRow dr = gridViewAccountType.GetFocusedDataRow();
            string acc_id = dr["CODE"].ToString();
            if (this._uIState_FMaterialClass == UIState.Add)
            {
                if (billMaterialClass.BillInsertMatClass(code, name, PID, inputcode1, inputcode2, state, remark, managetype,acc_id))
                {
                    MessageBoxUtil.ShowInformation("保存新物资成功");
                    this._uIState_FMaterialClass = UIState.browse;
                    ShowUIState();
                    GetDate();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("保存新物资失败，请重试");
                }
            }
            if (this._uIState_FMaterialClass == UIState.Edit)
            {
                if (billMaterialClass.updateMatClass(code, name, inputcode1, inputcode2, state, remark, managetype))
                {
                    MessageBoxUtil.ShowInformation("修改新物资成功");
                    this._uIState_FMaterialClass = UIState.browse;
                    ShowUIState();
                    GetDate();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("修改新物资失败，请重试");
                }
            }
        }
        /// <summary>
        /// 取消按钮的实现
        /// </summary>
        private void barButtonItemCancle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this._uIState_FMaterialClass = UIState.browse;
            ShowUIState();
        }
        /// <summary>
        /// 自动获取拼音码和五笔码
        /// </summary>
        string InputCode1;
        string InputCode2;
        private void textEditName_Leave_1(object sender, EventArgs e)
        {
            if (this._uIState_FMaterialClass == UIState.Add || this._uIState_FMaterialClass == UIState.Edit)
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
        /// <summary>
        /// 代码不能重复
        /// </summary>
        private void textEditCode_Leave(object sender, EventArgs e)
        {
            if (this._uIState_FMaterialClass == UIState.Add)
            {
                string code = textEditCode.EditValue.ToString();
                bool Bool = billMaterialClass.BillCheckCode(code);
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
