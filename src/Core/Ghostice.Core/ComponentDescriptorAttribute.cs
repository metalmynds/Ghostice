using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostice.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ComponentDescriptorAttribute : AutomationDescriptorAttribute
    {
        public ComponentDescriptorAttribute(DescriptorType Type, String Name, params String[] ControlProperties)
            : base(DescriptorType.Component, Name, ControlProperties)
        {
        }
    }
}
