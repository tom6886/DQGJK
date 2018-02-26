using DQGJK.Message;
using System;

namespace DQGJK.Winform
{
    internal class MessageToken
    {
        internal string UID { get; set; }

        internal RecieveMessage Message { get; set; }
    }

    //对服务端接收到的消息进行业务处理
    internal class MessageHandler
    {
        internal static void Set(MessageToken token)
        {
            switch (token.Message.FunctionCode)
            {
                case "F2": return;
                case "B0": break;
                case "C0": C0(token); break;
                case "B1": break;
                case "B2": break;
            }
        }

        /// <summary>
        /// 终端机自报数据
        /// </summary>
        private static void C0(MessageToken token)
        {
            SetCache(token);

            Response(token);

            SaveMongoData(token.Message);
        }

        #region 消息处理方法

        #region C0
        /// <summary>
        /// 查询缓存，是否存在相同code的UID，更新UID
        /// </summary>
        /// <param name="token"></param>
        internal static void SetCache(MessageToken token)
        {
            RecieveMessage message = token.Message;
            object cache = CacheUtil.GetCache(message.ClentCodeStr);
            CacheUtil.SetCache(message.ClentCodeStr, token.UID);
        }

        /// <summary>
        /// 回复终端机消息
        /// </summary>
        /// <param name="token"></param>
        internal static void Response(MessageToken token)
        {
            RecieveMessage message = token.Message;
            SendMessage res = new SendMessage();
            res.ClientCode = message.ClientCode;
            res.CenterCode = message.CenterCode;
            res.SendTime = DateTime.Now;
            res.Serial = message.Serial + 1;
            res.FunctionCode = "C0";

            Main.listener.Send(token.UID, res.ToByte());
        }

        /// <summary>
        /// 在mongodb中保存日志数据
        /// </summary>
        internal static void SaveMongoData(RecieveMessage message)
        {
            C0Data data = new C0Data(message);
            MongoHandler.Save(data);
        }
        #endregion

        #endregion
    }
}
