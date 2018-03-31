using DQGJK.Winform.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DQGJK.Winform.Operates
{
    public class OperateFactory
    {
        public static IOperate Create(string FunctionCode, List<DeviceOperate> deviceOperates, DateTime sendTime)
        {
            IOperateFactory factory = (IOperateFactory)Assembly.Load("DQGJK.Winform.Operates").CreateInstance("DQGJK.Winform.Operates." + FunctionCode + "OperateFactory");
            return factory.CreateOperate(deviceOperates, sendTime);
        }
    }

    interface IOperateFactory
    {
        IOperate CreateOperate(List<DeviceOperate> deviceOperates, DateTime sendTime);
    }

    public class B0OperateFactory : IOperateFactory
    {
        public IOperate CreateOperate(List<DeviceOperate> deviceOperates, DateTime sendTime)
        {
            return new B0Operate(sendTime);
        }
    }

    public class B1OperateFactory : IOperateFactory
    {
        public IOperate CreateOperate(List<DeviceOperate> deviceOperates, DateTime sendTime)
        {
            return new B1Operate(deviceOperates, sendTime);
        }
    }

    public class B2OperateFactory : IOperateFactory
    {
        public IOperate CreateOperate(List<DeviceOperate> deviceOperates, DateTime sendTime)
        {
            return new B2Operate(deviceOperates, sendTime);
        }
    }
}
