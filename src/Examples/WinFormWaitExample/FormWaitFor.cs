using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormWaitExample
{


    public partial class FormWaitFor : Form
    {
        Timer tmrActionOneTimer;
        Timer tmrActionThreeTimer;

        public FormWaitFor()
        {
            InitializeComponent();

            tmrActionOneTimer = new Timer();
            tmrActionOneTimer.Interval = 2000;
            tmrActionOneTimer.Tick += ActionOneTimer_Tick;

            tmrActionThreeTimer = new Timer();
            tmrActionThreeTimer.Interval = 2000;
            tmrActionThreeTimer.Tick += ActionThreeTimer_Tick;

        }

        private void ActionThreeTimer_Tick(object sender, EventArgs e)
        {

            tmrActionThreeTimer.Stop();

            MessageBox.Show("Delayed Action Message", "Action Message Box");
            
        }

        private void ActionOneTimer_Tick(object sender, EventArgs e)
        {
            lblActionOneResult.Text = "Action Completed at " + DateTime.Now.ToLongTimeString();

            tmrActionOneTimer.Stop();
        }

        private void FormWaitFor_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Shiney";
            lblStatus.TextAlign = ContentAlignment.TopLeft;
            lblActionOneResult.Text = "Not Done";

        }

        private void butActionOne_Click(object sender, EventArgs e)
        {
            tmrActionOneTimer.Start();
        }

        public void ClickActionOne()
        {
            butActionOne.PerformClick();
        }

        private void butActionThree_Click(object sender, EventArgs e)
        {
            tmrActionThreeTimer.Start();
        }
    }
}
