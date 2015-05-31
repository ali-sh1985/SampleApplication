using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleApplication.Domain;
using SampleApplication.Domain.Entities;
using SampleApplication.Domain.Repositories;
using SampleApplication.Service.SearchCriterias;

namespace SampleApplication.Service.Services
{

    public interface IChartService
    {
        List<ChartResult> GetData(ReportParameter parameter);
    }

    public class ChartService : IChartService
    {
        protected IUnitOfWork _unitOfWork;
        public ChartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<ChartResult> GetData(ReportParameter parameter)
        {
            BaseReport report = new BaseReport(parameter);
            switch (parameter.Metric)
            {
                case ReportMetric.Invoice:
                    switch (parameter.Type)
                    {
                        case ReportType.Monthly:
                            report = new InvoiceMonthlyReport(parameter, _unitOfWork.InvoiceRepository);
                            break;
                        case ReportType.Quarterly:
                            report = new InvoiceQuarterlyReport(parameter, _unitOfWork.InvoiceRepository);
                            break;
                        case ReportType.Annually:
                            report = new InvoiceAnnuallyReport(parameter, _unitOfWork.InvoiceRepository);
                            break;
                        default:
                            report = new InvoiceMonthlyReport(parameter, _unitOfWork.InvoiceRepository);
                            break;
                    }
                    break;
                case ReportMetric.Payment:
                    switch (parameter.Type)
                    {
                        case ReportType.Monthly:
                            report = new PaymentMonthlyReport(parameter, _unitOfWork.PaymentRepository);
                            break;
                        case ReportType.Quarterly:
                            report = new PaymentQuarterlyReport(parameter, _unitOfWork.PaymentRepository);
                            break;
                        case ReportType.Annually:
                            report = new PaymentAnnuallyReport(parameter, _unitOfWork.PaymentRepository);
                            break;
                        default:
                            report = new PaymentMonthlyReport(parameter, _unitOfWork.PaymentRepository);
                            break;
                    }
                    break;
            }

            return report.GetData();
        }
    }

    public class ChartResult
    {
        public string Label { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
    }

    public class ReportParameter
    {
        public ReportMetric Metric { get; set; }
        public ReportType Type { get; set; }
    }

    public enum ReportType
    {
        Monthly = 1,
        Quarterly = 2,
        Annually = 3
    }

    public enum ReportMetric
    {
        Payment = 1,
        Invoice = 2
    }

    public abstract class PeriodicReportInfo
    {
        public abstract DateTime GetStartDate();

        public virtual DateTime GetEndDate()
        {
            return DateTime.Now;
        }

        public abstract List<DateTime> GetGapList();
    }
    public class MonthlyReportInfo : PeriodicReportInfo
    {
        public override DateTime GetStartDate()
        {
            return new DateTime(DateTime.Now.AddYears(-2).Year, DateTime.Now.AddYears(-2).Month, 1, 0, 0, 0);
        }

        public override List<DateTime> GetGapList()
        {
            var gap = new List<DateTime>();
            DateTime lStartDate = GetStartDate();
            DateTime lEndDate = GetEndDate();
            while (true)
            {
                gap.Add(lStartDate);
                lStartDate = lStartDate.AddMonths(1);
                if (lStartDate.Date >= lEndDate.Date)
                {
                    break;
                }
            }

            return gap;
        }
    }
    public class QuarterlyReportInfo : PeriodicReportInfo
    {
        public override DateTime GetStartDate()
        {
            return new DateTime(DateTime.Now.AddYears(-3).Year, (((((DateTime.Now.AddYears(-3).Date.Month - 1) / 3) + 1) - 1) * 3) + 1, 1, 0, 0, 0);
        }

        public override List<DateTime> GetGapList()
        {
            var gap = new List<DateTime>();
            DateTime lStartDate = GetStartDate();
            DateTime lEndDate = GetEndDate();
            while (true)
            {
                gap.Add(lStartDate);
                lStartDate = lStartDate.AddMonths(3);
                if (lStartDate.Date >= lEndDate.Date)
                {
                    break;
                }
            }

            return gap;
        }
    }
    public class AnnuallyReportInfo : PeriodicReportInfo
    {
        public override DateTime GetStartDate()
        {
            return new DateTime(DateTime.Now.AddYears(-4).Year, 1, 1, 0, 0, 0);
        }

        public override List<DateTime> GetGapList()
        {
            var gap = new List<DateTime>();
            DateTime lStartDate = GetStartDate();
            DateTime lEndDate = GetEndDate();
            while (true)
            {
                gap.Add(lStartDate);
                lStartDate = lStartDate.AddYears(1);
                if (lStartDate.Date >= lEndDate.Date)
                {
                    break;
                }
            }

            return gap;
        }
    }

    public class BaseReport
    {
        protected PeriodicReportInfo reportInfo;

        public BaseReport(ReportParameter parameter)
        {
            switch (parameter.Type)
            {
                case ReportType.Monthly:
                    reportInfo = new MonthlyReportInfo();
                    break;
                case ReportType.Quarterly:
                    reportInfo = new QuarterlyReportInfo();
                    break;
                case ReportType.Annually:
                    reportInfo = new AnnuallyReportInfo();
                    break;
                default:
                    reportInfo = new MonthlyReportInfo();
                    break;
            }
        }

        public virtual List<ChartResult> GetData()
        {
            return null;
        }
    }

    public class InvoiceMonthlyReport : BaseReport
    {
        private IInvoiceRepository _repository;
        public InvoiceMonthlyReport(ReportParameter parameter,IInvoiceRepository repository)
            : base(parameter)
        {
            _repository = repository;
        }

        public override List<ChartResult> GetData()
        {
            DateTime startDate = reportInfo.GetStartDate();
            DateTime endDate = reportInfo.GetEndDate();
            var gapList = reportInfo.GetGapList();
            var invoiceList = _repository.GetAll(x => x.Date >= startDate && x.Date <= endDate, null, 0, int.MaxValue);
            var result = (from Item1 in gapList
                     join Item2 in invoiceList
                     on new { y = Item1.Date.Year, m = Item1.Date.Month } equals new { y = Item2.Date.Year, m = Item2.Date.Month }
                     into a
                     from b in a.DefaultIfEmpty(new Invoice())
                     group b by new { Item1.Date.Year, Item1.Date.Month } into c
                     select new ChartResult { Date = new DateTime(c.Key.Year, c.Key.Month, 1, 0, 0, 0), Label = c.Key.Year.ToString() + " - " + c.Key.Month.ToString(), Value = c.Sum(x => x.Total) }).OrderBy(x => x.Date);
            return result.ToList();
        }
    }

    public class InvoiceQuarterlyReport : BaseReport
    {
        private IInvoiceRepository _repository;
        public InvoiceQuarterlyReport(ReportParameter parameter,IInvoiceRepository repository)
            : base(parameter)
        {
            _repository = repository;
        }
        public override List<ChartResult> GetData()
        {
            DateTime startDate = reportInfo.GetStartDate();
            DateTime endDate = reportInfo.GetEndDate();
            var gapList = reportInfo.GetGapList();
            var invoiceList = _repository.GetAll(x => x.Date >= startDate && x.Date <= endDate, null, 0, int.MaxValue);
            var result = (from Item1 in gapList
                          join Item2 in invoiceList
                          on new { y = Item1.Date.Year, m = ((Item1.Date.Month - 1) / 3) + 1 } equals new { y = Item2.Date.Year, m = ((Item2.Date.Month - 1) / 3) + 1 }
                          into a
                          from b in a.DefaultIfEmpty(new Invoice())
                          group b by new { Item1.Date.Year, Q = ((Item1.Date.Month - 1) / 3) + 1 } into c
                          select new ChartResult(){ Date = new DateTime(c.Key.Year,c.Key.Q,1,0,0,0),Label = "Q" + (c.Key.Q).ToString() + " - " + c.Key.Year.ToString(), Value = c.Sum(x => x.Total) }).OrderBy(x=>x.Date);
            return result.ToList();
        }
    }

    public class InvoiceAnnuallyReport : BaseReport
    {
        private IInvoiceRepository _repository;
        public InvoiceAnnuallyReport(ReportParameter parameter, IInvoiceRepository repository)
            : base(parameter)
        {
            _repository = repository;
        }

        public override List<ChartResult> GetData()
        {
            DateTime startDate = reportInfo.GetStartDate();
            DateTime endDate = reportInfo.GetEndDate();
            var gapList = reportInfo.GetGapList();
            var invoiceList = _repository.GetAll(x => x.Date >= startDate && x.Date <= endDate, null, 0, int.MaxValue);
            var result = (from Item1 in gapList
                          join Item2 in invoiceList
                          on new { y = Item1.Date.Year} equals new { y = Item2.Date.Year}
                          into a
                          from b in a.DefaultIfEmpty(new Invoice())
                          group b by new { Item1.Date.Year} into c
                          select new ChartResult() { Date = new DateTime(c.Key.Year, 1, 1, 0, 0, 0), Label = c.Key.Year.ToString(), Value = c.Sum(x => x.Total) }).OrderBy(x => x.Date);
            return result.ToList();
        }
    }

    public class PaymentMonthlyReport : BaseReport
    {
        private IPaymentRepository _repository;
        public PaymentMonthlyReport(ReportParameter parameter, IPaymentRepository repository)
            : base(parameter)
        {
            _repository = repository;
        }

        public override List<ChartResult> GetData()
        {
            DateTime startDate = reportInfo.GetStartDate();
            DateTime endDate = reportInfo.GetEndDate();
            var gapList = reportInfo.GetGapList();
            var invoiceList = _repository.GetAll(x => x.PaymentDate >= startDate && x.PaymentDate <= endDate, null, 0, int.MaxValue);
            var result = (from Item1 in gapList
                     join Item2 in invoiceList
                     on new { y = Item1.Date.Year, m = Item1.Date.Month } equals new { y = Item2.PaymentDate.Year, m = Item2.PaymentDate.Month }
                     into a
                          from b in a.DefaultIfEmpty(new Payment())
                     group b by new { Item1.Date.Year, Item1.Date.Month } into c
                     select new ChartResult { Date = new DateTime(c.Key.Year, c.Key.Month, 1, 0, 0, 0), Label = c.Key.Year.ToString() + " - " + c.Key.Month.ToString(), Value = c.Sum(x => x.Total) }).OrderBy(x => x.Date);
            return result.ToList();
        }
    }

    public class PaymentQuarterlyReport : BaseReport
    {
        private IPaymentRepository _repository;
        public PaymentQuarterlyReport(ReportParameter parameter, IPaymentRepository repository)
            : base(parameter)
        {
            _repository = repository;
        }

        public override List<ChartResult> GetData()
        {
            DateTime startDate = reportInfo.GetStartDate();
            DateTime endDate = reportInfo.GetEndDate();
            var gapList = reportInfo.GetGapList();
            var invoiceList = _repository.GetAll(x => x.PaymentDate >= startDate && x.PaymentDate <= endDate, null, 0, int.MaxValue);
            var result = (from Item1 in gapList
                          join Item2 in invoiceList
                          on new { y = Item1.Date.Year, m = ((Item1.Date.Month - 1) / 3) + 1 } equals new { y = Item2.PaymentDate.Year, m = ((Item2.PaymentDate.Month - 1) / 3) + 1 }
                          into a
                          from b in a.DefaultIfEmpty(new Payment())
                          group b by new { Item1.Date.Year, Q = ((Item1.Date.Month - 1) / 3) + 1 } into c
                          select new ChartResult() { Date = new DateTime(c.Key.Year, c.Key.Q, 1, 0, 0, 0), Label = "Q" + (c.Key.Q).ToString() + " - " + c.Key.Year.ToString(), Value = c.Sum(x => x.Total) }).OrderBy(x => x.Date);
            return result.ToList();
        }
    }

    public class PaymentAnnuallyReport : BaseReport
    {
        private IPaymentRepository _repository;
        public PaymentAnnuallyReport(ReportParameter parameter, IPaymentRepository repository)
            : base(parameter)
        {
            _repository = repository;
        }

        public override List<ChartResult> GetData()
        {
            DateTime startDate = reportInfo.GetStartDate();
            DateTime endDate = reportInfo.GetEndDate();
            var gapList = reportInfo.GetGapList();
            var invoiceList = _repository.GetAll(x => x.PaymentDate >= startDate && x.PaymentDate <= endDate, null, 0, int.MaxValue);
            var result = (from Item1 in gapList
                          join Item2 in invoiceList
                          on new { y = Item1.Date.Year } equals new { y = Item2.PaymentDate.Year }
                          into a
                          from b in a.DefaultIfEmpty(new Payment())
                          group b by new { Item1.Date.Year} into c
                          select new ChartResult() { Date = new DateTime(c.Key.Year, 1, 1, 0, 0, 0), Label = c.Key.Year.ToString(), Value = c.Sum(x => x.Total) }).OrderBy(x => x.Date);
            return result.ToList();
        }
    }
}
