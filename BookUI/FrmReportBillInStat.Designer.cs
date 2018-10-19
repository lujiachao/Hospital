namespace BookUI
{
    partial class FrmReportBillInStat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReportBillInStat));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageCollectionTool = new DevExpress.Utils.ImageCollection(this.components);
            this.groupControlSearch = new DevExpress.XtraEditors.GroupControl();
            this.imageComboBoxEdit1 = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.imageComboBoxEditDateType = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.labelControlCompany = new DevExpress.XtraEditors.LabelControl();
            this.dateEditTimeEnd = new DevExpress.XtraEditors.DateEdit();
            this.dateEditTimeStart = new DevExpress.XtraEditors.DateEdit();
            this.imageComboBoxEditInvoiceState = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.imageComboBoxEditInType = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.labelControlInvoiceState = new DevExpress.XtraEditors.LabelControl();
            this.labelControlInType = new DevExpress.XtraEditors.LabelControl();
            this.simpleButtonSearch = new DevExpress.XtraEditors.SimpleButton();
            this.labelControlAccountType = new DevExpress.XtraEditors.LabelControl();
            this.imageComboBoxEditAccType = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.panelControlHeader = new DevExpress.XtraEditors.PanelControl();
            this.labelControlCreateDate = new DevExpress.XtraEditors.LabelControl();
            this.labelControlCreateEmp = new DevExpress.XtraEditors.LabelControl();
            this.labelControlInTypeShow = new DevExpress.XtraEditors.LabelControl();
            this.labelControlDateTimeShow = new DevExpress.XtraEditors.LabelControl();
            this.labelControlTitle = new DevExpress.XtraEditors.LabelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnInType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnCompany = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnBuyingPriceTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnRetailPriceTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDifference = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnRate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollectionTool)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlSearch)).BeginInit();
            this.groupControlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxEditDateType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditTimeEnd.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditTimeEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditTimeStart.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditTimeStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxEditInvoiceState.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxEditInType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxEditAccType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlHeader)).BeginInit();
            this.panelControlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Images = this.imageCollectionTool;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barButtonItem2});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 2;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "打印(&P)";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.ImageIndex = 14;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "打印预览(&V)";
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.ImageIndex = 15;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(920, 34);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 522);
            this.barDockControlBottom.Size = new System.Drawing.Size(920, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 34);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 488);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(920, 34);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 488);
            // 
            // imageCollectionTool
            // 
            this.imageCollectionTool.ImageSize = new System.Drawing.Size(24, 24);
            this.imageCollectionTool.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollectionTool.ImageStream")));
            this.imageCollectionTool.Images.SetKeyName(0, "Help.png");
            this.imageCollectionTool.Images.SetKeyName(1, "MenuBar_AboutInfo_Large.png");
            this.imageCollectionTool.Images.SetKeyName(2, "MenuBar_EditModel_Large.png");
            this.imageCollectionTool.Images.SetKeyName(3, "Modify.png");
            this.imageCollectionTool.Images.SetKeyName(4, "MsnPerson.png");
            this.imageCollectionTool.Images.SetKeyName(5, "MenuBar_Close_Large.png");
            this.imageCollectionTool.Images.SetKeyName(6, "Add.png");
            this.imageCollectionTool.Images.SetKeyName(7, "MenuBar_New_Large.png");
            this.imageCollectionTool.Images.SetKeyName(8, "MenuBar_Edit_Large.png");
            this.imageCollectionTool.Images.SetKeyName(9, "MenuBar_Refresh_Large.png");
            this.imageCollectionTool.Images.SetKeyName(10, "MenuBar_Save_Large.png");
            this.imageCollectionTool.Images.SetKeyName(11, "MenuBar_Exit_Large.png");
            this.imageCollectionTool.Images.SetKeyName(12, "Sub.png");
            this.imageCollectionTool.Images.SetKeyName(13, "Ok.png");
            this.imageCollectionTool.Images.SetKeyName(14, "MenuBar_Print_Large.png");
            this.imageCollectionTool.Images.SetKeyName(15, "PrintPreviewItemLarge.png");
            // 
            // groupControlSearch
            // 
            this.groupControlSearch.Controls.Add(this.imageComboBoxEdit1);
            this.groupControlSearch.Controls.Add(this.imageComboBoxEditDateType);
            this.groupControlSearch.Controls.Add(this.labelControlCompany);
            this.groupControlSearch.Controls.Add(this.dateEditTimeEnd);
            this.groupControlSearch.Controls.Add(this.dateEditTimeStart);
            this.groupControlSearch.Controls.Add(this.imageComboBoxEditInvoiceState);
            this.groupControlSearch.Controls.Add(this.imageComboBoxEditInType);
            this.groupControlSearch.Controls.Add(this.labelControlInvoiceState);
            this.groupControlSearch.Controls.Add(this.labelControlInType);
            this.groupControlSearch.Controls.Add(this.simpleButtonSearch);
            this.groupControlSearch.Controls.Add(this.labelControlAccountType);
            this.groupControlSearch.Controls.Add(this.imageComboBoxEditAccType);
            this.groupControlSearch.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControlSearch.Location = new System.Drawing.Point(0, 34);
            this.groupControlSearch.Name = "groupControlSearch";
            this.groupControlSearch.Size = new System.Drawing.Size(216, 488);
            this.groupControlSearch.TabIndex = 4;
            this.groupControlSearch.Text = "搜索条件";
            // 
            // imageComboBoxEdit1
            // 
            this.imageComboBoxEdit1.Location = new System.Drawing.Point(102, 202);
            this.imageComboBoxEdit1.MenuManager = this.barManager1;
            this.imageComboBoxEdit1.Name = "imageComboBoxEdit1";
            this.imageComboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.imageComboBoxEdit1.Size = new System.Drawing.Size(100, 21);
            this.imageComboBoxEdit1.TabIndex = 83;
            this.imageComboBoxEdit1.SelectedIndexChanged += new System.EventHandler(this.imageComboBoxEdit1_SelectedIndexChanged);
            // 
            // imageComboBoxEditDateType
            // 
            this.imageComboBoxEditDateType.EditValue = "Account";
            this.imageComboBoxEditDateType.EnterMoveNextControl = true;
            this.imageComboBoxEditDateType.Location = new System.Drawing.Point(22, 33);
            this.imageComboBoxEditDateType.Name = "imageComboBoxEditDateType";
            this.imageComboBoxEditDateType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.imageComboBoxEditDateType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("账务统计", "Account", -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("入账日期", "General", -1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("审核日期", "Checked", -1)});
            this.imageComboBoxEditDateType.Size = new System.Drawing.Size(74, 21);
            this.imageComboBoxEditDateType.TabIndex = 82;
            // 
            // labelControlCompany
            // 
            this.labelControlCompany.Location = new System.Drawing.Point(36, 202);
            this.labelControlCompany.Name = "labelControlCompany";
            this.labelControlCompany.Size = new System.Drawing.Size(60, 14);
            this.labelControlCompany.TabIndex = 11;
            this.labelControlCompany.Text = "供货单位：";
            // 
            // dateEditTimeEnd
            // 
            this.dateEditTimeEnd.EditValue = null;
            this.dateEditTimeEnd.Location = new System.Drawing.Point(102, 66);
            this.dateEditTimeEnd.Name = "dateEditTimeEnd";
            this.dateEditTimeEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditTimeEnd.Properties.DisplayFormat.FormatString = "yyyy-MM";
            this.dateEditTimeEnd.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEditTimeEnd.Properties.EditFormat.FormatString = "yyyy-MM";
            this.dateEditTimeEnd.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEditTimeEnd.Properties.Mask.EditMask = "y";
            this.dateEditTimeEnd.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditTimeEnd.Size = new System.Drawing.Size(100, 21);
            this.dateEditTimeEnd.TabIndex = 4;
            // 
            // dateEditTimeStart
            // 
            this.dateEditTimeStart.EditValue = null;
            this.dateEditTimeStart.Location = new System.Drawing.Point(102, 33);
            this.dateEditTimeStart.Name = "dateEditTimeStart";
            this.dateEditTimeStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditTimeStart.Properties.DisplayFormat.FormatString = "yyyy-MM";
            this.dateEditTimeStart.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEditTimeStart.Properties.EditFormat.FormatString = "yyyy-MM";
            this.dateEditTimeStart.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEditTimeStart.Properties.Mask.EditMask = "y";
            this.dateEditTimeStart.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditTimeStart.Size = new System.Drawing.Size(100, 21);
            this.dateEditTimeStart.TabIndex = 3;
            // 
            // imageComboBoxEditInvoiceState
            // 
            this.imageComboBoxEditInvoiceState.EnterMoveNextControl = true;
            this.imageComboBoxEditInvoiceState.Location = new System.Drawing.Point(102, 168);
            this.imageComboBoxEditInvoiceState.Name = "imageComboBoxEditInvoiceState";
            this.imageComboBoxEditInvoiceState.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.imageComboBoxEditInvoiceState.Size = new System.Drawing.Size(100, 21);
            this.imageComboBoxEditInvoiceState.TabIndex = 10;
            // 
            // imageComboBoxEditInType
            // 
            this.imageComboBoxEditInType.EnterMoveNextControl = true;
            this.imageComboBoxEditInType.Location = new System.Drawing.Point(102, 135);
            this.imageComboBoxEditInType.Name = "imageComboBoxEditInType";
            this.imageComboBoxEditInType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.imageComboBoxEditInType.Size = new System.Drawing.Size(100, 21);
            this.imageComboBoxEditInType.TabIndex = 8;
            // 
            // labelControlInvoiceState
            // 
            this.labelControlInvoiceState.Location = new System.Drawing.Point(36, 170);
            this.labelControlInvoiceState.Name = "labelControlInvoiceState";
            this.labelControlInvoiceState.Size = new System.Drawing.Size(60, 14);
            this.labelControlInvoiceState.TabIndex = 9;
            this.labelControlInvoiceState.Text = "发票状态：";
            // 
            // labelControlInType
            // 
            this.labelControlInType.Location = new System.Drawing.Point(36, 138);
            this.labelControlInType.Name = "labelControlInType";
            this.labelControlInType.Size = new System.Drawing.Size(60, 14);
            this.labelControlInType.TabIndex = 7;
            this.labelControlInType.Text = "入库方式：";
            // 
            // simpleButtonSearch
            // 
            this.simpleButtonSearch.Location = new System.Drawing.Point(102, 234);
            this.simpleButtonSearch.Name = "simpleButtonSearch";
            this.simpleButtonSearch.Size = new System.Drawing.Size(65, 21);
            this.simpleButtonSearch.TabIndex = 13;
            this.simpleButtonSearch.Text = "检索(&R)";
            this.simpleButtonSearch.Click += new System.EventHandler(this.simpleButtonSearch_Click);
            // 
            // labelControlAccountType
            // 
            this.labelControlAccountType.Location = new System.Drawing.Point(36, 104);
            this.labelControlAccountType.Name = "labelControlAccountType";
            this.labelControlAccountType.Size = new System.Drawing.Size(60, 14);
            this.labelControlAccountType.TabIndex = 5;
            this.labelControlAccountType.Text = "账册类型：";
            // 
            // imageComboBoxEditAccType
            // 
            this.imageComboBoxEditAccType.EnterMoveNextControl = true;
            this.imageComboBoxEditAccType.Location = new System.Drawing.Point(102, 101);
            this.imageComboBoxEditAccType.Name = "imageComboBoxEditAccType";
            this.imageComboBoxEditAccType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.imageComboBoxEditAccType.Size = new System.Drawing.Size(100, 21);
            this.imageComboBoxEditAccType.TabIndex = 6;
            this.imageComboBoxEditAccType.EditValueChanged += new System.EventHandler(this.imageComboBoxEditAccType_EditValueChanged);
            // 
            // panelControlHeader
            // 
            this.panelControlHeader.Controls.Add(this.labelControlCreateDate);
            this.panelControlHeader.Controls.Add(this.labelControlCreateEmp);
            this.panelControlHeader.Controls.Add(this.labelControlInTypeShow);
            this.panelControlHeader.Controls.Add(this.labelControlDateTimeShow);
            this.panelControlHeader.Controls.Add(this.labelControlTitle);
            this.panelControlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlHeader.Location = new System.Drawing.Point(216, 34);
            this.panelControlHeader.Name = "panelControlHeader";
            this.panelControlHeader.Size = new System.Drawing.Size(704, 87);
            this.panelControlHeader.TabIndex = 5;
            // 
            // labelControlCreateDate
            // 
            this.labelControlCreateDate.Location = new System.Drawing.Point(563, 44);
            this.labelControlCreateDate.Name = "labelControlCreateDate";
            this.labelControlCreateDate.Size = new System.Drawing.Size(60, 14);
            this.labelControlCreateDate.TabIndex = 12;
            this.labelControlCreateDate.Text = "制单日期：";
            // 
            // labelControlCreateEmp
            // 
            this.labelControlCreateEmp.Location = new System.Drawing.Point(412, 44);
            this.labelControlCreateEmp.Name = "labelControlCreateEmp";
            this.labelControlCreateEmp.Size = new System.Drawing.Size(48, 14);
            this.labelControlCreateEmp.TabIndex = 11;
            this.labelControlCreateEmp.Text = "制单人：";
            // 
            // labelControlInTypeShow
            // 
            this.labelControlInTypeShow.Location = new System.Drawing.Point(235, 44);
            this.labelControlInTypeShow.Name = "labelControlInTypeShow";
            this.labelControlInTypeShow.Size = new System.Drawing.Size(60, 14);
            this.labelControlInTypeShow.TabIndex = 10;
            this.labelControlInTypeShow.Text = "入库方式：";
            // 
            // labelControlDateTimeShow
            // 
            this.labelControlDateTimeShow.Location = new System.Drawing.Point(24, 44);
            this.labelControlDateTimeShow.Name = "labelControlDateTimeShow";
            this.labelControlDateTimeShow.Size = new System.Drawing.Size(60, 14);
            this.labelControlDateTimeShow.TabIndex = 9;
            this.labelControlDateTimeShow.Text = "年月范围：";
            // 
            // labelControlTitle
            // 
            this.labelControlTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControlTitle.Appearance.Options.UseFont = true;
            this.labelControlTitle.Location = new System.Drawing.Point(304, 12);
            this.labelControlTitle.Name = "labelControlTitle";
            this.labelControlTitle.Size = new System.Drawing.Size(140, 19);
            this.labelControlTitle.TabIndex = 0;
            this.labelControlTitle.Text = "采购分析报表(实物)";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(216, 121);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.MenuManager = this.barManager1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(704, 401);
            this.gridControl1.TabIndex = 6;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.DataSourceChanged += new System.EventHandler(this.gridControl1_DataSourceChanged);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnInType,
            this.gridColumnCompany,
            this.gridColumnBuyingPriceTotal,
            this.gridColumnRetailPriceTotal,
            this.gridColumnDifference,
            this.gridColumnRate});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // gridColumnInType
            // 
            this.gridColumnInType.Caption = "入库分类";
            this.gridColumnInType.FieldName = "TYPE_NAME";
            this.gridColumnInType.Name = "gridColumnInType";
            this.gridColumnInType.SummaryItem.DisplayFormat = "合计：   ";
            this.gridColumnInType.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.gridColumnInType.Visible = true;
            this.gridColumnInType.VisibleIndex = 0;
            // 
            // gridColumnCompany
            // 
            this.gridColumnCompany.Caption = "入库单位";
            this.gridColumnCompany.FieldName = "COM_NAME";
            this.gridColumnCompany.Name = "gridColumnCompany";
            this.gridColumnCompany.Visible = true;
            this.gridColumnCompany.VisibleIndex = 1;
            // 
            // gridColumnBuyingPriceTotal
            // 
            this.gridColumnBuyingPriceTotal.Caption = "进货总额";
            this.gridColumnBuyingPriceTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumnBuyingPriceTotal.FieldName = "BUYING_PRICE_TOTAL";
            this.gridColumnBuyingPriceTotal.Name = "gridColumnBuyingPriceTotal";
            this.gridColumnBuyingPriceTotal.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.gridColumnBuyingPriceTotal.Visible = true;
            this.gridColumnBuyingPriceTotal.VisibleIndex = 2;
            // 
            // gridColumnRetailPriceTotal
            // 
            this.gridColumnRetailPriceTotal.Caption = "零售总额";
            this.gridColumnRetailPriceTotal.DisplayFormat.FormatString = "0.000";
            this.gridColumnRetailPriceTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumnRetailPriceTotal.FieldName = "RETAIL_PRICE_TOTAL";
            this.gridColumnRetailPriceTotal.Name = "gridColumnRetailPriceTotal";
            this.gridColumnRetailPriceTotal.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.gridColumnRetailPriceTotal.Visible = true;
            this.gridColumnRetailPriceTotal.VisibleIndex = 3;
            // 
            // gridColumnDifference
            // 
            this.gridColumnDifference.Caption = "进销差额";
            this.gridColumnDifference.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumnDifference.FieldName = "BUYING_RETAIL_SUB";
            this.gridColumnDifference.Name = "gridColumnDifference";
            this.gridColumnDifference.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.gridColumnDifference.Visible = true;
            this.gridColumnDifference.VisibleIndex = 4;
            // 
            // gridColumnRate
            // 
            this.gridColumnRate.Caption = "扣率";
            this.gridColumnRate.FieldName = "RATE";
            this.gridColumnRate.Name = "gridColumnRate";
            this.gridColumnRate.Visible = true;
            this.gridColumnRate.VisibleIndex = 5;
            // 
            // pageSetupDialog1
            // 
            this.pageSetupDialog1.Document = this.printDocument1;
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Document = this.printDocument1;
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // FrmReportBillInStat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 522);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.panelControlHeader);
            this.Controls.Add(this.groupControlSearch);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmReportBillInStat";
            this.Text = "入库分析";
            this.Load += new System.EventHandler(this.FrmReportBillInStat_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollectionTool)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlSearch)).EndInit();
            this.groupControlSearch.ResumeLayout(false);
            this.groupControlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxEditDateType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditTimeEnd.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditTimeEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditTimeStart.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditTimeStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxEditInvoiceState.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxEditInType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxEditAccType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlHeader)).EndInit();
            this.panelControlHeader.ResumeLayout(false);
            this.panelControlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnInType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnCompany;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnBuyingPriceTotal;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnRetailPriceTotal;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDifference;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnRate;
        private DevExpress.XtraEditors.PanelControl panelControlHeader;
        private DevExpress.XtraEditors.LabelControl labelControlCreateDate;
        private DevExpress.XtraEditors.LabelControl labelControlCreateEmp;
        private DevExpress.XtraEditors.LabelControl labelControlInTypeShow;
        private DevExpress.XtraEditors.LabelControl labelControlDateTimeShow;
        private DevExpress.XtraEditors.LabelControl labelControlTitle;
        private DevExpress.XtraEditors.GroupControl groupControlSearch;
        private DevExpress.XtraEditors.ImageComboBoxEdit imageComboBoxEditDateType;
        private DevExpress.XtraEditors.LabelControl labelControlCompany;
        private DevExpress.XtraEditors.DateEdit dateEditTimeEnd;
        private DevExpress.XtraEditors.DateEdit dateEditTimeStart;
        private DevExpress.XtraEditors.ImageComboBoxEdit imageComboBoxEditInvoiceState;
        private DevExpress.XtraEditors.ImageComboBoxEdit imageComboBoxEditInType;
        private DevExpress.XtraEditors.LabelControl labelControlInvoiceState;
        private DevExpress.XtraEditors.LabelControl labelControlInType;
        private DevExpress.XtraEditors.SimpleButton simpleButtonSearch;
        private DevExpress.XtraEditors.LabelControl labelControlAccountType;
        private DevExpress.XtraEditors.ImageComboBoxEdit imageComboBoxEditAccType;
        private DevExpress.Utils.ImageCollection imageCollectionTool;
        private DevExpress.XtraEditors.ImageComboBoxEdit imageComboBoxEdit1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
    }
}