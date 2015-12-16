using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ghostice.Framework
{
    public class WinFormCheckBox : WinFormControlBase
    {
        public WinFormCheckBox(InterfaceControl Parent)
            : base(Parent)
        {

        }

        public Boolean Checked
        {
            get { return HandleResult<Boolean>(GetDispatcher().Perform(ActionRequest.Get(this.Path, "Checked"))); }
            set { GetDispatcher().Perform(ActionRequest.Set(this.Path, "Checked", value.ToString(), typeof(Boolean))); }
        }

        public CheckState CheckState
        {
            get { return HandleResult<CheckState>(GetDispatcher().Perform(ActionRequest.Get(this.Path, "CheckState"))); }
            set { GetDispatcher().Perform(ActionRequest.Set(this.Path, "CheckState", value.ToString(), typeof(CheckState))); }
        }

    }
}
