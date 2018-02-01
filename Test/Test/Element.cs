namespace Test
{
    internal class Element
    {
        /// <summary>
        /// 主从机地址
        /// </summary>
        internal string Code { get; set; }

        /// <summary>
        /// 湿度
        /// </summary>
        internal decimal Humidity { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        internal decimal Temperature { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        internal DeviceState State { get; set; }

        /// <summary>
        /// 数据长度
        /// </summary>
        internal int DataLength { get; set; }
    }

    internal class DeviceState
    {
        /// <summary>
        /// 继电器1输出状态
        /// </summary>
        internal int RelayOne { get; set; }

        /// <summary>
        /// 继电器2输出状态
        /// </summary>
        internal int RelayTwo { get; set; }

        /// <summary>
        /// 湿度超限报警状态
        /// </summary>
        internal int HumidityAlarm { get; set; }

        /// <summary>
        /// 温度超限报警状态
        /// </summary>
        internal int TemperatureAlarm { get; set; }

        /// <summary>
        /// 除湿状态
        /// </summary>
        internal int Dehumidify { get; set; }
    }
}
