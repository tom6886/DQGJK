using DQGJK.Message;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DQGJK.Winform
{
    internal class MessageHelper
    {
        internal static Operate OperateHandle(Operate item, List<DeviceOperate> subList, DateTime sendTime)
        {
            item.RetryCount += 1;

            //只要发送次数超过5次，则视为发送失败
            if (item.RetryCount > 5) { item.State = OperateState.Error; return item; }

            if (!Main.online.ContainsKey(item.ClientCode) || string.IsNullOrEmpty(Main.online[item.ClientCode])) { return item; }

            SendMessage msg = new SendMessage()
            {
                CenterCode = 0x01,
                ClientCode = BCDUtil.ConvertFrom(item.ClientCode, 6),
                SendTime = sendTime,
                Serial = 0,
                FunctionCode = item.FunctionCode
            };

            if (item.FunctionCode.Equals("B1"))
            {
                msg.Body = ConvertToB1(subList.Where(q => q.OperateID.Equals(item.ID)).ToList());
            }
            else if (item.FunctionCode.Equals("B2"))
            {
                msg.Body = ConvertToB2(subList.Where(q => q.OperateID.Equals(item.ID)).ToList());
            }

            byte[] content = msg.ToByte();
            item.State = OperateState.Sended;
            item.Content = BytesUtil.ToHexString(content);
            item.SendTime = sendTime;

            Main.listener.Send(Main.online[item.ClientCode], content);

            return item;
        }

        internal static byte[] ConvertToB1(List<DeviceOperate> operates)
        {
            List<byte> list = new List<byte>();

            foreach (var item in operates)
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

        internal static byte[] ConvertToB2(List<DeviceOperate> operates)
        {
            List<byte> list = new List<byte>();

            foreach (var item in operates)
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
