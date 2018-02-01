using System;
using System.Collections.Generic;

namespace Test
{
    internal enum DecodeType : byte
    {
        Code = 0x08,
        Humidity = 0x01,
        Temperature = 0x02,
        State = 0x03
    }

    internal class ElementDecode
    {
        private static int GetLength(byte type)
        {
            int length = 0;

            switch ((DecodeType)type)
            {
                case DecodeType.Code: length = 3; break;
                case DecodeType.Humidity: length = 5; break;
                case DecodeType.Temperature: length = 5; break;
                case DecodeType.State: length = 4; break;
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
                    element.Code = DecodeFunctions.Code(data);
                    break;

                case DecodeType.Humidity:
                    element.Humidity = DecodeFunctions.Humiture(data);
                    break;

                case DecodeType.Temperature:
                    element.Temperature = DecodeFunctions.Humiture(data);
                    break;

                case DecodeType.State:
                    element.State = DecodeFunctions.State(data);
                    break;
            }

            return BytesUtil.SubBytes(data, length);
        }

        internal static Element Read(byte[] data)
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

        internal static List<Element> ReadAll(byte[] data)
        {
            List<Element> list = new List<Element>();

            do
            {
                //如果数据头不是主从机地址，则截取到编号为止
                if (!data[0].Equals((byte)DecodeType.Code))
                {
                    int index = Array.IndexOf(data, (byte)DecodeType.Code);

                    if (index < 0) { break; }

                    data = BytesUtil.SubBytes(data, index);
                }

                Element element = Read(data);

                list.Add(element);

                data = BytesUtil.SubBytes(data, element.DataLength);
            } while (data.Length > 0);

            return list;
        }
    }
}
