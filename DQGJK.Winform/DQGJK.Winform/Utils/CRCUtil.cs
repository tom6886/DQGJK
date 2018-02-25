using System;
using System.Text;

namespace DQGJK.Winform
{
    internal class CRCUtil
    {
        internal static byte[] CRC16(byte[] data)
        {
            int len = data.Length;

            if (len == 0) { return new byte[] { 0, 0 }; }

            ushort crc = 0xFFFF;

            for (int i = 0; i < len; i++)
            {
                crc = (ushort)(crc ^ (data[i]));
                for (int j = 0; j < 8; j++)
                {
                    crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                }
            }
            byte hi = (byte)((crc & 0xFF00) >> 8);  //高位置
            byte lo = (byte)(crc & 0x00FF);         //低位置

            return new byte[] { lo, hi };
        }
    }
}
