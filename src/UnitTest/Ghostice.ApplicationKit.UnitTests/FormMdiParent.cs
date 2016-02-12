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
    public partial class FormMdiParent : Form
    {

        FormMdiChild child;

        public FormMdiParent()
        {
            InitializeComponent();

        }

        private void FormMdiParent_Load(object sender, EventArgs e)
        {

            child = new FormMdiChild();

            child.MdiParent = this;

            child.Show();

        }
    }
}
