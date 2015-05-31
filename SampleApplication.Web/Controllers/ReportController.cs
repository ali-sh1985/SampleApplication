using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using SampleApplication.Service.Services;

namespace SampleApplication.Web.Controllers
{
    [System.Web.Mvc.Authorize]
    public class ReportController : Controller
    {
        private IChartService _chartService;

        public ReportController(IChartService chartService)
        {
            _chartService = chartService;
        }
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetReport(string metric,string type)
        {
            try
            {
                ReportMetric reportMetric = (ReportMetric)Enum.Parse(typeof(ReportMetric), metric, true);
                ReportType reportType = (ReportType)Enum.Parse(typeof(ReportType), type, true);

                var result = _chartService.GetData(new ReportParameter() { Type = reportType, Metric = reportMetric });

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }
    }
}
