using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class DecodeResult<T>
    {
        /// <summary>
        /// 解码器分割出的数组
        /// </summary>
        internal byte[] Content { get; set; }

        /// <summary>
        /// 解码器解码后剩下的数组
        /// </summary>
        internal byte[] LeftContent { get; set; }

        /// <summary>
        /// 解码器解码后得到的结果
        /// </summary>
        internal T Result { get; set; }
    }

    internal enum DecodeType
    {
        Code = 0x08
    }

    internal class ElementDecode
    {
        internal static DecodeResult<string> CodeDecode(byte[] data)
        {
            return null;
        }
    }
}
