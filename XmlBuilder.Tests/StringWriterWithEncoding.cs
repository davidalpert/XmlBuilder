using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Should;
using Xunit;
using System.Xml;
using System.IO;

namespace SergioPereira.Xml
{
    public class StringWriterWithEncoding : StringWriter
    {
        protected Encoding encoding;

        public StringWriterWithEncoding(Encoding encoding)
            : base()
        {
            this.encoding = encoding;
        }
        public StringWriterWithEncoding(Encoding encoding, IFormatProvider formatProvider)
            : base(formatProvider)
        {
            this.encoding = encoding;
        }
        public StringWriterWithEncoding(Encoding encoding, StringBuilder sb)
            : base(sb)
        {
            this.encoding = encoding;
        }
        public StringWriterWithEncoding(Encoding encoding, StringBuilder sb, IFormatProvider formatProvider)
            : base(sb, formatProvider)
        {
            this.encoding = encoding;
        }

        public override Encoding Encoding
        {
            get
            {
                return encoding;
            }
        }
    }
}
