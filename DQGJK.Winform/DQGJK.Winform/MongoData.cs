using DQGJK.Message;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace DQGJK.Winform
{
    internal class MongoData
    {
        public ObjectId Id { get; set; }

        public string ClientCode { get; set; }

        public string Content { get; set; }

        public DateTime SendTime { get; set; }

        public bool IsChecked { get; set; }
    }

    //心跳包，不保存日志数据
    internal class F2Data : MongoData { }

    //终端机自报数据
    internal class C0Data : MongoData
    {
        internal C0Data(RecieveMessage message)
        {
            Id = new ObjectId();
            ClientCode = message.ClentCodeStr;
            Content = message.Content;
            SendTime = message.SendTime;
            IsChecked = message.IsChecked;
            Data = message.Data;
        }

        internal List<Element> Data { get; set; }
    }
}
