using Ghostice.Core;
using Newtonsoft.Json.Linq;
using System;

namespace Ghostice.Framework
{
    public abstract class WinFormControlBase : InterfaceControl
    {

        public WinFormControlBase(InterfaceControl Parent)
            : base(Parent)
        {

        }

        public ControlNode PrintTree()
        {

            return HandleResult<ControlNode>(GetDispatcher().Perform(ActionRequest.Map(this.Path, new String[] { "Location", "Size", "TopMost", "Text", "Value", "Selected", "Focused" })));

        }

        public Boolean WaitForReady(int TimeoutSeconds)
        {

            var result =  HandleResult<Boolean>(GetDispatcher().Perform(ActionRequest.Ready(this.Path, TimeoutSeconds)));

            return result;

        }

        public Boolean WaitForReady(int TimeoutSeconds, String value, bool anyValue)
        {

            var result = HandleResult<Boolean>(GetDispatcher().Perform(ActionRequest.Ready(this.Path, TimeoutSeconds, value, anyValue)));

            return result;

        }
    }
}
