using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using SampleApplication.Domain;
using SampleApplication.Domain.Entities;
using SampleApplication.Service.Common;
using SampleApplication.Service.SearchCriterias;
using SampleApplication.Service.Services;
using SampleApplication.Web.Common;
using SampleApplication.Web.Models;

namespace SampleApplication.Web.Controllers
{
    [System.Web.Mvc.Authorize]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // GET: Invoice
        public ActionResult Index()
        {
            return View();
        }

        // GET: Invoice/List/1
        public JsonResult List([Bind(Prefix = "id")]int clientId)
        {
            try
            {
                var invoiceList = _invoiceService.GetInvoiceListByClient(clientId);

                var result = invoiceList.Select(i => new InvoiceViewModel()
                {
                    Date = i.Date,
                    InvoiceId = i.InvoiceId,
                    Description = i.Description.Substring(0, Math.Min(i.Description.Length, 200)),
                    ClientId = i.ClientId,
                    ItemList = i.ItemList.Select(t => new ItemViewModel()
                    {
                        Description = t.Description,
                        InvoiceId = i.InvoiceId,
                        ItemId = t.ItemId,
                        Net = t.Net,
                        Tax = t.Tax,
                        IsDeleted = false
                    }).ToList()
                });

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            } 
        }

        [System.Web.Mvc.HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public JsonResult List(InvoiceFilterViewModel filter)
        {
            try
            {
                var invoiceSearchCriteria = new InvoiceSearchCriteria()
                {
                    Find = filter.Find,
                    InvoiceId = filter.InvoiceId,
                    ClientId = filter.ClientId,
                    TotalFrom = filter.TotalFrom,
                    TotalTo = filter.TotalTo,
                    NetFrom = filter.NetFrom,
                    NetTo = filter.NetTo,
                };
                var result = _invoiceService.Search(invoiceSearchCriteria, new Paging(filter.PageNumber, filter.PageSize), new Sort(filter.SortColumn, filter.Order));

                return Json(new
                {
                    TotalPages = result.TotalPageCount,
                    TotalItems = result.TotalCount,
                    PageNumber = result.PageIndex + 1,
                    Data = result.Select(i => new
                    {
                        Date = i.Date,
                        InvoiceId = i.InvoiceId,
                        Description = i.Description.Substring(0, Math.Min(i.Description.Length, 200)),
                        ClientId = i.Client.ClientId,
                        ClientName = i.Client.Name,
                        Total = i.Total,
                        Tax = i.Tax,
                        Net = i.Net,
                        CurrencySymbol = Helpers.GetCurrencySymbol("GBP"),
                        ItemList = i.ItemList.Select(t => new ItemViewModel()
                        {
                            Description = t.Description,
                            InvoiceId = i.InvoiceId,
                            ItemId = t.ItemId,
                            Net = t.Net,
                            Tax = t.Tax,
                            IsDeleted = false
                        }).ToList()
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            } 
        }
        public ActionResult CreateEdit(InvoiceViewModel model)
        {
            try
            {
                var invoice = new Invoice();
                invoice.InvoiceId = model.InvoiceId ?? 0;
                invoice.ClientId = model.ClientId;
                invoice.Date = model.Date;
                invoice.ItemList = new List<Item>();
                foreach (var item in model.ItemList)
                {
                    invoice.ItemList.Add(new Item()
                    {
                        Description = item.Description,
                        InvoiceId = model.InvoiceId ?? 0,
                        ItemId = item.ItemId ?? 0,
                        Net = item.Net,
                        Tax = item.Tax,
                        IsDeleted = item.IsDeleted
                    });
                }

                if (invoice.InvoiceId > 0)
                {
                    _invoiceService.Update(invoice);
                }
                else
                {
                    _invoiceService.Add(invoice);
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            } 
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _invoiceService.Remove(id);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            } 
        }

        public JsonResult Validate(int id)
        {
            try
            {
                bool result = false;
                var invoice = _invoiceService.Find(id);
                if (invoice != null && invoice.InvoiceId > 0)
                {
                    result = true;
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            } 
            
        }
    }
}