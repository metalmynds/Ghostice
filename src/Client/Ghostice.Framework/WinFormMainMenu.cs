using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Framework
{
    public class WinFormMainMenu : WinFormControlBase
    {

        public WinFormMainMenu(InterfaceControl Parent)
            : base(Parent)
        {

        }

        public void Click(String Path)
        {
            this.HandleResult(GetDispatcher().Perform(
                    ActionRequest.Execute(this.Path, "PerformClickMenu", new ActionParameter[] { ActionParameter.Create(Path) })
                ));
        }


    }
}
