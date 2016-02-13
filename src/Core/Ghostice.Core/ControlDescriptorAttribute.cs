using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostice.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class ControlDescriptorAttribute : AutomationDescriptorAttribute
    {
        public ControlDescriptorAttribute(String Name, params String[] ControlProperties)
            : base(DescriptorType.Control, Name, ControlProperties)
        {
        }
    }
}
