using SampleApplication.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SampleApplication.Domain.Entities;

namespace SampleApplication.Web.Models
{
    public class ClientViewModel
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Postcode { get; set; }
        public Currency DefaultCurrency { get; set; }
        public List<SelectListItem> CurrenciesList { get; set; }
        public List<SelectListItem> CountriesList { get; set; }
    }

    public class ClientFilterViewModel
    {
        private string _sortColumn;
        private string _order;
        private int _pageNumber;
        private int _pageSize;

        public ClientFilterViewModel()
        {
            _sortColumn = "ClientId";
            _order = "ASC";
            _pageNumber = 1;
            _pageSize = 10;
        }

        public string Find { get; set; }
        public decimal? InvoiceFrom { get; set; }
        public decimal? InvoiceTo { get; set; }
        public decimal? TotalPaidFrom { get; set; }
        public decimal? TotalPaidTo { get; set; }
        public decimal? BalanceFrom { get; set; }
        public decimal? BalanceTo { get; set; }
        public Currency DisplayCurrency { get; set; }

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