using Ghostice.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{
    public class ApplicationUnderTest : InterfaceControl, IDispatcherHost

    {
        protected ActionDispatcher _dispatcher;


        public ApplicationUnderTest(ActionDispatcher dispatcher)
            : base(null)
        {
            _dispatcher = dispatcher;
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

        public List<WindowInfo> GetWindows(WindowInfo parent)
        {

            var listRequest = ActionRequest.List(parent);

            var result = _dispatcher.Perform(listRequest);

            var dserialised = JsonConvert.DeserializeObject<List<WindowInfo>>(result.ReturnValue);

            return dserialised;

        }

        public List<WindowInfo> GetWindows(String[] additionalProperties)
        {

            var listRequest = ActionRequest.List(additionalProperties);

            var result = _dispatcher.Perform(listRequest);

            var dserialised = JsonConvert.DeserializeObject<List<WindowInfo>>(result.ReturnValue);

            return dserialised;

        }

        public List<WindowInfo> GetWindows(WindowInfo parent, String[] additionalProperties)
        {

            var listRequest = ActionRequest.List(parent, additionalProperties);

            var result = _dispatcher.Perform(listRequest);

            var dserialised = JsonConvert.DeserializeObject<List<WindowInfo>>(result.ReturnValue);

            return dserialised;

        }

        public ControlNode Map(Locator Target)
        {
            return HandleResult<ControlNode>(GetDispatcher().Perform(ActionRequest.Map(Target, new String[] { "Location", "Size", "TopMost", "Text", "Value", "Selected", "Focused" })));
        }

        //public JObject Tell(Locator Target)
        //{
        //    var tellRequest = ActionRequest.Tell(Target);

        //    var result = _dispatcher.Perform(tellRequest);

        //    var dserialised = JsonConvert.DeserializeObject<JObject>(result.ReturnValue);

        //    return dserialised;
        //}
    }
}
