using DQGJK.Message;
using DQGJK.Winform.Models;
using System;
using System.Collections.Generic;

namespace DQGJK.Winform.Operates
{
    class B1Operate : BaseOperate
    {
        public B1Operate(List<DeviceOperate> deviceOperates, DateTime sendTime)
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
                ele.State = new DeviceState()
                {
                    RelayOne = item.RelayOne,
                    RelayTwo = item.RelayTwo,
                    Dehumidify = item.Dehumidify
                };
                list.AddRange(new B1Element(ele).ToByte());
            }

            return list.ToArray();
        }
    }
}
