using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.ApplicationKit.Rpc
{
    public sealed class HttpRequestEventArgs : EventArgs
    {
        public System.Net.HttpListenerContext Context { get; private set; }

        public HttpRequestEventArgs(HttpListenerContext context)
        {
            this.Context = context;
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }

}
