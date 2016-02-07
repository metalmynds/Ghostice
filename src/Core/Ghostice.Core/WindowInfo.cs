﻿using Newtonsoft.Json;
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

        //[JsonConstructor]
        //public WindowInfo(long handle, String type, String name, String title, String tag)
        //{
        //    this.Handle = handle;
        //    this.Type = type;
        //    this.Name = name;
        //    this.Title = title;
        //    this.Tag = tag;
        //}

        [JsonConstructor]
        public WindowInfo(long handle, String type, String name, String title, String tag, Dictionary<String, String> CustomProperties)
        {
            this.Handle = handle;
            this.Type = type;
            this.Name = name;
            this.Title = title;
            this.Tag = tag;
            this.CustomProperties = CustomProperties;
        }

        public long Handle { get; protected set; }

        public String Type { get; protected set; }

        public String Name { get; protected set; }

        public String Title { get; protected set; }

        public String Tag { get; protected set; }

        public Dictionary<String,String> CustomProperties { get; protected set; }

        public static WindowInfo Create(Control Window)
        {
            if (Window.InvokeRequired)
            {
                return (WindowInfo)Window.Invoke(new UIThreadSafeCreate(Create), new Object[] { Window });
            }
            else
            {
                return new WindowInfo(Window.Handle.ToInt64(), Window.GetType().FullName, Window.Name, Window.Text, Window.Tag == null ? "null" : Window.Tag.ToString(), null);
            }
        }

        public static WindowInfo Create(Control Window, String[] CustomProperties)
        {
            if (Window.InvokeRequired)
            {
                return (WindowInfo)Window.Invoke(new UIThreadSafeCreate(Create), new Object[] { Window });
            }
            else
            {
                Dictionary<String, String> customPropertyValues = new Dictionary<string, string>();

                foreach (var propertyName in CustomProperties)
                {
                    var propertyValue = ReflectionManager.Get(Window, propertyName);

                    customPropertyValues.Add(propertyName, Convert.ToString(propertyValue));
                }

                return new WindowInfo(Window.Handle.ToInt64(), Window.GetType().FullName, Window.Name, Window.Text, Window.Tag == null ? "null" : Window.Tag.ToString(), customPropertyValues);
            }
        }

    }
}
