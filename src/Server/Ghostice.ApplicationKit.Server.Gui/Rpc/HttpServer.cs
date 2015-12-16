using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.ApplicationKit.Rpc
{
    public class HttpServer
    {

        protected delegate void HttpRequestEventHandler(object sender, HttpRequestEventArgs e);
        protected event HttpRequestEventHandler OnHttpRequest;

        public HttpServer(Uri url)
        {
            this.Url = url;

        }

        public Uri Url
        {
            get;
            protected set;
        }

        public void Listen()
        {
            var listener = new HttpListener();

            if (!HttpListener.IsSupported)

                throw new NotSupportedException("HttpListener not supported. Switch to mono provided one.");

            try
            {

                var address = this.Url.ToString();

                listener.Prefixes.Add(address.EndsWith("/") ? address : address += "/");

                listener.Start();


            }
            catch (Exception ex)
            {
                throw new HttpServerStartupFailedException(this.Url.ToString(), ex);
            }

            var tcs = new TaskCompletionSource<object>();

            listener.GetContextAsync().ContinueWith(async t =>
            {
                try
                {
                    while (true)
                    {
                        var context = await t;
                        this.HttpRequestRecieved(new HttpRequestEventArgs(context));
                        t = listener.GetContextAsync();
                    }
                }
                catch (Exception e)
                {
                    listener.Close();
                    tcs.TrySetException(e);
                }
            });
        }
        protected virtual void HttpRequestRecieved(HttpRequestEventArgs e)
        {
            var handler = OnHttpRequest;
            if (handler != null)
                handler(this, e);
        }
    }

    [Serializable]
    public class HttpServerStartupFailedException : Exception
    {

        protected HttpServerStartupFailedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }


        public HttpServerStartupFailedException(String Url, Exception InnerException)
            : base(String.Format("RPC Http Server Start-up Failed!\r\nEndPoint Url: {0}\r\nError: {1}", Url, InnerException.Message), InnerException)
        {

        }
    }

}
