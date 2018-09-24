using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http.HttpMessage.Message.Forms.FormMultipart
{
    public class Message
    {
        private static string[] splitter = { "\r\n\r\n" };
        public string ContentDisposition { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Content Content { get; set; }

        public static Message Parse(string data)
        {
            var body = data.Split(splitter, StringSplitOptions.None);
            if (body.Length > 1)
            {
                var content = body[1];
                var header = body[0];
                var headeritems = header.Split('\n', ';').Select(n => n.Trim('\r', '\n', ';', ' '));
                string contentdisposition = (headeritems.FirstOrDefault(n => n.ToLower().StartsWith("content-disposition")).Split(':').Last().Trim(' ') ?? throw new ArgumentException());
                string contenttype = (headeritems.FirstOrDefault(n => n.ToLower().StartsWith("content-type")) ?? "").Split(':').Last().TrimStart(' ');
                string name = (headeritems.FirstOrDefault(n => n.ToLower().StartsWith("name")) ?? "").Split('=').Last().TrimStart(' ');
                name = name.Remove(0, 1).Substring(0, name.Length - 2);
                string filename = (headeritems.FirstOrDefault(n => n.ToLower().StartsWith("filename")) ?? "").Split('=').Last().TrimStart(' ');
                if (filename.Length > 3)
                    filename = filename.Remove(0, 1).Substring(0, filename.Length - 2);
                Content dcontent = new Content() { ContentBytes = Encoding.Default.GetBytes(content) };
                return new Message() { Content = dcontent, ContentDisposition = contentdisposition, ContentType = contenttype, FileName = filename, Name = name };

            }
            return new Message();
        }
    }
}
