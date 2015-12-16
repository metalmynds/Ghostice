using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ghostice.Core;
using Newtonsoft.Json;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class CommandTests
    {

        [TestMethod]
        public void ConstructExecuteActionRequestWithArguments()
        {
            var locator = new ControlPath(new ControlDescription(new Property("Name", "FormA")), new ControlDescription(new Property("Name", "TabControl1")));

            var arguments = new ActionParameter[] { ActionParameter.Create(1) };

            var request = ActionRequest.Execute(locator, "SelectTab", arguments);

            Assert.AreEqual(request.Name, "SelectTab");

            Assert.AreEqual(2, request.Location.Path.Count);

            Assert.AreEqual(request.Location.Path[0].RequiredProperties[0], "Name");

            Assert.AreEqual(request.Location.Path[0].Properties[0].Name, "Name");

            Assert.AreEqual(request.Location.Path[0].Properties[0].Value, "FormA");

            Assert.AreEqual(request.Location.Path[1].RequiredProperties[0], "Name");

            Assert.AreEqual(request.Location.Path[1].Properties[0].Name, "Name");

            Assert.AreEqual(request.Location.Path[1].Properties[0].Value, "TabControl1");

            Assert.IsTrue(request.HasParameters);

            Assert.AreEqual(request.Parameters, arguments);

            var actual = request.ToJson();

            var expected = "{\"Operation\":\"Execute\",\"Name\":\"SelectTab\",\"Value\":null,\"ValueType\":null,\"Parameters\":[{\"ValueType\":null,\"TypeCode\":\"Int32\",\"Value\":1}],\"Location\":{\"Path\":[{\"Properties\":[{\"Name\":\"Name\",\"Value\":\"FormA\"}]},{\"Properties\":[{\"Name\":\"Name\",\"Value\":\"TabControl1\"}]}]}}";

            Assert.AreEqual<String>(expected, actual);

        }

        [TestMethod]
        public void ConstructActionExecuteRequest()
        {
            var locator = new ControlPath(new ControlDescription(new Property("Name", "FormA")), new ControlDescription(new Property("Name", "Control1")));

            var request = new ActionRequest(locator, ActionRequest.OperationType.Execute, "Click");

            Assert.AreEqual(request.Name, "Click");

            Assert.AreEqual(2, request.Location.Path.Count);

            Assert.AreEqual(request.Location.Path[0].RequiredProperties[0], "Name");

            Assert.AreEqual(request.Location.Path[0].Properties[0].Name, "Name");

            Assert.AreEqual(request.Location.Path[0].Properties[0].Value, "FormA");

            Assert.AreEqual(request.Location.Path[1].RequiredProperties[0], "Name");

            Assert.AreEqual(request.Location.Path[1].Properties[0].Name, "Name");

            Assert.AreEqual(request.Location.Path[1].Properties[0].Value, "Control1");

            var actual = request.ToJson();

            var expected = "{\"Operation\":\"Execute\",\"Name\":\"Click\",\"Value\":null,\"ValueType\":null,\"Parameters\":null,\"Location\":{\"Path\":[{\"Properties\":[{\"Name\":\"Name\",\"Value\":\"FormA\"}]},{\"Properties\":[{\"Name\":\"Name\",\"Value\":\"Control1\"}]}]}}";

            Assert.AreEqual<String>(expected, actual);
        }


        [TestMethod]
        public void SerialiseActionRequestToJson()
        {
            var locator = new ControlPath(new ControlDescription(new Property("Name", "FormMain")));

            var request = new ActionRequest(locator, ActionRequest.OperationType.Execute, "Close", null, null);

            Assert.AreEqual(request.Name, "Close");

            Assert.AreEqual(1, request.Location.Path.Count);

            Assert.AreEqual(request.Location.Path[0].RequiredProperties[0], "Name");

            Assert.AreEqual(request.Location.Path[0].Properties[0].Name, "Name");

            Assert.AreEqual(request.Location.Path[0].Properties[0].Value, "FormMain");

            var actual = request.ToJson();

            var expected = "{\"Operation\":\"Execute\",\"Name\":\"Close\",\"Value\":null,\"ValueType\":null,\"Parameters\":null,\"Location\":{\"Path\":[{\"Properties\":[{\"Name\":\"Name\",\"Value\":\"FormMain\"}]}]}}";

            Assert.AreEqual<String>(expected, actual);
        }

        [TestMethod]
        public void DeserialiseActionRequestFromJson()
        {

            var serialised = "{\"Name\":\"Click\",\"Value\":null,\"Operation\":\"Execute\",\"Arguments\":null,\"Location\":{\"Descriptors\":[{\"Properties\":[{\"Name\":\"Name\",\"Value\":\"butOK\"}]}]}}";
            
            var deserialised = JsonConvert.DeserializeObject<ActionRequest>(serialised);

            Assert.AreEqual<String>("Click", deserialised.Name);

            Assert.AreEqual<String>(null, deserialised.Value);



            Assert.IsNotNull(deserialised);

        }

        [TestMethod]
        public void DeserialiseActionRequestWithParametersFromJson()
        {

            var serialised = "{\"Operation\":\"Execute\",\"Name\":\"SelectTab\",\"Value\":null,\"ValueType\":null,\"Parameters\":[{\"ValueType\":\"System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\",\"Value\":1}],\"Location\":{\"Path\":[{\"Properties\":[{\"Name\":\"Name\",\"Value\":\"FormA\"}]},{\"Properties\":[{\"Name\":\"Name\",\"Value\":\"TabControl1\"}]}]}}";

            var deserialised = JsonConvert.DeserializeObject<ActionRequest>(serialised);

            Assert.IsNotNull(deserialised);

            Assert.AreEqual<String>("SelectTab", deserialised.Name);

            Assert.AreEqual<String>(null, deserialised.Value);

            Assert.IsTrue(deserialised.HasParameters, "Parameters are missing from request!");

        }

        [TestMethod]
        public void DeserialiseApplicationInfoFromJson()
        {

            var serialised = "{\"Status\":\"Started\",\"InstanceIdentifier\":\"1eba6886834f4fc6ae679588bd2d78f4\",\"ApplicationPath\":\"Example.PetShop.WinForms.exe\",\"CommandLineArguments\":\"\",\"ApplicationVersion\":\"Example.PetShop.WinForms v1.0.0.0\",\"Error\":null,\"MachineName\":\"LPT1254\",\"FullyQualifiedDomainName\":\"lpt1254.bjss.co.uk\",\"OperatingSystem\":\"Microsoft Windows NT 6.1.7601 Service Pack 1\",\"IPAddressList\":[\"fe80::fcb4:7e3d:8f78:1cdb%15\",\"fe80::840a:4e4b:fb79:b915%16\",\"fe80::98dc:fe71:e65c:81a6%18\",\"fe80::182c:416:d24e:6d9f%23\",\"fe80::540b:3dd3:94:bf52%12\",\"192.168.230.1\",\"192.168.17.1\",\"192.168.56.1\",\"192.168.35.1\",\"192.168.0.103\"],\"Pid\":0,\"StartupTime\":\"00:00:01.3608887\"}";

            var deserialised = JsonConvert.DeserializeObject<ApplicationInfo>(serialised);

            Assert.AreEqual<ApplicationInfo.ApplicationStatus>(ApplicationInfo.ApplicationStatus.Started, deserialised.Status);

            Assert.IsNotNull(deserialised);

        }

        //[TestMethod]
        //public void DeserialiseActionResultFromJson() 
        //{
        //}

    }
}
