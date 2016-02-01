using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ghostice.WinForms.Extensions
{
    [ControlExtensionProvider(typeof(ListView))]
    public static class ListViewExtensions
    {

        public static List<String> GetColumns(this ListView listView)
        {
            var columns = new List<String>();

            foreach (ColumnHeader column in listView.Columns) {
                columns.Add(column.Text);
            }

            return columns;
        }

    }
}
