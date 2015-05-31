using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SampleApplication.Domain.Entities;

namespace SampleApplication.Web.Models
{
    public class InvoiceViewModel
    {
        public int? InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public List<ItemViewModel> ItemList { get; set; }
        public string Description { get; set; }

        public decimal Total
        {
            get
            {
                decimal total = 0;
                if (ItemList != null) total = ItemList.Sum(t => t.Net - t.Tax);
                return total;
            }
        }

        public decimal Net
        {
            get
            {
                decimal net = 0;
                if (ItemList != null) net = ItemList.Sum(t => t.Net);
                return net;
            }
        }

        public decimal Tax
        {
            get
            {
                decimal tax = 0;
                if (ItemList != null) tax = ItemList.Sum(t => t.Tax);
                return tax;
            }
        }
    }

    public class InvoiceFilterViewModel
    {
        private string _sortColumn;
        private string _order;
        private int _pageNumber;
        private int _pageSize;

        public InvoiceFilterViewModel()
        {
            _sortColumn = "InvoiceId";
            _order = "ASC";
            _pageNumber = 1;
            _pageSize = 10;
        }
        public string Find { get; set; }

        public int? InvoiceId { get; set; }
        public int? ClientId { get; set; }
        public decimal? TotalFrom { get; set; }
        public decimal? TotalTo { get; set; }

        public decimal? NetFrom { get; set; }
        public decimal? NetTo { get; set; }
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = value; }
        }

        public string SortColumn
        {
            get { return _sortColumn; }
            set { _sortColumn = value; }
        }

        public string Order
        {
            get { return _order; }
            set { _order = value; }
        }
    }
}