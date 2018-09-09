using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Http.HttpMessage.Message.Forms;

namespace Http.HttpMessage.Message
{
    public class RequestHeader : Header        
    {
        public string RequestURI;
        public Methods Method;
        public RequestParameters RequestParameters;

        public RequestHeader() : base() { RequestURI = ""; Method = Methods.GET; }
        public RequestHeader(RequestHeader header) : base(new Header((header ?? new RequestHeader()).headerParameters ?? new HeaderParameter[0])) { header = header ?? new RequestHeader(); Method = header.Method; RequestURI = header.RequestURI ?? ""; }
        public RequestHeader(RequestHeader requestHeader, Header header) : base(header ?? new Header()) { requestHeader = requestHeader ?? new RequestHeader(); Method = requestHeader.Method; RequestURI = requestHeader.RequestURI; }
        public RequestHeader(Header header) : base(header) { Method = Methods.GET; RequestURI = ""; }
        public RequestHeader(HeaderParameter[] i) : base(i) { Method = Methods.GET; RequestURI = ""; }
        public RequestHeader(HeaderParameter[] i, Methods Method) : base(i ?? new HeaderParameter[0]) { this.Method = Method; RequestURI = ""; }
        public RequestHeader(HeaderParameter[] i, String RequestURI) : base(i ?? new HeaderParameter[0]) { this.RequestURI = RequestURI ?? ""; Method = Methods.GET; }
        public RequestHeader(HeaderParameter[] i, Methods Method, String RequestURI) : base(i ?? new HeaderParameter[0]) { this.Method = Method; this.RequestURI = RequestURI ?? ""; }
        public RequestHeader(IEnumerable<HeaderParameter> headerParameters) : base(headerParameters) { this.Method = Methods.GET; RequestURI = ""; }
        public RequestHeader(IEnumerable<HeaderParameter> i, Methods Method) : base(i ?? new HeaderParameter[0]) { this.Method = Method; RequestURI = ""; }
        public RequestHeader(IEnumerable<HeaderParameter> i, String RequestURI) : base(i ?? new HeaderParameter[0]) { Method = Methods.GET; this.RequestURI = RequestURI ?? ""; }
        public RequestHeader(IEnumerable<HeaderParameter> i, Methods Method, String RequestURI) : base (i ?? new HeaderParameter[0]) { this.Method = Method; this.RequestURI = RequestURI ?? ""; }

        public static RequestHeader Parse(string header)
        {
            Header newheader = Header.Parse(header);
            if (!(newheader != null && newheader.headerParameters != null)) { throw new Exception("I don't know"); }
            //Get the method and URI
            Methods method = Methods.GET;
            String uri = "";
            if (newheader.headerParameters.Length > 0)
            {
                //Get the method
                Enum.TryParse<Methods>(newheader.headerParameters[0].HeaderVariables[0].value, out method);
                //Get the URI
                uri = newheader.headerParameters[0].HeaderVariables[1].value;
            }
            RequestHeader toreturn = new RequestHeader(newheader);
            RequestParameters requestParameters = RequestParameters.Parse(newheader.headerParameters, method == Methods.POST);
            toreturn.Method = method;
            toreturn.RequestURI = uri;
            toreturn.RequestParameters = requestParameters;
            return toreturn;
        }

        public enum Methods
        {
            GET = 0,
            HEAD = 1,
            POST = 2,
            PUT = 3,
            DELETE = 4,
            CONNECT = 5,
            OPTIONS = 6,
            TRACE = 7
        }
    }
}
