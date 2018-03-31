using System;

namespace DQGJK.Winform.Operates
{
    class B0Operate : BaseOperate
    {
        public B0Operate(DateTime sendTime)
        {
            SendTime = sendTime;
        }

        public override byte[] GetBody()
        {
            return null;
        }
    }
}
