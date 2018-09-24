using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Httpv2.HttpMessage
{
    public class HeaderParameters : List<HeaderParameter>
    {
        public ContentTypes ContentType { get; set; }
        public uint ContentLength { get; set; }
        public CacheControls[] CacheControl { get; set; }
        public Pragmas Pragma { get; set; }
        public Encodings Encoding { get; set; }

        public HeaderParameters(ContentTypes contentType, uint contentLength) : base() { ContentType = contentType; ContentLength = contentLength; }

        public override string ToString()
        {
            return $"{ContentTypeToString()}\r\ncontent-length: {ContentLength}\r\n{HeaderParametersToString()}";
        }

        public string HeaderParametersToString()
        {
            string toreturn = "";
            foreach (var a in this)
            {
                toreturn += $"{a.name}: {a.value}\r\n";
            }
            return toreturn;
        }

        private string ContentTypeToString()
        {
            if (ContentType == ContentTypes.GIF || ContentType == ContentTypes.JPEG || ContentType == ContentTypes.PNG || ContentType == ContentTypes.PLAIN)
            {
                return $"content-type: {Stringify(ContentType)}\r\n";
            }
            else
            {
                return $"content-type: {Stringify(ContentType)}; charset={Stringify(Encoding)}\r\n";
            }
        }

        public string Stringify(ContentTypes contentType)
        {
            switch (contentType)
            {
                case ContentTypes.PLAIN:
                    return "plain";
                case ContentTypes.TEXT_HTML:
                    return "text/html";
                case ContentTypes.TEXT_JSON:
                    return "text/json";
                case ContentTypes.XML:
                    return "text/xml";
                case ContentTypes.URLENCODEDFORM:
                    return "urlencoded/form";
                default:
                    return "plain";
            }
        }

        private string Stringify(Pragmas pragma)
        {
            switch (pragma)
            {
                case Pragmas.NOCACHE:
                    return "no-cache";
                default:
                    return "no-cache";
            }
        }

        private string GetExtra()
        {
            string toreturn = "";
            toreturn += $"pragma: {Stringify(Pragma)}";
            foreach (var a in CacheControl)
            {

            }
            return "";
        }

        private string Stringify(CacheControls cacheControl)
        {
            switch (cacheControl)
            {
                
            }
            return "";
        }

        public string Stringify(Encodings encoding)
        {
            switch (encoding)
            {
                case Encodings.ASCII:
                    return "ascii";
                case Encodings.UTF8:
                    return "utf-8";
                default:
                    return "utf-8";
            }
        }

        public enum ContentTypes
        {
            PLAIN = 0,
            TEXT_HTML = 1,
            TEXT_JSON = 2,
            XML = 3,
            URLENCODEDFORM = 4,
            PNG = 5,
            JPEG = 6,
            GIF = 7,
        }

        public enum Encodings
        {
            UTF8 = 0,
            ASCII = 1,
            EXTENDEDAASCII_ANSI = 2,
        }

        public enum Pragmas
        {
            NOCACHE = 0,
        }

        public enum CacheControls
        {

        }
    }
}
