﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DQGJK.Message
{
    public class BytesUtil
    {
        public static List<byte[]> SplteBytes(byte[] data, int index)
        {
            List<byte[]> list = new List<byte[]>();

            byte[] front = new byte[index];

            byte[] after = new byte[data.Length - index];

            Array.Copy(data, 0, front, 0, front.Length);

            Array.Copy(data, index, after, 0, after.Length);

            list.Add(front);

            list.Add(after);

            return list;
        }

        public static byte[] SubBytes(byte[] data, int start)
        {
            byte[] copy = new byte[data.Length - start];

            Array.Copy(data, start, copy, 0, copy.Length);

            return copy;
        }

        public static byte[] SubBytes(byte[] data, int start, int length)
        {
            byte[] copy = new byte[length];

            Array.Copy(data, start, copy, 0, length);

            return copy;
        }

        public static string ToHexString(byte[] bytes)
        {
            if (bytes == null) { return null; }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public static byte[] ToHexArray(string hexString)
        {
            hexString = hexString.Replace(" ", "");

            if ((hexString.Length % 2) != 0) { hexString += " "; }

            byte[] returnBytes = new byte[hexString.Length / 2];

            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return returnBytes;
        }

        public static string ToBinString(byte[] bytes)
        {
            if (bytes == null) { return null; }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                string temp = Convert.ToString(bytes[i], 2);

                temp = temp.Insert(0, new string('0', 8 - temp.Length));

                sb.Append(temp);
            }

            return sb.ToString();
        }

        public static int[] ToBinaryArray(byte[] bytes)
        {
            string binString = ToBinString(bytes);

            if (string.IsNullOrEmpty(binString)) { return null; }

            char[] chars = binString.ToCharArray();

            List<int> list = new List<int>();

            foreach (var item in chars)
            {
                list.Add(Convert.ToInt32(item.ToString()));
            }

            return list.ToArray();
        }
    }
}
