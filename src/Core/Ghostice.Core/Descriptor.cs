using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{

    public enum DescriptorType
    {
        Unknown,
        Window,
        Component,
        Control,
        Custom
    }

    [Serializable]
    public class Descriptor
    {
        // Required for Serialisation
        public Descriptor()
        {

        }

        [JsonConstructor]
        public Descriptor(DescriptorType Type, List<Property> Properties)
        {
            this.Properties = Properties;
            this.Type = Type;
        }

        public Descriptor(DescriptorType Type)
        {
            Properties = new List<Property>();
            this.Type = Type;
        }

        public Descriptor(DescriptorType Type, params Property[] List)
        {
            this.Type = Type;
            Properties = new List<Property>();

            foreach (var property in List)
            {
                Properties.Add(property);
            }
        }

        public Descriptor(DescriptorType Type, params String[] KeyValueList)
        {
            this.Type = Type;
            Properties = new List<Property>();

            foreach (var keyValue in KeyValueList)
            {
                Properties.Add(Property.Create(keyValue));
            }
        }

        public static Descriptor Window(params String[] KeyValueList)
        {
            return new Descriptor(DescriptorType.Window, KeyValueList);
        }

        public static Descriptor Control(params String[] KeyValueList)
        {
            return new Descriptor(DescriptorType.Control, KeyValueList);
        }

        public static Descriptor Component(params String[] KeyValueList)
        {
            return new Descriptor(DescriptorType.Component, KeyValueList);
        }

        [JsonIgnore]
        public String[] RequiredProperties
        {
            get
            {
                var names = new List<String>();

                foreach (var property in Properties)
                {
                    names.Add(property.Name);
                }

                return names.ToArray();
            }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public DescriptorType Type
        {
            get;
            set;
        }

        public List<Property> Properties
        {
            get;
            set;
        }

        public Property GetProperty(String Name)
        {
            foreach (var property in Properties)
            {
                if (property.Name.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return property;
                }
            }

            return null;
        }

        public Boolean ContainsProperty(String PropertyName)
        {
            foreach (var property in Properties)
            {
                if (property.Name.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            var propertyBuilder = new StringBuilder();

            List<String> propertyClauses = new List<string>();

            foreach (var property in Properties)
            {
                propertyClauses.Add(property.ToString());
            }

            propertyBuilder.Append(String.Join(", ", propertyClauses));

            return String.Format("{0}[{1}]", Type.ToString(), propertyBuilder.ToString());
        }
    }
}
