using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ghostice.Core;
using Infragistics.Win.Misc;

namespace Ghostice.WinForms.Infragistics.Extensions
{
    [ControlExtensionProvider(typeof(UltraButton))]
    public static class UltraButtonExtensions
    {

        public static void ClickButton(this UltraButton target)
        {
            target.Select();
            target.PerformClick();
        }

    }
}
