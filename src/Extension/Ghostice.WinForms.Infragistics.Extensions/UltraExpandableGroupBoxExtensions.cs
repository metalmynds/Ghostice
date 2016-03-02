using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ghostice.Core;
using Infragistics.Win.Misc;

namespace Ghostice.WinForms.Infragistics.Extensions
{
    [ControlExtensionProvider(typeof(UltraExpandableGroupBox))]
    public static class UltraExpandableGroupBoxExtensions
    {

        public static void ExpandGroupBox(this UltraExpandableGroupBox target)
        {
            target.Expanded = true;
        }

        public static void CollapseGroupBox(this UltraExpandableGroupBox target)
        {
            target.Expanded = false;
        }

    }
}
