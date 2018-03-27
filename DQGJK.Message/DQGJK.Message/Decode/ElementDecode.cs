using System;
using System.Collections.Generic;

namespace DQGJK.Message
{
    public class ElementDecode
    {
        public byte[] Data;

        public byte[] LeftData;

        public ElementDecode(byte[] data)
        {
            Data = data;
            LeftData = data;
        }

        private enum DecodeType : byte
        {
            Code = 0x08,
            Humidity = 0x01,
            Temperature = 0x02,
            State = 0x03,
            HumidityLimit = 0x04,
            TemperatureLimit = 0x05
        }

        private int GetLength(byte type)
        {
            int length = 0;

            switch ((DecodeType)type)
            {
                case DecodeType.Code: length = 3; break;
                case DecodeType.Humidity: length = 5; break;
                case DecodeType.Temperature: length = 5; break;
                case DecodeType.State: length = 4; break;
                case DecodeType.HumidityLimit: length = 5; break;
                case DecodeType.TemperatureLimit: length = 5; break;
            }

            return length;
        }

        private byte[] Decode(byte[] data, ref Element element)
        {
            if (data.Length == 0 || !Enum.IsDefined(typeof(DecodeType), data[0])) { return new byte[0]; }

            int length = GetLength(data[0]);

            if (length == 0 || data.Length < length) { return new byte[0]; }

            switch ((DecodeType)data[0])
            {
                case DecodeType.Code:
                    element.Code = ElementDecodeFunctions.Code(data);
                    break;

                case DecodeType.Humidity:
                    element.Humidity = ElementDecodeFunctions.Humiture(data);
                    break;

                case DecodeType.Temperature:
                    element.Temperature = ElementDecodeFunctions.Humiture(data);
                    break;

                case DecodeType.State:
                    element.State = ElementDecodeFunctions.State(data);
                    break;

                case DecodeType.HumidityLimit:
                    element.HumidityLimit = ElementDecodeFunctions.Humiture(data);
                    break;

                case DecodeType.TemperatureLimit:
                    element.TemperatureLimit = ElementDecodeFunctions.Humiture(data);
                    break;
            }

            return BytesUtil.SubBytes(data, length);
        }

        public Element Read(byte[] data)
        {
            if (!data[0].Equals((byte)DecodeType.Code)) { return null; }

            int length = data.Length;

            Element element = new Element();

            do
            {
                data = Decode(data, ref element);
            } while (data.Length > 0 && !data[0].Equals((byte)DecodeType.Code));

            element.DataLength = length - data.Length;

            return element;
        }

        public List<Element> ReadAll()
        {
            List<Element> list = new List<Element>();

            do
            {
                //如果数据头不是主从机地址，则取消解析
                if (!(LeftData[0].Equals((byte)DecodeType.Code) && LeftData[1].Equals(0x08))) { break; }

                Element element = Read(LeftData);

                list.Add(element);

                LeftData = BytesUtil.SubBytes(LeftData, element.DataLength);
            } while (LeftData.Length > 0);

            return list;
        }
    }
}