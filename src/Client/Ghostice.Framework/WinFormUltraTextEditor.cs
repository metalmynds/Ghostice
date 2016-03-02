using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ghostice.Core;

namespace Ghostice.Framework
{

    public class WinFormUltraTextEditor : WinFormControlBase
    {

        public WinFormUltraTextEditor(InterfaceControl parent)
            : base(parent)
        {
        }

        public String Text
        {
            get { return HandleResult<String>(GetDispatcher().Perform(ActionRequest.Get(this.Path, "Text"))); }
            set { GetDispatcher().Perform(ActionRequest.Set(this.Path, "Text", value, typeof(String))); }
        }

        public void PressDown()
        {
            this.HandleResult(GetDispatcher().Perform(ActionRequest.Execute(this.Path, "PressDownKey")));
        }
    }
}
