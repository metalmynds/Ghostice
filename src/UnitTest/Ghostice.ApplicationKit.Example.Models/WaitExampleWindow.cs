using Ghostice.Core;
using Ghostice.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostice.ApplicationKit.Example.Models
{
    [WindowDescriptor("Example", "Name=FormWaitFor")]
    public class WaitExampleWindow : WinFormWindowBase
    {

        [ControlDescriptor("Action One List", "Name=butActionOne")]
        private PlaceHolder<WinFormButton> _actionOneButton;

        [ControlDescriptor("Action Three Messagebox", "Name=butActionThree")]
        private PlaceHolder<WinFormButton> _actionThreeButton;

        [ControlDescriptor("Action One Result", "Name=lblActionOneResult")]
        private PlaceHolder<WinFormLabel> _actionOneResult;

        [ControlDescriptor("Action One Status", "Name=lblStatus")]
        private PlaceHolder<WinFormLabel> _status;

        public WaitExampleWindow(InterfaceControl parent)
            : base(parent)
        {

        }

        public WinFormButton ActionOne
        {
            get { return _actionOneButton.GetControl(); }
        }

        public WinFormButton ActionThree
        {
            get { return _actionThreeButton.GetControl(); }
        }

        public WinFormLabel ActionOneResult
        {
            get { return _actionOneResult.GetControl(); }
        }

        public WinFormLabel Status
        {
            get { return _status.GetControl(); }
        }

    }
}
