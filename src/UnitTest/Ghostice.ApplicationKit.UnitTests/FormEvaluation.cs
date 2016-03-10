using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ghostice.ApplicationKit.UnitTests
{
    public partial class FormEvaluation : Form
    {
        public FormEvaluation()
        {
            InitializeComponent();
          
        }

        private void FormEvaluation_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Shiney";
            lblStatus.TextAlign = ContentAlignment.TopLeft;
        }
    }
}
