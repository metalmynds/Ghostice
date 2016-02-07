namespace Ghostice.ApplicationKit.UnitTests
{
    partial class FormNestedTabPageControls
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
            this.tabctrlTabControl = new System.Windows.Forms.TabControl();
            this.tabpgeTabPage1 = new System.Windows.Forms.TabPage();
            this.txtboxTextBox = new System.Windows.Forms.TextBox();
            this.lblTextBoxLabel = new System.Windows.Forms.Label();
            this.tabpgeTabPage2 = new System.Windows.Forms.TabPage();
            this.tabctrlTabControl.SuspendLayout();
            this.tabpgeTabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabctrlTabControl
            // 
            this.tabctrlTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabctrlTabControl.Controls.Add(this.tabpgeTabPage1);
            this.tabctrlTabControl.Controls.Add(this.tabpgeTabPage2);
            this.tabctrlTabControl.Location = new System.Drawing.Point(12, 12);
            this.tabctrlTabControl.Name = "tabctrlTabControl";
            this.tabctrlTabControl.SelectedIndex = 0;
            this.tabctrlTabControl.Size = new System.Drawing.Size(408, 293);
            this.tabctrlTabControl.TabIndex = 0;
            // 
            // tabpgeTabPage1
            // 
            this.tabpgeTabPage1.Controls.Add(this.txtboxTextBox);
            this.tabpgeTabPage1.Controls.Add(this.lblTextBoxLabel);
            this.tabpgeTabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabpgeTabPage1.Name = "tabpgeTabPage1";
            this.tabpgeTabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabpgeTabPage1.Size = new System.Drawing.Size(400, 267);
            this.tabpgeTabPage1.TabIndex = 0;
            this.tabpgeTabPage1.Text = "TabPage1";
            this.tabpgeTabPage1.UseVisualStyleBackColor = true;
            // 
            // txtboxTextBox
            // 
            this.txtboxTextBox.Location = new System.Drawing.Point(67, 17);
            this.txtboxTextBox.Name = "txtboxTextBox";
            this.txtboxTextBox.Size = new System.Drawing.Size(317, 20);
            this.txtboxTextBox.TabIndex = 1;
            this.txtboxTextBox.Text = "AMP Rules";
            // 
            // lblTextBoxLabel
            // 
            this.lblTextBoxLabel.AutoSize = true;
            this.lblTextBoxLabel.Location = new System.Drawing.Point(21, 19);
            this.lblTextBoxLabel.Name = "lblTextBoxLabel";
            this.lblTextBoxLabel.Size = new System.Drawing.Size(35, 13);
            this.lblTextBoxLabel.TabIndex = 0;
            this.lblTextBoxLabel.Text = "label1";
            // 
            // tabpgeTabPage2
            // 
            this.tabpgeTabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabpgeTabPage2.Name = "tabpgeTabPage2";
            this.tabpgeTabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabpgeTabPage2.Size = new System.Drawing.Size(400, 267);
            this.tabpgeTabPage2.TabIndex = 1;
            this.tabpgeTabPage2.Text = "TabPage2";
            this.tabpgeTabPage2.UseVisualStyleBackColor = true;
            // 
            // FormNestedTabPageControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(432, 317);
            this.Controls.Add(this.tabctrlTabControl);
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Name = "FormNestedTabPageControls";
            this.Text = "FormNestedTabPageControls";
            this.tabctrlTabControl.ResumeLayout(false);
            this.tabpgeTabPage1.ResumeLayout(false);
            this.tabpgeTabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabctrlTabControl;
        private System.Windows.Forms.TabPage tabpgeTabPage1;
        private System.Windows.Forms.TabPage tabpgeTabPage2;
        private System.Windows.Forms.TextBox txtboxTextBox;
        private System.Windows.Forms.Label lblTextBoxLabel;
    }
}