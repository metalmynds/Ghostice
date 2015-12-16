using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ghostice.ApplicationKit.UnitTests
{
    public partial class FormSimpleUserControl : Form
    {
        public FormSimpleUserControl()
        {
            InitializeComponent();

            var container = this.Container;

            var controls = this.Controls;

            var usercontrl = this.userControlSimple1.Controls;

        }
    }
}
