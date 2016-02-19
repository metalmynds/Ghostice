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

            Exception clientException = null;

            JsonResponse<ActionResult> serverResponse = null;

            //ActionResult result = null;

            //String error = null;

            //var request = Request.ToJson();

            var callback = _rpcClient.Invoke<ActionResult>("Perform", Request, Scheduler.Default);

            using (callback.Subscribe(
                onNext: (response) =>
                {
                    serverResponse = response;
                    //result = response.Result;
                },
                onError: (ex) =>
                {
                    clientException = ex;

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

            if (serverResponse == null)
            {
                if (clientException != null)
                    throw new ClientDispatchActionRequestFailedException(String.Format("ActionDispatcher Perform Failed!\r\nRequest:\r\n{0}\r\nError: {1}\r\nStack Trace:\r\n{2}", Request.ToString(), clientException.Message, clientException.StackTrace), clientException);
                else
                    throw new ClientDispatchActionRequestFailedException(String.Format("ActionDispatcher Perform Failed!\r\nError: Server Return Null! and No Exception was Reported!"));
            }
            else if (serverResponse.Error != null)
            {
                throw new ServerExecuteActionFailedException(Request, serverResponse.Error.Message, serverResponse.Error.Data, serverResponse.Error.StackTrace, serverResponse.Error.InnerException);
            }

            return serverResponse.Result;
        }

    }

    [Serializable]
    public class ClientDispatchActionRequestFailedException : Exception
    {

        protected ClientDispatchActionRequestFailedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }


        public ClientDispatchActionRequestFailedException(String Message)
            : base(Message)
        {

        }
        public ClientDispatchActionRequestFailedException(String Message, Exception Inner)
            : base(Message, Inner)
        {

        }

    }

    [Serializable]
    public class ServerExecuteActionFailedException : Exception
    {

        protected ServerExecuteActionFailedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }


        public ServerExecuteActionFailedException(String message)
            : base(message)
        {

        }
        public ServerExecuteActionFailedException(String message, Exception innerException)
            : base(message, innerException)
        {

        }

        public ServerExecuteActionFailedException(ActionRequest request, Exception innerException)
            : base(String.Format("Server Execute Action Failed!\r\nRequest:\r\n{0} ", request.ToJson()), innerException)
        {

        }

        public ServerExecuteActionFailedException(ActionRequest request, String error)
            : base(String.Format("Server Execute Action Failed!\r\nRequest:\r\n{0}\r\nError: {1}", request.ToJson(), error))
        {

        }

        public ServerExecuteActionFailedException(ActionRequest request, String error, System.Collections.IDictionary data, String stackTrace, Exception innerException)
            : base(String.Format("Server Execute Action Failed!\r\nRequest:\r\n{0}\r\nError: {1}\r\nError Data: {2}\r\nRemote Stack Trace: {3}", request.ToJson(), error, GetPropertyList(data), stackTrace), innerException)
        {

        }

        private static String GetPropertyList(System.Collections.IDictionary pairs)
        {
            StringBuilder listBuilder = new StringBuilder();

            foreach (String key in pairs.Keys)
            {
                listBuilder.AppendFormat("{0} = {1}\r\n", key, ValueConvert.ToString(pairs[key]));
            }

            return listBuilder.ToString();
        }
    }

}
