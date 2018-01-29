using System.Collections.Generic;
using static Test.DecodeFunctions;

namespace Test
{
    internal enum DecodeType
    {
        Code = 0x08
    }

    internal class DecodeItem
    {
        internal string PropertyName;

        internal DecodeHandler Function;
    }

    internal class DecodeDict
    {
        internal static Dictionary<byte, DecodeItem> dict = new Dictionary<byte, DecodeItem>();

        internal static DecodeDict _instance;

        internal static DecodeDict GetInstance()
        {
            if (_instance == null)
            {
                DecodeItem code = new DecodeItem() { PropertyName = "Code", Function = Code };
                dict.Add((byte)DecodeType.Code, code);
            }

            return _instance;
        }
    }
}
