using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostice.Framework
{
    public class WinFormUltraToolbar : WinFormWindowBase
    {

        public WinFormUltraToolbar(InterfaceControl Parent)
            : base(Parent)
        {

        }

        //public Boolean IsToolEnabled(String Key)
        //{

        //}

        //public Boolean IsToolVisible(String Key)
        //{

        //}

        public void Press(String Key)
        {
            this.HandleResult(GetDispatcher().Perform(ActionRequest.Execute(this.Path, "PerformToolButtonClick", new ActionParameter[] { ActionParameter.Create(Key) })));
        }
    }

}
