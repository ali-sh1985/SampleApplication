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

        public Paging(int? pageNumber, int? pageSize)
        {
            PageIndex = !pageNumber.HasValue || pageNumber < 1 ? 0 : pageNumber.Value - 1;
            PageSize = !pageSize.HasValue || pageSize < 1 ? 10 : pageSize.Value;
        }
    }
}
