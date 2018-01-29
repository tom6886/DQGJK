namespace Test
{
    internal class DecodeResult
    {
        /// <summary>
        /// 解码器分割出的数组
        /// </summary>
        internal byte[] Content { get; set; }

        /// <summary>
        /// 解码器解码后剩下的数组
        /// </summary>
        internal byte[] LeftContent { get; set; }

        /// <summary>
        /// 解码器解码后得到的结果
        /// </summary>
        internal object Result { get; set; }
    }

    internal class DecodeFunctions
    {
        internal delegate DecodeResult DecodeHandler(byte[] data);

        internal static DecodeResult Code(byte[] data)
        {


            return null;
        }
    }
}
