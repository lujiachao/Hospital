using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Nodes;
using BookBLL;
using DevExpress.XtraEditors;
using BookUI.Util;

namespace BookUI
{
    public partial class FormMain : DevExpress.XtraEditors.XtraForm
    {
        decimal sto_ID; //登录仓库控制
        string NameCode;
        BillMain billMain = new BillMain();
        public FormMain(string Code)
        {
            InitializeComponent();
            NameCode = Code;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            GetSysData();
            bsiOper.Caption = GetName();
            this.bsiTime.Caption = DateTime.Now.ToString("  yy-MM-dd HH:mm:ss  ");
            this.tlMenu.Appearance.FocusedCell.BackColor = System.Drawing.Color.LightGreen;
            ChooseStorage(NameCode);

        }
        /// <summary>
        /// 登录仓库控制
        /// </summary>
        public void ChooseStorage(string Code)
        {
            ServiceReference1.Service1Client TClient = new BookUI.ServiceReference1.Service1Client();
            if (TClient.GetPower(Code) == "999")
            {
                barButtonItem8.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                barButtonItem9.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else if (TClient.GetPower(Code) == "1")
            {
                barButtonItem8.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                barButtonItem9.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else if (TClient.GetPower(Code) == "2")
            {
                barButtonItem8.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barButtonItem9.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                barButtonItem8.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barButtonItem9.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                MessageBoxUtil.ShowError("当前账户信息有误");
            }
        }
        /// <summary>
        /// 系统设置绑定tlMenu数据源
        /// </summary>
        private void GetSysData()
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("NAME", System.Type.GetType("System.String"));
            dt.Columns.Add(dc1);
            DataRow row = dt.NewRow();
            row["NAME"] = "系统设置";
            dt.Rows.Add(row);
            this.tlMenu.DataSource = dt;
        }
        /// <summary>
        /// 下级库绑定tlMenu数据源
        /// </summary>
        private void GetMissList2()
        {
            DataTable dt = new DataTable();
            dt = billMain.BillGetMissList2();
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

            tlMenu.KeyFieldName = "PARENTID";//主键名称
            tlMenu.ParentFieldName = "PARENT_CODE";//父级ID 
            this.tlMenu.DataSource = dt;
            tlMenu.ExpandAll();//全部展开 

        }
        /// <summary>
        /// 总务科绑定tlMenu数据源
        /// </summary>
        private void GetMissList()
        {
            DataTable dt = new DataTable();
            dt = billMain.BillGetMissList();
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

            tlMenu.KeyFieldName = "PARENTID";//主键名称
            tlMenu.ParentFieldName = "PARENT_CODE";//父级ID 
            this.tlMenu.DataSource = dt;
            tlMenu.ExpandAll();//全部展开 

        }
        /// <summary>
        /// 判断界面是否已经打开
        /// </summary>
        public Form HaveOpen(string frmName)
        {
            Form form = null;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == frmName)
                {
                    form = frm;
                    break;
                }
            }
            return form;
        }   
        /// <summary>
        /// 焦点改变事件
        /// </summary>
        /// <param name="node"></param>
        /// <param name="check"></param>
        private void tlMenu_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            TreeListNode node = e.Node;
            string code = e.Node.GetValue("NAME").ToString();
            if (code == "系统设置")
            {
                string frmName = "FrmSysSetup";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmSysSetup frm = new FrmSysSetup();
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
                    this.xtraTabbedMdiManager1.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPagesAndTabControlHeader;    //设置标签后面添加删除按钮 ,  多个标签只需要设置一次..
                }
            }
            if (code == "账册维护")
            {
                string frmName = "FrmAccount";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmAccount frm = new FrmAccount(NameCode);
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
                }
            }
            if (code == "仓库维护")
            {
                string frmName = "FrmStorage";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmStorage frm = new FrmStorage();
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
                }
            }
            if (code == "厂家维护")
            {
                string frmName = "FrmFactory";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmFactory frm = new FrmFactory();
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
                }
            }
            if (code == "物资明细维护")
            {
                string frmName = "FrmMaterialInfo";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmMaterialInfo frm = new FrmMaterialInfo();
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
                }
            }
            if (code == "物资分类维护")
            {
                string frmName = "FrmMaterialClass";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmMaterialClass frm = new FrmMaterialClass();
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
                }
            }
            if (code == "物资目录维护")
            {
                string frmName = "FrmCatalogInfo";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmCatalogInfo frm = new FrmCatalogInfo();
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
                }
            }
            if (code == "调价处理")
            {
                string frmName = "FrmBillPriceMain";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmBillPriceMain frm = new FrmBillPriceMain();
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
                }
            }
            if (code == "入库处理")
            {
                string frmName = "FrmBillInMain";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmBillInMain frm = new FrmBillInMain(sto_ID,NameCode);
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
                }
            }
            if (code == "付款处理")
            {
                string frmName = "FrmBillAccInPayMain";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmBillAccInPayMain frm = new FrmBillAccInPayMain(NameCode);
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
                }
            }
            if (code == "会计入库")
            {
                string frmName = "FrmBillAccInMain";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmBillAccInMain frm = new FrmBillAccInMain(NameCode);
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
                }
            }
            if (code == "入库分析")
            {
                string frmName = "FrmReportBillInStat";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmReportBillInStat frm = new FrmReportBillInStat(sto_ID);
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
                }
            }
            if (code == "出库分析")
            {
                string frmName = "FrmReportBillOutStat";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmReportBillOutStat frm = new FrmReportBillOutStat(sto_ID);
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
                }
            }
            if (code == "出库处理")
            {
                string frmName = "FrmBillOutMain";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmBillOutMain frm = new FrmBillOutMain(sto_ID, NameCode);
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
                }
            }
            if (code == "会计出库")
            {
                string frmName = "FrmBillAccOutMain";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmBillAccOutMain frm = new FrmBillAccOutMain(sto_ID, NameCode);
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
                }
            }
            if (code == "发起申领")
            {
                string frmName = "FrmBillAppMain";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmBillAppMain frm = new FrmBillAppMain(sto_ID, NameCode);
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];
                }
            }
            if (code == "申领处理")
            {
                string frmName = "FrmBillAppTreatMain";
                if (HaveOpen(frmName) == null)
                {
                    xtraTabbedMdiManager1.MdiParent = this;
                    FrmBillAppTreatMain frm = new FrmBillAppTreatMain(sto_ID, NameCode);
                    frm.MdiParent = this;
                    frm.Show();
                    xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];
                }
            }
        }
        /// <summary>
        /// 关闭页面按钮
        /// </summary>
        private void barLargeButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Environment.Exit(0); 
        }
        /// <summary>
        /// 系统设置按钮设置
        /// </summary>
        private void barLargeButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GetSysData();
        }
        /// <summary>
        /// 获取系统当前操作人员
        /// </summary>
        private string GetName()
        { 
            string name;
            name = billMain.GetName(NameCode);
            return name;
        }
        /// <summary>
        /// 输入法切换
        /// </summary>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barStaticItem6.Caption == "首拼")
            {
                barStaticItem6.Caption = "五笔";
            }
            else
            {
                barStaticItem6.Caption = "首拼";
            }
        }
        /// <summary>
        /// 总务科按钮
        /// </summary>
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sto_ID = 1;
            GetMissList();
        }
        /// <summary>
        /// tlMenu图标设置
        /// </summary>
        private void tlMenu_CustomDrawNodeImages(object sender, DevExpress.XtraTreeList.CustomDrawNodeImagesEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                if (e.Node.Expanded)
                {
                    e.SelectImageIndex = 10;
                    return;
                }
                else
                {
                    e.SelectImageIndex = 11;
                }
            }
            else
            {
                e.SelectImageIndex = 2;
            }
        }
        /// <summary>
        /// 重新登录按钮点击事件
        /// </summary>
        public void bbiReLogin_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.MdiChildren.GetLength(0) == 0)
            {
                this.Close();
                this.Dispose();
                FormLogin frm = new FormLogin();
                frm.Show();
            }
            else
            {
                MessageBoxUtil.ShowWarning("子页面必须全部关闭");
            }
        }
        /// <summary>
        /// 关闭全部子界面
        /// </summary>
        private void bbiCloseChild_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form[] frmList = this.MdiChildren;
            foreach (Form frm in frmList)
            {
                frm.Close();
            }
        }
        /// <summary>
        /// 打开计算器
        /// </summary>
        private void bbiCalculator_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sto_ID = 2;
            GetMissList2();
        }
    }
}
