using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{
    [Serializable]
    public class Error
    {

        public Error()
        {

        }

        public String Message { get; set; }
        public String StackTrace { get; set; }

        public String InnerMessage { get; set; }
        public String InnerStackTrace { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendFormat("Error: {0}\r\nStackTrace:\r\n{1}", Message, String.IsNullOrWhiteSpace(StackTrace) ? "None" : StackTrace);

            if (String.IsNullOrWhiteSpace(InnerMessage))
            {
                builder.AppendFormat("\r\nInner Error: {0}\r\nInner StackTrace:\r\n{1}", InnerMessage, String.IsNullOrWhiteSpace(InnerStackTrace) ? "None" : InnerStackTrace);
            }

            return builder.ToString();
         }
    }
}
