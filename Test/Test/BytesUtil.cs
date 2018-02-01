using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    internal class BytesUtil
    {
        internal static List<byte[]> SplteBytes(byte[] data, int index)
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

        internal static byte[] SubBytes(byte[] data, int start)
        {
            byte[] copy = new byte[data.Length - start];

            Array.Copy(data, start, copy, 0, copy.Length);

            return copy;
        }

        internal static byte[] SubBytes(byte[] data, int start, int length)
        {
            byte[] copy = new byte[length];

            Array.Copy(data, start, copy, 0, length);

            return copy;
        }

        internal static string ToHexString(byte[] bytes)
        {
            if (bytes == null) { return null; }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("X2"));
            }

            return sb.ToString();
        }

        internal static string ToBinString(byte[] bytes)
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

        internal static int[] ToBinaryArray(byte[] bytes)
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
