using System;

namespace DQGJK.Web.PageModels
{
    public class Pager
    {
        public Pager(double _TotalCount, int _PageIndex, int _PageSize)
        {
            PageSize = _PageSize;
            TotalCount = _TotalCount;
            PageCount = Convert.ToInt32(Math.Ceiling(_TotalCount / (this.PageSize == 0 ? 10 : this.PageSize)));
            PageIndex = _PageIndex > PageCount ? PageCount : _PageIndex;
            PrevIndex = PageIndex == 1 ? 1 : PageIndex - 1;
            NextIndex = PageIndex == PageCount ? PageCount : PageIndex + 1;
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
