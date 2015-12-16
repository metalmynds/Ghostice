using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{

    [Serializable]
    public class ControlDescription
    {

        [JsonConstructor]
        public ControlDescription(List<Property> Properties)
        {
            this.Properties = Properties;
        }

        public ControlDescription()
        {
            Properties = new List<Property>();
        }

        public ControlDescription(params Property[] List)
        {
            Properties = new List<Property>();

            foreach (var property in List)
            {
                Properties.Add(property);
            }
        }

        public ControlDescription(params String[] KeyValueList)
        {
            Properties = new List<Property>();

            foreach (var keyValue in KeyValueList)
            {
                Properties.Add(Property.Create(keyValue));
            }
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

        public List<Property> Properties
        {
            get;
            protected set;
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
                if (property.Name.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase)) {
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

            return String.Format("Descriptor[{0}]", propertyBuilder.ToString());
        }
    }
}
