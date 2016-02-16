using Ghostice.Core;
using Ghostice.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.ApplicationKit.Example.Models
{
    [WindowDescriptor("ParentWindow", "Name=FormParent")]
    public class ParentFormWindow : WinFormWindowBase
    {

        [ControlDescriptor("Button1", "Name=button1")]
        private PlaceHolder<WinFormButton> _button;

        [ControlDescriptor("Label1", "Name=label1")]
        private PlaceHolder<WinFormLabel> _label
            ;

        [ControlDescriptor("ListBox1", "Name=listBox1")]
        private PlaceHolder<WinFormListBox> _listBox;

        public ParentFormWindow(InterfaceControl Parent)
            : base(Parent)
        {

        }

        public WinFormButton Button
        {
            get
            {
                return _button.GetControl();
            }
        }

        public WinFormLabel Label
        {
            get
            {
                return _label.GetControl();
            }
        }

        public WinFormListBox ListBox
        {
            get
            {
                return _listBox.GetControl();
            }
        }

    }
}
