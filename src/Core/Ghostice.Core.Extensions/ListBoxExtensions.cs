using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ghostice.Core.Extensions
{
    [ControlExtensionProvider(typeof(ListBox))]
    public static class ListBoxExtensions
    {
        public static List<String> GetSelectedItems(this ListBox listBox)
        {
            var selectedItems = new List<String>();

            foreach (var item in listBox.SelectedItems)
            {
                selectedItems.Add(listBox.GetItemText(item));
            }

            return selectedItems;
        }

        public static void SetSelectedItems(this ListBox listBox, String[] selectedItems)
        {

            var index = 0;

            var selectedTextList = new List<String>();

            foreach (var item in listBox.Items)
            {
                selectedTextList.Add(listBox.GetItemText(item));
            }

            foreach (var itemText in selectedTextList)
            {

                listBox.SetSelected(index, selectedItems.Contains(itemText));

                index++;

            }
        }

    }
}
