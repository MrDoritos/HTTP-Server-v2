using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http.HttpMessage.Message
{
    public class HeaderParameter
    {
        public HeaderVariable[] HeaderVariables { get; set; }
        public HeaderParameter(IEnumerable<HeaderVariable> headerVariables) { HeaderVariables = headerVariables.ToArray(); }
        public HeaderParameter(HeaderVariable[] headerVariables) { HeaderVariables = headerVariables; }
    }
}
