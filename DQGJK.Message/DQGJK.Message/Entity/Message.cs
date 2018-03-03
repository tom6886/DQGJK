using System;
using System.Collections.Generic;

namespace DQGJK.Message
{
    public class RecieveMessage
    {
        public string Content { get; set; }

        public byte CenterCode { get; set; }

        public byte[] ClientCode { get; set; }

        public string ClentCodeStr { get { return BytesUtil.ToHexString(ClientCode); } }

        public DateTime SendTime { get; set; }

        public int Serial { get; set; }

        public string FunctionCode { get; set; }

        public int DataLength { get; set; }

        public byte[] Body { get; set; }

        public object Data { get; set; }

        public int TotalLength { get { return DataLength + 23; } }

        public byte[] CRC { get; set; }

        public bool IsChecked { get; set; }
    }

    public class SendMessage
    {
        public byte CenterCode { get; set; }

        public byte[] ClientCode { get; set; }

        public DateTime SendTime { get; set; }

        public int Serial { get; set; }

        public string FunctionCode { get; set; }

        public byte[] Body { get; set; }

        public byte[] ToByte()
        {
            return MessageEncode.ConvertToByte(this);
        }
    }
}
