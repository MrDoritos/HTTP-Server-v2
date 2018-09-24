using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Httpv2.HttpMessage
{
    public class HttpRequestHeader : HeaderParameters
    {
        public HttpRequestHeader() : base(ContentTypes.GIF, 0) { }
    }
}
