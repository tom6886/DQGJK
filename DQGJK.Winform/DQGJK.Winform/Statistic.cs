using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DQGJK.Winform
{
    public partial class Statistic : Form
    {
        public Statistic()
        {
            InitializeComponent();
        }

        private async void btn_start_Click(object sender, EventArgs e)
        {
            //if (dt_start.Value > dt_end.Value) { MessageBox.Show("开始日期不得大于结束日期"); return; }

            //if (dt_end.Value > DateTime.Now) { MessageBox.Show("结束日期不得大于当前日期"); return; }

            int length = Convert.ToInt16((dt_end.Value - dt_start.Value).TotalDays);

            for (int i = 0; i < length; i++)
            {
                await Task.Factory.StartNew(() => StatMongoData(dt_start.Value.AddDays(i)));
            }
        }

        private void StatMongoData(DateTime date)
        {
            AppendLog("开始更新" + date.ToShortDateString() + "的数据\r\n");

            int count = GetDataCount(date);

            if (count > 0) { AppendLog("当日已存在统计数据，不再重复统计\r\n"); return; }

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

            AppendLog("获取数据" + datas.Count + " 条，准备推送到数据库\r\n");
            UpdateSql(datas);
        }

        private List<BsonDocument> GetMaxMinAvgStat(string sDate, string sNDate)
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

        private List<BsonDocument> GetAlarmStat(string sDate, string sNDate)
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

        private int GetDataCount(DateTime date)
        {
            using (DBContext db = new DBContext())
            {
                return db.CabinetData.Where(q => q.Year == date.Year && q.Month == date.Month && q.Day == date.Day).Count();
            }
        }

        private void UpdateSql(List<CabinetData> data)
        {
            using (DBContext db = new DBContext())
            {
                db.CabinetData.AddRange(data);
                db.SaveChanges();
            }

            AppendLog("推送完成\r\n");
        }

        #region 操作UI
        private delegate void appendLog(string log);

        private void AppendLog(string log)
        {
            if (edit_log.InvokeRequired)
            {
                BeginInvoke(new appendLog(AppendLog), log);
            }
            else
            {
                edit_log.MaskBox.AppendText(log);
            }
        }
        #endregion
    }
}
