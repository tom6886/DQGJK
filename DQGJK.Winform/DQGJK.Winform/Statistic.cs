using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            var stages = new List<IPipelineStageDefinition>();
            //根据日期筛选出数据
            stages.Add(new JsonPipelineStageDefinition<BsonDocument, BsonDocument>("{$match:{IsChecked:true,SendTime:{$gte:'" + date.ToString("yyyy-MM-dd") + " 00:00:00',$lte:'" + date.AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00'}}}"));
            //拆分嵌套文件
            stages.Add(new JsonPipelineStageDefinition<BsonDocument, BsonDocument>("{$unwind:\"$Data\"}"));
            //过滤无效数据
            stages.Add(new JsonPipelineStageDefinition<BsonDocument, BsonDocument>("{$match:{\"Data.Valid\":true}}"));
            //统计数据
            stages.Add(new JsonPipelineStageDefinition<BsonDocument, BsonDocument>("{$group:{ _id:{Client:\"$ClientCode\",DeviceCode:\"$Data.Code\"},maxHum:{$max:\"$Data.Humidity\"},minHum:{$min:\"$Data.Humidity\"},avgHum:{ $avg:\"$Data.Humidity\"},maxTem:{$max:\"$Data.Temperature\"},minTem:{$min:\"$Data.Temperature\"},avgTem:{$avg:\"$Data.Temperature\"}}}"));

            var pipeline = new PipelineStagePipelineDefinition<BsonDocument, BsonDocument>(stages);

            var list = MongoHandler.GetBsonCollection<B0C0Data>().AggregateAsync(pipeline).Result.ToList();


        }
    }
}
