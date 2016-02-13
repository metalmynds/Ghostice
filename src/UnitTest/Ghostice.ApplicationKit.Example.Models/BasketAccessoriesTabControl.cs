using Ghostice.Core;
using Ghostice.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.ApplicationKit.Example.Models
{
    public class BasketAccessoriesTabControl : WinFormTabControlBase
    {

        [AutomationDescriptor(DescriptorType.Control, "BasketTab", "Name=tabpgeBasket")]
        private PlaceHolder<BasketTabPage> basketTab;


        public BasketAccessoriesTabControl(InterfaceControl Parent)
            : base(Parent)
        {

        }

        public BasketTabPage BasketTab
        {
            get
            {
                return basketTab.GetControl();
            }
        }


    }
}
