using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http.HttpMessage.Message
{
    public class Header
    {
        private static char[] splitter = { ';' };
        public HeaderParameter[] headerParameters;
        public Header(Header header) { headerParameters = header.headerParameters; }
        public Header(HeaderParameter[] headerParameters) { this.headerParameters = headerParameters; }
        public Header(IEnumerable<HeaderParameter> headerParameters) { this.headerParameters = headerParameters.ToArray(); }
        public Header() { headerParameters = new HeaderParameter[0]; }
        static public Header Parse(string header)
        {
            var split = header.Split('\n').Select(n => n.Trim('\r')).ToArray();
            List<HeaderParameter> headerParameters = new List<HeaderParameter>();
            var dd = split[0].Split(' ');
            headerParameters.Add(new HeaderParameter(new HeaderVariable[] { new HeaderVariable("", dd[0]), new HeaderVariable("", dd[1]), new HeaderVariable("", dd[2]) }));
            for (int i = 1; i < split.Length; i++)
            {
                List<HeaderVariable> headerVariables = new List<HeaderVariable>();
                //var splita = split[i].Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                //foreach (var b in splita)
                //{
                    var splitb = split[i].Split(':');
                    headerVariables.Add(new HeaderVariable(splitb[0], System.Web.HttpUtility.UrlDecode(split[i].Remove(0, splitb[0].Length + 1).Trim())));
                //}
                headerParameters.Add(new HeaderParameter(headerVariables));
            }
            return new Header(headerParameters);
        }
    }
}
