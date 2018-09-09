using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Http.HttpMessage.Message.Forms.FormMultipart;

namespace Http.HttpMessage.Message.Forms
{
    public class Multipart
    {
        public FormMultipart.Message[] messages;
        public Multipart(FormMultipart.Message[] messages) { this.messages = messages; }
        public static Multipart Parse(string data, string boundary)
        {
            var split = data.Split(new string[] { boundary }, StringSplitOptions.RemoveEmptyEntries).ToList();
            FormMultipart.Message[] messages = split.Select(n => FormMultipart.Message.Parse(n)).ToArray();
            return new Multipart(messages);
        }
    }
}
