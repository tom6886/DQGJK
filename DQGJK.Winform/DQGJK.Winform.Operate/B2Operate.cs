using DQGJK.Message;
using DQGJK.Winform.Models;
using System;
using System.Collections.Generic;

namespace DQGJK.Winform.Operates
{
    class B2Operate : BaseOperate
    {
        public B2Operate(List<DeviceOperate> deviceOperates, DateTime sendTime)
        {
            DeviceOperates = deviceOperates;
            SendTime = sendTime;
        }

        public override byte[] GetBody()
        {
            List<byte> list = new List<byte>();

            foreach (var item in DeviceOperates)
            {
                Element ele = new Element();
                ele.Code = item.DeviceCode;
                ele.HumidityLimit = Convert.ToDouble(item.HumidityLimit);
                ele.TemperatureLimit = Convert.ToDouble(item.TemperatureLimit);
                list.AddRange(new B2Element(ele).ToByte());
            }

            return list.ToArray();
        }
    }
}
