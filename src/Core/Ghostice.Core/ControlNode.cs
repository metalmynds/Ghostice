using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Ghostice.Core
{
    [JsonObject("Control")]
    public class ControlNode
    {

        public ControlNode(String name, String type)
        {
            this.Name = name;
            this.Properties = new Dictionary<String, Object>();
            this.Children = new List<ControlNode>();
            this.Type = type;
        }

        public String Name { get; protected set; }

        public String Type { get; protected set; }

        public Dictionary<String, Object> Properties { get; protected set; }

        public List<ControlNode> Children { get; protected set; }

        public static ControlNode GetControlHierarchy(Control Root, String[] RequestedProperties)
        {
            ControlNode rootNode = null;

            MapControl(Root, RequestedProperties, out rootNode);

            return rootNode;
        }

        private static void MapControl(Control Target, String[] RequestedProperties, out ControlNode MappedControl)
        {

            var ignoreProperties = new String[] { "RightToLeft" };

            var controlNode = new ControlNode(Target.Name, Target.GetType().FullName);

            var properties = Target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

            var propertyList = from property in properties where RequestedProperties == null || RequestedProperties.Length == 0 ? true : RequestedProperties.Contains<String>(property.Name) && !ignoreProperties.Contains<String>(property.Name) select property;

            foreach (var property in propertyList)
            {

                var name = property.Name;

                Object value = null;

                try
                {

                    //value = property.GetValue(Target); // 4.5
                    value = property.GetValue(Target, null); // 4.0

                }
                catch (Exception ex)
                {

                    value = "Exception Getting Value! Error Message: " + ex.Message;

                }

                if (controlNode.Properties.ContainsKey(name))
                {
                    int count = 1;

                    String baseName = name;

                    while (controlNode.Properties.ContainsKey(name))
                    {
                        name = String.Format("{0}_{1}", baseName, count);

                        count++;
                    }
                }

                controlNode.Properties.Add(name, value);

            }

            foreach (Control childControl in Target.Controls)
            {
                ControlNode mappedChildControl = null;

                MapControl(childControl, RequestedProperties, out mappedChildControl);

                controlNode.Children.Add(mappedChildControl);
            }

            MappedControl = controlNode;


        }

        public override string ToString()
        {
            return String.Format("Name: {0} Type: {1} Children: {2}", this.Name, this.Type, this.Children != null ? this.Children.Count : 0);
        }
    }
}