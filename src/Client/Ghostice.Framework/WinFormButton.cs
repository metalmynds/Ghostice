using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ghostice.Core;

namespace Ghostice.Framework
{
    public class WinFormButton : WinFormControlBase
    {
        public WinFormButton(InterfaceControl Parent) : base(Parent)
        {
        }

        public void Press()
        {
            this.HandleResult(GetDispatcher().Perform(ActionRequest.Execute(this.Path, "PerformClick")));
        }

    }
}
