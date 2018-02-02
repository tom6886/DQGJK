using System;
using System.Text;

namespace Test
{
    internal class MessageDecode
    {
        private byte[] Data;

        private const byte Start = 0x7E;

        private const byte BodyStart = 0x02;

        private const byte BodyEnd = 0x16;

        private const string DateTimePattern = "20{0}-{1}-{2} {3}:{4}:{5}";

        internal MessageDecode(byte[] data)
        {
            Data = data;
        }

        #region 解析方法

        //获取中心站位置,一个字节,3位
        internal byte CenterCode()
        {
            return Data[2];
        }

        //获取遥测站位置,五个字节,4-8位
        internal byte[] ClientCode()
        {
            byte[] client = new byte[5];

            Array.Copy(Data, 3, client, 0, 5);

            return client;
        }

        //获取信息发送时间,六个字节,发包时间,BCD码,9-14位
        internal DateTime SendTime()
        {
            string[] strs = new string[6];
            StringBuilder sb = new StringBuilder(2);

            //默认取前六个字节 举例：16 12 12 08 59 59
            for (int i = 8; i < 14; i++)
            {
                sb.Append(Data[i] >> 4);
                sb.Append(Data[i] & 0x0f);
                strs[i - 8] = sb.ToString();
                sb.Clear();
            }

            string timeStr = string.Format(DateTimePattern, strs);

            return Convert.ToDateTime(timeStr);
        }

        //获取流水号,两个字节,15-16位
        internal int Serial()
        {
            byte[] serial = new byte[2];

            Array.Copy(Data, 14, serial, 0, 2);

            return BitConverter.ToInt16(serial, 0);
        }

        //获取功能码,一个字节,17位
        //F2H 数据链路报（心跳包）
        //B0H 中心站召测设备实时数据参数
        //C0H 终端机自报数据
        //B1H 中心站遥控设备
        //B2H 修改终端机参数
        internal string FunctionCode()
        {
            return Data[16].ToString("X2");
        }

        //获取上下行标识及报文长度
        //分两个字节,前一个字节代表发送方,无需解析,后一个字节代表数据长度 18-19位
        internal int DataLength()
        {
            return Convert.ToInt16(Data[18]);
        }

        //获取正文数据,02开头,后面的长度等于前面解析得到的报文长度
        //因为前面的数据宽度都是固定的,若正文不是以02开头,则说明解析有误
        internal byte[] Body(int length)
        {
            if (!Data[19].Equals(BodyStart)) { return null; }

            byte[] body = new byte[length];

            Array.Copy(Data, 20, body, 0, length);

            return body;
        }
        #endregion

        private static int GetStartPosition(byte[] data)
        {
            int index = -1;

            for (int i = 0, length = data.Length; i < length; i++)
            {
                //连续两个7E代表报文起始符
                if (data[i] == Start && data[i + 1] == Start)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        internal static int GetDataLength(byte[] data, out int headLength)
        {
            headLength = GetStartPosition(data);

            return Convert.ToInt16(data[headLength + 18]) + 23;
        }

        internal Message Read()
        {
            Message message = new Message();

            message.CenterCode = CenterCode();
            message.ClientCode = ClientCode();
            message.SendTime = SendTime();
            message.Serial = Serial();
            message.FunctionCode = FunctionCode();
            message.DataLength = DataLength();
            message.Body = Body(message.DataLength);
            message.Data = ElementDecode.ReadAll(message.Body);

            return message;
        }
    }
}
