namespace DQGJK.Message
{
    public class B2Element : IElement
    {
        public B2Element(Element element)
        {
            Code = element.Code;
            HumidityLimit = element.HumidityLimit;
            TemperatureLimit = element.TemperatureLimit;
        }

        /// <summary>
        /// 主从机地址
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 湿度阈值
        /// </summary>
        public double HumidityLimit { get; set; }

        /// <summary>
        /// 温度阈值
        /// </summary>
        public double TemperatureLimit { get; set; }
    }
}
