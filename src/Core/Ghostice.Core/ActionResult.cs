using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ghostice.Core
{
    [Serializable]
    public class ActionResult
    {
        public enum ActionStatus
        {
            Unknown,
            Successful,
            Failed
        }

        public ActionResult()
        {
        }

        [JsonConstructor]
        public ActionResult(String Target, ActionStatus Status, Type Type, String Value, Error Error)
        {
            this.Target = Target;
            this.Status = Status;
            this.ReturnType = Type;
            this.ReturnValue = Value;
            this.Error = Error;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public ActionStatus Status { get; set; }

        public String Target { get; set; }

        public Type ReturnType { get; set; }

        public String ReturnValue { get; set; }

        public Error Error { get; set; }
      
        public static ActionResult Successful(String Target)
        {
            return new ActionResult(Target, ActionResult.ActionStatus.Successful, null, null, null);
        }

        public static ActionResult Successful(String Target, Type Type, String Value)
        {
            return new ActionResult(Target, ActionResult.ActionStatus.Successful, Type, Value, null);
        }

        public static ActionResult Failed(String Target, Exception Exception)
        {
            var error = new Error() { Message = Exception.Message, StackTrace = Exception.StackTrace, InnerMessage = Exception.InnerException != null ? Exception.InnerException.Message : "None", InnerStackTrace = Exception.InnerException != null ?  Exception.InnerException.StackTrace : String.Empty } ;

            return new ActionResult(Target, ActionResult.ActionStatus.Failed, null, null, error);
        }

        public static ActionResult Failed(String Target, String Message)
        {
            var error = new Error() { Message = Message, StackTrace = null, InnerMessage = null };

            return new ActionResult(Target, ActionResult.ActionStatus.Failed, null, null, error);
        }

        public static ActionResult Failed(String Target, String Message, Type ReturnedType, String Value)
        {
            var error = new Error() { Message = Message, StackTrace = null, InnerMessage = null };

            return new ActionResult(Target, ActionResult.ActionStatus.Failed, ReturnedType, Value, error);
        }

        public virtual String ToJson()
        {
            var serialiser = new Newtonsoft.Json.JsonSerializer();

            using (var writer = new StringWriter())
            {

                serialiser.Serialize(writer, this);

                return writer.ToString();
            }

        }

        public static ActionResult FromJson(String JsonResult)
        {
            return JsonConvert.DeserializeObject<ActionResult>(JsonResult);
        }

        public override string ToString()
        {

            StringBuilder outputBuilder = new StringBuilder();

            outputBuilder.AppendFormat("ActionResult: Status: {0} Type: {1} Value: {2} Target: {3}", Status.ToString(), ReturnType == null ? "N/A" : ReturnType.ToString(), ReturnValue == null ? "N/A" : ReturnValue.ToString(), Target);

            if (Status == ActionStatus.Failed || Status == ActionStatus.Unknown)
            {
                outputBuilder.AppendFormat("\r\nMessage: {0} StackTrace:\r\n{1}", Error.Message, Error.StackTrace);
            }

            return outputBuilder.ToString();
        }

    }
}
