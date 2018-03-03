namespace DQGJK.Message
{
    public class B0C0Element
    {
        public B0C0Element(Element element)
        {
            Code = element.Code;
            Humidity = element.Humidity;
            Temperature = element.Temperature;
            State = element.State;
        }

        /// <summary>
        /// 主从机地址
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 湿度
        /// </summary>
        public decimal Humidity { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        public decimal Temperature { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public DeviceState State { get; set; }
    }
}
