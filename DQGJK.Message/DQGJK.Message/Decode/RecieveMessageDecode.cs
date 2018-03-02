using System;

namespace DQGJK.Message
{
    public class RecieveMessageDecode
    {
        private MessageDecode BaseDecode;

        private enum DataPosition
        {
            CenterCode = 2,
            ClientCode = 3,
            SendTime = 8,
            Serial = 14,
            FunctionCode = 16,
            DataLength = 18,
            BodyStart = 19
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
            return BaseDecode.SendTime((int)DataPosition.SendTime);
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

            if (message.FunctionCode.Equals("B0") || message.FunctionCode.Equals("C0"))
            {
                message.DataLength = DataLength();
                message.Body = Body(message.DataLength);
                message.Data = ElementDecode.ReadAll(message.Body);
            }



            message.CRC = CRC(message.DataLength + 20);

            bool isChecked = BaseDecode.IsChecked;
            //如果起始符和结束符位置都正确，则校验最后的CRC码
            if (isChecked) { isChecked = BaseDecode.CheckCRC(message.CRC, message.DataLength + 21); }

            message.IsChecked = isChecked;

            return message;
        }
    }
}
