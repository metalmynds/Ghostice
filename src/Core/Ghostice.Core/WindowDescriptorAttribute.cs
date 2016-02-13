using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghostice.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WindowDescriptorAttribute : AutomationDescriptorAttribute
    {
        public WindowDescriptorAttribute(String Name, params String[] ControlProperties)
            : base(DescriptorType.Window, Name, ControlProperties)
        {
        }

    }
}
