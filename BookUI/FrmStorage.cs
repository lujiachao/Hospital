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

namespace BookUI
{
    public partial class FrmStorage : Form
    {
        /// <summary>
        /// 获取设置界面状态
        /// </summary>
        private UIState _uIState_FStorage { get; set; }
        BillStorage billStorage = new BillStorage();
        BillAccount billAccount = new BillAccount();
        BillImageComBox billImageComBox = new BillImageComBox();
        public FrmStorage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 获取初始化仓库表格数据
        /// </summary>
        public void GetStorage()
        {
            DataTable dt = billStorage.BillGetStorage();
            gridControlStorage.DataSource = dt;
        }
        /// <summary>
        /// load函数
        /// </summary>
        private void FrmStorage_Load(object sender, EventArgs e)
        {
            GetStorage();
            this.FuzhuType(this.imageComboBoxEdit3);
            this.FuzhuAccount(this.imageComboBoxEdit4);
            this._uIState_FStorage = UIState.browse;
            ShowUIState();
        }
        public void ShowUIState()
        {
            if (this._uIState_FStorage == UIState.browse)
            {
                textEditCode.Properties.ReadOnly = true;
                textEditName.Properties.ReadOnly = true;
                textEditInputCode1.Properties.ReadOnly = true;
                textEditInputCode2.Properties.ReadOnly = true;
                imageComboBoxEdit1.Properties.ReadOnly = true;
                buttonEdit1.Properties.ReadOnly = true;
                imageComboBoxEdit2.Properties.ReadOnly = true;
                imageComboBoxEdit5.Properties.ReadOnly = true;
                textEdit1.Properties.ReadOnly = true;
                barButtonItemAdd.Enabled = true;
                barButtonItemEdit.Enabled = true;
                barButtonItemDelete.Enabled = true;
                barButtonItemSave.Enabled = false;
                barButtonItemQuit.Enabled = false;
                gridControlStorage.Enabled = true;
                gridControlStorageAccount.Enabled = true;
                gridControlAccountStorageTop.Enabled = true;
                gridControlAccountStorageDown.Enabled = true;
                gridControl1.Enabled = true;
            }
            if (this._uIState_FStorage == UIState.Add)
            {
                textEditCode.Properties.ReadOnly = false;
                textEditName.Properties.ReadOnly = false;
                textEditInputCode1.Properties.ReadOnly = true;
                textEditInputCode2.Properties.ReadOnly = true;
                imageComboBoxEdit1.Properties.ReadOnly = false;
                buttonEdit1.Properties.ReadOnly = false;
                imageComboBoxEdit2.Properties.ReadOnly = false;
                imageComboBoxEdit5.Properties.ReadOnly = false;
                textEdit1.Properties.ReadOnly = false;
                barButtonItemAdd.Enabled = false;
                barButtonItemEdit.Enabled = false;
                barButtonItemDelete.Enabled = false;
                barButtonItemSave.Enabled = true;
                barButtonItemQuit.Enabled = true;
                gridControlStorage.Enabled = false;
                gridControlStorageAccount.Enabled = false;
                gridControlAccountStorageTop.Enabled = false;
                gridControlAccountStorageDown.Enabled = false;
                gridControl1.Enabled = false;
                imageComboBoxEdit5.EditValue = "";
                textEditCode.EditValue = "";
                textEditName.EditValue = "";
                textEditInputCode1.EditValue = "";
                textEditInputCode2.EditValue = "";
                imageComboBoxEdit1.EditValue = "";
                buttonEdit1.EditValue = "";
                imageComboBoxEdit2.EditValue = "";
                textEdit1.EditValue = "";
            }
            if (this._uIState_FStorage == UIState.Edit)
            {
                textEditCode.Properties.ReadOnly = true;
                textEditName.Properties.ReadOnly = false;
                textEditInputCode1.Properties.ReadOnly = true;
                textEditInputCode2.Properties.ReadOnly = true;
                imageComboBoxEdit1.Properties.ReadOnly = false;
                buttonEdit1.Properties.ReadOnly = false;
                imageComboBoxEdit2.Properties.ReadOnly = false;
                imageComboBoxEdit5.Properties.ReadOnly = false;
                textEdit1.Properties.ReadOnly = false;
                barButtonItemAdd.Enabled = false;
                barButtonItemEdit.Enabled = false;
                barButtonItemDelete.Enabled = false;
                barButtonItemSave.Enabled = true;
                barButtonItemQuit.Enabled = true;
                gridControlStorage.Enabled = false;
                gridControlStorageAccount.Enabled = false;
                gridControlAccountStorageTop.Enabled = false;
                gridControlAccountStorageDown.Enabled = false;
                gridControl1.Enabled = false;
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
                imageComboBoxEdit.SelectedIndex = -1;
            }
            else
            {
                MessageBoxUtil.ShowInformation("数据丢失");
            }
        }
        /// <summary>
        /// 仓库主单焦点行改变事件
        /// </summary>
        private void gridViewStorage_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            gridViewStorageAccount.FocusedRowHandle = -1;
            DataRow dr = gridViewStorage.GetFocusedDataRow();
            if (dr != null)
            {
                textEditCode.EditValue = dr["CODE"];
                textEditName.EditValue = dr["NAME"];
                imageComboBoxEdit1.EditValue = dr["TYPE"];
                textEditInputCode1.EditValue = dr["INPUTCODE1"];
                textEditInputCode2.EditValue = dr["INPUTCODE2"];
                buttonEdit1.EditValue = dr["TOLINK"];
                imageComboBoxEdit2.EditValue = dr["STATE"];
                textEdit1.EditValue = dr["REMARK"];
                imageComboBoxEdit5.EditValue = dr["GRADE"];
                DataTable dt = billStorage.BillAccount(dr["CODE"].ToString());
                gridControlStorageAccount.DataSource = dt;
                DataTable dt1 = billStorage.BillEmp(dr["CODE"].ToString());
                gridControl1.DataSource = dt1;
                gridViewStorageAccount.FocusedRowHandle = -1;
            }

        }
        /// <summary>
        /// 对应账册焦点行改变事件
        /// </summary>
        private void gridViewStorageAccount_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            string sto_id = textEditCode.EditValue.ToString();
            DataRow dr = gridViewStorageAccount.GetFocusedDataRow();
            if (dr != null)
            {
                string acc_id = dr["CODE"].ToString();
                DataTable dt = billStorage.BillTopStorage(sto_id, acc_id);
                gridControlAccountStorageTop.DataSource = dt;
                DataTable dt1 = billStorage.BillDownStorage(sto_id, acc_id);
                gridControlAccountStorageDown.DataSource = dt1;
            }
        }
        /// <summary>
        /// 新增按钮的实现
        /// </summary>
        private void barButtonItemAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this._uIState_FStorage = UIState.Add;
            ShowUIState();
        }
        /// <summary>
        /// 修改按钮的实现
        /// </summary>
        private void barButtonItemEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this._uIState_FStorage = UIState.Edit;
            ShowUIState();
        }
        /// <summary>
        /// 判断是否输入中文
        /// </summary>
        private void textEditName_EditValueChanged(object sender, EventArgs e)
        {
            if (this._uIState_FStorage == UIState.Add || this._uIState_FStorage == UIState.Edit)
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
        string InputCode1;
        string InputCode2;
        private void textEditName_Leave(object sender, EventArgs e)
        {
            if (this._uIState_FStorage == UIState.Add || this._uIState_FStorage == UIState.Edit)
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
        /// 取消按钮的实现
        /// </summary>
        private void barButtonItemQuit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow dr = gridViewStorage.GetFocusedDataRow();
            if (dr != null)
            {
                textEditCode.EditValue = dr["CODE"];
                textEditName.EditValue = dr["NAME"];
                imageComboBoxEdit1.EditValue = dr["TYPE"];
                textEditInputCode1.EditValue = dr["INPUTCODE1"];
                textEditInputCode2.EditValue = dr["INPUTCODE2"];
                buttonEdit1.EditValue = dr["TOLINK"];
                imageComboBoxEdit2.EditValue = dr["STATE"];
                textEdit1.EditValue = dr["REMARK"];
                imageComboBoxEdit5.EditValue = dr["GRADE"];
            }
            this._uIState_FStorage = UIState.browse;
            ShowUIState();
        }
        /// <summary>
        /// 保存按钮的实现
        /// </summary>
        private void barButtonItemSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string code = textEditCode.EditValue.ToString();
            string name = textEditName.EditValue.ToString();
            string grade = imageComboBoxEdit5.EditValue.ToString();
            string type = imageComboBoxEdit1.EditValue.ToString();
            string tolink = buttonEdit1.EditValue.ToString();
            string inputcode1 = textEditInputCode1.EditValue.ToString();
            string inputcode2 = textEditInputCode2.EditValue.ToString();
            string state = imageComboBoxEdit2.EditValue.ToString();
            string remark = textEdit1.EditValue.ToString();
            if (textEditCode.EditValue.ToString() == "" || textEditCode.EditValue == null)
            {
                MessageBoxUtil.ShowWarning("代码不可为空");
                textEditCode.Focus();
                return;
            }
            if (this._uIState_FStorage == UIState.Add)
            {
                if (billStorage.insertStorage(code, name, grade, type, tolink, inputcode1, inputcode2, state, remark))
                {
                    MessageBoxUtil.ShowInformation("保存新仓库成功");
                    this._uIState_FStorage = UIState.browse;
                    ShowUIState();
                    GetStorage();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("保存新仓库失败，请重试");
                }
            }
            if (this._uIState_FStorage == UIState.Edit)
            {
                if (billStorage.updateStorage(code, name, grade, type, tolink, inputcode1, inputcode2, state, remark))
                {
                    MessageBoxUtil.ShowInformation("修改仓库成功");
                    this._uIState_FStorage = UIState.browse;
                    ShowUIState();
                    GetStorage();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("修改仓库失败，请重试");
                }
            }
        }
        /// <summary>
        /// 控制代码不为空，不重复
        /// </summary>
        private void textEditCode_Leave(object sender, EventArgs e)
        {
            if (this._uIState_FStorage == UIState.Add)
            {
                if (textEditCode.EditValue.ToString() == "" || textEditCode.EditValue == null)
                {
                    MessageBoxUtil.ShowWarning("代码不可为空");
                    textEditCode.Focus();
                }
                string code = textEditCode.EditValue.ToString();
                bool Bool = billStorage.BillCheckCode(code);
                if (Bool == true)
                {
                    MessageBoxUtil.ShowWarning("该代码已存在，    请重新输入");
                    textEditCode.EditValue = null;
                    textEditCode.Focus();
                }
            }
        }
        /// <summary>
        /// 删除按钮的实现
        /// </summary>
        private void barButtonItemDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string code = textEditCode.EditValue.ToString();
            if (billStorage.deleteStorage(code))
            {
                MessageBoxUtil.ShowQuestion("删除成功！");
                GetStorage();
            }
            else
            {
                MessageBoxUtil.ShowError("删除失败！");
            }
        }
        /// <summary>
        /// 绑定数据改变事件
        /// </summary>
        private void gridControlStorageAccount_DataSourceChanged(object sender, EventArgs e)
        {
            string sto_id = textEditCode.EditValue.ToString();
            DataRow dr = gridViewStorageAccount.GetFocusedDataRow();
            if (dr != null)
            {
                string acc_id = dr["CODE"].ToString();
                DataTable dt = billStorage.BillTopStorage(sto_id, acc_id);
                gridControlAccountStorageTop.DataSource = dt;
                DataTable dt1 = billStorage.BillDownStorage(sto_id, acc_id);
                gridControlAccountStorageDown.DataSource = dt1;
            }
            else
            {
                gridControlAccountStorageTop.DataSource = null;
                gridControlAccountStorageDown.DataSource = null;
            }
        }
        /// <summary>
        /// 仓库人员删除
        /// </summary>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (this._uIState_FStorage == UIState.browse)
            {
                DataRow dr = gridView1.GetFocusedDataRow();
                string emp_id = dr["CODE"].ToString();
                string sto_id = textEditCode.EditValue.ToString();
                if (billStorage.deleteStorageEmp(sto_id, emp_id))
                {
                    MessageBoxUtil.ShowWarning("仓库人员删除成功！");
                    DataTable dt1 = billStorage.BillEmp(sto_id);
                    gridControl1.DataSource = dt1;
                }
                else
                {
                    MessageBoxUtil.ShowWarning("仓库人员删除失败！");
                }
            }
        }
        /// <summary>
        /// 判断该仓库是否已存在某个会计人员
        /// </summary>
        private void imageComboBoxEdit3_EditValueChanged(object sender, EventArgs e)
        {
            if (this._uIState_FStorage == UIState.browse)
            {
                string emp_id = imageComboBoxEdit3.EditValue.ToString();
                DataTable dt = gridControl1.DataSource as DataTable;
                foreach (DataRow dr in dt.Rows)
                {
                    if (emp_id == dr["CODE"].ToString())
                    {
                        MessageBoxUtil.ShowWarning("该仓库下已有该员工 ， 请重新选择");
                        imageComboBoxEdit3.EditValue = "";
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 仓库人员新增
        /// </summary>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this._uIState_FStorage == UIState.browse)
            {
                string sto_id = textEditCode.EditValue.ToString();
                string emp_id = imageComboBoxEdit3.EditValue.ToString();
                if (billStorage.insertStorageEmp(sto_id, emp_id))
                {
                    MessageBoxUtil.ShowWarning("仓库人员新增成功！");
                    DataTable dt1 = billStorage.BillEmp(sto_id);
                    gridControl1.DataSource = dt1;
                }
                else
                {
                    MessageBoxUtil.ShowWarning("仓库人员新增失败！");
                }
            }
        }
        /// <summary>
        /// 打开关系设置页面
        /// </summary
        private void simpleButton3_Click(object sender, EventArgs e)
        {

            string grade = imageComboBoxEdit5.EditValue.ToString();
            string code = textEditCode.EditValue.ToString();
            FrmStorageRlation frm = new FrmStorageRlation(grade,code);
            frm.Show();
        }
        //删除仓库关系
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            DataRow dr1 = gridViewStorage.GetFocusedDataRow();
            DataRow dr2 = gridViewStorageAccount.GetFocusedDataRow();
            DataRow dr3 = gridViewAccountStorageTop.GetFocusedDataRow();
            DataRow dr4 = gridViewAccountStorageDown.GetFocusedDataRow();
            if (dr1 != null && dr2 != null)
            {
                string code = dr1["CODE"].ToString();
                string acc_id = dr2["CODE"].ToString();
                if(dr3 != null)
                {
                    string sto_id = dr3["CODE"].ToString();
                    if (billStorage.deleteRelation(sto_id, code, acc_id))
                    {
                        MessageBoxUtil.ShowWarning("删除仓库上级库成功！");
                    }
                    else
                    {
                        MessageBoxUtil.ShowWarning("删除仓库上级库失败！");
                    }
                }
                if (dr4 != null)
                {
                    string to_id = dr4["CODE"].ToString();
                    if (billStorage.deleteRelation(code, to_id, acc_id))
                    {
                        MessageBoxUtil.ShowWarning("删除仓库下级库成功！");
                    }
                    else
                    {
                        MessageBoxUtil.ShowWarning("删除仓库下级库失败！");
                    }
                }
            }            
         }
    }
}
