using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ghostice.Core;
using Infragistics.Win.UltraWinEditors;

namespace Ghostice.WinForms.Infragistics.Extensions
{
    [ControlExtensionProvider(typeof(UltraCheckEditor))]
    public static class UltraCheckEditorExtensions
    {

        public static void CheckBox(this UltraCheckEditor target)
        {
            target.Checked = true;
        }

        public static void UncheckBox(this UltraCheckEditor target)
        {
            target.Checked = false;
        }

    }
}
