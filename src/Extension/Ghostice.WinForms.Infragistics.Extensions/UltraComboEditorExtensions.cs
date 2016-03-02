using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ghostice.Core;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;

namespace Ghostice.WinForms.Infragistics.Extensions
{
    [ControlExtensionProvider(typeof(UltraComboEditor))]
    public static class UltraComboEditorExtensions
    {
        public static void SelectComboItem(this UltraComboEditor target, String text)
        {

            foreach (var item in target.Items)
            {
                if (item.DisplayText == text)
                {
                    target.SelectedItem = item;
                    break;
                    
                }
            }

        }
    }
}
