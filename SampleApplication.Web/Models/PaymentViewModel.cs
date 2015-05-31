using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SampleApplication.Domain.Enums;

namespace SampleApplication.Web.Models
{
    public class PaymentViewModel
    {
        public int? PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Total { get; set; }
        public PaymentMethod Method { get; set; }
        public string Description { get; set; }
        public int InvoiceId { get; set; }
    }

    public class PaymentFilterViewModel
    {
        private string _sortColumn;
        private string _order;
        private int _pageNumber;
        private int _pageSize;

        public PaymentFilterViewModel()
        {
            _sortColumn = "PaymentId";
            _order = "ASC";
            _pageNumber = 1;
            _pageSize = 10;
        }
        public int? ClientId { get; set; }

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