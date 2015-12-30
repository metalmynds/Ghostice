using System;

namespace Ghostice.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class AutomationDescriptorAttribute : Attribute
    {
        private readonly Descriptor _descriptor;

        public AutomationDescriptorAttribute(String Name, params String[] ControlProperties)
        {
            _descriptor = new Descriptor(ControlProperties);

        }

        public virtual Descriptor Descriptor
        {
            get { return _descriptor; }
        }

    }


}