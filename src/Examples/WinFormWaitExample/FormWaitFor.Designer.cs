namespace WinFormWaitExample
{
    partial class FormWaitFor
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
            this.lblActionOneResult = new System.Windows.Forms.Label();
            this.butActionOne = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.butActionTwo = new System.Windows.Forms.Button();
            this.butActionThree = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblActionOneResult
            // 
            this.lblActionOneResult.AutoSize = true;
            this.lblActionOneResult.Location = new System.Drawing.Point(126, 49);
            this.lblActionOneResult.Name = "lblActionOneResult";
            this.lblActionOneResult.Size = new System.Drawing.Size(53, 13);
            this.lblActionOneResult.TabIndex = 5;
            this.lblActionOneResult.Text = "Not Done";
            // 
            // butActionOne
            // 
            this.butActionOne.Location = new System.Drawing.Point(15, 44);
            this.butActionOne.Name = "butActionOne";
            this.butActionOne.Size = new System.Drawing.Size(105, 23);
            this.butActionOne.TabIndex = 4;
            this.butActionOne.Text = "Action One (While)";
            this.butActionOne.UseVisualStyleBackColor = true;
            this.butActionOne.Click += new System.EventHandler(this.butActionOne_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(89, 13);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Designer Content";
            // 
            // butActionTwo
            // 
            this.butActionTwo.Location = new System.Drawing.Point(15, 73);
            this.butActionTwo.Name = "butActionTwo";
            this.butActionTwo.Size = new System.Drawing.Size(105, 23);
            this.butActionTwo.TabIndex = 6;
            this.butActionTwo.Text = "Action Two (Until)";
            this.butActionTwo.UseVisualStyleBackColor = true;
            // 
            // butActionThree
            // 
            this.butActionThree.Location = new System.Drawing.Point(15, 102);
            this.butActionThree.Name = "butActionThree";
            this.butActionThree.Size = new System.Drawing.Size(141, 23);
            this.butActionThree.TabIndex = 7;
            this.butActionThree.Text = "Action Three (Message)";
            this.butActionThree.UseVisualStyleBackColor = true;
            this.butActionThree.Click += new System.EventHandler(this.butActionThree_Click);
            // 
            // FormWaitFor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 262);
            this.Controls.Add(this.butActionThree);
            this.Controls.Add(this.butActionTwo);
            this.Controls.Add(this.lblActionOneResult);
            this.Controls.Add(this.butActionOne);
            this.Controls.Add(this.lblStatus);
            this.Name = "FormWaitFor";
            this.Text = "Wait For";
            this.Load += new System.EventHandler(this.FormWaitFor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblActionOneResult;
        private System.Windows.Forms.Button butActionOne;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button butActionTwo;
        private System.Windows.Forms.Button butActionThree;
    }
}

