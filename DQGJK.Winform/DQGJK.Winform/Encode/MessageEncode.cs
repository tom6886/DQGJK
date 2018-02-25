using System;
using System.Collections.Generic;

namespace DQGJK.Winform
{
    internal class MessageEncode
    {
        private static byte[] Head = { 0x7E, 0x7E };

        private static byte Mark = 0x80;

        private static byte BodyStart = 0x02;

        private static byte Tail = 0x16;

        private static byte[] TimeToByte(DateTime time)
        {
            string timeStr = string.Format("{0}{1}{2}{3}{4}{5}", time.Year.ToString().Substring(2), ZeroFill(time.Month), ZeroFill(time.Day), ZeroFill(time.Hour), ZeroFill(time.Minute), ZeroFill(time.Second));

            return BCDUtil.ConvertFrom(timeStr, 6);
        }

        private static string ZeroFill(int number)
        {
            return number > 9 ? number.ToString() : "0" + number;
        }

        internal static byte[] ConvertToByte(Message message)
        {
            List<byte> list = new List<byte>();

            list.AddRange(Head);

            list.AddRange(message.ClientCode);
            list.Add(message.CenterCode);
            list.AddRange(TimeToByte(message.SendTime));
            list.AddRange(BCDUtil.ConvertFrom(message.Serial.ToString(), 2));
            list.AddRange(BytesUtil.ToHexArray(message.FunctionCode));
            list.Add(Mark);
            list.Add((byte)message.DataLength);
            list.Add(BodyStart);
            list.Add(Tail);

            message.CRC = CRCUtil.CRC16(list.ToArray());
            list.AddRange(message.CRC);

            return list.ToArray();
        }

    }
}
