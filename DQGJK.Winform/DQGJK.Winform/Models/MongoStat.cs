using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQGJK.Winform
{
    public class MaxMinAvgStat
    {
        public DataID _id { get; set; }

        public double maxHum { get; set; }

        public double minHum { get; set; }

        public double avgHum { get; set; }

        public double maxTem { get; set; }

        public double minTem { get; set; }

        public double avgTem { get; set; }
    }

    public class AlarmStat
    {
        public DataID _id { get; set; }

        public int HumAlarm { get; set; }

        public int TemAlarm { get; set; }
    }

    public class DataID
    {
        public string Client { get; set; }

        public string Device { get; set; }
    }
}
