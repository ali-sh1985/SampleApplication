using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SampleApplication.Domain;
using SampleApplication.Data.EntityFramework;
using SampleApplication.Domain.Entities;
using SampleApplication.Domain.Enums;
using SampleApplication.Domain.Repositories;
using SampleApplication.Service.Common;
using SampleApplication.Service.SearchCriterias;
using SampleApplication.Service.Services;
using SampleApplication.Web.Models;

namespace SampleApplication.Web.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IInvoiceService _invoiceService;

        public ClientController(IClientService clientService, IInvoiceService invoiceService)
        {
            _clientService = clientService;
            _invoiceService = invoiceService;
        }

        //
        // GET: Client
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: Client/List
        public ActionResult List()
        {
            var result = _clientService.GetAll();

            return Json(result.Select(c => new
            {
                clientId = c.ClientId,
                name = c.Name,
                totalInvoiced = c.TotalInvoiced,
                totalPaid = c.TotalPaid,
                balance = c.Balance
            }), JsonRequestBehavior.AllowGet);
        }

        //
        // Post: Client/List
        [HttpPost]
        public ActionResult List(ClientFilterViewModel filter, int? pageNumber, int? pageSize, string sortColumn, string order)
        {
            var clientSearchCriteria = new ClientSearchCriteria()
            {
                Find = filter.Find,
                InvoiceFrom = filter.InvoiceFrom,
                InvoiceTo = filter.InvoiceTo,
                PaymentFrom = filter.TotalPaidFrom,
                PaymentTo = filter.TotalPaidTo,
                BalanceFrom = filter.BalanceFrom,
                BalanceTo = filter.BalanceTo,
                Currency = filter.DisplayCurrency
            };
            var result = _clientService.Search(clientSearchCriteria, new Paging(pageNumber, pageSize), new Sort(sortColumn, order));

            return Json(result.Select(c => new
            {
                clientId = c.ClientId,
                name = c.Name,
                totalInvoiced = c.TotalInvoiced,
                totalPaid = c.TotalPaid,
                balance = c.Balance
            }), JsonRequestBehavior.AllowGet);
        }

        // GET: Client/Create
        public ActionResult CreateEdit(int? id)
        {
            var listOfCountries = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new SelectListItem
            {
                Value = new RegionInfo(x.LCID).EnglishName,
                Text = new RegionInfo(x.LCID).EnglishName,
            })
                         .GroupBy(c => c.Value)
                         .Select(c => c.First())
                         .OrderBy(x => x.Text);

            ViewBag.CountriesList = listOfCountries;

            return View();
        }

        // POST: Client/Create
        [HttpPost]
        public ActionResult CreateEdit(ClientViewModel model)
        {
            var client = new Client()
            {
                ClientId = model.ClientId,
                Name = model.Name,
                Company = model.Company,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                DefaultCurrency = model.DefaultCurrency,
                Country = model.Country,
                Town = model.Town,
                Postcode = model.Postcode
            };

            if (client.ClientId > 0 )
            {
                _clientService.Update(client);
            }
            else
            {
                _clientService.Add(client);
            }
            

            return Json("OK");
        }

        // GET: Client/Edit/1
        public ActionResult Edit(int id)
        {
            return View();
        }

        // GET: Client/Detail/1
        public ActionResult Detail(int id)
        {
            Client client = _clientService.Find(id);
            ClientViewModel clientViewModel = new ClientViewModel()
            {
                ClientId = client.ClientId,
                Name = client.Name,
                Company = client.Company,
                AddressLine1 = client.AddressLine1,
                AddressLine2 = client.AddressLine2,
                Postcode = client.Postcode,
                Town = client.Town,
                Country = client.Country,
                DefaultCurrency = client.DefaultCurrency
            };

            return View(clientViewModel);
        }

        // POST: Client/Detail/1
        [HttpPost]
        public ActionResult Detail(int id, ClientViewModel model)
        {
            return View();
        }

        // POST: Client/Delete/1
        [HttpPost]
        public ActionResult Delete(int id)
        {
            _clientService.Remove(id);
            return Json("ok");
        }

        // POST: Client/Balance/1
        public ActionResult Balance(int id)
        {
            var invoiceList = _invoiceService.GetInvoiceListByClient(id);

            List<BalanceSheet> balanceSheets = new List<BalanceSheet>();
            foreach (var invoice in invoiceList)
            {
                balanceSheets.Add(new BalanceSheet()
                {
                    Date = invoice.Date,
                    Type = "Invoice",
                    Reference = invoice.InvoiceId,
                    Invoiced = invoice.Total,
                    Paid = 0,
                    Balance = invoice.Total,

                });
                var balance = invoice.Total;
                foreach (var payment in invoice.PaymentList)
                {
                    balance = balance - payment.Total;
                    balanceSheets.Add(new BalanceSheet()
                    {
                        Date = payment.PaymentDate,
                        Type = "Payment",
                        Reference = payment.PaymentId,
                        Invoiced = 0,
                        Paid = payment.Total,
                        Balance = balance,
                    });
                }
            }


            return Json(balanceSheets, JsonRequestBehavior.AllowGet);
        }

        // POST: Client/GetClient/1
        public ActionResult GetClient(int id)
        {
            var client = _clientService.Find(id);
            var clientViewMode = new ClientViewModel()
            {
                ClientId = client.ClientId,
                Name = client.Name,
                Company = client.Company,
                AddressLine1 = client.AddressLine1,
                AddressLine2 = client.AddressLine2,
                Postcode = client.Postcode,
                Town = client.Town,
                Country = client.Country,
                DefaultCurrency = client.DefaultCurrency
            };
            return Json(clientViewMode, JsonRequestBehavior.AllowGet);
        }
    }

    public class BalanceSheet
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public int Reference { get; set; }
        public decimal Invoiced { get; set; }
        public decimal Paid { get; set; }
        public decimal Balance { get; set; }
    }
}