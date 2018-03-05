using System;
using System.Collections.Generic;

namespace DQGJK.Message
{
    public class ElementDecode
    {
        private enum DecodeType : byte
        {
            Code = 0x08,
            Humidity = 0x01,
            Temperature = 0x02,
            State = 0x03,
            HumidityLimit = 0x04,
            TemperatureLimit = 0x05
        }

        private static int GetLength(byte type)
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

        private static byte[] Decode(byte[] data, ref Element element)
        {
            if (data.Length == 0 || !Enum.IsDefined(typeof(DecodeType), data[0])) { return null; }

            int length = GetLength(data[0]);

            if (length == 0 || data.Length < length) { return null; }

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

        public static Element Read(byte[] data)
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

        public static List<Element> ReadAll(byte[] data)
        {
            List<Element> list = new List<Element>();

            do
            {
                //如果数据头不是主从机地址，则取消解析
                if (!(data[0].Equals((byte)DecodeType.Code) && data[1].Equals(0x08))) { break; }

                Element element = Read(data);

                list.Add(element);

                data = BytesUtil.SubBytes(data, element.DataLength);
            } while (data.Length > 0);

            return list;
        }
    }
}
