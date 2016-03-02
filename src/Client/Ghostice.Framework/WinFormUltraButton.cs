using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ghostice.Core;

namespace Ghostice.Framework
{
    public class WinFormUltraButton : WinFormControlBase
    {

        public WinFormUltraButton(InterfaceControl parent)
            : base(parent)
        {
            
        }

        public void Click()
        {
            this.HandleResult(
                GetDispatcher()
                    .Perform(ActionRequest.Execute(this.Path, "ClickButton")));
        }

    }
}
