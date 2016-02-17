using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Ghostice.Core.Utilities;

namespace Ghostice.Core
{

    public class InterfaceControlFactory
    {
        public delegate Object ConstructControl(Type controlType, InterfaceControl parent, String name);

        private static ConstructControl _controlConstructor;

        /// <summary>
        /// Creates a Window and Initialises all of its PlaceHolders. Window Must Define a Constructor taking One Parameter of ReturnType AutomationElement.
        /// </summary>
        /// <typeparam name="T">Window Class ReturnType</typeparam>
        /// <param name="parent">AutomationElement that is Window to the Window and All Element Searches.</param>
        /// <returns></returns>
        public static T Create<T>(InterfaceControl parent)
        {
            // Create Window 

            T window = default(T);

            Type windowClassType = typeof(T);

            // Find Required Constructor (A Single Parameter which is passed the AutomationElement passed to the WindowFactory.Create Method.)

            ConstructorInfo ctor = windowClassType.GetConstructor(new[] { typeof(InterfaceControl) });

            if (ctor == null)
            {
                throw new ArgumentException("The Window Must Define a Constructor taking a Single Argument of Type ClientControl! (or Super Class)");
            }

            // Instantiate the Window 

            window = (T)ctor.Invoke(new object[] { parent });

            var windowDescriptor = GetDescriptorFor(typeof(T));

            if (windowDescriptor != null)
            {
                var windowObject = window as InterfaceControl;

                if (windowObject != null)
                {
                    windowObject.SetDescription(windowDescriptor);
                }
            }

            // Initialise the Place Holder Objects.

            InitPlaceHolders(window);

            // Over to you.

            return window;
        }

        public static Object Create(Type ControlType, InterfaceControl root)
        {
            Object window = null;

            Type windowClassType = ControlType;

            // Find Required Constructor (A Single Parameter which is passed the AutomationElement passed to the WindowFactory.Create Method.)

            ConstructorInfo ctor = windowClassType.GetConstructor(new[] { typeof(InterfaceControl) });

            if (ctor == null)
            {
                throw new ArgumentException("The Window Must Define a Constructor taking a Single Argument of Type ClientControl! (or Sub Class)");
            }

            // Instantiate the Window 

            window = ctor.Invoke(new object[] { root });

            var windowDescriptor = GetDescriptorFor(ControlType);

            if (windowDescriptor != null)
            {
                var windowObject = window as InterfaceControl;

                if (windowObject != null)
                {
                    windowObject.SetDescription(windowDescriptor);
                }
            }

            // Initialise the Place Holder Objects.

            InitPlaceHolders(window);

            // Over to you.

            return window;
        }

        protected static void InitPlaceHolders(object window)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window", "Parameter window Cannot Be Null!");
            }

            var windowPlaceHolderList = new List<IHoldPlace>();

            var windowType = window.GetType();

            var typeMembers = new List<MemberInfo>();

            const BindingFlags publicBindingOptions = BindingFlags.Instance | BindingFlags.Public;

            typeMembers.AddRange(windowType.GetFields(publicBindingOptions));
            typeMembers.AddRange(windowType.GetProperties(publicBindingOptions));

            while (windowType != null)
            {
                const BindingFlags nonPublicBindingOptions = BindingFlags.Instance | BindingFlags.NonPublic;

                typeMembers.AddRange(windowType.GetFields(nonPublicBindingOptions));
                typeMembers.AddRange(windowType.GetProperties(nonPublicBindingOptions));

                windowType = windowType.BaseType;
            }

            foreach (var member in typeMembers)
            {
                String wellKnownName = null;

                Descriptor descriptor = GetDescriptor(member);

                if (descriptor != null)
                {

                    object placeholderObject = null;

                    var field = member as FieldInfo;
                    var property = member as PropertyInfo;

                    if (field != null)
                    {
                        placeholderObject = CreatePlaceHolder(windowPlaceHolderList, field.Name, field.FieldType, (InterfaceControl)window, descriptor, window, wellKnownName);

                        if (placeholderObject == null)
                        {
                            throw new CreatePlaceHolderFailedException("Create PlaceHolder for Field [" + field.Name + "] Failed! Null Returned.");
                        }

                        field.SetValue(window, placeholderObject);
                    }
                    else if (property != null)
                    {
                        placeholderObject = CreatePlaceHolder(windowPlaceHolderList, property.Name, property.PropertyType, (InterfaceControl)window, descriptor, window, wellKnownName);

                        if (placeholderObject == null)
                        {
                            throw new CreatePlaceHolderFailedException("Create PlaceHolder for Property [" + property.Name + "] Failed! Null Returned.");
                        }

                        property.SetValue(window, placeholderObject, null);
                    }
                }
            }

        }

        private static object CreatePlaceHolder(List<IHoldPlace> windowPlaceHolderList, String fieldPropertyName, Type genericTypedPlaceHolder, InterfaceControl parent, Descriptor descriptor, Object window, String wellKnownName)
        {

            object placeHolderObject = null;

            var arguments = new object[6];

            arguments[0] = windowPlaceHolderList;
            arguments[1] = fieldPropertyName;
            arguments[2] = parent;
            arguments[3] = descriptor;
            arguments[4] = window;
            arguments[5] = wellKnownName;

            // CreateInstance is shit in 2.0 =3.5 AGHHHH!

            // Construct Method Will be in Other CR3.5 PRoject.

            MethodInfo sudoContructor = genericTypedPlaceHolder.GetMethod("Construct");

            placeHolderObject = sudoContructor.Invoke(null, arguments);

            return placeHolderObject;
        }

        public static ConstructControl ControlConstructor
        {
            get
            {
                return _controlConstructor;
            }
            set
            {
                _controlConstructor = value;
            }
        }

        private static Descriptor GetDescriptor(MemberInfo member)
        {            

            var attribute = (AutomationDescriptorAttribute)Attribute.GetCustomAttribute(member, typeof(AutomationDescriptorAttribute), true);

            if (attribute != null)
            {
                return attribute.Descriptor;
            }

            return null;
        }

        private static Descriptor GetDescriptorFor(Type Class)
        {
            var locators = new List<UIProperty>();

            var attribute = (AutomationDescriptorAttribute)Attribute.GetCustomAttribute(Class, typeof(AutomationDescriptorAttribute), true);

            if (attribute != null)
            {
                return attribute.Descriptor;
            }

            return null;
        }

        public static bool IsWindow(Type Class)
        {
            if (AttributeHelper.HasNotAutomationWindow(Class)) return false;

            if (AttributeHelper.HasAutomationWindow(Class)) return true; // Force Automation Window

            const BindingFlags nonPublicBindingOptions =
                            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            var typeMembers = new List<FieldInfo>();

            typeMembers.AddRange(Class.GetFields(nonPublicBindingOptions));

            foreach (FieldInfo field in typeMembers)
            {

                if (field.FieldType.IsGenericType)
                {

                    Type type = field.FieldType.GetGenericTypeDefinition();

                    if (type.Name.StartsWith(typeof(PlaceHolder<>).Name))
                    {

                        return true;

                    }

                }
            }
            return false;
        }

        public static void Forget(Object WindowInstance)
        {

            //var windowGlove = new Glove(WindowInstance);

            Type windowType = WindowInstance.GetType();

            const BindingFlags nonPublicBindingOptions = BindingFlags.Instance | BindingFlags.NonPublic;

            var typeFields = windowType.GetFields(nonPublicBindingOptions);

            foreach (FieldInfo fieldInfo in typeFields)
            {
                //var placeholder = windowGlove.GetField(fieldInfo.Name) as IHoldPlace;

                FieldInfo field = WindowInstance.GetType().GetField(fieldInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

                var placeholder = field.GetValue(WindowInstance) as IHoldPlace;

                if (placeholder != null)
                {
                    placeholder.Forget();
                }
            }

            var forgetMethods = AttributeHelper.GetForgets(WindowInstance.GetType());

            foreach (MethodInfo methodInfo in forgetMethods)
            {

                methodInfo.Invoke(WindowInstance, null);

            }
        }
    }
}
