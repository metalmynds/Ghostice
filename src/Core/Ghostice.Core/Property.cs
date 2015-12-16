using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{
    [Serializable]
    public class Property
    {

        public Property()
        {
            // Required for Serialisation 
        }

        public Property(String Name)
        {
            this.Name = Name;
        }

        [JsonConstructor]
        public Property(String Name, String Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        public static Property Create(String Expresion)
        {
            var parts = Expresion.Split(new char[] { '=' }, 2);

            if (parts.Length != 2)
            {
                throw new ArgumentException("Create Property Failed! Expression is Not Valid Format e.g. Name=FormMain!\r\nExpression: {0}", Expresion);
            }

            return Create(parts[0], parts[1]);
        }

        public static Property Create(String Name, String Value)
        {
            return new Property(Name, Value);
        }

        public String Name { get; set; }

        public String Value { get; set; }


        public override string ToString()
        {
            return String.Format("{0}={1}", Name, Value);
        }
    }
}
