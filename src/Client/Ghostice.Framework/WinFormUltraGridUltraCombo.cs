using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ghostice.Core;

namespace Ghostice.Framework
{
    public class WinFormUltraGridUltraCombo : WinFormControlBase
    {
        public WinFormUltraGridUltraCombo(InterfaceControl parent)
            : base(parent)
        {
        }

        public void Select(String column, String name)
        {
            this.HandleResult(GetDispatcher().Perform(ActionRequest.Execute(this.Path, "SelectListItem", ActionParameter.Create(column), ActionParameter.Create(name))));
        }

    }
}
