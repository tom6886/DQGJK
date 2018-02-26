namespace DQGJK.Message
{
    public class Element
    {
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

        /// <summary>
        /// 数据长度
        /// </summary>
        public int DataLength { get; set; }
    }

    public class DeviceState
    {
        /// <summary>
        /// 继电器1输出状态
        /// </summary>
        public int RelayOne { get; set; }

        /// <summary>
        /// 继电器2输出状态
        /// </summary>
        public int RelayTwo { get; set; }

        /// <summary>
        /// 湿度超限报警状态
        /// </summary>
        public int HumidityAlarm { get; set; }

        /// <summary>
        /// 温度超限报警状态
        /// </summary>
        public int TemperatureAlarm { get; set; }

        /// <summary>
        /// 除湿状态
        /// </summary>
        public int Dehumidify { get; set; }
    }
}
