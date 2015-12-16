using Ghostice.Core;
using Ghostice.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.ApplicationKit.Example.Models
{
    public class AccessoriesTabPage : WinFormControlBase
    {

        [AutomationDescription("Accessories", "Name=lstAccessories")]
        private PlaceHolder<WinFormListBox> accessoriesList;

        public AccessoriesTabPage(InterfaceControl Parent)
            : base(Parent)
        {

        }

        public WinFormListBox Accessories
        {
            get
            {
                return accessoriesList.GetControl();
            }
        }
    }
}
