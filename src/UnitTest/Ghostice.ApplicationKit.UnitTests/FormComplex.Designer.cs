namespace Ghostice.ApplicationKit.UnitTests
{
    partial class FormComplex
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
            this.tabctrlAdmin = new System.Windows.Forms.TabControl();
            this.tabpgRegistration = new System.Windows.Forms.TabPage();
            this.butSave = new System.Windows.Forms.Button();
            this.lblRules = new System.Windows.Forms.Label();
            this.lstRules = new System.Windows.Forms.ListBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.cmbEats = new System.Windows.Forms.ComboBox();
            this.lblEats = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblSelectPrevious = new System.Windows.Forms.Label();
            this.conmnuSelectExisting = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tabpgHistory = new System.Windows.Forms.TabPage();
            this.lstvewHistory = new System.Windows.Forms.ListView();
            this.colhedHistoryName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhedHistoryType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhedHistoryPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhedHistorySold = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtSummary = new System.Windows.Forms.TextBox();
            this.lblSummary = new System.Windows.Forms.Label();
            this.tabctrlSales = new System.Windows.Forms.TabControl();
            this.tabpgeBasket = new System.Windows.Forms.TabPage();
            this.butBasketPurchase = new System.Windows.Forms.Button();
            this.butBasketReset = new System.Windows.Forms.Button();
            this.chkVATReceipt = new System.Windows.Forms.CheckBox();
            this.rdobutCard = new System.Windows.Forms.RadioButton();
            this.rdobutCheque = new System.Windows.Forms.RadioButton();
            this.rdobutCash = new System.Windows.Forms.RadioButton();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lstvewBasketPurchases = new System.Windows.Forms.ListView();
            this.colhedBasketItem = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhedBasketPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblPurchases = new System.Windows.Forms.Label();
            this.cmbPetBasket = new System.Windows.Forms.ComboBox();
            this.lblSelectPurchases = new System.Windows.Forms.Label();
            this.tbpgeAccessories = new System.Windows.Forms.TabPage();
            this.lstAccessories = new System.Windows.Forms.ListBox();
            this.spltcontMain = new System.Windows.Forms.SplitContainer();
            this.tolstrpcntMain = new System.Windows.Forms.ToolStripContainer();
            this.mnustrpMain = new System.Windows.Forms.MenuStrip();
            this.tolstrpmnuitmTab = new System.Windows.Forms.ToolStripMenuItem();
            this.tolstrpmnuitemHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.tolstrpmnuitmBasket = new System.Windows.Forms.ToolStripMenuItem();
            this.tolstrpmuitmRegistration = new System.Windows.Forms.ToolStripMenuItem();
            this.tolstrpmuitmAccessories = new System.Windows.Forms.ToolStripMenuItem();
            this.tabctrlAdmin.SuspendLayout();
            this.tabpgRegistration.SuspendLayout();
            this.tabpgHistory.SuspendLayout();
            this.tabctrlSales.SuspendLayout();
            this.tabpgeBasket.SuspendLayout();
            this.tbpgeAccessories.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spltcontMain)).BeginInit();
            this.spltcontMain.Panel1.SuspendLayout();
            this.spltcontMain.Panel2.SuspendLayout();
            this.spltcontMain.SuspendLayout();
            this.tolstrpcntMain.ContentPanel.SuspendLayout();
            this.tolstrpcntMain.TopToolStripPanel.SuspendLayout();
            this.tolstrpcntMain.SuspendLayout();
            this.mnustrpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabctrlAdmin
            // 
            this.tabctrlAdmin.Controls.Add(this.tabpgRegistration);
            this.tabctrlAdmin.Controls.Add(this.tabpgHistory);
            this.tabctrlAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabctrlAdmin.Location = new System.Drawing.Point(0, 0);
            this.tabctrlAdmin.Margin = new System.Windows.Forms.Padding(2);
            this.tabctrlAdmin.Name = "tabctrlAdmin";
            this.tabctrlAdmin.SelectedIndex = 0;
            this.tabctrlAdmin.Size = new System.Drawing.Size(288, 426);
            this.tabctrlAdmin.TabIndex = 0;
            // 
            // tabpgRegistration
            // 
            this.tabpgRegistration.Controls.Add(this.butSave);
            this.tabpgRegistration.Controls.Add(this.lblRules);
            this.tabpgRegistration.Controls.Add(this.lstRules);
            this.tabpgRegistration.Controls.Add(this.txtPrice);
            this.tabpgRegistration.Controls.Add(this.lblPrice);
            this.tabpgRegistration.Controls.Add(this.cmbEats);
            this.tabpgRegistration.Controls.Add(this.lblEats);
            this.tabpgRegistration.Controls.Add(this.cmbType);
            this.tabpgRegistration.Controls.Add(this.lblType);
            this.tabpgRegistration.Controls.Add(this.txtName);
            this.tabpgRegistration.Controls.Add(this.lblName);
            this.tabpgRegistration.Controls.Add(this.lblSelectPrevious);
            this.tabpgRegistration.Location = new System.Drawing.Point(4, 22);
            this.tabpgRegistration.Margin = new System.Windows.Forms.Padding(2);
            this.tabpgRegistration.Name = "tabpgRegistration";
            this.tabpgRegistration.Padding = new System.Windows.Forms.Padding(2);
            this.tabpgRegistration.Size = new System.Drawing.Size(280, 400);
            this.tabpgRegistration.TabIndex = 0;
            this.tabpgRegistration.Text = "Registration";
            this.tabpgRegistration.UseVisualStyleBackColor = true;
            // 
            // butSave
            // 
            this.butSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butSave.Location = new System.Drawing.Point(206, 205);
            this.butSave.Margin = new System.Windows.Forms.Padding(2);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(56, 19);
            this.butSave.TabIndex = 11;
            this.butSave.Text = "Save";
            this.butSave.UseVisualStyleBackColor = true;
            // 
            // lblRules
            // 
            this.lblRules.AutoSize = true;
            this.lblRules.Location = new System.Drawing.Point(6, 155);
            this.lblRules.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRules.Name = "lblRules";
            this.lblRules.Size = new System.Drawing.Size(37, 13);
            this.lblRules.TabIndex = 10;
            this.lblRules.Text = "Rules:";
            // 
            // lstRules
            // 
            this.lstRules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstRules.FormattingEnabled = true;
            this.lstRules.Items.AddRange(new object[] {
            "Special Environment",
            "Dangerous",
            "Sell In Pairs",
            "No Children"});
            this.lstRules.Location = new System.Drawing.Point(47, 132);
            this.lstRules.Margin = new System.Windows.Forms.Padding(2);
            this.lstRules.Name = "lstRules";
            this.lstRules.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstRules.Size = new System.Drawing.Size(216, 69);
            this.lstRules.TabIndex = 9;
            // 
            // txtPrice
            // 
            this.txtPrice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrice.Location = new System.Drawing.Point(47, 109);
            this.txtPrice.Margin = new System.Windows.Forms.Padding(2);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(216, 20);
            this.txtPrice.TabIndex = 8;
            this.txtPrice.Text = "0.00";
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(6, 109);
            this.lblPrice.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(34, 13);
            this.lblPrice.TabIndex = 7;
            this.lblPrice.Text = "Price:";
            // 
            // cmbEats
            // 
            this.cmbEats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbEats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEats.FormattingEnabled = true;
            this.cmbEats.Items.AddRange(new object[] {
            "Herbivorous",
            "Carnivorous",
            "Omnivorous",
            "Insectivorous",
            "Lunivorous",
            "Eats People"});
            this.cmbEats.Location = new System.Drawing.Point(47, 84);
            this.cmbEats.Margin = new System.Windows.Forms.Padding(2);
            this.cmbEats.Name = "cmbEats";
            this.cmbEats.Size = new System.Drawing.Size(216, 21);
            this.cmbEats.TabIndex = 6;
            // 
            // lblEats
            // 
            this.lblEats.AutoSize = true;
            this.lblEats.Location = new System.Drawing.Point(6, 84);
            this.lblEats.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEats.Name = "lblEats";
            this.lblEats.Size = new System.Drawing.Size(31, 13);
            this.lblEats.TabIndex = 5;
            this.lblEats.Text = "Eats:";
            // 
            // cmbType
            // 
            this.cmbType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            "Cat",
            "Dog",
            "Rabbit",
            "Rodent",
            "Large Bird",
            "Small Bird",
            "Reptile",
            "Fish"});
            this.cmbType.Location = new System.Drawing.Point(47, 60);
            this.cmbType.Margin = new System.Windows.Forms.Padding(2);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(216, 21);
            this.cmbType.TabIndex = 4;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(6, 60);
            this.lblType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(34, 13);
            this.lblType.TabIndex = 3;
            this.lblType.Text = "Type:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(47, 36);
            this.txtName.Margin = new System.Windows.Forms.Padding(2);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(216, 20);
            this.txtName.TabIndex = 2;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(6, 36);
            this.lblName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name:";
            // 
            // lblSelectPrevious
            // 
            this.lblSelectPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectPrevious.AutoSize = true;
            this.lblSelectPrevious.ContextMenuStrip = this.conmnuSelectExisting;
            this.lblSelectPrevious.Location = new System.Drawing.Point(20, 12);
            this.lblSelectPrevious.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSelectPrevious.Name = "lblSelectPrevious";
            this.lblSelectPrevious.Size = new System.Drawing.Size(236, 13);
            this.lblSelectPrevious.TabIndex = 0;
            this.lblSelectPrevious.Text = "(Right-click here to copy an existing pet\'s details)";
            // 
            // conmnuSelectExisting
            // 
            this.conmnuSelectExisting.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.conmnuSelectExisting.Name = "conmnuSelectExisting";
            this.conmnuSelectExisting.Size = new System.Drawing.Size(61, 4);
            // 
            // tabpgHistory
            // 
            this.tabpgHistory.Controls.Add(this.lstvewHistory);
            this.tabpgHistory.Controls.Add(this.txtSummary);
            this.tabpgHistory.Controls.Add(this.lblSummary);
            this.tabpgHistory.Location = new System.Drawing.Point(4, 22);
            this.tabpgHistory.Margin = new System.Windows.Forms.Padding(2);
            this.tabpgHistory.Name = "tabpgHistory";
            this.tabpgHistory.Padding = new System.Windows.Forms.Padding(2);
            this.tabpgHistory.Size = new System.Drawing.Size(280, 400);
            this.tabpgHistory.TabIndex = 1;
            this.tabpgHistory.Text = "History";
            this.tabpgHistory.UseVisualStyleBackColor = true;
            // 
            // lstvewHistory
            // 
            this.lstvewHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstvewHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colhedHistoryName,
            this.colhedHistoryType,
            this.colhedHistoryPrice,
            this.colhedHistorySold});
            this.lstvewHistory.Location = new System.Drawing.Point(7, 299);
            this.lstvewHistory.Margin = new System.Windows.Forms.Padding(2);
            this.lstvewHistory.Name = "lstvewHistory";
            this.lstvewHistory.Size = new System.Drawing.Size(265, 97);
            this.lstvewHistory.TabIndex = 2;
            this.lstvewHistory.UseCompatibleStateImageBehavior = false;
            this.lstvewHistory.View = System.Windows.Forms.View.Details;
            // 
            // colhedHistoryName
            // 
            this.colhedHistoryName.Text = "Name";
            this.colhedHistoryName.Width = 120;
            // 
            // colhedHistoryType
            // 
            this.colhedHistoryType.Text = "Type";
            // 
            // colhedHistoryPrice
            // 
            this.colhedHistoryPrice.Text = "Price";
            // 
            // colhedHistorySold
            // 
            this.colhedHistorySold.Text = "Sold";
            // 
            // txtSummary
            // 
            this.txtSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSummary.Location = new System.Drawing.Point(7, 19);
            this.txtSummary.Margin = new System.Windows.Forms.Padding(2);
            this.txtSummary.Multiline = true;
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.Size = new System.Drawing.Size(265, 276);
            this.txtSummary.TabIndex = 1;
            this.txtSummary.Text = "History so far:";
            // 
            // lblSummary
            // 
            this.lblSummary.AutoSize = true;
            this.lblSummary.Location = new System.Drawing.Point(4, 2);
            this.lblSummary.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(233, 13);
            this.lblSummary.TabIndex = 0;
            this.lblSummary.Text = "A short summary of what has already happened:";
            // 
            // tabctrlSales
            // 
            this.tabctrlSales.Controls.Add(this.tabpgeBasket);
            this.tabctrlSales.Controls.Add(this.tbpgeAccessories);
            this.tabctrlSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabctrlSales.Location = new System.Drawing.Point(0, 0);
            this.tabctrlSales.Margin = new System.Windows.Forms.Padding(2);
            this.tabctrlSales.Name = "tabctrlSales";
            this.tabctrlSales.SelectedIndex = 0;
            this.tabctrlSales.Size = new System.Drawing.Size(576, 426);
            this.tabctrlSales.TabIndex = 1;
            // 
            // tabpgeBasket
            // 
            this.tabpgeBasket.Controls.Add(this.butBasketPurchase);
            this.tabpgeBasket.Controls.Add(this.butBasketReset);
            this.tabpgeBasket.Controls.Add(this.chkVATReceipt);
            this.tabpgeBasket.Controls.Add(this.rdobutCard);
            this.tabpgeBasket.Controls.Add(this.rdobutCheque);
            this.tabpgeBasket.Controls.Add(this.rdobutCash);
            this.tabpgeBasket.Controls.Add(this.lblTotalValue);
            this.tabpgeBasket.Controls.Add(this.lblTotal);
            this.tabpgeBasket.Controls.Add(this.lstvewBasketPurchases);
            this.tabpgeBasket.Controls.Add(this.lblPurchases);
            this.tabpgeBasket.Controls.Add(this.cmbPetBasket);
            this.tabpgeBasket.Controls.Add(this.lblSelectPurchases);
            this.tabpgeBasket.Location = new System.Drawing.Point(4, 22);
            this.tabpgeBasket.Margin = new System.Windows.Forms.Padding(2);
            this.tabpgeBasket.Name = "tabpgeBasket";
            this.tabpgeBasket.Padding = new System.Windows.Forms.Padding(2);
            this.tabpgeBasket.Size = new System.Drawing.Size(568, 400);
            this.tabpgeBasket.TabIndex = 0;
            this.tabpgeBasket.Text = "Basket";
            this.tabpgeBasket.UseVisualStyleBackColor = true;
            // 
            // butBasketPurchase
            // 
            this.butBasketPurchase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butBasketPurchase.Enabled = false;
            this.butBasketPurchase.Location = new System.Drawing.Point(488, 240);
            this.butBasketPurchase.Margin = new System.Windows.Forms.Padding(2);
            this.butBasketPurchase.Name = "butBasketPurchase";
            this.butBasketPurchase.Size = new System.Drawing.Size(68, 19);
            this.butBasketPurchase.TabIndex = 11;
            this.butBasketPurchase.Text = "Purchase";
            this.butBasketPurchase.UseVisualStyleBackColor = true;
            // 
            // butBasketReset
            // 
            this.butBasketReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butBasketReset.Location = new System.Drawing.Point(427, 240);
            this.butBasketReset.Margin = new System.Windows.Forms.Padding(2);
            this.butBasketReset.Name = "butBasketReset";
            this.butBasketReset.Size = new System.Drawing.Size(56, 19);
            this.butBasketReset.TabIndex = 10;
            this.butBasketReset.Text = "Reset";
            this.butBasketReset.UseVisualStyleBackColor = true;
            // 
            // chkVATReceipt
            // 
            this.chkVATReceipt.AutoSize = true;
            this.chkVATReceipt.Location = new System.Drawing.Point(14, 232);
            this.chkVATReceipt.Margin = new System.Windows.Forms.Padding(2);
            this.chkVATReceipt.Name = "chkVATReceipt";
            this.chkVATReceipt.Size = new System.Drawing.Size(87, 17);
            this.chkVATReceipt.TabIndex = 9;
            this.chkVATReceipt.Text = "VAT Receipt";
            this.chkVATReceipt.UseVisualStyleBackColor = true;
            // 
            // rdobutCard
            // 
            this.rdobutCard.AutoSize = true;
            this.rdobutCard.Location = new System.Drawing.Point(124, 208);
            this.rdobutCard.Margin = new System.Windows.Forms.Padding(2);
            this.rdobutCard.Name = "rdobutCard";
            this.rdobutCard.Size = new System.Drawing.Size(47, 17);
            this.rdobutCard.TabIndex = 8;
            this.rdobutCard.TabStop = true;
            this.rdobutCard.Text = "Card";
            this.rdobutCard.UseVisualStyleBackColor = true;
            // 
            // rdobutCheque
            // 
            this.rdobutCheque.AutoSize = true;
            this.rdobutCheque.Location = new System.Drawing.Point(62, 208);
            this.rdobutCheque.Margin = new System.Windows.Forms.Padding(2);
            this.rdobutCheque.Name = "rdobutCheque";
            this.rdobutCheque.Size = new System.Drawing.Size(62, 17);
            this.rdobutCheque.TabIndex = 7;
            this.rdobutCheque.TabStop = true;
            this.rdobutCheque.Text = "Cheque";
            this.rdobutCheque.UseVisualStyleBackColor = true;
            // 
            // rdobutCash
            // 
            this.rdobutCash.AutoSize = true;
            this.rdobutCash.Location = new System.Drawing.Point(11, 208);
            this.rdobutCash.Margin = new System.Windows.Forms.Padding(2);
            this.rdobutCash.Name = "rdobutCash";
            this.rdobutCash.Size = new System.Drawing.Size(49, 17);
            this.rdobutCash.TabIndex = 6;
            this.rdobutCash.TabStop = true;
            this.rdobutCash.Text = "Cash";
            this.rdobutCash.UseVisualStyleBackColor = true;
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalValue.AutoSize = true;
            this.lblTotalValue.Location = new System.Drawing.Point(519, 211);
            this.lblTotalValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(28, 13);
            this.lblTotalValue.TabIndex = 5;
            this.lblTotalValue.Text = "0.00";
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(482, 211);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(34, 13);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "Total:";
            // 
            // lstvewBasketPurchases
            // 
            this.lstvewBasketPurchases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstvewBasketPurchases.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colhedBasketItem,
            this.colhedBasketPrice});
            this.lstvewBasketPurchases.Location = new System.Drawing.Point(13, 61);
            this.lstvewBasketPurchases.Margin = new System.Windows.Forms.Padding(2);
            this.lstvewBasketPurchases.Name = "lstvewBasketPurchases";
            this.lstvewBasketPurchases.Size = new System.Drawing.Size(543, 146);
            this.lstvewBasketPurchases.TabIndex = 3;
            this.lstvewBasketPurchases.UseCompatibleStateImageBehavior = false;
            this.lstvewBasketPurchases.View = System.Windows.Forms.View.Details;
            // 
            // colhedBasketItem
            // 
            this.colhedBasketItem.Text = "Item";
            this.colhedBasketItem.Width = 300;
            // 
            // colhedBasketPrice
            // 
            this.colhedBasketPrice.Text = "Price";
            // 
            // lblPurchases
            // 
            this.lblPurchases.AutoSize = true;
            this.lblPurchases.Location = new System.Drawing.Point(9, 39);
            this.lblPurchases.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPurchases.Name = "lblPurchases";
            this.lblPurchases.Size = new System.Drawing.Size(86, 13);
            this.lblPurchases.TabIndex = 2;
            this.lblPurchases.Text = "Purchases so far";
            // 
            // cmbPetBasket
            // 
            this.cmbPetBasket.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPetBasket.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPetBasket.FormattingEnabled = true;
            this.cmbPetBasket.Location = new System.Drawing.Point(104, 10);
            this.cmbPetBasket.Margin = new System.Windows.Forms.Padding(2);
            this.cmbPetBasket.Name = "cmbPetBasket";
            this.cmbPetBasket.Size = new System.Drawing.Size(452, 21);
            this.cmbPetBasket.TabIndex = 1;
            // 
            // lblSelectPurchases
            // 
            this.lblSelectPurchases.AutoSize = true;
            this.lblSelectPurchases.Location = new System.Drawing.Point(9, 12);
            this.lblSelectPurchases.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSelectPurchases.Name = "lblSelectPurchases";
            this.lblSelectPurchases.Size = new System.Drawing.Size(92, 13);
            this.lblSelectPurchases.TabIndex = 0;
            this.lblSelectPurchases.Text = "Select purchases:";
            // 
            // tbpgeAccessories
            // 
            this.tbpgeAccessories.Controls.Add(this.lstAccessories);
            this.tbpgeAccessories.Location = new System.Drawing.Point(4, 22);
            this.tbpgeAccessories.Margin = new System.Windows.Forms.Padding(2);
            this.tbpgeAccessories.Name = "tbpgeAccessories";
            this.tbpgeAccessories.Padding = new System.Windows.Forms.Padding(2);
            this.tbpgeAccessories.Size = new System.Drawing.Size(568, 400);
            this.tbpgeAccessories.TabIndex = 1;
            this.tbpgeAccessories.Text = "Accessories";
            this.tbpgeAccessories.UseVisualStyleBackColor = true;
            // 
            // lstAccessories
            // 
            this.lstAccessories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAccessories.FormattingEnabled = true;
            this.lstAccessories.Location = new System.Drawing.Point(2, 2);
            this.lstAccessories.Margin = new System.Windows.Forms.Padding(2);
            this.lstAccessories.Name = "lstAccessories";
            this.lstAccessories.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstAccessories.Size = new System.Drawing.Size(564, 396);
            this.lstAccessories.TabIndex = 0;
            // 
            // spltcontMain
            // 
            this.spltcontMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltcontMain.Location = new System.Drawing.Point(0, 0);
            this.spltcontMain.Margin = new System.Windows.Forms.Padding(2);
            this.spltcontMain.Name = "spltcontMain";
            // 
            // spltcontMain.Panel1
            // 
            this.spltcontMain.Panel1.Controls.Add(this.tabctrlAdmin);
            // 
            // spltcontMain.Panel2
            // 
            this.spltcontMain.Panel2.Controls.Add(this.tabctrlSales);
            this.spltcontMain.Size = new System.Drawing.Size(867, 426);
            this.spltcontMain.SplitterDistance = 288;
            this.spltcontMain.SplitterWidth = 3;
            this.spltcontMain.TabIndex = 2;
            // 
            // tolstrpcntMain
            // 
            // 
            // tolstrpcntMain.ContentPanel
            // 
            this.tolstrpcntMain.ContentPanel.Controls.Add(this.spltcontMain);
            this.tolstrpcntMain.ContentPanel.Margin = new System.Windows.Forms.Padding(2);
            this.tolstrpcntMain.ContentPanel.Size = new System.Drawing.Size(867, 426);
            this.tolstrpcntMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tolstrpcntMain.Location = new System.Drawing.Point(0, 0);
            this.tolstrpcntMain.Margin = new System.Windows.Forms.Padding(2);
            this.tolstrpcntMain.Name = "tolstrpcntMain";
            this.tolstrpcntMain.Size = new System.Drawing.Size(867, 450);
            this.tolstrpcntMain.TabIndex = 3;
            this.tolstrpcntMain.Text = "toolStripContainer1";
            // 
            // tolstrpcntMain.TopToolStripPanel
            // 
            this.tolstrpcntMain.TopToolStripPanel.Controls.Add(this.mnustrpMain);
            // 
            // mnustrpMain
            // 
            this.mnustrpMain.Dock = System.Windows.Forms.DockStyle.None;
            this.mnustrpMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnustrpMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tolstrpmnuitmTab});
            this.mnustrpMain.Location = new System.Drawing.Point(0, 0);
            this.mnustrpMain.Name = "mnustrpMain";
            this.mnustrpMain.Size = new System.Drawing.Size(867, 24);
            this.mnustrpMain.TabIndex = 0;
            this.mnustrpMain.Text = "mnustrpMain";
            // 
            // tolstrpmnuitmTab
            // 
            this.tolstrpmnuitmTab.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tolstrpmnuitemHistory,
            this.tolstrpmnuitmBasket,
            this.tolstrpmuitmRegistration,
            this.tolstrpmuitmAccessories});
            this.tolstrpmnuitmTab.Name = "tolstrpmnuitmTab";
            this.tolstrpmnuitmTab.Size = new System.Drawing.Size(37, 20);
            this.tolstrpmnuitmTab.Text = "&File";
            // 
            // tolstrpmnuitemHistory
            // 
            this.tolstrpmnuitemHistory.Name = "tolstrpmnuitemHistory";
            this.tolstrpmnuitemHistory.Size = new System.Drawing.Size(137, 22);
            this.tolstrpmnuitemHistory.Text = "History";
            // 
            // tolstrpmnuitmBasket
            // 
            this.tolstrpmnuitmBasket.Name = "tolstrpmnuitmBasket";
            this.tolstrpmnuitmBasket.Size = new System.Drawing.Size(137, 22);
            this.tolstrpmnuitmBasket.Text = "Basket";
            // 
            // tolstrpmuitmRegistration
            // 
            this.tolstrpmuitmRegistration.Name = "tolstrpmuitmRegistration";
            this.tolstrpmuitmRegistration.Size = new System.Drawing.Size(137, 22);
            this.tolstrpmuitmRegistration.Text = "Registration";
            // 
            // tolstrpmuitmAccessories
            // 
            this.tolstrpmuitmAccessories.Name = "tolstrpmuitmAccessories";
            this.tolstrpmuitmAccessories.Size = new System.Drawing.Size(137, 22);
            this.tolstrpmuitmAccessories.Text = "Accessories";
            // 
            // FormComplex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 450);
            this.Controls.Add(this.tolstrpcntMain);
            this.MainMenuStrip = this.mnustrpMain;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormComplex";
            this.Text = "Petshop WinForms";
            this.tabctrlAdmin.ResumeLayout(false);
            this.tabpgRegistration.ResumeLayout(false);
            this.tabpgRegistration.PerformLayout();
            this.tabpgHistory.ResumeLayout(false);
            this.tabpgHistory.PerformLayout();
            this.tabctrlSales.ResumeLayout(false);
            this.tabpgeBasket.ResumeLayout(false);
            this.tabpgeBasket.PerformLayout();
            this.tbpgeAccessories.ResumeLayout(false);
            this.spltcontMain.Panel1.ResumeLayout(false);
            this.spltcontMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spltcontMain)).EndInit();
            this.spltcontMain.ResumeLayout(false);
            this.tolstrpcntMain.ContentPanel.ResumeLayout(false);
            this.tolstrpcntMain.TopToolStripPanel.ResumeLayout(false);
            this.tolstrpcntMain.TopToolStripPanel.PerformLayout();
            this.tolstrpcntMain.ResumeLayout(false);
            this.tolstrpcntMain.PerformLayout();
            this.mnustrpMain.ResumeLayout(false);
            this.mnustrpMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabctrlAdmin;
        private System.Windows.Forms.TabPage tabpgRegistration;
        private System.Windows.Forms.TabPage tabpgHistory;
        private System.Windows.Forms.TabControl tabctrlSales;
        private System.Windows.Forms.TabPage tabpgeBasket;
        private System.Windows.Forms.TabPage tbpgeAccessories;
        private System.Windows.Forms.Label lblSelectPrevious;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox cmbEats;
        private System.Windows.Forms.Label lblEats;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblRules;
        private System.Windows.Forms.ListBox lstRules;
        private System.Windows.Forms.Button butSave;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.ListView lstvewHistory;
        private System.Windows.Forms.ColumnHeader colhedHistoryName;
        private System.Windows.Forms.ColumnHeader colhedHistoryType;
        private System.Windows.Forms.ColumnHeader colhedHistoryPrice;
        private System.Windows.Forms.ColumnHeader colhedHistorySold;
        private System.Windows.Forms.TextBox txtSummary;
        private System.Windows.Forms.Label lblSelectPurchases;
        private System.Windows.Forms.ComboBox cmbPetBasket;
        private System.Windows.Forms.Label lblPurchases;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.ListView lstvewBasketPurchases;
        private System.Windows.Forms.ColumnHeader colhedBasketItem;
        private System.Windows.Forms.ColumnHeader colhedBasketPrice;
        private System.Windows.Forms.Button butBasketPurchase;
        private System.Windows.Forms.Button butBasketReset;
        private System.Windows.Forms.CheckBox chkVATReceipt;
        private System.Windows.Forms.RadioButton rdobutCard;
        private System.Windows.Forms.RadioButton rdobutCheque;
        private System.Windows.Forms.RadioButton rdobutCash;
        private System.Windows.Forms.ListBox lstAccessories;
        private System.Windows.Forms.SplitContainer spltcontMain;
        private System.Windows.Forms.ToolStripContainer tolstrpcntMain;
        private System.Windows.Forms.MenuStrip mnustrpMain;
        private System.Windows.Forms.ToolStripMenuItem tolstrpmnuitmTab;
        private System.Windows.Forms.ToolStripMenuItem tolstrpmnuitemHistory;
        private System.Windows.Forms.ToolStripMenuItem tolstrpmnuitmBasket;
        private System.Windows.Forms.ToolStripMenuItem tolstrpmuitmRegistration;
        private System.Windows.Forms.ToolStripMenuItem tolstrpmuitmAccessories;
        private System.Windows.Forms.ContextMenuStrip conmnuSelectExisting;
    }
}

