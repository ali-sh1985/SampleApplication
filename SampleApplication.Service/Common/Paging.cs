using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApplication.Service.Common
{
    public class Paging
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }

        public Paging(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex < 1 ? 0 : pageIndex - 1;
            PageSize = pageSize;
        }
    }
}
