using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ghostice.Core;

namespace Ghostice.Framework
{
    public class WinFormTextBox : WinFormControlBase
    {
        public WinFormTextBox(InterfaceControl Parent)
            : base(Parent)
        {
        }

        public String Text
        {
            get { return HandleResult<String>(GetDispatcher().Perform(ActionRequest.Get(this.Path, "Text"))); }
            set { GetDispatcher().Perform(ActionRequest.Set(this.Path, "Text", value, typeof(String))); }
        }

    }
}
