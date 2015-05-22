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
        public Sort(string columnName, string order = null)
        {
            ColumnName = columnName;
            ColumnOrder = !String.IsNullOrWhiteSpace(order) && order.Trim().ToUpper() == "DESC" ? Order.Descending : Order.Ascending;
        }

        public string ColumnName { get; private set; }

        public Order ColumnOrder { get; private set; }
    }
}
