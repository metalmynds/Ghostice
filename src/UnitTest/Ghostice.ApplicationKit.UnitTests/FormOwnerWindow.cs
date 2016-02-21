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
    public partial class FormOwnerWindow : Form
    {

        FormOwnedWindow _ownedWindow;
        FormUnownedWindow _unOwnedWindow;
         

        public FormOwnerWindow()
        {
            InitializeComponent();

            _ownedWindow = new FormOwnedWindow();

            _unOwnedWindow = new FormUnownedWindow();

        }

        private void FormOwnerWindow_Shown(object sender, EventArgs e)
        {
            _ownedWindow.Show(this);

            _unOwnedWindow.Show();

            Application.DoEvents();
        }

        private void FormOwnerWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
