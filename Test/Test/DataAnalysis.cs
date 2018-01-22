using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class DataReader
    {
        private byte[] Start = { 0x7E, 0x7E };

        public int StartIndex { get; private set; }

        internal DataReader(byte[] data)
        {

        }
    }
}
