// -----------------------------------------------------------------------
// <copyright file="NotWindowAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Ghostice.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Ignores Class as Automation Window . This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class NotAutomationWindowAttribute : Attribute
    {

    }
}
