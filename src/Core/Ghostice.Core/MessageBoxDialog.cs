using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostice.Core
{
    public class MessageBoxDialog : InterfaceControl
    {

        public MessageBoxDialog(ApplicationUnderTest parent)
            : base(parent)
        {
        }

        public String Text
        {
            get { return HandleResult<String>(GetDispatcher().Perform(ActionRequest.Get(this.Path, "Text"))); }            
        }

        public String Caption
        {
            get { return HandleResult<String>(GetDispatcher().Perform(ActionRequest.Get(this.Path, "Caption"))); }
        }

        public void Press(String button)
        {
            this.HandleResult(GetDispatcher().Perform(ActionRequest.Execute(this.Path, "PressButton", ActionParameter.Create(button))));
        }

        public String[] Buttons
        {
            get { return HandleResult<String[]>(GetDispatcher().Perform(ActionRequest.Get(this.Path, "Buttons"))); }
        }
    }
}
