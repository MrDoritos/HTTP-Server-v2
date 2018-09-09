using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http.HttpMessage.Message.Forms.FormUrlEncode
{
    public class Message
    {
        public string name;
        public string value;
        public Message(string name, string value) { this.name = name; this.value = value; }
    }
}
