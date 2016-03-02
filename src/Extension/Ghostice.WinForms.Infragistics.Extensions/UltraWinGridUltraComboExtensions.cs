using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ghostice.Core;

using Infragistics.Win.UltraWinGrid;

namespace Ghostice.WinForms.Infragistics.Extensions
{
    [ControlExtensionProvider(typeof(UltraCombo))]
    public static class UltraWinGridUltraComboExtensions
    {

        public static void SelectListItem(this UltraCombo target, String column, String name)
        {

            target.PerformAction(UltraComboAction.Dropdown);

            var tableRows = target.Rows.All;

            //foreach (var row in target.Rows)
            //{
            //    row.GetCellValue() 
            //}

            foreach (UltraGridRow row in tableRows)
            {
                

                foreach (var cell in row.Cells)
                {

                    var caption = cell.Column.Header.Caption;

                    if (caption != null)
                    {


                        if (caption.Equals(column, StringComparison.InvariantCultureIgnoreCase))
                        {


                            if (Convert.ToString(cell.Value) == name)
                            {

                                target.SelectedRow = row;
                                break;

                            }
                        }

                    }

                }

            }

            target.PerformAction(UltraComboAction.CloseDropdown);
        }
    }

}

