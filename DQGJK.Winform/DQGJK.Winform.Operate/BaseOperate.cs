using DQGJK.Message;
using DQGJK.Winform.Models;
using System;
using System.Collections.Generic;

namespace DQGJK.Winform.Operates
{
    abstract class BaseOperate : IDeviceOperate, IOperate
    {
        public DateTime SendTime { get; set; }

        public List<DeviceOperate> DeviceOperates { get; set; }

        public SendMessage Handle(ref Operate opeate)
        {
            SendMessage msg = new SendMessage()
            {
                CenterCode = 0x01,
                ClientCode = BCDUtil.ConvertFrom(opeate.ClientCode, 6),
                SendTime = SendTime,
                Serial = 0,
                FunctionCode = opeate.FunctionCode
            };

            msg.Body = GetBody();

            opeate.State = OperateState.Sended;
            opeate.Content = BytesUtil.ToHexString(msg.ToByte());
            opeate.SendTime = SendTime;

            return msg;
        }

        public abstract byte[] GetBody();
    }
}
