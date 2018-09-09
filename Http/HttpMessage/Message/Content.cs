using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http.HttpMessage.Message
{
    public class Content
    {
        public byte[] ContentBytes;
        public int ContentLength
        {
            get
            {
                return ContentBytes.Length;
            }
        }

        public Content() { ContentBytes = new byte[0]; }
        public Content(byte[] content) { ContentBytes = content ?? new byte[0]; }
    }
}
