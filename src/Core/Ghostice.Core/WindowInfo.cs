using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ghostice.Core
{
    public class WindowInfo
    {

        delegate WindowInfo UIThreadSafeCreate(Control Window);

        [JsonConstructor]
        public WindowInfo(long handle, String type, String name, String title, String tag)
        {
            this.Handle = handle;
            this.Type = type;
            this.Name = name;
            this.Title = title;
            this.Tag = tag;
        }

        public long Handle { get; protected set; }

        public String Type { get; protected set; }

        public String Name { get; protected set; }

        public String Title { get; protected set; }

        public String Tag { get; protected set; }

        public static WindowInfo Create(Control Window)
        {
            if (Window.InvokeRequired)
            {
                return (WindowInfo)Window.Invoke(new UIThreadSafeCreate(Create), new Object[] { Window });
            }
            else
            {
                return new WindowInfo(Window.Handle.ToInt64(), Window.GetType().FullName, Window.Name, Window.Text, Window.Tag == null ? "null" : Window.Tag.ToString());
            }
        }
    }
}
