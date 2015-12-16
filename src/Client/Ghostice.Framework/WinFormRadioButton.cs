using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Framework
{
    public class WinFormRadioButton : WinFormControlBase
    {
        public WinFormRadioButton(InterfaceControl Parent)
            : base(Parent)
        {

        }

        public Boolean Checked
        {
            get { return HandleResult<Boolean>(GetDispatcher().Perform(ActionRequest.Get(this.Path, "Checked"))); }
            set { GetDispatcher().Perform(ActionRequest.Set(this.Path, "Checked", value.ToString(), typeof(Boolean))); }
        }

    }
}
