using Ghostice.Core;
using Ghostice.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.ApplicationKit.Example.Models
{
    public class RegistrationTabPage : WinFormControlBase
    {

        [AutomationDescription("RegistrationName", "Name=txtName")]
        private PlaceHolder<WinFormTextBox> registrationName;

        [AutomationDescription("RegistrationType", "Name=cmbType")]
        private PlaceHolder<WinFormComboBox> registrationType;

        [AutomationDescription("RegistrationEats", "Name=cmbEats")]
        private PlaceHolder<WinFormComboBox> registrationEats;

        [AutomationDescription("RegistrationPrice", "Name=txtPrice")]
        private PlaceHolder<WinFormTextBox> registrationPrice;

        [AutomationDescription("RegistrationRules", "Name=lstRules")]
        private PlaceHolder<WinFormListBox> registrationRules;

        [AutomationDescription("SaveRegistration", "Name=butSave")]
        private PlaceHolder<WinFormButton> buttonSave;        

        public RegistrationTabPage(InterfaceControl Parent)
            : base(Parent)
        {


        }

        public WinFormTextBox Name
        {
            get
            {
                return registrationName.GetControl();
            }
        }

        public WinFormComboBox Type
        {
            get
            {
                return registrationType.GetControl();
            }
        }
        public WinFormComboBox Eats
        {
            get
            {
                return registrationEats.GetControl();
            }
        }

        public WinFormTextBox Price
        {
            get
            {
                return registrationPrice.GetControl();
            }
        }

        public WinFormListBox Rules
        {
            get
            {
                return registrationRules.GetControl();
            }
        }

        public WinFormButton Save
        {
            get
            {
                return buttonSave.GetControl();
            }
        }
    }
}
