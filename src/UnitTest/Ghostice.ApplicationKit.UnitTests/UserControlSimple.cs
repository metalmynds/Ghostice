using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ghostice.ApplicationKit.UnitTests
{
    public partial class UserControlSimple : UserControl
    {
        public UserControlSimple()
        {
            InitializeComponent();

            AnIdentifier = Guid.NewGuid();
        }

        public Guid AnIdentifier { get; protected set; }
    }
}
