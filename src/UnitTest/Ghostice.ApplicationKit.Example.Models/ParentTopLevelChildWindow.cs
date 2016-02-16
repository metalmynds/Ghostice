using Ghostice.Core;
using Ghostice.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.ApplicationKit.Example.Models
{
    [WindowDescriptor("ParentWindow", "Name=FormTopLevelChild")]
    public class ParentTopLevelChildWindow : WinFormWindowBase
    {

        [ControlDescriptor("ListBox1", "Name=listView1")]
        private PlaceHolder<WinFormListView> _listView;

        [ControlDescriptor("TextBox1", "Name=textBox1")]
        private PlaceHolder<WinFormTextBox> _textBox;

        public ParentTopLevelChildWindow(InterfaceControl Parent)
            : base(Parent)
        {

        }

        public WinFormListView ListView
        {
            get
            {
                return _listView.GetControl();
            }
        }

        public WinFormTextBox TextBox
        {
            get
            {
                return _textBox.GetControl();
            }
        }
        
    }
}
