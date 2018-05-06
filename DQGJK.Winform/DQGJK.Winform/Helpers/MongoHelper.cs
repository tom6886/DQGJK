using DQGJK.Winform.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DQGJK.Winform
{
    internal class MongoHelper
    {
        public static void StatMongoData(DateTime date)
        {
            int count = GetDataCount(date);

            if (count > 0) { return; }

            DateTime nextDate = date.AddDays(1);

            string sDate = (DateTime.Parse(date.ToString("yyyy-MM-dd"))).ToString("u");

            string sNDate = (DateTime.Parse(nextDate.ToString("yyyy-MM-dd"))).ToString("u");

            var m_list = GetMaxMinAvgStat(sDate, sNDate);

            List<CabinetData> datas = new List<CabinetData>();

            foreach (var item in m_list)
            {
                MaxMinAvgStat stat = JsonConvert.DeserializeObject<MaxMinAvgStat>(item.ToJson());

                CabinetData data = new CabinetData(date);
                data.ClientCode = stat._id.Client;
                data.DeviceCode = stat._id.Device;
                data.MaxHumidity = Convert.ToDecimal(stat.maxHum);
                data.MinHumidity = Convert.ToDecimal(stat.minHum);
                data.AverageHumidity = Convert.ToDecimal(Math.Round(stat.avgHum, 2));
                data.MaxTemperature = Convert.ToDecimal(stat.maxTem);
                data.MinTemperature = Convert.ToDecimal(stat.minTem);
                data.AverageTemperature = Convert.ToDecimal(Math.Round(stat.avgTem, 2));

                datas.Add(data);
            }

            var a_list = GetAlarmStat(sDate, sNDate);

            foreach (var item in a_list)
            {
                AlarmStat stat = JsonConvert.DeserializeObject<AlarmStat>(item.ToJson());

                CabinetData data = datas.Where(q => q.ClientCode.Equals(stat._id.Client) && q.DeviceCode.Equals(stat._id.Device)).FirstOrDefault();

                if (data == null) { continue; }

                data.HumidityAlarm = stat.HumAlarm;
                data.TemperatureAlarm = stat.TemAlarm;
            }

            UpdateSql(datas);
        }

        private static int GetDataCount(DateTime date)
        {
            using (DBContext db = new DBContext())
            {
                return db.CabinetData.Where(q => q.Year == date.Year && q.Month == date.Month && q.Day == date.Day).Count();
            }
        }

        private static List<BsonDocument> GetMaxMinAvgStat(string sDate, string sNDate)
        {
            var stages = new List<IPipelineStageDefinition>();
            //根据日期筛选出数据
            stages.Add(new JsonPipelineStageDefinition<BsonDocument, BsonDocument>("{$match:{IsChecked:true,SendTime:{$gte:new Date(\"" + sDate + "\"),$lte:new Date(\"" + sNDate + "\")}}}"));
            //拆分嵌套文件
            stages.Add(new JsonPipelineStageDefinition<BsonDocument, BsonDocument>("{$unwind:\"$Data\"}"));
            //过滤无效数据
            stages.Add(new JsonPipelineStageDefinition<BsonDocument, BsonDocument>("{$match:{\"Data.Valid\":true}}"));
            //统计数据
            stages.Add(new JsonPipelineStageDefinition<BsonDocument, BsonDocument>("{$group:{ _id:{Client:\"$ClientCode\",Device:\"$Data.Code\"},maxHum:{$max:\"$Data.Humidity\"},minHum:{$min:\"$Data.Humidity\"},avgHum:{ $avg:\"$Data.Humidity\"},maxTem:{$max:\"$Data.Temperature\"},minTem:{$min:\"$Data.Temperature\"},avgTem:{$avg:\"$Data.Temperature\"}}}"));

            var pipeline = new PipelineStagePipelineDefinition<BsonDocument, BsonDocument>(stages);

            return MongoHandler.GetBsonCollection<B0C0Data>().AggregateAsync(pipeline).Result.ToList();
        }

        private static List<BsonDocument> GetAlarmStat(string sDate, string sNDate)
        {
            var stages = new List<IPipelineStageDefinition>();
            //根据日期筛选出数据
            stages.Add(new JsonPipelineStageDefinition<BsonDocument, BsonDocument>("{$match:{IsChecked:true,SendTime:{$gte:new Date(\"" + sDate + "\"),$lte:new Date(\"" + sNDate + "\")}}}"));
            //拆分嵌套文件
            stages.Add(new JsonPipelineStageDefinition<BsonDocument, BsonDocument>("{$unwind:\"$Data\"}"));
            //统计数据
            stages.Add(new JsonPipelineStageDefinition<BsonDocument, BsonDocument>("{$group:{ _id:{Client:\"$ClientCode\",Device:\"$Data.Code\"},HumAlarm:{$sum:\"$Data.State.HumidityAlarm\"},TemAlarm:{$sum:\"$Data.State.TemperatureAlarm\"}}}"));

            var pipeline = new PipelineStagePipelineDefinition<BsonDocument, BsonDocument>(stages);

            return MongoHandler.GetBsonCollection<B0C0Data>().AggregateAsync(pipeline).Result.ToList();
        }

        private static void UpdateSql(List<CabinetData> data)
        {
            using (DBContext db = new DBContext())
            {
                db.CabinetData.AddRange(data);
                db.SaveChanges();
            }
        }
    }
}
