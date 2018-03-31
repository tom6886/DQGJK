using DQGJK.Message;
using System;

namespace DQGJK.Winform.Operates
{
    public interface IOperate
    {
        DateTime SendTime { get; set; }

        SendMessage Handle(ref Models.Operate operate);
    }
}
