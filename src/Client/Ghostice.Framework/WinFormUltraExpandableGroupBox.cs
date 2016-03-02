using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ghostice.Core;

namespace Ghostice.Framework
{
    public class WinFormUltraExpandableGroupBox : WinFormControlBase
    {

        public WinFormUltraExpandableGroupBox(InterfaceControl parent)
            : base(parent)
        {
            
        }

        public void Expand()
        {
            this.HandleResult(GetDispatcher().Perform(ActionRequest.Execute(this.Path, "ExpandGroupBox")));
        }

        public void Collapse()
        {
            this.HandleResult(GetDispatcher().Perform(ActionRequest.Execute(this.Path, "CollapseGroupBox")));
        }

    }
}
