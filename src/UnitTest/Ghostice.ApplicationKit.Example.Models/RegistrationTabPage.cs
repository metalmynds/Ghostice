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

        [ControlDescriptor("RegistrationName", "Name=txtName")]
        private PlaceHolder<WinFormTextBox> registrationName;

        [ControlDescriptor("RegistrationType", "Name=cmbType")]
        private PlaceHolder<WinFormComboBox> registrationType;

        [ControlDescriptor("RegistrationEats", "Name=cmbEats")]
        private PlaceHolder<WinFormComboBox> registrationEats;

        [ControlDescriptor("RegistrationPrice", "Name=txtPrice")]
        private PlaceHolder<WinFormTextBox> registrationPrice;

        [ControlDescriptor("RegistrationRules", "Name=lstRules")]
        private PlaceHolder<WinFormListBox> registrationRules;

        [ControlDescriptor("SaveRegistration", "Name=butSave")]
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
