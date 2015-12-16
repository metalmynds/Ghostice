using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Framework
{
    public class WinFormLabel : WinFormControlBase
    {
        public WinFormLabel(InterfaceControl Parent) : base(Parent)
        {
        }

        public String Text
        {
            get { return HandleResult<String>(GetDispatcher().Perform(ActionRequest.Get(this.Path, "Text"))); }
            set { GetDispatcher().Perform(ActionRequest.Set(this.Path, "Text", value, typeof(String))); }
        }

    }
}
