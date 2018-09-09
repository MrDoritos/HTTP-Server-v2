using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http.HttpMessage.Message
{
    public class HeaderVariable
    {
        public HeaderVariable(string name, string value) { this.name = name; this.value = value; }
        public HeaderVariable(string value) { this.value = value; }
        public string name;
        public string value;
    }
}
