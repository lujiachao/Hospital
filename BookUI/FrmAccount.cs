using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BookBLL;
using BookUI.Util;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Tool;
using BookModel;
using System.Reflection;


namespace BookUI
{
    public partial class FrmAccount : Form
    {
        /// <summary>
        /// 获取设置界面状态
        /// </summary>
        private UIState _uIState_FAccount { get; set; }
        int sMode ;//控制账册人员添加
        public string InCode;//获取登录人员代码 
        BillAccount billAccount = new BillAccount();
        BillImageComBox billImageComBox = new BillImageComBox();
        public FrmAccount(string NameCode)
        {
            InitializeComponent();
            InCode = NameCode;
        }
        /// <summary>
        /// Load
        /// </summary>
        private void FrmAccount_Load(object sender, EventArgs e)
        {
            string State = radioGroup1.EditValue.ToString();
            GetAccount(State);
            this._uIState_FAccount = UIState.browse;
            sMode = 0;
            ShowUIState();
        }
        /// <summary>
        /// 界面状态设置
        /// </summary>
        public void ShowUIState()
        {
            if (this._uIState_FAccount == UIState.browse)
            {
                radioGroup1.Enabled = true;
                btSelect.Enabled = true;
                gridControlAccount.Enabled = true;
                textEditCode.Properties.ReadOnly = true;
                textEditName.Properties.ReadOnly = true;
                imageComboBoxEditPriceRule.Properties.ReadOnly = true;
                imageComboBoxEditManageType.Properties.ReadOnly = true;
                textEditInputCode1.Properties.ReadOnly = true;
                textEditInputCode2.Properties.ReadOnly = true;
                textEditBillPrefix.Properties.ReadOnly = true;
                imageComboBoxEditState.Properties.ReadOnly = true;
                imageComboBoxEditAccEmpId.Properties.ReadOnly = true;
                textEditRemark.Properties.ReadOnly = true;
                checkEditUseStock.Properties.ReadOnly = true;
                barButtonItem2.Enabled = true;
                barButtonItem3.Enabled = true;
                barButtonItem1.Enabled = true;
                barButtonItem4.Enabled = false;
                barButtonItem5.Enabled = false;
            }
            if (sMode == 0)
            {
                textEdit1.Properties.ReadOnly = true;
                textEdit2.Properties.ReadOnly = true;
                textEdit3.Properties.ReadOnly = true;
                textEdit4.Properties.ReadOnly = true;
                simpleButton1.Enabled = true;
                simpleButton3.Enabled = true;
                simpleButton2.Enabled = true;
                simpleButton4.Enabled = false;
                simpleButton5.Enabled = false;
                gridControlEmp.Enabled = true;
            }
            if (sMode == 1)
            {
                textEdit1.Properties.ReadOnly = false;
                textEdit2.Properties.ReadOnly = false;
                textEdit3.Properties.ReadOnly = false;
                textEdit4.Properties.ReadOnly = false;
                simpleButton1.Enabled = false;
                simpleButton3.Enabled = false;
                simpleButton2.Enabled = false;
                simpleButton4.Enabled = true;
                simpleButton5.Enabled = true;
                gridControlEmp.Enabled = true;
            }
            if (sMode == 2 )
            {
                textEdit1.Properties.ReadOnly = true;
                textEdit2.Properties.ReadOnly = false;
                textEdit3.Properties.ReadOnly = false;
                textEdit4.Properties.ReadOnly = false;
                simpleButton1.Enabled = false;
                simpleButton3.Enabled = false;
                simpleButton2.Enabled = false;
                simpleButton4.Enabled = true;
                simpleButton5.Enabled = true;
                gridControlEmp.Enabled = true;
            }
            if (this._uIState_FAccount == UIState.Add)
            {
                radioGroup1.Enabled = false;
                btSelect.Enabled = false;
                gridControlAccount.Enabled = false;
                textEdit1.Enabled = false;
                textEdit2.Enabled = false;
                textEdit3.Enabled = false;
                textEdit4.Enabled = false;
                simpleButton1.Enabled = false;
                simpleButton3.Enabled = false;
                simpleButton2.Enabled = false;
                simpleButton4.Enabled = false;
                gridControlEmp.Enabled = false;
                textEditCode.Properties.ReadOnly = false;
                textEditName.Properties.ReadOnly = false;
                imageComboBoxEditPriceRule.Properties.ReadOnly = false;
                imageComboBoxEditManageType.Properties.ReadOnly = false;
                textEditInputCode1.Properties.ReadOnly = true;
                textEditInputCode2.Properties.ReadOnly = true;
                textEditBillPrefix.Properties.ReadOnly = false;
                imageComboBoxEditState.Properties.ReadOnly = false;
                imageComboBoxEditAccEmpId.Properties.ReadOnly = false;
                textEditRemark.Properties.ReadOnly = false;
                checkEditUseStock.Properties.ReadOnly = false;
                barButtonItem2.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem1.Enabled = false;
                barButtonItem4.Enabled = true;
                barButtonItem5.Enabled = true;
            }
            if (this._uIState_FAccount == UIState.Edit)
            {
                radioGroup1.Enabled = false;
                btSelect.Enabled = false;
                gridControlAccount.Enabled = false;
                textEdit1.Enabled = false;
                textEdit2.Enabled = false;
                textEdit3.Enabled = false;
                textEdit4.Enabled = false;
                simpleButton1.Enabled = false;
                simpleButton3.Enabled = false;
                simpleButton2.Enabled = false;
                simpleButton4.Enabled = false;
                gridControlEmp.Enabled = false;
                textEditCode.Properties.ReadOnly = true;
                textEditName.Properties.ReadOnly = false;
                imageComboBoxEditPriceRule.Properties.ReadOnly = false;
                imageComboBoxEditManageType.Properties.ReadOnly = false;
                textEditInputCode1.Properties.ReadOnly = true;
                textEditInputCode2.Properties.ReadOnly = true;
                textEditBillPrefix.Properties.ReadOnly = false;
                imageComboBoxEditState.Properties.ReadOnly = false;
                imageComboBoxEditAccEmpId.Properties.ReadOnly = false;
                textEditRemark.Properties.ReadOnly = false;
                checkEditUseStock.Properties.ReadOnly = false;
                barButtonItem2.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem1.Enabled = false;
                barButtonItem4.Enabled = true;
                barButtonItem5.Enabled = true;
            }
            if (this._uIState_FAccount == UIState.ReadOnly)
            {
                barButtonItem1.Enabled = false;
                barButtonItem2.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem4.Enabled = false;
                barButtonItem5.Enabled = false;
                barButtonItem6.Enabled = false;
                radioGroup1.Enabled = false;
                btSelect.Enabled = false;
                gridControlAccount.Enabled = false;
                textEditCode.Properties.ReadOnly = true;
                textEditName.Properties.ReadOnly = true;
                imageComboBoxEditPriceRule.Properties.ReadOnly = true;
                imageComboBoxEditManageType.Properties.ReadOnly = true;
                textEditInputCode1.Properties.ReadOnly = true;
                textEditInputCode2.Properties.ReadOnly = true;
                textEditBillPrefix.Properties.ReadOnly = true;
                imageComboBoxEditState.Properties.ReadOnly = true;
                imageComboBoxEditAccEmpId.Properties.ReadOnly = true;
                textEditRemark.Properties.ReadOnly = true;
                checkEditUseStock.Properties.ReadOnly = true;
                textEdit1.Enabled = true;
                textEdit2.Enabled = true;
                textEdit3.Enabled = true;
                textEdit4.Enabled = true;
            }
        }
        /// <summary>
        /// 获取账册信息
        /// </summary>
        public void GetAccount(string state)
        {
            DataTable dt = billAccount.BillGetAccount(state);
            gridControlAccount.DataSource = dt;
            this.FuzhuType(this.imageComboBoxEditAccEmpId);
        }
        /// <summary>
        /// imagecombox数据绑定
        /// </summary>
        public void FuzhuType(ImageComboBoxEdit imageComboBoxEdit)
        {
            string stateIn = string.Empty;
            imageComboBoxEdit.Properties.Items.Clear();
            DataTable dt = billImageComBox.BillImageCombox();
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
        /// 账册表焦点行改变事件
        /// </summary>
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {         
            DataRow dr = gridView1.GetFocusedDataRow();
            if(dr != null )
            {
                string code = dr["CODE"].ToString();
                textEditCode.EditValue = code;
                textEditName.EditValue = dr["NAME"];
                imageComboBoxEditPriceRule.EditValue = dr["PRICE_RULE"].ToString();
                imageComboBoxEditManageType.EditValue = dr["MANAGE_TYPE"].ToString();
                textEditInputCode1.EditValue = dr["INPUTCODE1"];
                textEditInputCode2.EditValue = dr["INPUTCODE2"];
                textEditBillPrefix.EditValue = dr["DANJU"];
                imageComboBoxEditState.EditValue = dr["STATE"];
                textEditRemark.EditValue = dr["REMARK"];
                if (dr["USE_STOCK"].ToString() == "1")
                {
                    checkEditUseStock.Checked = true;
                }
                else
                {
                    checkEditUseStock.Checked = false;
                }
                imageComboBoxEditAccEmpId.EditValue = dr["ACC_EMP_ID"];
                DataTable dt = billAccount.BillGetEmpID(code);
                gridView2.FocusedRowHandle = 0;
                gridControlEmp.DataSource = dt;
                DataRow drGridControlEmp = gridView2.GetFocusedDataRow();
                if (drGridControlEmp != null)
                {
                    textEdit1.EditValue = drGridControlEmp["EMP_ID"];
                    textEdit2.EditValue = drGridControlEmp["NAME"];
                    textEdit3.EditValue = drGridControlEmp["ROLE"];
                    textEdit4.EditValue = drGridControlEmp["DEPARTMENT"];
                }  
            }
        }
        /// <summary>
        /// 人员表焦点行改变事件
        /// </summary>
        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow drGridControlEmp = gridView2.GetFocusedDataRow();
            if(drGridControlEmp != null)
            {
                textEdit1.EditValue = drGridControlEmp["EMP_ID"];
                textEdit2.EditValue = drGridControlEmp["NAME"];
                textEdit3.EditValue = drGridControlEmp["ROLE"];
                textEdit4.EditValue = drGridControlEmp["DEPARTMENT"];
            }
        }
        /// <summary>
        /// 检索按钮的实现
        /// </summary>
        private void btSelect_Click(object sender, EventArgs e)
        {
            gridView1.FocusedRowHandle = 2;
            string State = radioGroup1.EditValue.ToString();
            GetAccount(State);
        }
        /// <summary>
        /// 新增按钮的实现
        /// </summary>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this._uIState_FAccount = UIState.Add;
            ShowUIState();
            textEditCode.EditValue = "";
            textEditName.EditValue = "";
            imageComboBoxEditPriceRule.EditValue = "";
            imageComboBoxEditManageType.EditValue = "";
            textEditInputCode1.EditValue = "";
            textEditInputCode2.EditValue = "";
            textEditBillPrefix.EditValue = "";
            imageComboBoxEditState.EditValue = "";
            imageComboBoxEditAccEmpId.EditValue = "";
            textEditRemark.EditValue = "";
            checkEditUseStock.Checked = false;
        }
        /// <summary>
        /// 修改按钮的实现
        /// </summary>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this._uIState_FAccount = UIState.Edit;
            ShowUIState();
        }
        /// <summary>
        /// 删除按钮的实现
        /// </summary>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string code = textEditCode.EditValue.ToString();
            if (billAccount.deleteAccount(code))
            {
                MessageBoxUtil.ShowQuestion("删除成功！");
                gridView1.FocusedRowHandle = 1;
                string state = radioGroup1.EditValue.ToString();
                GetAccount(state);
            }
            else
            {
                MessageBoxUtil.ShowError("删除失败！");
            }
        }
        /// <summary>
        /// 保存按钮的实现
        /// </summary>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string code = textEditCode.EditValue.ToString();
            string name = textEditName.EditValue.ToString();
            string Price_rule = imageComboBoxEditPriceRule.EditValue.ToString();
            string Manage_type = imageComboBoxEditManageType.EditValue.ToString();
            string Use_stock;
            if (checkEditUseStock.Checked == true)
            {
                Use_stock = "1";
            }
            else
            {
                Use_stock = "0";
            }
            string inputcode1 = textEditInputCode1.EditValue.ToString();
            string inputcode2 = textEditInputCode2.EditValue.ToString();
            string State = imageComboBoxEditState.EditValue.ToString();
            string remark = textEditRemark.EditValue.ToString();
            string acc_emp_id = imageComboBoxEditAccEmpId.EditValue.ToString();
            string danju = textEditBillPrefix.EditValue.ToString();
            if (this._uIState_FAccount == UIState.Add)
            {
                if (billAccount.insertAccount(code, name, Price_rule, Manage_type, Use_stock, inputcode1, inputcode2, State, remark, acc_emp_id, danju))
                {
                    MessageBoxUtil.ShowInformation("保存新账册成功");
                    gridView1.FocusedRowHandle = 1;
                    string state = radioGroup1.EditValue.ToString();
                    GetAccount(state);
                    this._uIState_FAccount = UIState.browse;
                    sMode = 0;
                    ShowUIState();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("保存新账册失败，请重试");
                }
            }
            if (this._uIState_FAccount == UIState.Edit)
            {
                if (billAccount.updateAccount(code, name, Price_rule, Manage_type, Use_stock, inputcode1, inputcode2, State, remark, acc_emp_id, danju))
                {
                    MessageBoxUtil.ShowInformation("更新" + name + "账册成功");
                    gridView1.FocusedRowHandle = 1;
                    string state = radioGroup1.EditValue.ToString();
                    GetAccount(state);
                    this._uIState_FAccount = UIState.browse;
                    sMode = 0;
                    ShowUIState();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("更新" + name + "账册失败");
                }
            }
        }
        /// <summary>
        /// 取消按钮的实现
        /// </summary>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string State = radioGroup1.EditValue.ToString();
            GetAccount(State);
            this._uIState_FAccount = UIState.browse;
            sMode = 0;
            ShowUIState();
        }
        /// <summary>
        /// 打印按钮的实现
        /// </summary>
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
        /// <summary>
        /// 关闭按钮的实现
        /// </summary>
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 人员新增的实现
        /// </summary>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this._uIState_FAccount = UIState.ReadOnly;
            sMode = 1;
            ShowUIState();
            textEdit1.EditValue = "";
            textEdit2.EditValue = "";
            textEdit3.EditValue = "";
            textEdit4.EditValue = "";
        }
        /// <summary>
        /// 人员修改的实现
        /// </summary>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this._uIState_FAccount = UIState.ReadOnly;
            sMode = 2;
            ShowUIState();
        }
        /// <summary>
        /// 人员删除的实现
        /// </summary>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string Acc_ID = textEditCode.EditValue.ToString();
            string Emp_ID = textEdit1.EditValue.ToString();
            if (billAccount.deletePubEmp(Emp_ID) && billAccount.deleteAccEmp(Acc_ID, Emp_ID))
            {
                MessageBoxUtil.ShowInformation("删除人员信息成功");
                gridView1.FocusedRowHandle = 1;
                string State = radioGroup1.EditValue.ToString();
                GetAccount(State);
                this._uIState_FAccount = UIState.browse;
                sMode = 0;
                ShowUIState();
            }
            else
            {
                MessageBoxUtil.ShowInformation("删除人员信息失败");
            }
        }
        /// <summary>
        /// 人员数据保存的实现
        /// </summary>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string code = textEditCode.EditValue.ToString();
            string number = textEdit1.EditValue.ToString();
            string name = textEdit2.EditValue.ToString();
            string role = textEdit3.EditValue.ToString();
            string price = textEdit4.EditValue.ToString();
            string str_TestAtrrubute = insert();
            if (sMode == 1)
            {
                if (billAccount.insertPubEmp(number, name) && billAccount.insertAccEmp(code,number,role,price))
                {
                    MessageBoxUtil.ShowWarning("人员新增成功");
                    gridView1.FocusedRowHandle = 1;
                    string State = radioGroup1.EditValue.ToString();
                    GetAccount(State);
                    this._uIState_FAccount = UIState.browse;
                    sMode = 0;
                    ShowUIState();
                }
                else
                {
                    MessageBoxUtil.ShowWarning("人员新增失败");
                }
            }
            if (sMode == 2)
            {
                if (billAccount.updatePubEmp(number, name) && billAccount.updateAccEmp(code, number, role, price))
                {
                    MessageBoxUtil.ShowWarning("人员信息修改成功");
                    gridView1.FocusedRowHandle = 1;
                    string State = radioGroup1.EditValue.ToString();
                    GetAccount(State);
                    this._uIState_FAccount = UIState.browse;
                    sMode = 0;
                    ShowUIState();
                }
                else
                {
                    MessageBoxUtil.ShowWarning("人员信息修改失败");
                }
            }
        }
        /// <summary>
        /// 取消按钮的实现
        /// </summary>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            gridView1.FocusedRowHandle = 1;
            string State = radioGroup1.EditValue.ToString();
            GetAccount(State);
            this._uIState_FAccount = UIState.browse;
            sMode = 0;
            ShowUIState();
        }
        /// <summary>
        /// 判断代码是否重复
        /// </summary>
        private void textEditCode_Leave(object sender, EventArgs e)
        {
            if (this._uIState_FAccount == UIState.Add)
            {
                if (textEditCode.EditValue.ToString() == "" || textEditCode.EditValue == null)
                {
                    MessageBoxUtil.ShowWarning("代码不可为空");
                    textEditCode.Focus();
                }
                string code = textEditCode.EditValue.ToString();
                bool Bool = billAccount.BillCheckCode(code);
                if (Bool == true)
                {
                    MessageBoxUtil.ShowWarning("该代码已存在，    请重新输入");
                    textEditCode.EditValue = null;
                    textEditCode.Focus();
                }
            }
        }
        /// <summary>
        /// 判断是否输入中文
        /// </summary>
        private void textEditName_EditValueChanged(object sender, EventArgs e)
        {
            if (this._uIState_FAccount == UIState.Add || this._uIState_FAccount == UIState.Edit)
            {
                string name = textEditName.EditValue.ToString();
                for (int i = 0; i < name.Length; i++)
                {
                    if ((int)name[i] <= 127)
                    {
                        MessageBoxUtil.ShowWarning("不是汉字,请重新输入");
                        textEditName.Focus();
                        return;
                    }                   
                }
            }
        }
        /// <summary>
        /// 自动获取拼音码和五笔码
        /// </summary>
        string InputCode1 ;
        string InputCode2;
        private void textEditName_Leave(object sender, EventArgs e)
        {
            if (this._uIState_FAccount == UIState.Add || this._uIState_FAccount == UIState.Edit)
            {
                string name = textEditName.EditValue.ToString();
                string InputCode1 = "";
                string InputCode2 = "";
                for (int j = 0; j < name.Length; j++)
                {
                    string mark = name.Substring(j,1);
                    string py = "PY";
                    string wb = "WB";
                    string PY = billAccount.BillPY(py, mark);
                    string WB = billAccount.BillPY(wb, mark);  
                    string WB1 = WB.Substring(0,1);
                    InputCode1 = InputCode1 + PY;
                    InputCode2 += WB1;
                }
                textEditInputCode1.EditValue = InputCode1;
                textEditInputCode2.EditValue = InputCode2;
            }
        }
        E_PUBEMP mp = new E_PUBEMP();
        public string insert()
        { 
            mp.E_ID = Convert.ToInt32(textEditCode.EditValue);
            mp.E_CODE = textEditCode.EditValue.ToString();
            mp.E_NAME = textEdit2.EditValue.ToString();
            mp.E_PASSWORD = "1";
            mp.E_PAOWER = "1";
            PropertyInfo[] infos = mp.GetType().GetProperties();
            string Str_TestAtrrubute = "";

            object[] objDataFieldAttribute = null;
            foreach (PropertyInfo info in infos)
            {
                objDataFieldAttribute = info.GetCustomAttributes(typeof(DataFieldAttribute), false);
                if (objDataFieldAttribute != null)
                {
                    Str_TestAtrrubute = Str_TestAtrrubute + (info.Name + "->数据库字段：" + ((DataFieldAttribute)objDataFieldAttribute[0]).FieldName) + " --------";
                }
            }
            return Str_TestAtrrubute;
        }
    }
}
