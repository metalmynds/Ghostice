using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ghostice.Core;

namespace Ghostice.Framework
{

    public class WinFormUltraComboEditor : WinFormControlBase
    {

        public WinFormUltraComboEditor(InterfaceControl parent)
            : base(parent)
        {
        }

        public void Select(String text)
        {
            this.HandleResult(
                GetDispatcher()
                    .Perform(ActionRequest.Execute(this.Path, "SelectComboItem", ActionParameter.Create(text))));
        }
    }
}
