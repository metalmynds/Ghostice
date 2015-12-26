using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core.Server.Utilities
{
    public static class Extensions
    {

        public static void DeserialiseParameters(this ActionRequest request)
        {
            if (request.HasParameters)
            {

                foreach (var parameter in request.Parameters)
                {

                    switch (parameter.TypeCode)
                    {
                        case TypeCode.Boolean:
                            parameter.Value = Boolean.Parse(parameter.Value.ToString());
                            break;

                        case TypeCode.Byte:
                            parameter.Value = Byte.Parse(parameter.Value.ToString());
                            break;

                        case TypeCode.Char:
                            parameter.Value = Char.Parse(parameter.Value.ToString());
                            break;

                        case TypeCode.DateTime:
                            parameter.Value = DateTime.Parse(parameter.Value.ToString());
                            break;

                        case TypeCode.DBNull:
                        case TypeCode.Empty:
                        case TypeCode.String:
                            // Take No Action
                            break;

                        case TypeCode.Decimal:
                            parameter.Value = Decimal.Parse(parameter.Value.ToString());
                            break;

                        case TypeCode.Double:
                            parameter.Value = Double.Parse(parameter.Value.ToString());
                            break;

                        case TypeCode.Int16:
                            parameter.Value = Int16.Parse(parameter.Value.ToString());
                            break;

                        case TypeCode.Int32:
                            parameter.Value = Int32.Parse(parameter.Value.ToString());
                            break;

                        case TypeCode.Int64:
                            parameter.Value = Int64.Parse(parameter.Value.ToString());
                            break;

                        case TypeCode.Object:
                            parameter.Value = JsonConvert.DeserializeObject(parameter.Value.ToString(), parameter.ValueType);
                            break;

                        case TypeCode.SByte:
                            parameter.Value = SByte.Parse(parameter.Value.ToString());
                            break;

                        case TypeCode.Single:
                            parameter.Value = Single.Parse(parameter.Value.ToString());
                            break;

                    }

                    //if (parameter.Value.GetType() == typeof(JArray))
                    //{

                    //    var typedValue = JsonConvert.DeserializeObject(parameter.Value.ToString(), parameter.ValueType);

                    //    parameter.Value = typedValue;

                    //}
                }
            }
        }

    }
}
