namespace DQGJK.Message
{
    public class B1Element: IElement
    {
        public B1Element(Element element)
        {
            Code = element.Code;
            State = element.State;
        }

        /// <summary>
        /// 主从机地址
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public DeviceState State { get; set; }
    }
}
