using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class Element
    {


        internal ElementItem Code { get; private set; }

        /// <summary>
        /// 构造函数返回根据数组解析出的要素
        /// 要求第一个元素必须是从机地址
        /// 解析到下一个从机地址标识符为止
        /// </summary>
        /// <param name="data"></param>
        internal Element(byte[] data)
        {
            if (!data[0].Equals(ElementItemType.Code)) { return; }


        }
    }

    internal enum ElementItemType
    {
        Code = 0x08
    }

    internal abstract class ElementItem
    {
        public byte[] Content { get; set; }
    }

    internal class Code
    {
        internal const byte Marker = (byte)ElementItemType.Code;

        internal const int Length = 1;
    }

    internal class ElementItems
    {
        internal static Dictionary<byte, int> Enums = new Dictionary<byte, int>();

        internal static ElementItem _instance;

        internal static ElementItem GetInstance()
        {
            if (_instance == null)
            {
                Enums.Add(0x08, 1);
            }

            return _instance;
        }
    }
}
