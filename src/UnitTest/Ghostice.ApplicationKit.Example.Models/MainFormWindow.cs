using Ghostice.Core;
using Ghostice.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.ApplicationKit.Example.Models
{
    [AutomationDescription("MainWindow", "Name=FormMain")]
    public class MainFormWindow : WinFormWindowBase
    {

        [AutomationDescription("HistoryRegistration", "Name=tabctrlAdmin")]
        private PlaceHolder<RegistrationHistoryTabControl> _registrationHistoryTabControl;

        [AutomationDescription("BasketAccessories", "Name=tabctrlSales")]
        private PlaceHolder<BasketAccessoriesTabControl> _basketAccessoriesTabControl;

        [AutomationDescription("MainMenu", "Name=mnustrpMain")]
        private PlaceHolder<WinFormMainMenu> _mainMenu;

        public MainFormWindow(InterfaceControl Parent)
            : base(Parent)
        {

        }

        public RegistrationHistoryTabControl RegistrationHistory
        {
            get
            {
                return _registrationHistoryTabControl.GetControl();
            }
        }

        public BasketAccessoriesTabControl BasketAccessories
        {
            get
            {
                return _basketAccessoriesTabControl.GetControl();
            }
        }

        public WinFormMainMenu MainMenu
        {
            get
            {
                return _mainMenu.GetControl();
            }
        }

        public void RegisterAnimal(String name, String type, String eats, double registrationPrice, String[] rules)
        {
            this.RegistrationHistory.SelectTab(0);

            //this.registrationTab.Control.Select();

            this.RegistrationHistory.RegistrationTab.Name.Text = name;
            //this.name.Control.Text = name;

            //var items = this.type.Control.Items;

            this.RegistrationHistory.RegistrationTab.Type.Text = String.Format("{0}", type);

            //this.type.Control.Text = String.Format("{0}", type);

            this.RegistrationHistory.RegistrationTab.Eats.Text = String.Format("{0}", eats);

            //this.eats.Control.SetValue((String.Format("{0}", eats)));

            this.RegistrationHistory.RegistrationTab.Price.Text = String.Format("{0:0.00}", registrationPrice);

            //this.price.Control.Text = String.Format("{0:0.00}", registrationPrice);

            this.RegistrationHistory.RegistrationTab.Rules.ClearSelection();

            //this.rules.Control.ClearSelection();

            this.RegistrationHistory.RegistrationTab.Rules.SelectedItems = new List<String>(rules);

            //this.rules.Control.Select(rules);

            //this.rules.Control.EnsureVisible();

            this.RegistrationHistory.RegistrationTab.Save.Press();

            //this.save.Control.Click();
        }

        public void ShowHistory()
        {
            this.RegistrationHistory.SelectTab(1);
        }
    }
}
