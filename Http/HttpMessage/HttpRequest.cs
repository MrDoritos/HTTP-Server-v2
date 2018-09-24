using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Http.HttpMessage.Message;
using Http.HttpMessage.Message.Forms;

namespace Http.HttpMessage
{
    public class HttpRequest : RequestHeader
    {
        public Content content;
        public Form form;

        public HttpRequest() : base() { content = new Content(); }
        public HttpRequest(Content content) : base() { this.content = content ?? new Content(); }
        public HttpRequest(RequestHeader requestHeader) : base (requestHeader ?? new RequestHeader()) { content = new Content(); }
        public HttpRequest(RequestHeader requestHeader, Content content) : base(requestHeader ?? new RequestHeader()) { this.content = content ?? new Content(); this.form = null; }
        public HttpRequest(RequestHeader requestHeader, Content content, Form form) : base(requestHeader ?? new RequestHeader()) { this.content = content ?? new Content(); this.form = form; }

        private static string[] bodySplit = { "\r\n\r\n" };
        public static HttpRequest Parse(byte[] raw)
        {
            string dat = Encoding.Default.GetString(raw ?? throw new ArgumentNullException("byte[] raw"));
            if (dat.Length < 1) { return new HttpRequest(); }
            string[] splitbody;
            if ((splitbody = dat.Split(bodySplit, StringSplitOptions.None)).Length < 2) { return new HttpRequest(); };
            RequestHeader request = RequestHeader.Parse(splitbody[0]);
            byte[] content = Encoding.Default.GetBytes(dat.Remove(0, splitbody[0].Length + 4).Take(request.RequestParameters.ContentLength).ToArray());
            Form form = null;
            //REMOVE THIS

            //request.RequestParameters.contentType = RequestParameters.ContentTypes.FORMMULTIPART;

            //
            if (request.Method == Methods.POST) if (request.RequestParameters.contentType == RequestParameters.ContentTypes.FORMMULTIPART) { form = Form.Parse(Form.FormTypes.Mutlipart, Encoding.Default.GetString(content), request.RequestParameters.boundary); } else if (request.RequestParameters.contentType == RequestParameters.ContentTypes.URLENCODEDFORM) { form = Form.Parse(Form.FormTypes.UrlEncode, splitbody[1]); }
            HttpRequest toreturn = new HttpRequest(request, new Content() { ContentBytes = content}, form);
            return toreturn;
        }
    }
}
