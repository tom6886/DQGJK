﻿using DQGJK.Message;
using System;

namespace DQGJK.Service
{
    //对服务端接收到的消息进行业务处理
    internal class MessageHandler
    {
        private string _UID { get; set; }

        private RecieveMessage _Message { get; set; }

        public delegate void OnIPChangedHandler(string UID);

        public event OnIPChangedHandler OnIPChanged;

        public delegate void OnMsgSendHandler(string UID, byte[] msg);

        public event OnMsgSendHandler OnMsgSend;

        internal MessageHandler(string UID, RecieveMessage Message)
        {
            _UID = UID;
            _Message = Message;
        }

        internal void Set()
        {
            //首先更新缓存中的UID
            SetCache();

            switch (_Message.FunctionCode)
            {
                case "F2": return;//心跳包不做处理
                case "B0": B0(); break;
                case "C0": C0(); break;
                case "B1": B1(); break;
                case "B2": B2(); break;
                case "B3": B3(); break;
            }
        }

        /// <summary>
        /// 中心站召测数据
        /// </summary>
        /// <param name="token"></param>
        private void B0()
        {
            MongoHandler.Save(new B0C0Data(_Message));
            //todo 更新环网柜时间，设备状态
        }

        /// <summary>
        /// 终端机自报数据
        /// </summary>
        private void C0()
        {
            Response();

            MongoHandler.Save(new B0C0Data(_Message));
            //todo 更新环网柜时间，设备状态
        }

        /// <summary>
        /// 中心站遥控设备
        /// </summary>
        private void B1()
        {
            MongoHandler.Save(new B1Data(_Message));
        }

        /// <summary>
        /// 修改终端机参数
        /// </summary>
        private void B2()
        {
            MongoHandler.Save(new B2Data(_Message));
        }

        /// <summary>
        /// 修改终端机通信参数
        /// </summary>
        private void B3()
        {
            MongoHandler.Save(new B3Data(_Message));
        }

        #region 消息处理方法

        /// <summary>
        /// 更新缓存，是否存在相同code的UID，更新UID
        /// </summary>
        /// <param name="token"></param>
        internal void SetCache()
        {
            object cache = CacheUtil.GetCache(_Message.ClentCodeStr);

            //如果IP地址变化，则关闭之前的连接
            if (cache != null && !cache.ToString().Equals(_UID))
            {
                OnIPChanged?.Invoke(cache.ToString());
            }

            CacheUtil.SetCache(_Message.ClentCodeStr, _UID);
        }

        #region B0/C0
        /// <summary>
        /// 回复终端机消息
        /// </summary>
        /// <param name="token"></param>
        internal void Response()
        {
            SendMessage res = new SendMessage();
            res.ClientCode = _Message.ClientCode;
            res.CenterCode = _Message.CenterCode;
            res.SendTime = DateTime.Now;
            res.Serial = 0;
            res.FunctionCode = "C0";

            OnMsgSend?.Invoke(_UID, res.ToByte());
        }

        #endregion

        #endregion
    }
}
