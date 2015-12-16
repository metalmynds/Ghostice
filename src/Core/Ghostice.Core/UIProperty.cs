using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ghostice.Core
{
    
    public class UIProperty
    {
        protected UIProperty(String Name, String Value)
        {
            this.PropertyName = Name;
            this.Value = Value;
        }

        public String PropertyName { get; internal set;  }

        public String Value { get; internal set; }

        public static UIProperty Create(String Name, String Value)
        {            
            return new UIProperty(String.Format("{0}", Name), Value);
        }
    }    
}
