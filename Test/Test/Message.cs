using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class Message
    {
        internal byte CenterCode { get; set; }

        internal byte[] ClientCode { get; set; }

        internal DateTime SendTime { get; set; }

        internal int Serial { get; set; }

        internal string FunctionCode { get; set; }

        internal int DataLength { get; set; }

        internal byte[] Body { get; set; }

        internal List<Element> Data { get; set; }

        internal int TotalLength { get { return DataLength + 23; } }
    }
}
