using Ghostice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{
    public interface IDispatcherHost
    {

        ActionDispatcher GetDispatcher();

    }
}
