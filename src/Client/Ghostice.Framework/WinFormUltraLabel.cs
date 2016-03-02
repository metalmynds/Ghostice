using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ghostice.Core;

namespace Ghostice.Framework
{
    public class WinFormUltraLabel: WinFormControlBase
    {
        public WinFormUltraLabel(InterfaceControl Parent) : base(Parent)
        {
        }

        public String Text
        {
            get { return HandleResult<String>(GetDispatcher().Perform(ActionRequest.Get(this.Path, "Text"))); }
            set { GetDispatcher().Perform(ActionRequest.Set(this.Path, "Text", value, typeof(String))); }
        }

    }
    
}
