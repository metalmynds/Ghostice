namespace Ghostice.ApplicationKit.UnitTests
{
    partial class FormSimpleUserControl
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
            this.userControlSimple1 = new Ghostice.ApplicationKit.UnitTests.UserControlSimple();
            this.SuspendLayout();
            // 
            // userControlSimple1
            // 
            this.userControlSimple1.Location = new System.Drawing.Point(13, 19);
            this.userControlSimple1.Name = "userControlSimple1";
            this.userControlSimple1.Size = new System.Drawing.Size(250, 50);
            this.userControlSimple1.TabIndex = 0;
            // 
            // FormUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.userControlSimple1);
            this.Name = "FormUserControl";
            this.Text = "FormUserControl";
            this.ResumeLayout(false);

        }

        #endregion

        private UserControlSimple userControlSimple1;
    }
}