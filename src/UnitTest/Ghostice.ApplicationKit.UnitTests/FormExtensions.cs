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
    public partial class FormExtensions : Form
    {
        public FormExtensions()
        {
            InitializeComponent();
        }

        public ListView ListView { get { return this.listView1; } }

        public MenuStrip MenuStrip { get { return this.menuStrip1; } }
    }
}
