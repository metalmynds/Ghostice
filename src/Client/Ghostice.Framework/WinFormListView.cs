using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Framework
{
    public class WinFormListView : WinFormControlBase
    {

        public WinFormListView(InterfaceControl Parent)
            : base(Parent)
        {

        }

        public StringCollection Columns
        {
            get
            {
                return HandleResult<StringCollection>(GetDispatcher().Perform(
                    ActionRequest.Execute(this.Path, "GetColumns")
                ));
            }
        }

    }
}
