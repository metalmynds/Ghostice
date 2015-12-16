using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using Ghostice.Core;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Ghostice.ApplicationKit.UnitTests
{
    [TestClass]
    public class SerialisationTests
    {
        [TestMethod]
        public void SerialiseActionRequestToXml()
        {

            var simpleLocator = new ControlPath(new ControlDescription(new Property("Name", "FormMain")));

            var executeRequest = ActionRequest.Execute(simpleLocator, "Close", new ActionParameter[] { });
        
            XmlSerializer serializer = new XmlSerializer(typeof(ActionRequest));

            var builder = new StringBuilder();

            using (TextWriter writer = new StringWriter(builder))
            {
                serializer.Serialize(writer, executeRequest);
            }

            var actual = RemoveWhitespace(builder.ToString());

            var expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?><ActionRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Operation>Execute</Operation><Name>Close</Name><Parameters /><Location><Path><ControlDescription><Properties><Property><Name>Name</Name><Value>FormMain</Value></Property></Properties></ControlDescription></Path></Location></ActionRequest>";

            Assert.AreEqual<String>(expected, actual);
        }

        [TestMethod]
        public void SerialiseActionResultToXml()
        {

            ActionResult result = ActionResult.Failed("ButtonA", new InvalidOperationException("Dummy Exception", new FileNotFoundException("Missing Some File")));

            XmlSerializer serializer = new XmlSerializer(typeof(ActionResult));

            var builder = new StringBuilder();

            using (TextWriter writer = new StringWriter(builder))
            {
                serializer.Serialize(writer, result);
            }

            var actual = RemoveWhitespace(builder.ToString());

            var expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?><ActionResult xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Status>Failed</Status><Target>ButtonA</Target><Error><Message>Dummy Exception</Message><InnerMessage>Missing Some File</InnerMessage></Error></ActionResult>";

            Assert.AreEqual<String>(expected, actual);
        }

        [TestMethod]
        public void DeserialiseActionRequestFromXml()
        {

            var serialized = "<?xml version=\"1.0\" encoding=\"utf-16\"?><ActionRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Operation>Execute</Operation><Name>Close</Name><Arguments /><Location><Descriptors><Descriptor><Properties><Property><Name>Name</Name><Value>FormMain</Value></Property></Properties></Descriptor></Descriptors></Location></ActionRequest>";

            var serializer = new XmlSerializer(typeof(ActionRequest));

            using (var reader = new StringReader(serialized))
            {
                var request = (ActionRequest)serializer.Deserialize(reader);

                Assert.IsNotNull(request);
            }
        }

        public static string RemoveWhitespace(string xml)
        {
            Regex regex = new Regex(@">\s*<");
            xml = regex.Replace(xml, "><");

            return xml.Trim();
        }
    }
}
