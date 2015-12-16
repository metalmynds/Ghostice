using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{
    public class ApplicationManagerSponsor : MarshalByRefObject, ISponsor
    {
        private readonly TimeSpan _renewalWindow;

        public ApplicationManagerSponsor(TimeSpan RenewalWindow)
        {
            _renewalWindow = RenewalWindow;
        }

        public TimeSpan Renewal(ILease lease)
        {
            return _renewalWindow;
        }

    }
}
