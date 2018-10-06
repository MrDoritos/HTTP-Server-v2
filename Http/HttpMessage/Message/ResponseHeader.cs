using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http.HttpMessage.Message
{
    public class ResponseHeader : Header
    {
        public string HttpVersion = "HTTP/1.0";
        public ResponseCodes ResponseCode;
        public ContentTypes ContentType;
        public Encodings Encoding;
                
        public ResponseHeader() : base() { ResponseCode = ResponseCodes.OK; ContentType = ContentTypes.TEXTHTML;  Encoding = Encodings.UTF8; }
        public ResponseHeader(ResponseHeader responseHeader) : base(responseHeader ?? new ResponseHeader()) { responseHeader = responseHeader ?? new ResponseHeader(); ResponseCode = responseHeader.ResponseCode; ContentType = responseHeader.ContentType; Encoding = responseHeader.Encoding; }
        public ResponseHeader(Header baseHeader) : base(baseHeader ?? new Header()) { ResponseCode = ResponseCodes.OK; ContentType = ContentTypes.TEXTHTML; Encoding = Encodings.UTF8; }
        public ResponseHeader(Header baseHeader, ContentTypes contentType) : base(baseHeader ?? new Header()) { ResponseCode = ResponseCodes.OK; ContentType = contentType; Encoding = Encodings.UTF8; }
        public ResponseHeader(ContentTypes contentType) : base() { ResponseCode = ResponseCodes.OK; ContentType = contentType; Encoding = Encodings.UTF8; }
        
        public override string ToString()
        {
            if (ContentType == ContentTypes.IMAGEJPEG || ContentType == ContentTypes.IMAGEPNG || ContentType == ContentTypes.PLAIN)
            {
                return $"{HttpVersion} {(int)ResponseCode} {ResponseCode}\r\ncontent-type: {ContentTypeToString(ContentType)}";
            }
            else
            {
                return $"{HttpVersion} {(int)ResponseCode} {ResponseCode}\r\ncontent-type: {ContentTypeToString(ContentType)}; charset={EncodingToString(Encoding)}";
            }
        }

        private static string ContentTypeToString(ContentTypes contentType)
        {
            switch (contentType)
            {
                case ContentTypes.IMAGEJPEG:
                    return "image/jpeg";
                case ContentTypes.IMAGEPNG:
                    return "image/png";
                case ContentTypes.ICO:
                    return "image/ico";
                case ContentTypes.TEXTHTML:
                    return "text/html";
                case ContentTypes.TEXTXML:
                    return "text/xml";
                case ContentTypes.TEXTCSS:
                    return "text/css";
                case ContentTypes.ZIP:
                    return "application/zip";
                default:
                    return "plain";
            }
        }

        private static string EncodingToString(Encodings encoding)
        {
            switch (encoding)
            {
                case Encodings.ASCII:
                    return "ASCII";
                case Encodings.UTF8:
                default:
                    return "UTF-8";
            }
        }

        public enum ResponseCodes
        {
            OK = 200,
            FORBIDDEN = 403,
            NOTFOUND = 404,
            PERMREDIRECT = 301,
            IAMATEAPOT = 418,
            INTERNALSERVERERROR = 505,
        }

        public enum Encodings
        {
            ASCII,
            UTF8,
        }

        public enum ContentTypes
        {
            IMAGEPNG = 0,
            TEXTHTML = 1,
            IMAGEJPEG = 2,
            TEXTCSS = 3,
            TEXTXML = 4,
            PLAIN = 5,
            ICO = 6,
            ZIP = 7,
        }
    }
}
