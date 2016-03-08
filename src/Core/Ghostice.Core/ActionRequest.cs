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
            Wait,
            //Custom
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

        public static ActionRequest FromJson(String jsonRequest)
        {
            return JsonConvert.DeserializeObject<ActionRequest>(jsonRequest);
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

        public static ActionRequest List(params String[] additionalProperties)
        {
            return new ActionRequest(null, OperationType.List, ActionParameter.Create(additionalProperties));
        }

        public static ActionRequest List(WindowInfo parent, String[] additionalProperties)
        {
            return new ActionRequest(null, OperationType.List, new ActionParameter[] { ActionParameter.Create(parent), ActionParameter.Create(additionalProperties) });
        }

        public static ActionRequest Ready(Locator target, int timeoutSeconds)
        {
            return new ActionRequest(target, OperationType.Ready, ActionParameter.Create(timeoutSeconds));
        }

        public static ActionRequest Ready(Locator target, int timeoutSeconds, String value, bool anyValue)
        {
            return new ActionRequest(target, OperationType.Ready, ActionParameter.Create(timeoutSeconds), ActionParameter.Create(value), ActionParameter.Create(anyValue));
        }

        public static ActionRequest Wait(Locator target, string type, String expression, int timeoutSeconds, int interval)
        {
            return new ActionRequest(target, OperationType.Wait, ActionParameter.Create(type), ActionParameter.Create(expression), ActionParameter.Create(timeoutSeconds), ActionParameter.Create(interval));
        }

        //public static ActionRequest Tell(Locator Target)
        //{
        //    return new ActionRequest(Target, OperationType.Tell);
        //}

        public static ActionRequest Map(Locator target, String[] properties)
        {
            return new ActionRequest(target, OperationType.Map, "Map", null, null, ActionParameter.Create(properties));
        }

        public static ActionRequest Execute(Locator target, String name)
        {
            return new ActionRequest(target, OperationType.Execute, name);
        }

        public static ActionRequest Execute(Locator target, String name, params ActionParameter[] parameters)
        {
            return new ActionRequest(target, OperationType.Execute, name, null, null, parameters);
        }

        public static ActionRequest Get(Locator target, String name)
        {
            return new ActionRequest(target, OperationType.Get, name);
        }

        public static ActionRequest Set(Locator target, String name, String value, Type valueType)
        {
            return new ActionRequest(target, OperationType.Set, name, value, valueType);
        }

    }
}
