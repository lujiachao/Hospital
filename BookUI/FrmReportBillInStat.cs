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
using System.Drawing.Printing;

namespace BookUI
{
    public partial class FrmReportBillInStat : Form
    {
        BillReportIn billReportIn = new BillReportIn();
        BillImageComBox billImageComBox = new BillImageComBox();
        decimal _sto_ID;//获取登录仓库
        #region 打印公共元素
        DataTable dt1;
        //以下用户可自定义
        //当前要打印文本的字体及字号
        private static Font TableFont = new Font("Verdana", 10, FontStyle.Regular);
        //表头字体
        private Font HeadFont = new Font("Verdana", 20, FontStyle.Bold);
        private Font HeadFont1 = new Font("Verdana", 12, FontStyle.Bold);
        //表头文字
        private string HeadText = string.Empty;
        //表头高度
        private int HeadHeight = 40;
        //表的基本单位
        private int[] XUnit;
        private int YUnit = TableFont.Height * 2;
        //以下为模块内部使用
        private PrintDocument DataTablePrinter;
        private DataRow DataGridRow;
        private DataTable DataTablePrint;
        //当前要所要打印的记录行数,由计算得到
        private int PageRecordNumber;
        //正要打印的页号
        private int PrintingPageNumber = 1;
        //已经打印完的记录数
        private int PrintRecordComplete;
        private int PLeft;
        private int PTop;
        private int PRight;
        private int PBottom;
        private int PWidth;
        private int PHeigh;
        //当前画笔颜色
        private SolidBrush DrawBrush = new SolidBrush(Color.Black);
        private SolidBrush DrawBlue = new SolidBrush(Color.Blue);
        //每页打印的记录条数
        private int PrintRecordNumber;
        //第一页打印的记录条数
        private int FirstPrintRecordNumber;
        //总共应该打印的页数
        private int TotalPage;
        //与列名无关的统计数据行的类目数（如，总计，小计......）
        public int TotalNum = 0;
        #endregion
        public FrmReportBillInStat(decimal Sto_id)
        {
            InitializeComponent();
            _sto_ID = Sto_id;
            this.printDocument1.OriginAtMargins = true;//启用页边距
            this.pageSetupDialog1.EnableMetric = true; //以毫米为单位
        }
        //Load函数
        private void FrmReportBillInStat_Load(object sender, EventArgs e)
        {
            this.FuzhuAccount(this.imageComboBoxEditAccType);
            this.LoadInvoiceState(this.imageComboBoxEditInvoiceState, true);
            this.FuzhuCommpany(this.imageComboBoxEdit1);
            dateEditTimeStart.EditValue = DateTime.Now;
            dateEditTimeEnd.EditValue = DateTime.Now;
        }
        /// <summary>
        /// 往来单位数据绑定
        /// </summary>
        public void FuzhuCommpany(ImageComboBoxEdit imageComboBoxEdit)
        {
            string stateIn = string.Empty;
            imageComboBoxEdit.Properties.Items.Clear();
            DataTable dt = billImageComBox.BillImageComboxCompany1();
            dt.Rows.Add(0, "清空");
            foreach (DataRow dr in dt.Rows)
            {
                imageComboBoxEdit.Properties.Items.Add(new ImageComboBoxItem(dr["NAME"].ToString(),
                                                                                        dr["ID"].ToString(),
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
        /// 加载发票状态(包含全部项)
        /// </summary>
        /// <param name="imageComboBoxEditInvoiceState">发票状态下拉框控件</param>
        /// <param name="isAll">是否包含全部选项</param>
        protected void LoadInvoiceState(ImageComboBoxEdit imageComboBoxEditInvoiceState, bool isAll)
        {
            if (isAll)
            {
                imageComboBoxEditInvoiceState.Properties.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem("全部", 4, -1));
            }

            imageComboBoxEditInvoiceState.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("票到货到", 1, -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("货到票未到", 2, -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("票到货未到", 3, -1)});
            if (imageComboBoxEditInvoiceState.Properties.Items.Count > 0)
            {
                imageComboBoxEditInvoiceState.SelectedIndex = 0;
            }
            if (imageComboBoxEditInvoiceState.Name == "imageComboBoxEditInvoiceState")
            {
                imageComboBoxEditInvoiceState.SelectedIndex = -1;
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
                imageComboBoxEdit.SelectedIndex = -1;
            }
            else
            {
                MessageBoxUtil.ShowInformation("数据丢失");
            }
        }
        //账册值改变事件
        private void imageComboBoxEditAccType_EditValueChanged(object sender, EventArgs e)
        {
            decimal acc_ID = decimal.Parse(imageComboBoxEditAccType.EditValue.ToString());
            this.FuzhuBinType(this.imageComboBoxEditInType, acc_ID);
        }
        /// <summary>
        /// 入库分类绑定
        /// </summary>
        public void FuzhuBinType(ImageComboBoxEdit imageComboBoxEdit, decimal acc_ID)
        {
            string stateIn = string.Empty;
            imageComboBoxEdit.Properties.Items.Clear();
            DataTable dt = billImageComBox.DalImageComboxBinType(acc_ID);
            foreach (DataRow dr in dt.Rows)
            {
                imageComboBoxEdit.Properties.Items.Add(new ImageComboBoxItem(dr["NAME"].ToString(),
                                                                                        dr["ID"].ToString(),
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

        private void imageComboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (imageComboBoxEdit1.Text == "清空")
            {
                imageComboBoxEdit1.SelectedIndex = -1;
            }
        }
        //检索按钮功能实现
        private void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            string searchState = imageComboBoxEditDateType.EditValue.ToString();
            DateTime? dateStart = Convert.ToDateTime(this.dateEditTimeStart.EditValue);
            DateTime? dateEnd = Convert.ToDateTime(this.dateEditTimeEnd.EditValue);
            if (Convert.ToDecimal(imageComboBoxEditAccType.EditValue) == 0)
            {
                MessageBoxUtil.ShowWarning("账册不可为空");
                imageComboBoxEditAccType.Focus();
                return;
            }
            decimal acc_id = Convert.ToDecimal(imageComboBoxEditAccType.EditValue);
            decimal? binType = Convert.ToDecimal(imageComboBoxEditInType.EditValue);
            string invoiceState = Convert.ToString(imageComboBoxEditInvoiceState.EditValue);
            decimal? com_ID = Convert.ToDecimal(imageComboBoxEdit1.EditValue);
            labelControlDateTimeShow.Text = "年月范围：" + dateEditTimeStart.Text + "到" + dateEditTimeEnd.Text;
            DataTable dt = billReportIn.GetBillInStatList(_sto_ID,acc_id,binType,invoiceState,dateStart,dateEnd,com_ID,searchState);
            gridControl1.DataSource = dt;
            dt1 = dt;
        }
        //打印功能的实现
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string Title = labelControlTitle.Text;
                string Title1 = labelControlDateTimeShow.Text;
                string Title2 = labelControlInTypeShow.Text;
                string Title3 = labelControlCreateEmp.Text;
                string Title4 = labelControlCreateDate.Text;
                CreatePrintDocument(dt1, Title, Title1, Title2, Title3, Title4).Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("打印错误，请检查打印设置！");

            }
        }
        //打印预览功能的实现
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string Title = labelControlTitle.Text;
                string Title1 = labelControlDateTimeShow.Text;
                string Title2 = labelControlInTypeShow.Text;
                string Title3 = labelControlCreateEmp.Text;
                string Title4 = labelControlCreateDate.Text;
                PrintPreviewDialog PrintPriview = new PrintPreviewDialog();
                PrintPriview.Document = CreatePrintDocument(dt1, Title, Title1, Title2, Title3, Title4);
                PrintPriview.WindowState = FormWindowState.Maximized;
                PrintPriview.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("打印错误，请检查打印设置！");

            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            int tableWith = 0;
            string ColumnText;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            //打印表格线格式
            Pen Pen = new Pen(Brushes.Black, 1);

            #region 设置列宽

            foreach (DataRow dr in DataTablePrint.Rows)
            {
                for (int i = 0; i < DataTablePrint.Columns.Count; i++)
                {
                    int colwidth = Convert.ToInt32(e.Graphics.MeasureString(dr[i].ToString().Trim(), TableFont).Width);
                    if (colwidth > XUnit[i])
                    {
                        XUnit[i] = colwidth;
                    }
                }
            }

            if (PrintingPageNumber == 1)
            {
                for (int Cols = 0; Cols <= DataTablePrint.Columns.Count - 1; Cols++)
                {
                    ColumnText = DataTablePrint.Columns[Cols].ColumnName.ToString().Trim();
                    int colwidth = Convert.ToInt32(e.Graphics.MeasureString(ColumnText, TableFont).Width) + 20;//列宽
                    if (colwidth > XUnit[Cols])
                    {
                        XUnit[Cols] = colwidth;
                    }
                }
            }
            for (int i = 0; i < XUnit.Length; i++)
            {
                tableWith += XUnit[i];
            }
            #endregion

            PLeft = (e.PageBounds.Width - tableWith) / 2;
            int x = PLeft;
            int y = PTop;
            int stringY = PTop + (YUnit - TableFont.Height) / 2;
            int rowOfTop = PTop;

            //第一页
            if (PrintingPageNumber == 1)
            {
                //打印表头
                e.Graphics.DrawString(HeadText, HeadFont, DrawBrush, new Point(e.PageBounds.Width / 2, PTop), sf);
                e.Graphics.DrawString(labelControlDateTimeShow.Text, HeadFont1, DrawBrush, 250, 150);
                e.Graphics.DrawString(labelControlInTypeShow.Text, HeadFont1, DrawBrush, 850, 150);
                //e.Graphics.DrawString(labelControlCreateEmp.Text, HeadFont1, DrawBrush, 650, 150);
                //e.Graphics.DrawString(labelControlCreateDate.Text, HeadFont1, DrawBrush, 850, 150);



                //设置为第一页时行数
                PageRecordNumber = FirstPrintRecordNumber;
                rowOfTop = y = PTop + HeadFont.Height + 10;
                stringY = PTop + HeadFont.Height + 10 + (YUnit - TableFont.Height) / 2;
            }
            else
            {
                //计算,余下的记录条数是否还可以在一页打印,不满一页时为假
                if (DataTablePrint.Rows.Count - PrintRecordComplete >= PrintRecordNumber)
                {
                    PageRecordNumber = PrintRecordNumber;
                }
                else
                {
                    PageRecordNumber = DataTablePrint.Rows.Count - PrintRecordComplete;
                }
            }

            #region 列名
            if (PrintingPageNumber == 1 || PageRecordNumber > TotalNum)//最后一页只打印统计行时不打印列名
            {
                //得到datatable的所有列名
                for (int Cols = 0; Cols <= DataTablePrint.Columns.Count - 1; Cols++)
                {
                    ColumnText = DataTablePrint.Columns[Cols].ColumnName.ToString().Trim();
                    string ColumnName = string.Empty;
                    if(ColumnText == "CLASS")
                    {
                        ColumnName = "入库编号";
                    }
                    else if(ColumnText =="TYPE_NAME")
                    {
                        ColumnName = "入库分类";
                    }
                    else if (ColumnText == "COM_ID")
                    {
                        ColumnName = "单位编号";
                    }
                    else if (ColumnText == "COM_NAME")
                    {
                        ColumnName = "入库单位";
                    }
                    else if (ColumnText == "BUYING_PRICE_TOTAL")
                    {
                        ColumnName = "进货总额";
                    }
                    else if (ColumnText == "RETAIL_PRICE_TOTAL")
                    {
                        ColumnName = "零售总额";
                    }
                    else if (ColumnText == "BUYING_PRICE_SUB")
                    {
                        ColumnName = "进销差额";
                    }
                    else 
                    {
                        ColumnName = "扣率";
                    }
                    int colwidth = Convert.ToInt32(e.Graphics.MeasureString(ColumnText, TableFont).Width);
                    e.Graphics.DrawString(ColumnName, TableFont, DrawBlue, x, stringY + 30);//列名
                    x += XUnit[Cols];
                }
            }
            #endregion



            e.Graphics.DrawLine(Pen, PLeft, rowOfTop+30, x, rowOfTop+30);
            stringY += YUnit;
            y += YUnit;
            e.Graphics.DrawLine(Pen, PLeft, y+30, x, y+30);

            //当前页面已经打印的记录行数
            int PrintingLine = 0;
            while (PrintingLine < PageRecordNumber)
            {
                x = PLeft;
                //确定要当前要打印的记录的行号
                DataGridRow = DataTablePrint.Rows[PrintRecordComplete];
                for (int Cols = 0; Cols <= DataTablePrint.Columns.Count - 1; Cols++)
                {
                    e.Graphics.DrawString(DataGridRow[Cols].ToString().Trim(), TableFont, DrawBrush, x, stringY+30);
                    x += XUnit[Cols];
                }
                stringY += YUnit;
                y += YUnit;
                e.Graphics.DrawLine(Pen, PLeft, y+30, x, y+30);

                PrintingLine += 1;
                PrintRecordComplete += 1;
                if (PrintRecordComplete >= DataTablePrint.Rows.Count)
                {
                    e.HasMorePages = false;
                    PrintRecordComplete = 0;
                }
            }

            e.Graphics.DrawLine(Pen, PLeft, rowOfTop+30, PLeft, y+30);
            x = PLeft;
            for (int Cols = 0; Cols < DataTablePrint.Columns.Count; Cols++)
            {
                x += XUnit[Cols];
                e.Graphics.DrawLine(Pen, x, rowOfTop+30, x, y+30);
            }



            PrintingPageNumber += 1;

            if (PrintingPageNumber > TotalPage)
            {
                e.HasMorePages = false;
                PrintingPageNumber = 1;
                PrintRecordComplete = 0;
            }
            else
            {
                e.HasMorePages = true;
            }
        }
        /// <summary>
        /// 创建打印文件
        /// </summary>
        private PrintDocument CreatePrintDocument(DataTable dt, string Title, string Title1, string Title2, string Title3, string Title4)
        {

            DataTablePrint = dt;
            HeadText = Title;
            DataTablePrinter = new PrintDocument();

            PageSetupDialog PageSetup = new PageSetupDialog();
            PageSetup.Document = DataTablePrinter;
            DataTablePrinter.DefaultPageSettings = PageSetup.PageSettings;
            DataTablePrinter.DefaultPageSettings.Landscape = true;//设置打印横向还是纵向
            //PLeft = 30; //DataTablePrinter.DefaultPageSettings.Margins.Left;
            PTop = DataTablePrinter.DefaultPageSettings.Margins.Top;
            //PRight = DataTablePrinter.DefaultPageSettings.Margins.Right;
            PBottom = DataTablePrinter.DefaultPageSettings.Margins.Bottom;
            PWidth = DataTablePrinter.DefaultPageSettings.Bounds.Width;
            PHeigh = DataTablePrinter.DefaultPageSettings.Bounds.Height;
            XUnit = new int[DataTablePrint.Columns.Count];
            PrintRecordNumber = Convert.ToInt32((PHeigh - PTop - PBottom - YUnit) / YUnit);
            FirstPrintRecordNumber = Convert.ToInt32(gridView1.RowCount);
            if (DataTablePrint.Rows.Count > PrintRecordNumber)
            {
                if ((DataTablePrint.Rows.Count - FirstPrintRecordNumber) % PrintRecordNumber == 0)
                {
                    TotalPage = (DataTablePrint.Rows.Count - FirstPrintRecordNumber) / PrintRecordNumber + 1;
                }
                else
                {
                    TotalPage = (DataTablePrint.Rows.Count - FirstPrintRecordNumber) / PrintRecordNumber + 2;
                }
            }
            else
            {
                TotalPage = 1;
            }

            DataTablePrinter.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            DataTablePrinter.DocumentName = HeadText;

            return DataTablePrinter;
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetFocusedDataRow();
            if (dr != null)
            {
                labelControlInTypeShow.Text = "入库方式: " + dr["TYPE_NAME"].ToString();
                //labelControlCreateEmp.Text = "制单人： " + dr[""].ToString();
                //labelControlCreateEmp.Text = "制单日期： " + dr[""].ToString();
            }
        }

        private void gridControl1_DataSourceChanged(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetFocusedDataRow();
            if (dr != null)
            {
                labelControlInTypeShow.Text = "入库方式: " + dr["TYPE_NAME"].ToString();
                //labelControlCreateEmp.Text = "制单人： " + dr[""].ToString();
                //labelControlCreateEmp.Text = "制单日期： " + dr[""].ToString();
            }
        }
        //public string getName(decimal ID)
        //{
             
        //}
    }
}