using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class DataReader
    {
        private byte Start = 0x7E;

        private string DateTimePattern = "20{0}-{1}-{2} {3}:{4}:{5}";

        public enum ReaderState
        {
            Success = 1,
            NoStart = -1
        }

        public ReaderState State { get; private set; }

        public int StartIndex { get; private set; }

        public byte CenterIP { get; private set; }

        public byte[] ClientIP { get; private set; }

        public DateTime SendTime { get; private set; }

        public uint Serial { get; private set; }

        public string FunctionCode { get; private set; }

        public int DataLength { get; private set; }

        internal DataReader(byte[] data)
        {
            if (!GetStartPosition(data)) { State = ReaderState.NoStart; return; }

            //获取中心站位置
            byte[] subStart = SubBytes(data, StartIndex + 2);

            CenterIP = subStart[0];

            //获取遥测站位置
            byte[] subCenter = SubBytes(subStart, 1);

            Array.Copy(subCenter, ClientIP, 5);

            //获取信息发送时间
            byte[] subClient = SubBytes(subCenter, 5);

            SendTime = GetSendTime(subClient);

            //获取流水号
            byte[] subTime = SubBytes(subClient, 6);

            Serial = GetSerial(subTime);

            //获取功能码
            byte[] subSerial = SubBytes(subTime, 2);

            FunctionCode = subSerial[0].ToString("X2");

            //获取上下行标识及报文长度
            byte[] subCode = SubBytes(subSerial, 1);

            DataLength = Convert.ToInt16(subCode[1]);

            //获取正文数据
            byte[] subLength = SubBytes(subCode, 2);


        }

        #region 操作数据
        private bool GetStartPosition(byte[] data)
        {
            bool isGet = false;

            for (int i = 0, length = data.Length; i < length; i++)
            {
                //连续两个7E代表报文起始符
                if (data[i] == Start && data[i + 1] == Start)
                {
                    StartIndex = i;
                    isGet = true;
                    break;
                }
            }

            return isGet;
        }

        private DateTime GetSendTime(byte[] data)
        {
            string[] strs = new string[6];
            StringBuilder sb = new StringBuilder(2);

            //默认取前六个字节 举例：16 12 12 08 59 59
            for (int i = 0; i < 6; i++)
            {
                sb.Append(data[i] >> 4);
                sb.Append(data[i] & 0x0f);
                strs[i] = sb.ToString();
                sb.Clear();
            }

            string timeStr = string.Format(DateTimePattern, strs);

            return Convert.ToDateTime(timeStr);
        }

        private uint GetSerial(byte[] data)
        {
            byte[] copy = new byte[2];

            Array.Copy(data, copy, 2);

            return BitConverter.ToUInt32(copy, 0);
        }
        #endregion

        private byte[] SubBytes(byte[] data, int start)
        {
            byte[] copy = new byte[data.Length - start - 1];

            Array.Copy(data, start, copy, 0, copy.Length);

            return copy;
        }
    }
}
