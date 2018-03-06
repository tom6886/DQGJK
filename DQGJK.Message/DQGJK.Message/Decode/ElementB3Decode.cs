using System;

namespace DQGJK.Message
{
    public class ElementB3Decode
    {
        public byte[] Data;

        public byte[] LeftData;

        public bool IsChecked = true;

        public ElementB3Decode(byte[] data)
        {
            Data = data;
            LeftData = data;
        }

        private enum DecodeType : byte
        {
            CenterCode = 0xF0,
            ClientCode = 0xF1,
            IPPort = 0xF2,
            Interval = 0xF3
        }

        private int GetLength(byte type)
        {
            int length = 0;

            switch ((DecodeType)type)
            {
                case DecodeType.CenterCode: length = 3; break;
                case DecodeType.ClientCode: length = 8; break;
                case DecodeType.IPPort: length = 11; break;
                case DecodeType.Interval: length = 4; break;
            }

            return length;
        }

        private byte[] Decode(byte[] data, ref B3Element element)
        {
            if (data.Length == 0 || !Enum.IsDefined(typeof(DecodeType), data[0])) { IsChecked = false; return new byte[0]; }

            int length = GetLength(data[0]);

            if (length == 0 || data.Length < length) { IsChecked = false; return new byte[0]; }

            switch ((DecodeType)data[0])
            {
                case DecodeType.CenterCode:
                    element.CenterCode = ElementDecodeFunctions.CenterCode(data);
                    break;

                case DecodeType.ClientCode:
                    element.ClientCode = ElementDecodeFunctions.ClientCode(data);
                    break;

                case DecodeType.IPPort:
                    element.IPPort = ElementDecodeFunctions.IPPort(data);
                    break;

                case DecodeType.Interval:
                    element.Interval = ElementDecodeFunctions.Interval(data);
                    break;
            }

            return BytesUtil.SubBytes(data, length);
        }

        public B3Element Read()
        {
            B3Element element = new B3Element();

            do
            {
                LeftData = Decode(LeftData, ref element);
            } while (LeftData.Length > 0 && IsChecked);

            return element;
        }
    }
}
