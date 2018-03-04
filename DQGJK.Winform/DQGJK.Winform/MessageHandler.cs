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
            //首先更新缓存中的UID
            SetCache(token);

            switch (token.Message.FunctionCode)
            {
                case "F2": return;//心跳包不做处理
                case "B0": B0(token); break;
                case "C0": C0(token); break;
                case "B1": B1(token); break;
                case "B2": break;
            }
        }

        /// <summary>
        /// 中心站召测数据
        /// </summary>
        /// <param name="token"></param>
        private static void B0(MessageToken token)
        {
            SaveMongoData(token.Message);
            //todo 更新环网柜时间，设备状态
        }

        /// <summary>
        /// 终端机自报数据
        /// </summary>
        private static void C0(MessageToken token)
        {
            Response(token);
            SaveMongoData(token.Message);
            //todo 更新环网柜时间，设备状态
        }


        private static void B1(MessageToken token)
        {
            B1Data data = new B1Data(token.Message);
            MongoHandler.Save(data);
            //mongodb不能直接保存内嵌对象 所以需要更新一下
            data.UpdateData();
        }
        #region 消息处理方法

        #region B0/C0
        /// <summary>
        /// 查询缓存，是否存在相同code的UID，更新UID
        /// </summary>
        /// <param name="token"></param>
        internal static void SetCache(MessageToken token)
        {
            RecieveMessage message = token.Message;
            object cache = CacheUtil.GetCache(message.ClentCodeStr);

            //如果IP地址变化，则关闭之前的连接
            if (cache != null && !cache.ToString().Equals(token.UID))
            {
                try
                {
                    Main.listener.CloseClientSocket(cache.ToString());
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("关闭连接时发生错误", ex.Message, ex.StackTrace);
                }
            }

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
            res.Serial = 0;
            res.FunctionCode = "C0";

            Main.listener.Send(token.UID, res.ToByte());
        }

        /// <summary>
        /// 在mongodb中保存日志数据
        /// </summary>
        internal static void SaveMongoData(RecieveMessage message)
        {
            B0C0Data data = new B0C0Data(message);
            MongoHandler.Save(data);
            //mongodb不能直接保存内嵌对象 所以需要更新一下
            data.UpdateData();
        }
        #endregion

        #endregion
    }
}
