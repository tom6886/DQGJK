using DQGJK.Message;
using MongoDB.Bson;
using MongoDB.Driver;
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
            IsChecked = message.IsChecked;
            Data = (List<B0C0Element>)message.Data;
        }

        internal List<B0C0Element> Data { get; set; }

        internal void UpdateData()
        {
            var _collection = MongoHandler.GetCollection<B0C0Data>();
            var _filter = Builders<B0C0Data>.Filter.Eq(q => q.Id, this.Id);
            var _update = Builders<B0C0Data>.Update.Set("Data", this.Data);
            _collection.UpdateOneAsync(_filter, _update);
        }
    }

    internal class B1Data : MongoData
    {

    }
}
