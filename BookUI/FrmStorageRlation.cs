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

namespace BookUI
{
    public partial class FrmStorageRlation : Form
    {
        BillImageComBox billImageComBox = new BillImageComBox();
        BillStorage billStorage = new BillStorage();
        string Grade = string.Empty;
        string Code = string.Empty;
        public FrmStorageRlation(string grade,string code)
        {
            InitializeComponent();
            Grade = grade;
            Code = code;
        }

        private void FrmStorageRlation_Load(object sender, EventArgs e)
        {
            if (Grade == "1")
            {
                imageComboBoxEdit2.Enabled = false;
            }
            if (Grade == "3")
            {
                imageComboBoxEdit3.Enabled = false;
            }
            this.FuzhuAccount(this.imageComboBoxEdit1);
            this.FuzhuType(this.imageComboBoxEdit2);
            this.FuzhuType(this.imageComboBoxEdit3);
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
        public void FuzhuType(ImageComboBoxEdit imageComboBoxEdit)
        {
            string stateIn = string.Empty;
            imageComboBoxEdit.Properties.Items.Clear();
            DataTable dt = billImageComBox.BillImageComboxStorage();
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            imageComboBoxEdit1.SelectedIndex = -1;
            imageComboBoxEdit2.SelectedIndex = -1;
            imageComboBoxEdit3.SelectedIndex = -1;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (imageComboBoxEdit1.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowError("账册不可以为空");
                return;
            }
            if (Grade == "1" && imageComboBoxEdit3.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowError("下级库不可以为空");
                return;
            }
            if (Grade == "3" && imageComboBoxEdit2.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowError("上级库不可以为空");
                return;
            }
            if (Grade == "2" && imageComboBoxEdit2.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowError("上级库不可以为空");
                return;
            }
            if (Grade == "2" && imageComboBoxEdit3.SelectedIndex == -1)
            {
                MessageBoxUtil.ShowError("下级库不可以为空");
                return;
            }
            if (imageComboBoxEdit2.SelectedIndex != -1)
            {
                string sto_id = imageComboBoxEdit2.EditValue.ToString();
                string to_id = Code;
                string acc_id = imageComboBoxEdit1.EditValue.ToString();
                if (billStorage.insertRelation(sto_id, to_id, acc_id))
                {
                    MessageBoxUtil.ShowWarning("上级库关系添加成功");
                }
                else
                {
                    MessageBoxUtil.ShowWarning("上级库关系添加失败");
                }
            }
            if (imageComboBoxEdit3.SelectedIndex != -1)
            {
                string sto_id = Code;
                string to_id = imageComboBoxEdit3.EditValue.ToString();
                string acc_id = imageComboBoxEdit1.EditValue.ToString();
                if (billStorage.insertRelation(sto_id, to_id, acc_id))
                {
                    MessageBoxUtil.ShowWarning("下级库关系添加成功");
                }
                else
                {
                    MessageBoxUtil.ShowWarning("下级库关系添加失败");
                }
            }
        }

        private void imageComboBoxEdit2_EditValueChanged(object sender, EventArgs e)
        {
            if (imageComboBoxEdit1.SelectedIndex != -1 && imageComboBoxEdit2.SelectedIndex != -1)
            {
                string sto_id = imageComboBoxEdit2.EditValue.ToString();
                string to_id = Code;
                string acc_id = imageComboBoxEdit1.EditValue.ToString();
                if (billStorage.BillRelation(sto_id, to_id, acc_id))
                {
                    MessageBoxUtil.ShowWarning("该仓库关系已存在，请重试");
                }
            }
        }

        private void imageComboBoxEdit3_EditValueChanged(object sender, EventArgs e)
        {
            if (imageComboBoxEdit1.SelectedIndex != -1 && imageComboBoxEdit3.SelectedIndex != -1)
            {
                string sto_id = Code;
                string to_id = imageComboBoxEdit3.EditValue.ToString();
                string acc_id = imageComboBoxEdit1.EditValue.ToString();
                if (billStorage.BillRelation(sto_id, to_id, acc_id))
                {
                    MessageBoxUtil.ShowWarning("该仓库关系已存在，请重试");
                }
            }
        }
    }
}
