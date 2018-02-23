using System.Collections.Generic;

namespace DQGJK.Web.PageModels
{
    public class Pboard
    {
        public string Name { get; set; }

        public BoardConfig Config { get; set; }
    }

    public class BoardConfig
    {
        public int Min { get; set; }

        public int Max { get; set; }

        public string Title { get; set; }

        public List<PlotBand> PlotBands { get; set; }
    }

    public class PlotBand
    {
        public int From { get; set; }

        public int To { get; set; }

        public string Color { get; set; }
    }
}
