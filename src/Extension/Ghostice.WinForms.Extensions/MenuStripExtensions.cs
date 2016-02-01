using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ghostice.WinForms.Extensions
{
    [ControlExtensionProvider(typeof(MenuStrip))]
    public static class MenuStripExtensions
    {

        public static Boolean PerformClickMenu(this MenuStrip mainMenu, String path)
        {
            String[] menuItemNames = path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

            ToolStripMenuItem current = null;

            foreach (var menuItemName in menuItemNames)
            {

                if (current == null)
                {
                    current = (from ToolStripMenuItem rootItem in mainMenu.Items where rootItem.Text.Equals(menuItemName, StringComparison.InvariantCultureIgnoreCase) select rootItem).FirstOrDefault<ToolStripMenuItem>();

                    if (current == null)
                    {
                        throw new SelectMenuItemFailedException(String.Format("Select Root Menu Item Failed!\r\nMenuItem: [{0}]\r\nFull Path: {1}", menuItemName, path));
                    }
                }
                else
                {
                    if (current.HasDropDownItems)
                    {

                        foreach (var dropItem in current.DropDownItems)
                        {
                            var subItem = dropItem as ToolStripMenuItem;

                            if (subItem != null)
                            {
                                if (subItem.Text.Equals(menuItemName, StringComparison.InvariantCultureIgnoreCase))
                                {

                                    current = subItem;

                                }
                            }
                        }

                    }
                    else
                    {
                        throw new SelectMenuItemFailedException(String.Format("Select Drop Menu Item Failed!\r\nMenuItem: [{0}]\r\nFull Path: {1}", menuItemName, path));
                    }

                }

            }

            if (current != null)
                current.PerformClick();

            return current != null;
        }
    }

    [Serializable]
    public class SelectMenuItemFailedException : Exception
    {

        protected SelectMenuItemFailedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public SelectMenuItemFailedException(String message)
            : base(message)
        {

        }

        public SelectMenuItemFailedException(String message, Exception innerException)
            : base(message, innerException)
        {

        }

    }

}