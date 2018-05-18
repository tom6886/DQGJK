using System;
using System.Collections.Generic;

namespace DQGJK.Message
{
    public class MessageEncode
    {
        private static byte[] Head = { 0x7E, 0x7E };

        //private static byte Mark = 0x80;

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

        public static byte[] ConvertToByte(SendMessage message)
        {
            List<byte> list = new List<byte>();

            list.AddRange(Head);

            list.AddRange(message.ClientCode);
            list.Add(message.CenterCode);
            list.AddRange(TimeToByte(message.SendTime));
            list.AddRange(BCDUtil.ConvertFrom(message.Serial.ToString(), 2));
            list.AddRange(BytesUtil.ToHexArray(message.FunctionCode));

            int DataLength = (message.Body == null) ? 0 : message.Body.Length;
            string DataLengthStr = "0" + DataLength.ToString("X3");
            list.AddRange(BCDUtil.ConvertFrom(DataLengthStr, 2));

            list.Add(BodyStart);

            if (message.Body != null) { list.AddRange(message.Body); }

            byte[] CRC = CRCUtil.CRC16(list.ToArray());

            list.Add(Tail);
            list.AddRange(CRC);

            return list.ToArray();
        }

    }
}
