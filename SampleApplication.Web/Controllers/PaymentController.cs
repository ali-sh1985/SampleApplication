using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SampleApplication.Domain;
using SampleApplication.Domain.Entities;
using SampleApplication.Domain.Repositories;
using SampleApplication.Service.Common;
using SampleApplication.Service.SearchCriterias;
using SampleApplication.Service.Services;
using SampleApplication.Web.Common;
using SampleApplication.Web.Models;

namespace SampleApplication.Web.Controllers
{
    [System.Web.Mvc.Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        // GET: Payment
        public ActionResult Index()
        {

            return View();
        }
        // GET: Payment
        public JsonResult GetAll()
        {
            var paymentList = _paymentService.GetAll();

            var result = paymentList.Select(p => new
            {
                PaymentId = p.PaymentId,
                Client = p.Invoice.Client.Name,
                Date = p.PaymentDate,
                Total = p.Total,
                Description = p.Description
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Payments By Client
        public JsonResult List([Bind(Prefix = "id")]int clientId)
        {
            var paymentList = _paymentService.GetPaymentListByClient(clientId);

            var result = paymentList.Select(p => new
            {
                paymentId = p.PaymentId,
                paymentDate = p.PaymentDate,
                total = p.Total,
                description = p.Description,
                invoiceId=p.InvoiceId,
                method = p.Method
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public JsonResult List(PaymentFilterViewModel filter)
        {

            var paymentSearchCriteria = new PaymentSearchCriteria()
            {
                ClientId = filter.ClientId
            };
            var result = _paymentService.Search(paymentSearchCriteria, new Paging(filter.PageNumber, filter.PageSize), new Sort(filter.SortColumn, filter.Order));

            return Json(new
            {
                TotalPages = result.TotalPageCount,
                TotalItems = result.TotalCount,
                PageNumber = result.PageIndex + 1,
                Data = result.Select(p => new
                {
                    PaymentId = p.PaymentId,
                    InvoiceId = p.Invoice.InvoiceId,
                    ClientName = p.Invoice.Client.Name,
                    ClientId = p.Invoice.Client.ClientId,
                    Date = p.PaymentDate,
                    Total = p.Total,
                    Method = p.Method,
                    Description = p.Description,
                    CurrencySymbol = Helpers.GetCurrencySymbol("GBP")
                })
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public JsonResult CreateEdit(PaymentViewModel model)
        {
            Payment payment = new Payment()
            {
                PaymentId = model.PaymentId ?? 0,
                InvoiceId = model.InvoiceId,
                PaymentDate = model.PaymentDate,
                Description = model.Description,
                Method = model.Method,
                Total = model.Total
            };

            if (payment.PaymentId > 0)
            {
                _paymentService.Update(payment);
            }
            else
            {
                _paymentService.Add(payment);
            }
            

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            _paymentService.Remove(id);
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}