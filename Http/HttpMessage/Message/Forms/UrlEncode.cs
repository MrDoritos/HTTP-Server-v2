using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Http.HttpMessage.Message.Forms
{
    public class UrlEncode
    {
        public FormUrlEncode.Message[] messages { get; private set; }
        public UrlEncode(FormUrlEncode.Message[] messages) { this.messages = messages; }

        public static UrlEncode Parse(string content)
        {
            List<FormUrlEncode.Message> messages = new List<FormUrlEncode.Message>();
            foreach (var variable in (content.Split('&').Select(n => n.Trim('\n', '\r', '\0'))))
            {
                var split = variable.Split('=');
                var name = split[0];
                var value = split[1];
                messages.Add(new FormUrlEncode.Message(HttpUtility.UrlDecode(name), HttpUtility.UrlDecode(value)));
            }
            return new UrlEncode(messages.ToArray());
        }
    }
}
