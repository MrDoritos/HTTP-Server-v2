using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http.HttpMessage.Message.Forms
{
    public class Form
    {
        public FormTypes FormType { get; }
        public Multipart Multipart { get; }
        public UrlEncode UrlEncode { get; }
        public Form(Multipart multipart) { Multipart = multipart; FormType = FormTypes.Mutlipart; }
        public Form(UrlEncode urlEncode) { UrlEncode = urlEncode; FormType = FormTypes.UrlEncode; }

        public static Form Parse(FormTypes formType, string data, string boundary = null)
        {
            Multipart multipart;
            UrlEncode urlEncode;
            switch (formType)
            {
                case FormTypes.Mutlipart:
                    multipart = Multipart.Parse(data, boundary ?? throw new ArgumentNullException("boundary"));
                    return new Form(multipart);
                case FormTypes.UrlEncode:
                    urlEncode = UrlEncode.Parse(data);
                    return new Form(urlEncode);
            }
            return null;
        }

        public enum FormTypes
        {
            Mutlipart = 1,
            UrlEncode = 2,
        }
    }
}
