using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

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
            //Tell,
            Execute,
            Map,
            Ready,
            List,
            Custom
        }


        public ActionRequest()
        {

        }

        public ActionRequest(Locator Target, OperationType Operation, String Name)
        {
            this.Operation = Operation;
            this.Name = Name;
            this.Target = Target;
        }

        public ActionRequest(Locator Target, OperationType Operation)
        {
            this.Operation = Operation;
            this.Target = Target;
        }

        public ActionRequest(Locator Target, OperationType Operation, params ActionParameter[] Parameters)
        {
            this.Operation = Operation;
            this.Name = Name;
            this.Target = Target;
            this.Parameters = Parameters;
        }

        public ActionRequest(Locator Target, OperationType Operation, String Name, String Value, Type ValueType)
        {
            this.Target = Target;
            this.Operation = Operation;
            this.Name = Name;
            this.Value = Value;
            this.ValueType = ValueType;
        }

        [JsonConstructor]
        public ActionRequest(Locator Target, OperationType Operation, String Name, String Value, Type ValueType, params ActionParameter[] Parameters)
        {
            this.Operation = Operation;
            this.Name = Name;
            this.Value = Value;
            this.Target = Target;
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

        public Locator Target { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public Boolean HasTarget { get { return Target != null; } }

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
            return new ActionRequest(null, OperationType.List);
        }

        public static ActionRequest List(WindowInfo parent)
        {
            return new ActionRequest(null, OperationType.List, ActionParameter.Create(parent));
        }

        public static ActionRequest List(params String[] AditionalProperties)
        {
            return new ActionRequest(null, OperationType.List, ActionParameter.Create(AditionalProperties));
        }

        public static ActionRequest List(WindowInfo parent, String[] AditionalProperties)
        {
            return new ActionRequest(null, OperationType.List, new ActionParameter[] { ActionParameter.Create(parent), ActionParameter.Create(AditionalProperties) });
        }
        public static ActionRequest Ready(Locator Target, int TimeoutSeconds)
        {
            return new ActionRequest(Target, OperationType.Ready, ActionParameter.Create(TimeoutSeconds));
        }

        //public static ActionRequest Tell(Locator Target)
        //{
        //    return new ActionRequest(Target, OperationType.Tell);
        //}

        public static ActionRequest Map(Locator Target, String[] Properties)
        {
            return new ActionRequest(Target, OperationType.Map, "Map", null, null, ActionParameter.Create(Properties));
        }

        public static ActionRequest Execute(Locator Target, String Name)
        {
            return new ActionRequest(Target, OperationType.Execute, Name);
        }

        public static ActionRequest Execute(Locator Target, String Name, params ActionParameter[] Parameters)
        {
            return new ActionRequest(Target, OperationType.Execute, Name, null, null, Parameters);
        }

        public static ActionRequest Get(Locator Target, String Name)
        {
            return new ActionRequest(Target, OperationType.Get, Name);
        }

        public static ActionRequest Set(Locator Target, String Name, String Value, Type ValueType)
        {
            return new ActionRequest(Target, OperationType.Set, Name, Value, ValueType);
        }

    }
}
