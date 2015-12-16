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
    public class ActionRequest
    {
        [Serializable]
        public enum OperationType
        {
            Unknown,
            Get,
            Set,
            Tell,
            Execute,
            Map,
            Ready,
            List
        }


        public ActionRequest()
        {

        }

        public ActionRequest(ControlPath Target, OperationType Operation, String Name)
        {
            this.Operation = Operation;
            this.Name = Name;
            this.Location = Target;
        }

        public ActionRequest(ControlPath Target, OperationType Operation, String Name, String Value, Type ValueType)
        {
            this.Location = Target;
            this.Operation = Operation;
            this.Name = Name;
            this.Value = Value;
            this.ValueType = ValueType;
        }

        [JsonConstructor]
        public ActionRequest(ControlPath Location, OperationType Operation, String Name, String Value, Type ValueType, ActionParameter[] Parameters)
        {
            this.Operation = Operation;
            this.Name = Name;
            this.Value = Value;
            this.Location = Location;
            this.Parameters = Parameters;
            this.ValueType = ValueType;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public OperationType Operation { get; set; }

        public String Name { get; set; }

        public String Value { get; set; }

        public Type ValueType { get; set; }

        public ActionParameter[] Parameters { get; set; }

        [JsonIgnore]
        public Boolean HasParameters
        {
            get { return this.Parameters != null; }
        }

        public ControlPath Location { get; set; }

        public virtual String ToJson()
        {
            var serialiser = new Newtonsoft.Json.JsonSerializer();

            using (var writer = new StringWriter())
            {

                serialiser.Serialize(writer, this);

                return writer.ToString();
            }

        }

        public static ActionRequest FromJson(String JsonRequest)
        {
            return JsonConvert.DeserializeObject<ActionRequest>(JsonRequest);
        }

        public override string ToString()
        {
            return String.Format("ActionRequest: Operation: {0} Name: {1}", Operation, Name);
        }


        public static ActionRequest List()
        {
            return new ActionRequest(null, OperationType.List, null);
        }

        public static ActionRequest Ready(ControlPath Target, int TimeoutSeconds)
        {
            return new ActionRequest(Target, OperationType.Ready, null, null, null, new ActionParameter[] { ActionParameter.Create(TimeoutSeconds) });
        }

        public static ActionRequest Tell(ControlPath Target, String Name)
        {
            return new ActionRequest(Target, OperationType.Tell, Name);
        }

        public static ActionRequest Map(ControlPath Target, String[] Properties)
        {
            return new ActionRequest(Target, OperationType.Map, "Map", null, null, new ActionParameter[] { ActionParameter.Create(Properties) } );
        }

        public static ActionRequest Execute(ControlPath Target, String Name)
        {
            return new ActionRequest(Target, OperationType.Execute, Name);
        }

        public static ActionRequest Execute(ControlPath Target, String Name, ActionParameter[] Parameters)
        {
            return new ActionRequest(Target, OperationType.Execute, Name, null, null, Parameters);
        }

        public static ActionRequest Get(ControlPath Target, String Name)
        {
            return new ActionRequest(Target, OperationType.Get, Name);
        }

        public static ActionRequest Set(ControlPath Target, String Name, String Value, Type ValueType)
        {
            return new ActionRequest(Target, OperationType.Set, Name, Value, ValueType);
        }

    }
}
