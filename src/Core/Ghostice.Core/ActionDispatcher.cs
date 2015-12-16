using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Ghostice.Core;
using System.Reactive.Concurrency;
using System.Threading;
using Newtonsoft.Json;
using AustinHarris.JsonRpc;

namespace Ghostice.Core
{
    public class ActionDispatcher
    {
        protected JsonRpcClient _rpcClient;

        public ActionDispatcher(JsonRpcClient Client)
        {
            _rpcClient = Client;
        }

        public ActionResult Perform(ActionRequest Request)
        {            

            AutoResetEvent completed = new AutoResetEvent(false);

            Exception exception = null;

            ActionResult result = null;

            String error = null;

            //var request = Request.ToJson();

            var callback = _rpcClient.Invoke<ActionResult>("Perform", Request, Scheduler.Default);

            using (callback.Subscribe(
                onNext: (response) =>
                {
                    //System.Diagnostics.Debug.WriteLine(response);
                    error = response.Error != null ? response.Error.data.ToString() : null;

                    result = response.Result;
                },
                onError: (ex) =>
                {
                    exception = ex;

                    completed.Set();
                },
                onCompleted:
                    () =>
                    {
                        completed.Set();
                    }
                ))
            {
                completed.WaitOne();
            }

            if (exception != null) throw new DispatcherException(String.Format("ActionDispatcher Perform Failed!\r\nRequest:\r\n{0}\r\nError: {1}\r\nStack Trace:\r\n{2}", Request.ToString(), exception.Message, exception.StackTrace), exception);

            if (error != null)
            {
                //throw new GhosticeClientException(String.Format("Ghostice Start Application Failed!\r\nApplication Path: {0}\r\nArguments: {1}\r\nResponse:\r\n{2}", ApplicationPath, Arguments, result.Error.data));
                throw new DispatcherException(error);
            }


            return result;
        }

    }

    [Serializable]
    public class DispatcherException : Exception
    {

        protected DispatcherException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }


        public DispatcherException(String Message)
            : base(Message)
        {

        }

        public DispatcherException(String Message, Exception Inner)
            : base(Message, Inner)
        {

        }

    }
}
