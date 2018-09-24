using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Http.HttpMessage.Message;

namespace Http.HttpMessage
{
    public class HttpResponse : ResponseHeader
    {
        public Content content;
        
        public HttpResponse() : base() { content = new Content(); }
        public HttpResponse(ResponseHeader responseHeader) : base(responseHeader ?? new ResponseHeader()) { content = new Content(); }
        public HttpResponse(ResponseHeader responseHeader, Content content) : base (responseHeader ?? new ResponseHeader()) { this.content = content ?? new Content(); }

        public byte[] Serialize(Encoding encoding)
        {
            return System.Text.Encoding.Convert(encoding, System.Text.Encoding.UTF8, System.Text.Encoding.ASCII.GetBytes((HeaderToString() + "\r\n")).Append(content.ContentBytes));
        }

        public byte[] Serialize()
        {
            return System.Text.Encoding.UTF8.GetBytes(HeaderToString() + "\r\n").Append(content.ContentBytes);
        }

        public string HeaderToString()
        {
            string toreturn = $"{base.ToString()}\r\ncontent-length: {content.ContentLength}\r\n";
            foreach (var a in headerParameters)
                foreach (var b in a.HeaderVariables)
                    toreturn += $"{b.name}: {b.value}\r\n";
            return toreturn;
        }

        public static byte[] Serialize(Content content, ResponseHeader responseHeader)
        {
            return System.Text.Encoding.UTF8.GetBytes(HeaderToString(responseHeader) + $"content-length: {content.ContentLength}\r\n\r\n").Append(content.ContentBytes);
        }

        public static string HeaderToString(ResponseHeader responseHeader)
        {
            string toreturn = $"{responseHeader.ToString()}\r\n";
            foreach (var a in responseHeader.headerParameters)
                foreach (var b in a.HeaderVariables)
                    toreturn += $"{b.name}: {b.value}\r\n";
            return toreturn;
        }
    }
}
