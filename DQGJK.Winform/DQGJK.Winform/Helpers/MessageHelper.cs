using DQGJK.Message;
using DQGJK.Winform.Models;
using DQGJK.Winform.Operates;
using System;
using System.Collections.Generic;

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

            try
            {
                IOperate operate = OperateFactory.Create(item.FunctionCode, subList, sendTime);
                SendMessage msg = operate.Handle(ref item);
                Main.listener.Send(Main.online[item.ClientCode], msg.ToByte());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("创建并下发命令时出错", ex.Message, ex.StackTrace);
            }

            return item;
        }
    }
}
