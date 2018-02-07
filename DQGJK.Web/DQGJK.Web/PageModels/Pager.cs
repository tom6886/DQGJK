using System;

namespace DQGJK.Web.PageModels
{
    public class Pager
    {
        public Pager(double _TotalCount, int _PageIndex, int _PageSize)
        {
            this.PageSize = _PageSize;
            this.TotalCount = _TotalCount;
            this.PageCount = Convert.ToInt32(Math.Ceiling(_TotalCount / (this.PageSize == 0 ? 10 : this.PageSize)));
            this.PageIndex = _PageIndex;
            this.PrevIndex = _PageIndex == 1 ? 1 : _PageIndex - 1;
            this.NextIndex = _PageIndex == PageCount ? PageCount : _PageIndex + 1;
        }

        public Pager(double _TotalCount, int _PageIndex)
            : this(_TotalCount, _PageIndex, 10)
        {

        }

        public int PageSize { get; private set; }

        public double TotalCount { get; private set; }

        public int PageCount { get; private set; }

        public int PageIndex { get; private set; }

        public int PrevIndex { get; private set; }

        public int NextIndex { get; private set; }
    }
}
