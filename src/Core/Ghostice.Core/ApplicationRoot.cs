using Ghostice.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{
    public class ApplicationRoot : InterfaceControl, IDispatcherHost

    {
        protected ActionDispatcher _dispatcher;


        public ApplicationRoot(ActionDispatcher Dispatcher)
            : base(null)
        {
            _dispatcher = Dispatcher;
        }


        public new ActionDispatcher GetDispatcher()
        {
            return _dispatcher;
        }

        public List<WindowInfo> GetWindows()
        {

            var listRequest = ActionRequest.List();

            var result = _dispatcher.Perform(listRequest);

            var dserialised = JsonConvert.DeserializeObject<List<WindowInfo>>(result.ReturnValue);

            return dserialised;

        }

    }
}
