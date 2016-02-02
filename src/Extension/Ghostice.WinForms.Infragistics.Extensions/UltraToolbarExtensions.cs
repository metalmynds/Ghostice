using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ghostice.Core;
using Infragistics.Win.UltraWinToolbars;

namespace Ghostice.WinForms.Infragistics.Extensions
{
    [ControlExtensionProvider(typeof(UltraToolbarsManager))]
    public static class UltraToolbarExtensions
    {

        public static Boolean ToolExists(this UltraToolbarsManager manager, String Name)
        {
            var button = manager.Tools[Name];

            if (button != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean PerformToolButtonClick(this UltraToolbarsManager manager, String Name)
        {
            var button = manager.Tools[Name] as ButtonTool;

            if (button != null)
            {
                ReflectionHelper.ExecuteMethod(button, "DoDefaultAction", null);

                return true;

            }
            else
            {
                return false;
            }
        }

    }
}
