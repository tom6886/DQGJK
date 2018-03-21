using System.Collections.Generic;

namespace DQGJK.Message
{
    public class B1Element : IElement
    {
        public B1Element(Element element)
        {
            Code = element.Code;
            State = element.State;
        }

        /// <summary>
        /// 主从机地址
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public DeviceState State { get; set; }

        public byte[] ToByte()
        {
            string[] code = new string[] { "15", "A5" };

            List<byte> list = new List<byte>();

            list.AddRange(new byte[] { 0x08, 0x08 });
            list.AddRange(BCDUtil.ConvertFrom(Code, 1));
            list.Add(0x06);
            list.AddRange(BCDUtil.ConvertFrom(code[State.RelayOne], 1));
            list.Add(0x07);
            list.AddRange(BCDUtil.ConvertFrom(code[State.RelayTwo], 1));
            list.Add(0x09);
            list.AddRange(BCDUtil.ConvertFrom(code[State.Dehumidify], 1));

            return list.ToArray();
        }
    }
}
