using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{
    [Serializable] 
    public class PropertyCollection
    {
        private List<Property> _properties = new List<Property>();

        public PropertyCollection()
        {

        }

        public PropertyCollection(params Property[] properties)
        {
            _properties.AddRange(properties);
        }

        public List<Property> List
        {
            get
            {
                return _properties;
            }
        }

        public Boolean HasProperty(String Name)
        {
            foreach (var property in _properties)
            {
                if (property.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase)) return true;
            }

            return false;
        }

    }
}
