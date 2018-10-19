using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BookBLL;
using DevExpress.XtraTreeList.Nodes;
using Tool;
using System.IO;
using BookUI.Util;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace BookUI
{
    public partial class FrmSysSetup : DevExpress.XtraEditors.XtraForm
    {
        BillSysSetup billSysSetup = new BillSysSetup();
        /// <summary>
        /// 获取设置界面状态
        /// </summary>
        private UIState _uIState_FSysSetup{ get; set; }
        public int sMode = 0; //全局变量，判断执行操作.普通状态 0，添加新功能 1，添加新模块 2，导入模块 3,修改 4
        public FrmSysSetup()
        {
            InitializeComponent();
        }

        private void FrmSysSetup_Load(object sender, EventArgs e)
        {
            this._uIState_FSysSetup = UIState.browse;
            GetSysList();
            ShowUIState();
            this.FuzhuType(this.imageComboBoxEdit1);
        }
        /// <summary>
        /// imagecombox数据绑定
        /// </summary>
        public void FuzhuType(ImageComboBoxEdit imageComboBoxEdit)
        {
            string stateIn = string.Empty;
            imageComboBoxEdit.Properties.Items.Clear();
            DataTable dt = billSysSetup.BillImageCombox();
            foreach (DataRow dr in dt.Rows)
            {
                imageComboBoxEdit.Properties.Items.Add(new ImageComboBoxItem(dr["NAME"].ToString(),
                                                                                        dr["TYPE"].ToString(),
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
        /// tlSysMenu数据绑定
        /// </summary>
        private void GetSysList()
        {
            DataTable dt = new DataTable();
            dt = billSysSetup.BillGetSysList();
            dt.Columns.Add("PARENT_CODE");
            string locationConfig = "2,4,6,8";
            //根据输入码的库位获取该库位的父库位，截取Code的父库位查找是否存在该父库位
            string[] configs = locationConfig.Split(',');
            int parentNum = -1; //父库位编码位数
            foreach (DataRow dr in dt.Rows)
            {
                //设置父节点
                for (int i = 0; i < configs.Length; i++)
                {
                    if (configs[i] == dr["PARENTID"].ToString().Length.ToString())
                    {
                        parentNum = i - 1;
                        break;
                    }
                }
                //如果是顶节点
                if (parentNum != -1)
                {
                    string parentCode = dr["PARENTID"].ToString().Substring(0, int.Parse(configs[parentNum]));
                    dr["PARENT_CODE"] = parentCode;
                }
                else
                {
                    dr["PARENT_CODE"] = "-1";
                }
            }

            tlSysMenu.KeyFieldName = "PARENTID";//主键名称
            tlSysMenu.ParentFieldName = "PARENT_CODE";//父级ID 
            this.tlSysMenu.DataSource = dt;
            tlSysMenu.ExpandAll();//全部展开 
        }
        /// <summary>
        /// tlSysMenu数据绑定
        /// </summary>
        private void tlSysMenu_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            TreeListNode node = e.Node;
            string code = node.GetValue("CODE").ToString();
            if (code != null && code != "")   //代码
            {
                textEditCode.EditValue = code;
            }
            string isUser = node.GetValue("IS_FUNCTION_POINT").ToString();
            if (isUser == "1")    //是否在用
            {
                checkEdit1.Checked = true;
            }
            else
            {
                checkEdit1.Checked = false;
            }
            string isINFORMATION = node.GetValue("IS_INFORMATION").ToString();
            if (isINFORMATION == "1")   //1.紧凑版 2.强制版 3.完全版
            {
                checkEdit2.Checked = true;
                checkEdit3.Checked = false;
            }
            else if (isINFORMATION == "2")
            {
                checkEdit2.Checked = false;
                checkEdit3.Checked = true;
            }
            else
            {
                checkEdit2.Checked = false;
                checkEdit3.Checked = false;
            }
            string name = node.GetValue("NAME").ToString();
            if (name != null && name != "")
            {
                textEditName.EditValue = name;
            }
            string Object = node.GetValue("OBJECT").ToString();
            if(Object != null && Object != "")
            {
                textEdit1.EditValue = Object;
            }
            string parentid = node.GetValue("PARENTID1").ToString();
            if (parentid != null && parentid != "")
            {
                textEdit2.EditValue = parentid;
            }
            string inputTime = node.GetValue("INPUT_TIME").ToString();
            if (inputTime != null && inputTime != "")
            {
                textEdit3.EditValue = inputTime;
            }
            string seq = node.GetValue("SEQ").ToString();
            if (inputTime != null && inputTime != "")
            {
                textEdit4.EditValue = seq;
            }
            string remake = node.GetValue("REMAKE").ToString();
            if (remake != null && remake != "")
            {
                memoEdit1.EditValue = remake;
            }
        }
         /// <summary>
        /// 界面状态调整
        /// </summary>
        public void ShowUIState()
        {
            if (this._uIState_FSysSetup == UIState.browse)
            {
                labelControl9.Visible = false;
                imageComboBoxEdit1.Visible = false;
                tlSysMenu.Enabled = true;
                barButtonItem2.Enabled = true;
                barButtonItem1.Enabled = true;
                barButtonItem3.Enabled = true;
                barButtonItem4.Enabled = true;
                barButtonItem5.Enabled = true;
                barButtonItem8.Enabled = true;
                barLargeButtonItem1.Enabled = true;
                barButtonItemSave.Enabled = false;
                barButtonItemQute.Enabled = false;
                textEditCode.Properties.ReadOnly = true;
                checkEdit1.Properties.ReadOnly = true;
                checkEdit2.Properties.ReadOnly = true;
                checkEdit3.Properties.ReadOnly = true;
                textEditName.Properties.ReadOnly = true;
                textEdit1.Properties.ReadOnly = true;
                textEdit2.Properties.ReadOnly = true;
                textEdit3.Properties.ReadOnly = true;
                textEdit4.Properties.ReadOnly = true;
                memoEdit1.Properties.ReadOnly = true;
            }
            if (this._uIState_FSysSetup == UIState.Add)
            {
                labelControl9.Visible = true;
                imageComboBoxEdit1.Visible = true;
                tlSysMenu.Enabled = false;
                barButtonItem2.Enabled = false;
                barButtonItem1.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem4.Enabled = false;
                barButtonItem5.Enabled = false;
                barButtonItem8.Enabled = false;
                barLargeButtonItem1.Enabled = false;
                barButtonItemSave.Enabled = true;
                barButtonItemQute.Enabled = true;
                textEditCode.Properties.ReadOnly = false;
                checkEdit1.Properties.ReadOnly = false;
                checkEdit2.Properties.ReadOnly = false;
                checkEdit3.Properties.ReadOnly = false;
                textEditName.Properties.ReadOnly = false;
                textEdit1.Properties.ReadOnly = false;
                textEdit2.Properties.ReadOnly = false;
                textEdit3.Properties.ReadOnly = false;
                textEdit4.Properties.ReadOnly = false;
                memoEdit1.Properties.ReadOnly = false;
            }
            if (this._uIState_FSysSetup == UIState.Edit)
            {
                labelControl9.Visible = false;
                imageComboBoxEdit1.Visible = false;
                tlSysMenu.Enabled = false;
                barButtonItem2.Enabled = false;
                barButtonItem1.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem4.Enabled = false;
                barButtonItem5.Enabled = false;
                barButtonItem8.Enabled = false;
                barLargeButtonItem1.Enabled = false;
                barButtonItemSave.Enabled = true;
                barButtonItemQute.Enabled = true;
                textEditCode.Properties.ReadOnly = true;
                checkEdit1.Properties.ReadOnly = false;
                checkEdit2.Properties.ReadOnly = false;
                checkEdit3.Properties.ReadOnly = false;
                textEditName.Properties.ReadOnly = false;
                textEdit1.Properties.ReadOnly = false;
                textEdit2.Properties.ReadOnly = false;
                textEdit3.Properties.ReadOnly = false;
                textEdit4.Properties.ReadOnly = false;
                memoEdit1.Properties.ReadOnly = false;
            }
        }
        /// <summary>
        /// 导出数据
        /// </summary>
        private void barLargeButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string code = textEditCode.EditValue.ToString();
            string fileName = "//"+code+".txt";
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "选择所有文件存放目录";
            if (folder.ShowDialog() == DialogResult.OK)
            {
                string sPath = folder.SelectedPath;
                if (!File.Exists("" + sPath + "" + fileName + ""))
                {
                    FileStream fs1 = new FileStream("" + sPath + "" + fileName + "", FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(this.textEditCode.EditValue.ToString().Trim() + "|" + 
                                 this.textEditName.EditValue.ToString().Trim() + "|" + 
                                 textEdit1.EditValue.ToString().Trim() + "|" +
                                 textEdit2.EditValue.ToString().Trim());//开始写入值
                    sw.Close();
                    sw.Dispose();
                    fs1.Close();
                    fs1.Dispose();
                }
                else
                {
                    FileStream fs = new FileStream("" + sPath + "" + fileName + "", FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(this.textEditCode.EditValue.ToString().Trim() + "|" + 
                                 this.textEditName.EditValue.ToString().Trim() + "|" +
                                 textEdit1.EditValue.ToString().Trim() + "|" +
                                 textEdit2.EditValue.ToString().Trim());//开始写入值
                    sw.Close();
                    sw.Dispose();
                    fs.Close();
                    fs.Dispose();
                }

            }
             
        }
        /// <summary>
        /// 导入数据
        /// </summary>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sMode = 3;
            this._uIState_FSysSetup = UIState.Add;
            ShowUIState();
            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = "Excel文件(*.xls;*.xlsx)|*.xls;*.xlsx|所有文件|*.*";
            ofd.Filter = "txt文件(*.txt)|*.txt|所有文件|*.*";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string strFileName = ofd.FileName;
                FileStream fs = new FileStream("" + strFileName + "", FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader sR = new StreamReader(fs, Encoding.UTF8);
                string nextLine = sR.ReadToEnd();
                string[] sArray = nextLine.Split('|');
                textEditCode.EditValue = sArray[0];
                textEditName.EditValue = sArray[1];
                textEdit1.EditValue = sArray[2];
                checkEdit1.Checked = false;
                checkEdit2.Checked = false;
                checkEdit3.Checked = false;
                textEdit2.EditValue = sArray[3];
                textEdit3.EditValue = DateTime.Now.ToString("  yy-MM-dd HH:mm:ss  ");
                textEdit4.EditValue = null;
                memoEdit1.EditValue = null;
            }
        }
        /// <summary>
        /// 添加新模块
        /// </summary>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sMode = 1;
            this._uIState_FSysSetup = UIState.Add;
            ShowUIState();
            labelControl1.Text = "请输入您要添加的信息";
            textEditCode.EditValue = "";
            checkEdit1.Checked = false;
            checkEdit2.Checked = false;
            checkEdit3.Checked = false;
            textEditName.EditValue = "";
            textEdit1.EditValue = "";
            textEdit2.EditValue = "";
            textEdit3.EditValue = "";
            textEdit4.EditValue = "";
            memoEdit1.EditValue = "";
        }
        /// <summary>
        /// 添加新功能
        /// </summary>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sMode = 2;
            this._uIState_FSysSetup = UIState.Add;
            ShowUIState();
            labelControl1.Text = "请输入您要添加的信息";
            textEditCode.EditValue = "";
            checkEdit1.Checked = false;
            checkEdit2.Checked = false;
            checkEdit3.Checked = false;
            textEditName.EditValue = "";
            textEdit1.EditValue = "";
            textEdit2.EditValue = "";
            textEdit3.EditValue = "";
            textEdit4.EditValue = "";
            memoEdit1.EditValue = "";
        }
        /// <summary>
        /// 取消按钮
        /// </summary>
        private void barButtonItemQute_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sMode = 0;
            this._uIState_FSysSetup = UIState.browse;
            ShowUIState();
            GetSysList();
        }
        /// <summary>
        /// 保存按钮的实现
        /// </summary>
        private void barButtonItemSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (sMode == 1) //新模块
            {
                labelControl9.Visible = false;
                imageComboBoxEdit1.Visible = false;
                string code = textEditCode.EditValue.ToString();
                string isUser = string.Empty;
                string edition = string.Empty;
                string name = textEditName.EditValue.ToString();
                string Object = textEdit1.EditValue.ToString();
                string version = textEdit2.EditValue.ToString();
                string seq = textEdit4.EditValue.ToString();
                string remake = string.Empty;
                string type = imageComboBoxEdit1.EditValue.ToString();
                string parentID = (Convert.ToInt32(billSysSetup.BillMaxType())+11).ToString();
                if (memoEdit1.EditValue == null)
                {
                    remake = "";
                }
                else
                {
                    remake = memoEdit1.EditValue.ToString();
                }
                if (checkEdit1.Checked == true)
                {
                    isUser = "1";
                }
                else
                {
                    isUser = "0";
                }
                if (checkEdit2.Checked == true)
                {
                    version = "1";
                }
                if (checkEdit3.Checked == true)
                {
                    version = "2";
                }
                if (checkEdit2.Checked == false && checkEdit3.Checked == false)
                {
                    version = "3";
                }
                if (billSysSetup.InsertSysObject(code, isUser, edition, name, Object, version, seq, remake, parentID))
                {
                    MessageBoxUtil.ShowInformation("保存新模块成功");
                    this._uIState_FSysSetup = UIState.browse;
                    ShowUIState();
                    sMode = 0;
                    GetSysList();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("保存新模块失败，请重试");
                }
            }
            if (sMode == 2) //新功能
            {
                string code = textEditCode.EditValue.ToString();
                string isUser = string.Empty;
                string edition = string.Empty;
                string name = textEditName.EditValue.ToString();
                string Object = textEdit1.EditValue.ToString();
                string version = textEdit2.EditValue.ToString();
                string seq = textEdit4.EditValue.ToString();
                string remake = string.Empty;
                string type = imageComboBoxEdit1.EditValue.ToString();
                string MaxParentID = (Convert.ToInt32(billSysSetup.BillMaxParentID(type)) + 1).ToString();
                if (memoEdit1.EditValue == null)
                {
                    remake = "";
                }
                else
                {
                    remake = memoEdit1.EditValue.ToString();
                }
                if (checkEdit1.Checked == true)
                {
                    isUser = "1";
                }
                else
                {
                    isUser = "0";
                }
                if (checkEdit2.Checked == true)
                {
                    version = "1";
                }
                if (checkEdit3.Checked == true)
                {
                    version = "2";
                }
                if (checkEdit2.Checked == false && checkEdit3.Checked == false)
                {
                    version = "3";
                }
                if (billSysSetup.InsertSysObject(code, isUser, edition, name, Object, version, seq, remake, MaxParentID))
                {
                    MessageBoxUtil.ShowInformation("保存新功能成功");
                    this._uIState_FSysSetup = UIState.browse;
                    ShowUIState();
                    sMode = 0;
                    GetSysList();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("保存新功能失败，请重试");
                }
            }
            if (sMode == 3) //导入模板
            {
                string code = textEditCode.EditValue.ToString();
                string isUser = string.Empty;
                string edition = string.Empty;
                string name = textEditName.EditValue.ToString();
                string Object = textEdit1.EditValue.ToString();
                string version = textEdit2.EditValue.ToString();
                string seq = textEdit4.EditValue.ToString();
                string remake = string.Empty;
                string type = imageComboBoxEdit1.EditValue.ToString();
                string MaxParentID = (Convert.ToInt32(billSysSetup.BillMaxParentID(type))+1).ToString();
                if (memoEdit1.EditValue == null)
                {
                    remake = "";
                }
                else
                {
                    remake = memoEdit1.EditValue.ToString();
                }
                if (checkEdit1.Checked == true)
                {
                    isUser = "1";
                }
                else
                {
                    isUser = "0";
                }
                if (checkEdit2.Checked == true)
                {
                    version = "1";
                }
                if (checkEdit3.Checked == true)
                {
                    version = "2";
                }
                if (checkEdit2.Checked == false && checkEdit3.Checked == false)
                {
                    version = "3";
                }
                if (billSysSetup.InsertSysObject(code, isUser, edition, name, Object, version, seq, remake, MaxParentID))
                {
                    MessageBoxUtil.ShowInformation("保存模板成功");
                    this._uIState_FSysSetup = UIState.browse;
                    ShowUIState();
                    sMode = 0;
                    GetSysList();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("保存模板失败，请重试");
                }
            }
            if (sMode == 4) //修改
            {
                string code = textEditCode.EditValue.ToString();
                string isUser = string.Empty;
                string edition = string.Empty;
                string name = textEditName.EditValue.ToString();
                string Object = textEdit1.EditValue.ToString();
                string version = textEdit2.EditValue.ToString();
                string date = DateTime.Now.ToString();
                string seq = textEdit4.EditValue.ToString();
                //int? avaCurNum = dr["AVA_CUR_NUM"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(dr["AVA_CUR_NUM"]);
                string remake = string.Empty ; 
                if( memoEdit1.EditValue == null)
                {
                    remake = "";
                }
                else
                {
                   remake = memoEdit1.EditValue.ToString();
                }
                if (checkEdit1.Checked == true)
                {
                    isUser = "1";
                }
                else
                {
                    isUser = "0";
                }
                if (checkEdit2.Checked == true)
                {
                    version = "1";
                }
                if (checkEdit3.Checked == true)
                {
                    version = "2";
                }
                if (checkEdit2.Checked == false && checkEdit3.Checked == false)
                {
                    version = "3";
                }
                if (billSysSetup.UpdateSysObject(code, isUser, edition, name, Object, version, date, seq, remake))
                {
                    MessageBoxUtil.ShowInformation("修改成功");
                    this._uIState_FSysSetup = UIState.browse;
                    ShowUIState();
                    sMode = 0;
                    GetSysList();
                }
                else
                {
                    MessageBoxUtil.ShowInformation("修改失败，请重试");
                }
            }
        }
        /// <summary>
        /// 删除按钮的实现
        /// </summary>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string code = textEditCode.EditValue.ToString();
            if (billSysSetup.DeleteSysObject(code))
            {
                MessageBoxUtil.ShowInformation("删除"+code+"成功");
                this._uIState_FSysSetup = UIState.browse;
                ShowUIState();
                sMode = 0;
                GetSysList();
            }
            else
            {
                MessageBoxUtil.ShowInformation("删除" + code + "失败");
            }
        }   
        /// <summary>
        /// 修改按钮的实现
        /// </summary>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sMode = 4;
            this._uIState_FSysSetup = UIState.Edit;
            ShowUIState();
        }
        /// <summary>
        /// 退出按钮的实现
        /// </summary>
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
            this.Dispose();
        }
        /// <summary>
        /// 判断代码是否可用,leave事件
        /// </summary>
        private void textEditCode_Leave(object sender, EventArgs e)
        {
            if (textEditCode.EditValue.ToString() == "" || textEditCode.EditValue == null)
            {
                MessageBoxUtil.ShowWarning("代码不可为空");
            }
            if (sMode == 1 && textEditCode.EditValue.ToString() != "" && textEditCode.EditValue != null)
            {
                string Code = textEditCode.EditValue.ToString();
                bool Bool = billSysSetup.BillCheckCode(Code);
                if (Bool == true)
                {
                    MessageBoxUtil.ShowWarning("该代码已存在，    请重新输入");
                    textEditCode.EditValue = null;
                    textEditCode.Focus();
                }
            }
            if (sMode == 2 && textEditCode.EditValue.ToString() != "" && textEditCode.EditValue != null)
            {
                string Code = textEditCode.EditValue.ToString();
                bool Bool = billSysSetup.BillCheckCode(Code);
                if (Bool == true)
                {
                    MessageBoxUtil.ShowWarning("该代码已存在，    请重新输入");
                    textEditCode.EditValue = null;
                    textEditCode.Focus();
                }
            }
            if (sMode == 3 && textEditCode.EditValue.ToString() != "" && textEditCode.EditValue != null)
            {
                string Code = textEditCode.EditValue.ToString();
                bool Bool = billSysSetup.BillCheckCode(Code);
                if (Bool == true)
                {
                    MessageBoxUtil.ShowWarning("该代码已存在，    请重新输入");
                    textEditCode.EditValue = null;
                    textEditCode.Focus();
                }
            }
        }
        /// <summary>
        /// 判断名称是否可用,leave事件
        /// </summary
        private void textEditName_Leave(object sender, EventArgs e)
        {
            if (sMode == 1 || sMode ==2)
            {
                if (textEditName.EditValue.ToString() == "" || textEditName.EditValue == null)
                {
                    MessageBoxUtil.ShowWarning("名称不可为空");
                }
            }
        }
        /// <summary>
        /// 判断对象名称是否可用,leave事件
        /// </summary
        private void textEdit1_Leave(object sender, EventArgs e)
        {
            if (sMode == 1 || sMode == 2)
            {
                if (textEdit1.EditValue.ToString() == "" || textEdit1.EditValue == null)
                {
                    MessageBoxUtil.ShowWarning("对象名称不可为空");
                }
            }
        }
        /// <summary>
        /// 版本控件控制
        /// </summary
        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit2.Checked == true)
            {
                checkEdit3.Checked = false;
            }
        }
        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit3.Checked == true)
            {
                checkEdit2.Checked = false;
            }
        }
    
    }
}
