using DynamicExpresso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ghostice.Core
{
    public static class ExpressionManager
    {
        delegate Boolean UIThreadSafeEvaluate(Control target, Lambda expression);

        public static Lambda Prepare(Object target, String expression)
        {

            var interpreter = new Interpreter();

            interpreter.EnableReflection();

            interpreter.Reference(target.GetType());

            foreach (var referenceType in ReflectionManager.GetReferencedTypes(target.GetType()))
            {

                interpreter.Reference(referenceType);

            }

            var condition = interpreter.Parse(expression, new Parameter("target", target.GetType(), target));

            return condition;
        }

        public static Boolean Evaluate(Object target, Lambda expression)
        {

            var control = target as Control;

            if (control != null && (control.InvokeRequired))
            {

                return (Boolean)control.Invoke(new UIThreadSafeEvaluate(Evaluate), new Object[] { target, expression });

            }
            else
            {
                var typedTarget = Convert.ChangeType(target, target.GetType());

                return (Boolean)expression.Invoke(typedTarget);
            }

        }


    }
}
