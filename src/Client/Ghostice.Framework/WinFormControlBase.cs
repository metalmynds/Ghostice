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

        public String PrintTree()
        {

            return HandleResult<JObject>(GetDispatcher().Perform(ActionRequest.Map(this.Path, new String[] { "Location", "Size", "TopMost", "Text", "Value", "Selected", "Focused" }))).ToString();

        }

        public Boolean WaitForReady(int TimeoutSeconds)
        {

            var result =  HandleResult<Boolean>(GetDispatcher().Perform(ActionRequest.Ready(this.Path, TimeoutSeconds)));

            return result;

        }
    }
}
