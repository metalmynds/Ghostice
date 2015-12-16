using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Framework
{
    public class WinFormComboBox : WinFormControlBase
    {
        public WinFormComboBox(InterfaceControl Parent) 
            : base(Parent)
        {
        }

        public String Text
        {
            get { return HandleResult<String>(GetDispatcher().Perform(ActionRequest.Get(this.Path, "Text"))); }
            set { GetDispatcher().Perform(ActionRequest.Set(this.Path, "Text", value, typeof(String))); }
        }

        public virtual void SelectItem(int Index)
        {
            this.HandleResult(GetDispatcher().Perform(
                                ActionRequest.Set(this.Path, "SelectedIndex", Index.ToString(), typeof(int))));
        }

        public virtual void SelectItem(string Item)
        {
            this.HandleResult(GetDispatcher().Perform(
                                ActionRequest.Set(this.Path, "SelectedItem", Item, typeof(String))));
        }


    }
}
