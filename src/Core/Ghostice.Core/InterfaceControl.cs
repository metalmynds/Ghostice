using Ghostice.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{
    public abstract class InterfaceControl
    {
        protected Descriptor _descriptor = null;
        protected InterfaceControl _parent;

        public InterfaceControl(InterfaceControl Parent)
        {
            _parent = Parent;
        }

        public void SetDescription(Descriptor Descriptor)
        {
            _descriptor = Descriptor;
        }

        //public Property AddIdentifier(String Name, String Value)
        //{
        //    var property = new Property(Name, Value);

        //    _descriptor.Properties.Add(property);

        //    return property;
        //}

        //public void RemoveIdentifier(int Index)
        //{
        //    _descriptor.Properties.RemoveAt(Index);
        //}

        //public void RemoveIdentifier(String Name)
        //{

        //    Property deleteTarget = null;

        //    foreach (var property in _descriptor.Properties)
        //    {
        //        if (property.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            deleteTarget = property;
        //            break;
        //        }
        //    }

        //    if (deleteTarget != null)
        //    {
        //        _descriptor.Properties.Remove(deleteTarget);
        //    }

        //}

        public Descriptor Description
        {
            get
            {
                return _descriptor;
            }
        }

        //public void ClearIdentifiers()
        //{
        //    _descriptor.Properties.Clear();
        //}

        public Locator Path
        {
            get
            {

                var fullPath = new Locator();

                InterfaceControl target = this;

                while (target != null && target as IDispatcherHost == null)
                {

                    fullPath.Path.Insert(0, target.Description);

                    target = target.Parent;

                }

                return fullPath;

            }

        }

        protected void HandleResult(ActionResult Result)
        {
            if (Result == null) { throw new GhosticeClientException("Client Received Unexpected Null Response from Server!"); }

            if (Result.Error != null)
            {
                throw new GhosticeClientException(Result.Error.ToString());
            }
        }

        protected T HandleResult<T>(ActionResult Result)
        {
            
            if (Result == null) { throw new GhosticeClientException("Client Received Unexpected Null Response from Server!"); }

            if (Result.Error != null)
            {
                throw new GhosticeClientException(Result.Error.ToString());
            }
            else
            {

                try
                {
                    return JsonConvert.DeserializeObject<T>(Result.ReturnValue);
                }
                catch (Exception ex)
                {
                    throw new GhosticeClientException("Failed to De-serialize Returned Value!", ex);
                }
            }

        }

        public InterfaceControl Parent { get { return _parent; } }

        protected ActionDispatcher GetDispatcher()
        {

                InterfaceControl target = this;

                while (target != null && target as IDispatcherHost == null)
                {

                    target = target.Parent;

                }

                if (target == null)
                {

                    throw new ClientDispatchActionRequestFailedException("No Client Control Implementing IDispatcherHost Interface Found! Check Top Level Window Class or ApplicationControl!");
                }
                else
                {
                    return (target as IDispatcherHost).GetDispatcher();

            }
        }


        [Serializable]
        public class GhosticeClientException : Exception
        {

            protected GhosticeClientException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }

            public GhosticeClientException(String Message) :
                base(Message)
            {

            }

            public GhosticeClientException(String Message, Exception Inner) :
                base(Message, Inner)
            {

            }
        }
    }
}
