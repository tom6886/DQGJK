using System;
using System.Collections.Generic;
using System.Linq;

namespace DQGJK.Message
{
    public class RecieveMessageDecode
    {
        private MessageDecode BaseDecode;

        private enum DataPosition
        {
            CenterCode = 2,
            ClientCode = 3,
            SendTime = 9,
            Serial = 15,
            FunctionCode = 17,
            DataLength = 19,
            BodyStart = 20
        }

        public RecieveMessageDecode(byte[] data)
        {
            BaseDecode = new MessageDecode(data);
        }

        #region 解析方法
        public byte CenterCode()
        {
            return BaseDecode.CenterCode((int)DataPosition.CenterCode);
        }

        public byte[] ClientCode()
        {
            return BaseDecode.ClientCode((int)DataPosition.ClientCode);
        }

        public DateTime SendTime()
        {
            //return BaseDecode.SendTime((int)DataPosition.SendTime);
            return DateTime.Now;
        }

        public int Serial()
        {
            return BaseDecode.Serial((int)DataPosition.Serial);
        }

        public string FunctionCode()
        {
            return BaseDecode.FunctionCode((int)DataPosition.FunctionCode);
        }

        public int DataLength()
        {
            return BaseDecode.DataLength((int)DataPosition.DataLength);
        }

        public byte[] Body(int length)
        {
            return BaseDecode.Body((int)DataPosition.BodyStart, length);
        }

        public DateTime DataTime(byte[] data)
        {
            return BaseDecode.DataTime(data);
        }

        public byte[] CRC(int endPosition)
        {
            return BaseDecode.CRC(endPosition);
        }
        #endregion

        public RecieveMessage Read()
        {
            RecieveMessage message = new RecieveMessage();

            message.Content = BytesUtil.ToHexString(BaseDecode.Data);
            message.CenterCode = CenterCode();
            message.ClientCode = ClientCode();
            message.SendTime = SendTime();
            message.Serial = Serial();
            message.FunctionCode = FunctionCode();
            message.DataLength = DataLength();
            message.CRC = CRC(message.DataLength + 21);
            message.IsChecked = false;

            //只有数据主体起始符和结束符位置都正确，才会继续解析数据主体
            if (!BaseDecode.IsChecked) { return message; }

            //心跳包不进行解析
            if (!message.FunctionCode.Equals("F2"))
            {
                byte[] bodyData = Body(message.DataLength);
                message.Body = bodyData;

                //如果是召测数据或者客户端自报数据，数据主体以采集时间开头，截取后在进行解码
                if (message.FunctionCode.Equals("B0") || message.FunctionCode.Equals("C0"))
                {
                    message.DataTime = DataTime(bodyData);
                    //如果不是以E0E0开头，则说明包体内容错误，无需向下解析
                    if (!BaseDecode.IsChecked) { return message; }
                    bodyData = BytesUtil.SubBytes(bodyData, 7);
                }

                if (message.FunctionCode.Equals("B3"))
                {
                    ElementB3Decode b3Decode = new ElementB3Decode(bodyData);
                    message.Data = b3Decode.Read();
                }
                else
                {
                    List<Element> dataList = ElementDecode.ReadAll(bodyData);

                    if (message.FunctionCode.Equals("B0") || message.FunctionCode.Equals("C0"))
                        message.Data = dataList.Select(q => new B0C0Element(q)).ToList();
                    else if (message.FunctionCode.Equals("B1"))
                        message.Data = dataList.Select(q => new B1Element(q)).ToList();
                    else if (message.FunctionCode.Equals("B2"))
                        message.Data = dataList.Select(q => new B2Element(q)).ToList();
                }
            }

            bool isChecked = BaseDecode.IsChecked;

            //如果以上都通过，则校验最后的CRC码
            if (isChecked) { isChecked = BaseDecode.CheckCRC(message.CRC, message.DataLength + 21); }

            message.IsChecked = isChecked;

            return message;
        }
    }
}
