// -----------------------------------------------------------------------
// <copyright file="IHoldPlace.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Ghostice.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IHoldPlace
    {
        String WellKnownAs { get; }
        Boolean IsWellKnown { get; }
        ControlDescription Descriptor { get; }
        Type HeldType { get; }
        Object GetObject();
        InterfaceControl Parent { get; }
        void Forget();
        Object Window { get; }
    }
}
