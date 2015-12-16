using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ghostice.ApplicationKit;
using Ghostice.Core;
using AustinHarris.JsonRpc;
using Ghostice.Core.Server.Rpc;
using Ghostice.Core.Server;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class RpcTests
    {


        //[TestMethod]
        //public void ExecuteActionRequest()
        //{

        //    var endPoint = new Uri("http://localhost:21888");

        //    var server = new GhosticeServer(null);

        //    server.Start(endPoint);

        //    var client = new JsonRpcClient(endPoint);

        //    try
        //    {

        //        var locator = new Locator(new Descriptor(new Property("Name", "FormA")));

        //        var executor = new ActionDispatcher(client);

        //        var request = new ActionRequest(locator, ActionRequest.OperationType.Get, "Text");

        //        request.Path.Path.Add(new Descriptor(new Property[] { new Property("Name", "textbox1") }));

        //        var result = executor.Execute(request);

        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        //server.Stop(); NEED TO ADD
        //    }
        //}


    }
}
