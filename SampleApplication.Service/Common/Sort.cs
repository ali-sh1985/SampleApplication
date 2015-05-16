using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApplication.Service.Common
{
    public enum Order
    {
        Ascending,
        Descending
    }
    public class Sort
    {
        
        public string ColumnName { get; set; }
        public Order ColumnOrder { get; set; }
    }
}
