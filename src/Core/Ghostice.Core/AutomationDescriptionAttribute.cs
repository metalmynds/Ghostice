using System;

namespace Ghostice.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class AutomationDescriptionAttribute : Attribute
    {
        private readonly ControlDescription _descriptor;

        public AutomationDescriptionAttribute(String Name, params String[] ControlProperties)
        {
            _descriptor = new ControlDescription(ControlProperties);

        }

        public virtual ControlDescription Descriptor
        {
            get { return _descriptor; }
        }

    }


}