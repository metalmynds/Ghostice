using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ghostice.Core;

namespace Ghostice.Framework
{

    public class WinFormUltraCheckEditor : WinFormControlBase
    {

        public WinFormUltraCheckEditor(InterfaceControl parent)
            : base(parent)
        {
        }

        public void Check()
        {
            this.HandleResult(
                GetDispatcher()
                    .Perform(ActionRequest.Execute(this.Path, "CheckBox")));
        }

        public void Uncheck()
        {
            this.HandleResult(
                GetDispatcher()
                    .Perform(ActionRequest.Execute(this.Path, "UncheckBox")));
        }

    }
}
