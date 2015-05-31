using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using SampleApplication.Domain;
using SampleApplication.Data.EntityFramework;
using SampleApplication.Domain.Entities;
using SampleApplication.Domain.Enums;
using SampleApplication.Domain.Repositories;
using SampleApplication.Service.Common;
using SampleApplication.Service.SearchCriterias;
using SampleApplication.Service.Services;
using SampleApplication.Web.Common;
using SampleApplication.Web.Models;

namespace SampleApplication.Web.Controllers
{
    [System.Web.Mvc.Authorize]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IInvoiceService _invoiceService;
        private readonly ICurrencyService _currencyService;

        public ClientController(IClientService clientService, IInvoiceService invoiceService, ICurrencyService currencyService)
        {
            _clientService = clientService;
            _invoiceService = invoiceService;
            _currencyService = currencyService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            try
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
            catch (Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }

        [System.Web.Mvc.HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult List(ClientFilterViewModel filter)
        {
            try
            {
                var clientSearchCriteria = new ClientSearchCriteria()
                {
                    Find = filter.Find,
                    InvoiceFrom = filter.InvoiceFrom,
                    InvoiceTo = filter.InvoiceTo,
                    PaymentFrom = filter.TotalPaidFrom,
                    PaymentTo = filter.TotalPaidTo,
                    BalanceFrom = filter.BalanceFrom,
                    BalanceTo = filter.BalanceTo
                };

                decimal currencyFactor = 1;
                if (filter.DisplayCurrency.ToString() != "GBP")
                {
                    currencyFactor = _currencyService.Convert(1, "GBP", filter.DisplayCurrency.ToString());
                }

                string currencyCode = filter.DisplayCurrency.ToString();

                var result = _clientService.Search(clientSearchCriteria, new Paging(filter.PageNumber, filter.PageSize), new Sort(filter.SortColumn, filter.Order));

                return Json(new
                {
                    TotalPages = result.TotalPageCount,
                    TotalItems = result.TotalCount,
                    PageNumber = result.PageIndex + 1,
                    Data = result.Select(c => new
                    {
                        clientId = c.ClientId,
                        name = c.Name,
                        totalInvoiced = Decimal.Round(c.TotalInvoiced * currencyFactor, 2),
                        totalPaid = Decimal.Round(c.TotalPaid * currencyFactor, 2),
                        balance = Decimal.Round(c.Balance * currencyFactor, 2),
                        currencyCode = currencyCode,
                        currencySymbol = Helpers.GetCurrencySymbol(currencyCode)
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
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
        [System.Web.Mvc.HttpPost]
        public ActionResult CreateEdit(ClientViewModel model)
        {
            try
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

                if (client.ClientId > 0)
                {
                    _clientService.Update(client);
                }
                else
                {
                    _clientService.Add(client);
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

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
        [System.Web.Mvc.HttpPost]
        public ActionResult Detail(int id, ClientViewModel model)
        {
            return View();
        }

        // POST: Client/Delete/1
        [System.Web.Mvc.HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _clientService.Remove(id);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }

        // POST: Client/Balance/1
        public ActionResult Balance(int id)
        {
            try
            {
                var invoiceList = _invoiceService.GetInvoiceListByClient(id);

                List<BalanceSheetViewModel> balanceSheets = new List<BalanceSheetViewModel>();
                decimal balance = 0;
                decimal invoiced = 0;
                decimal paid = 0;
                foreach (var invoice in invoiceList)
                {
                    balance += invoice.Total;
                    invoiced += invoice.Total;
                    balanceSheets.Add(new BalanceSheetViewModel()
                    {
                        Date = invoice.Date,
                        Type = "Invoice",
                        Reference = invoice.InvoiceId,
                        Invoiced = invoice.Total,
                        Paid = 0,
                        Balance = balance

                    });
                    foreach (var payment in invoice.PaymentList)
                    {
                        balance -= payment.Total;
                        paid += payment.Total;
                        balanceSheets.Add(new BalanceSheetViewModel()
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


                return Json(new { BalanceSheet = balanceSheets, TotalInvoiced = invoiced, TotalPaid = paid, Balance = balance }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

        }

        // POST: Client/GetClient/1
        public ActionResult GetClient(int id)
        {
            try
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
            catch (Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

        }
    }
}