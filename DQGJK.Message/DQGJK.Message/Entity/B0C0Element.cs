namespace DQGJK.Message
{
    public class B0C0Element : IElement
    {
        public B0C0Element(Element element)
        {
            Code = element.Code;
            Humidity = element.Humidity;
            Temperature = element.Temperature;
            HumidityLimit = element.HumidityLimit;
            TemperatureLimit = element.TemperatureLimit;
            State = element.State;
            Valid = ((Temperature > -50 && Temperature < 100)
                && (Humidity > 0 && Humidity < 100)
                && (HumidityLimit > 0 && HumidityLimit < 100)
                && (TemperatureLimit > -50 && TemperatureLimit < 100));
        }

        /// <summary>
        /// 主从机地址
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 湿度
        /// </summary>
        public double Humidity { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// 湿度
        /// </summary>
        public double HumidityLimit { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        public double TemperatureLimit { get; set; }

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
