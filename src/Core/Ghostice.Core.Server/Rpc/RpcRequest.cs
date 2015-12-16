using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core.Server.Rpc
{
    public class RpcRequest
    {
        public string Text { get; private set; }

        public HttpListenerResponse Response { get; private set; }

        public RpcRequest(string text, HttpListenerResponse response)
        {
            this.Text = text;
            this.Response = response;
        }
    }

}
