﻿using System;

namespace Test
{
    internal enum DecodeType
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
            if (data.Length == 0 || Enum.IsDefined(typeof(DecodeType), data[0])) { return null; }

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

            Element element = new Element();

            do
            {
                data = Decode(data, ref element);
            } while (data.Length > 0 && !data[0].Equals((byte)DecodeType.Code));

            return element;
        }
    }
}
