using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http.HttpMessage.Message
{
    public class RequestParameters
    {
        public string ContentType;
        public ContentTypes contentType;
        public string UserAgent;
        public string Version;
        public int ContentLength;

        public RequestParameters() { Version = "HTTP/1.0"; }
        public RequestParameters(ContentTypes ContentType, String contentType, String UserAgent, String Version, Int32 ContentLength) { this.ContentLength = ContentLength; this.Version = Version ?? "HTTP/1.0"; this.UserAgent = UserAgent ?? ""; this.contentType = ContentType; this.ContentType = contentType ?? ""; }
        public RequestParameters(String UserAgent, String Version) { this.UserAgent = UserAgent ?? ""; this.Version = Version ?? "HTTP/1.0"; }
        
        public static RequestParameters Parse(HeaderParameter[] headerParamters, bool isPost)
        {
            string contentType = "";
            ContentTypes ContentType = ContentTypes.URLENCODEDFORM;
            string userAgent = "";
            string version = "";
            int contentlength = 0;

            foreach (var headerVariables in headerParamters.Select(n => n.HeaderVariables))
            foreach (var a in headerVariables)
                switch ((a.name ?? a.value).ToLower())
                {
                    case "user-agent":
                        userAgent = a.value;
                        break;
                    case "content-length":
                        if (int.TryParse(a.value, out int num)) { contentlength = num; }
                        break;
                    case "http/1.0":
                        version = "HTTP/1.0";
                        break;
                    case "http/1.1":
                        version = "HTTP/1.1";
                        break;
                    case "http/2.0":
                        version = "HTTP/2.0";
                        break;
                    case "content-type":
                        if (ContentTypeTryParse(a.value, out ContentTypes types)) { contentType = types.ToString(); ContentType = types; }
                        break;
                }
            if (!isPost)
                return new RequestParameters(userAgent, version);
            else
                return new RequestParameters(ContentType, contentType, userAgent, version, contentlength);
        }

        public static bool ContentTypeTryParse(string source, out ContentTypes content)
        {
            content = ContentTypes.URLENCODEDFORM;
            switch (source.ToLower())
            {
                case "multipart/form-data":
                    content = ContentTypes.FORMMULTIPART;
                    return true;
                default:
                    content = ContentTypes.URLENCODEDFORM;
                    return false;
            }
        }        

        public enum ContentTypes
        {
            FORMMULTIPART = 1,
            URLENCODEDFORM = 2,
        }
    }
}
