using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SampleApplication.Domain;
using SampleApplication.Domain.Entities;
using SampleApplication.Service.Services;
using SampleApplication.Web.Models;

namespace SampleApplication.Web.Controllers
{
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
            var invoiceList = _invoiceService.GetInvoiceListByClient(clientId);

            var result = invoiceList.Select(i => new
            {
                date = i.Date,
                reference = i.InvoiceId,
                total = i.Total,
                net = i.Net,
                tax = i.Tax,
                description = i.Description.Substring(0, Math.Min(i.Description.Length, 200))
            });

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateEdit(InvoiceViewModel model)
        {
            return Json("");
        }

        public JsonResult Validate(int id)
        {
            bool result = false;
            var invoice = _invoiceService.Find(id);
            if (invoice!=null && invoice.InvoiceId > 0)
            {
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}