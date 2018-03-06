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

    //终端机自报数据或者中心站召测数据
    internal class B0C0Data : MongoData
    {
        internal B0C0Data(RecieveMessage message)
        {
            Id = new ObjectId();
            ClientCode = message.ClentCodeStr;
            Content = message.Content;
            SendTime = message.SendTime;
            DataTime = message.DataTime;
            IsChecked = message.IsChecked;
            Data = (List<B0C0Element>)message.Data;
        }

        public DateTime DataTime { get; set; }

        public List<B0C0Element> Data { get; set; }
    }

    //中心站遥控设备除湿机以及继电器
    internal class B1Data : MongoData
    {
        internal B1Data(RecieveMessage message)
        {
            Id = new ObjectId();
            ClientCode = message.ClentCodeStr;
            Content = message.Content;
            SendTime = message.SendTime;
            IsChecked = message.IsChecked;
            Data = (List<B1Element>)message.Data;
        }

        public List<B1Element> Data { get; set; }
    }

    internal class B2Data : MongoData
    {
        internal B2Data(RecieveMessage message)
        {
            Id = new ObjectId();
            ClientCode = message.ClentCodeStr;
            Content = message.Content;
            SendTime = message.SendTime;
            IsChecked = message.IsChecked;
            Data = (List<B2Element>)message.Data;
        }

        public List<B2Element> Data { get; set; }
    }

    internal class B3Data : MongoData
    {
        internal B3Data(RecieveMessage message)
        {
            Id = new ObjectId();
            ClientCode = message.ClentCodeStr;
            Content = message.Content;
            SendTime = message.SendTime;
            IsChecked = message.IsChecked;
            Data = (B3Element)message.Data;
        }

        public B3Element Data { get; set; }
    }
}
