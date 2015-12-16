// -----------------------------------------------------------------------
// <copyright file="ControlHolder.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

//using BJSS.Puppeteer;

namespace Ghostice.Core
{
    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    /// 
    public class PlaceHolder<T> : IHoldPlace
    {
        private String _fieldPropertyName;
        private ControlDescription _descriptor;
        private InterfaceControl _parent;
        private String _wellKnownAs;
        private Object _control;
        private List<IHoldPlace> _windowPlaceHolderList;
        private TimeSpan _implitictWaitTimeout = new TimeSpan(0, 1, 0);
        private InterfaceControl _object;
        private Object _window;

        public PlaceHolder()
        {

        }

        public void Initialise(List<IHoldPlace> windowPlaceHolderList, String fieldPropertyName, InterfaceControl parent,
                           ControlDescription descriptor, Object window, String wellKnownName)
        {

            windowPlaceHolderList.Add(this);

            _windowPlaceHolderList = windowPlaceHolderList;
            _fieldPropertyName = fieldPropertyName;
            _parent = parent;
            _descriptor = descriptor;
            _window = window;
            _wellKnownAs = wellKnownName;

        }

        // Use Static Method to Create PlaceHolder Instance, because Reflection Can't Pass Parameters in Net 3.5 :(

        public static PlaceHolder<T> Construct(List<IHoldPlace> windowPlaceHolderList, String fieldPropertyName, InterfaceControl parent,
                           ControlDescription descriptors, Object window, String wellKnownName)
        {

            var placeholder = new PlaceHolder<T>();

            placeholder.Initialise(windowPlaceHolderList, fieldPropertyName, parent, descriptors, window, wellKnownName);

            return placeholder;
        }

        public TimeSpan ImplicitTimeout
        {
            get { return _implitictWaitTimeout; }
            set { _implitictWaitTimeout = value; }
        }

        public List<IHoldPlace> WindowPlaceHolderList
        {
            get { return _windowPlaceHolderList; }
        }

        protected virtual InterfaceControl GetElement()
        {

            InterfaceControl childElement = null;

            // TODO: Review this methods usage.

            return childElement;
        }

        public T GetControl()
        {
                if (_control == null)
                {

                    if (ControlFactory.ControlConstructor == null)
                    {
                        throw new ControlFactoryConstructorDelegateNullException();
                    }

                    Type heldControlType = typeof(T);

                    try
                    {
                        // Create ControlType
                        _control = ControlFactory.ControlConstructor.Invoke(heldControlType, this.Parent,
                                                                          Guid.NewGuid().ToString("N"));
                        // Add Path List
                        ((InterfaceControl)_control).SetDescription(this.Descriptor);

                    }
                    catch (Exception ex)
                    {
                        String message = String.Format("Failed to Create Control!\nClass: [{0}]\nError:\n{1}",
                                                       heldControlType.FullName, ex.Message);

                        throw new CreatePlaceHeldControlFailedException(message, ex);
                    }
                }

                return (T)_control;
        }

        public Boolean IsWellKnown
        {
            get { return !String.IsNullOrEmpty(_wellKnownAs); }
        }

        public String WellKnownAs
        {
            get { return _wellKnownAs; }
        }

        public ControlDescription Descriptor
        {
            get { return _descriptor; }
        }

        public Type HeldType
        {
            get { return typeof(T); }
        }

        public Object GetObject()
        {
            return this.GetControl();
        }

        public void Forget()
        {
            _object = null;
            _control = null;
        }

        public InterfaceControl Parent
        {
            get
            {
                return _parent;
            }
        }

        public Object Window
        {
            get
            {
                return _window;
            }
        }
    }

    [Serializable]
    public class CreatePlaceHeldControlFailedException : Exception
    {

        protected CreatePlaceHeldControlFailedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }


        public CreatePlaceHeldControlFailedException(String Message, Exception InnerException)
            : base(Message, InnerException)
        {
        }
    }

    [Serializable]
    public class CreatePlaceHolderFailedException : Exception
    {

        protected CreatePlaceHolderFailedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }


        public CreatePlaceHolderFailedException(String Message)
            : base(Message)
        {
        }

        public CreatePlaceHolderFailedException(String Message, Exception InnerException)
            : base(Message, InnerException)
        {
        }
    }

    [Serializable]
    public class ControlFactoryConstructorDelegateNullException : Exception
    {

        protected ControlFactoryConstructorDelegateNullException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }


        public ControlFactoryConstructorDelegateNullException()
            : base("Control Constructor Delegate Property on WindowFactory Has Not Been Set!")
        {
        }
    }

    public class PlaceHolder
    {

        public static PlaceHolder<T> Create<T>(InterfaceControl Parent, List<IHoldPlace> WellKnownList, AutomationDescriptionAttribute FindBy)
        {
            return Create<T>(Parent, null, WellKnownList, FindBy);
        }

        public static PlaceHolder<T> Create<T>(InterfaceControl Parent, Object Window, List<IHoldPlace> WellKnownList, AutomationDescriptionAttribute FindBy)
        {

            var placeHolder = new PlaceHolder<T>();

            placeHolder.Initialise(WellKnownList == null ? new List<IHoldPlace>() : WellKnownList,
                                    "",
                                    Parent,
                                    FindBy.Descriptor,
                                    Window,
                                    null);

            return placeHolder;
        }


    }

}
