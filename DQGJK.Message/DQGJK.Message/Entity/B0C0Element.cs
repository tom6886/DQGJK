namespace DQGJK.Message
{
    public class B0C0Element : IElement
    {
        public B0C0Element(Element element)
        {
            Code = element.Code;
            Humidity = element.Humidity;
            Temperature = element.Temperature;
            State = element.State;
            Valid = ((Temperature > -50 && Temperature < 100) && (Humidity > 0 && Humidity < 100));
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
        /// 数据有效性
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public DeviceState State { get; set; }
    }
}
