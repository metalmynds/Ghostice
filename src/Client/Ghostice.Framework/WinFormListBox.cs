using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Framework
{
    public class WinFormListBox : WinFormControlBase
    {
        public WinFormListBox(InterfaceControl Parent)
            : base(Parent)
        {

        }

        public StringCollection Items
        {
            get { return HandleResult<StringCollection>(GetDispatcher().Perform(ActionRequest.Get(this.Path, "Items"))); }
        }

        public List<String> SelectedItems
        {
            get { return HandleResult<List<String>>(GetDispatcher().Perform(ActionRequest.Execute(this.Path, "GetSelectedItems"))); }
            set { HandleResult(GetDispatcher().Perform(ActionRequest.Execute(this.Path, "SetSelectedItems", new ActionParameter[] { ActionParameter.Create(value.ToArray()) }))); }
        }

        public void ClearSelection()
        {
            SelectedItems = new List<String>();
        }

    }
}
