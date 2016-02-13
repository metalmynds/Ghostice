using System;

namespace Ghostice.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class AutomationDescriptorAttribute : Attribute
    {
        private readonly Descriptor _descriptor;

        public AutomationDescriptorAttribute(DescriptorType Type, String Name, params String[] ControlProperties)
        {
            _descriptor = new Descriptor(Type, ControlProperties);

        }

        public virtual Descriptor Descriptor
        {
            get { return _descriptor; }
        }

    }


}