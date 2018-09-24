using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Httpv2.HttpMessage
{
    public class HttpResponseHeader : HeaderParameters
    {
        public HttpVersions HttpVersion { get; set; }
        public ResponseCodes ResponseCode { get; set; }

        public HttpResponseHeader() :base(ContentTypes.GIF, 0){ }

        public override string ToString()
        {
            return $"{ResponseCode} {ResponseCode.ToString()} {Stringify(HttpVersion)}\r\n{base.ToString()}";
        }
                
        public enum ResponseCodes
        {
            OK = 200,
            NOTFOUND = 404,
            PERMREDIRECT = 301,
            TEMPREDIRECT = 302,
            IAMATEAPOT = 418,
            SERVERERROR = 505,
        }

        private string Stringify(HttpVersions version)
        {
            switch (version)
            {
                case HttpVersions.HTTP1_0:
                    return "HTTP/1.0";
                case HttpVersions.HTTP1_1:
                    return "HTTP/1.1";
                case HttpVersions.HTTP2_0:
                    return "HTTP/2.0";
                default:
                    return "HTTP/1.0";
            }
        }

        public enum HttpVersions
        {
            HTTP1_0 = 0,
            HTTP1_1 = 1,
            HTTP2_0 = 2,
        }
    }
}
