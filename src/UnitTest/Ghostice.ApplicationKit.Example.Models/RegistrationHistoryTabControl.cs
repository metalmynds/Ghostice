using Ghostice.Core;
using Ghostice.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.ApplicationKit.Example.Models
{
    public class RegistrationHistoryTabControl : WinFormTabControlBase
    {

        [ControlDescriptor("RegistrationTab", "Name=tabpgRegistration")]
        private PlaceHolder<RegistrationTabPage> registrationTab;

        [ControlDescriptor("HistoryTab", "Name=tabpgHistory")]
        private PlaceHolder<HistoryTabPage> historyTab;

       
        public RegistrationHistoryTabControl(InterfaceControl Parent)
            : base(Parent)
        {

        }

        public RegistrationTabPage RegistrationTab
        {
            get
            {
                return registrationTab.GetControl();
            }
        }

        public HistoryTabPage HistoryTab
        {
            get
            {
                return historyTab.GetControl();
            }
        }

    }
}
