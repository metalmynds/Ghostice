namespace Ghostice.ApplicationKit
{
    partial class FormAppKit
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
        /// Required extensionMethod for Designer support - do not modify
        /// the contents of this extensionMethod with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblApplication = new System.Windows.Forms.Label();
            this.opnfledlgApplication = new System.Windows.Forms.OpenFileDialog();
            this.lblArguments = new System.Windows.Forms.Label();
            this.lstvewLog = new System.Windows.Forms.ListView();
            this.colhedIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhedTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhedRequest = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhedResult = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtArguments = new System.Windows.Forms.TextBox();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.lblEndPointAddress = new System.Windows.Forms.Label();
            this.txtRpcAddress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblApplication
            // 
            this.lblApplication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblApplication.AutoSize = true;
            this.lblApplication.Location = new System.Drawing.Point(11, 11);
            this.lblApplication.Name = "lblApplication";
            this.lblApplication.Size = new System.Drawing.Size(59, 13);
            this.lblApplication.TabIndex = 0;
            this.lblApplication.Text = "Application";
            // 
            // opnfledlgApplication
            // 
            this.opnfledlgApplication.DefaultExt = "exe";
            this.opnfledlgApplication.Filter = "Executable|*.exe";
            this.opnfledlgApplication.Title = "Select the Application Executable";
            // 
            // lblArguments
            // 
            this.lblArguments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblArguments.AutoSize = true;
            this.lblArguments.Location = new System.Drawing.Point(11, 37);
            this.lblArguments.Name = "lblArguments";
            this.lblArguments.Size = new System.Drawing.Size(57, 13);
            this.lblArguments.TabIndex = 5;
            this.lblArguments.Text = "Arguments";
            // 
            // lstvewLog
            // 
            this.lstvewLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstvewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colhedIndex,
            this.colhedTime,
            this.colhedRequest,
            this.colhedResult});
            this.lstvewLog.FullRowSelect = true;
            this.lstvewLog.Location = new System.Drawing.Point(12, 64);
            this.lstvewLog.Name = "lstvewLog";
            this.lstvewLog.Size = new System.Drawing.Size(689, 196);
            this.lstvewLog.TabIndex = 7;
            this.lstvewLog.UseCompatibleStateImageBehavior = false;
            this.lstvewLog.View = System.Windows.Forms.View.Details;
            // 
            // colhedIndex
            // 
            this.colhedIndex.Text = "#";
            this.colhedIndex.Width = 35;
            // 
            // colhedTime
            // 
            this.colhedTime.Text = "Time";
            // 
            // colhedRequest
            // 
            this.colhedRequest.Text = "Request";
            this.colhedRequest.Width = 250;
            // 
            // colhedResult
            // 
            this.colhedResult.Text = "Result";
            // 
            // txtArguments
            // 
            this.txtArguments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtArguments.BackColor = System.Drawing.SystemColors.Control;
            this.txtArguments.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Ghostice.ApplicationKit.Properties.Settings.Default, "AppKitTargetArguments", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtArguments.Location = new System.Drawing.Point(81, 34);
            this.txtArguments.Name = "txtArguments";
            this.txtArguments.ReadOnly = true;
            this.txtArguments.Size = new System.Drawing.Size(620, 20);
            this.txtArguments.TabIndex = 6;
            this.txtArguments.Text = global::Ghostice.ApplicationKit.Properties.Settings.Default.AppKitTargetArguments;
            // 
            // txtTarget
            // 
            this.txtTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTarget.BackColor = System.Drawing.SystemColors.Control;
            this.txtTarget.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Ghostice.ApplicationKit.Properties.Settings.Default, "AppKitTargetPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtTarget.Location = new System.Drawing.Point(81, 8);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.ReadOnly = true;
            this.txtTarget.Size = new System.Drawing.Size(620, 20);
            this.txtTarget.TabIndex = 1;
            this.txtTarget.Text = global::Ghostice.ApplicationKit.Properties.Settings.Default.AppKitTargetPath;
            // 
            // lblEndPointAddress
            // 
            this.lblEndPointAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEndPointAddress.AutoSize = true;
            this.lblEndPointAddress.Location = new System.Drawing.Point(11, 269);
            this.lblEndPointAddress.Name = "lblEndPointAddress";
            this.lblEndPointAddress.Size = new System.Drawing.Size(98, 13);
            this.lblEndPointAddress.TabIndex = 10;
            this.lblEndPointAddress.Text = "Listening End Point";
            // 
            // txtRpcAddress
            // 
            this.txtRpcAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRpcAddress.BackColor = System.Drawing.SystemColors.Control;
            this.txtRpcAddress.Location = new System.Drawing.Point(120, 266);
            this.txtRpcAddress.Name = "txtRpcAddress";
            this.txtRpcAddress.ReadOnly = true;
            this.txtRpcAddress.Size = new System.Drawing.Size(581, 20);
            this.txtRpcAddress.TabIndex = 11;
            // 
            // FormAppKit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 293);
            this.Controls.Add(this.txtRpcAddress);
            this.Controls.Add(this.lblEndPointAddress);
            this.Controls.Add(this.lstvewLog);
            this.Controls.Add(this.txtArguments);
            this.Controls.Add(this.lblArguments);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.lblApplication);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormAppKit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = " Ghostice Application Kit Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HandleAppKitFormClosed);
            this.Load += new System.EventHandler(this.HandleAppKitFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblApplication;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.OpenFileDialog opnfledlgApplication;
        private System.Windows.Forms.TextBox txtArguments;
        private System.Windows.Forms.Label lblArguments;
        private System.Windows.Forms.ListView lstvewLog;
        private System.Windows.Forms.ColumnHeader colhedTime;
        private System.Windows.Forms.ColumnHeader colhedRequest;
        private System.Windows.Forms.ColumnHeader colhedIndex;
        private System.Windows.Forms.ColumnHeader colhedResult;
        private System.Windows.Forms.Label lblEndPointAddress;
        private System.Windows.Forms.TextBox txtRpcAddress;
    }
}

