namespace DQGJK.Message
{
    public class B3Element
    {
        public byte CenterCode { get; set; }

        public byte[] ClientCode { get; set; }

        public string ClentCodeStr { get { return BytesUtil.ToHexString(ClientCode); } }

        public string IPPort { get; set; }

        public int Interval { get; set; }
    }
}
