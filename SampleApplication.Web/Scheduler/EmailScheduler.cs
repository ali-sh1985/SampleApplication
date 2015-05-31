using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using Quartz;
using Quartz.Impl;
using SampleApplication.Data.EntityFramework;
using SampleApplication.Domain.Entities;
using SampleApplication.Service.Services;

namespace SampleApplication.Web.Scheduler
{
    public class EmailScheduler
    {
        private static InvoiceService _invoiceService = new InvoiceService(new UnitOfWork("SampleApplication"));
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Context.Put("InvoiceService", _invoiceService);
            scheduler.Context.Put("fromAddress", EmailSchedulerSettings.Settings.FromAddress);
            scheduler.Context.Put("toAddress", EmailSchedulerSettings.Settings.ToAddress);
            scheduler.Context.Put("host", EmailSchedulerSettings.Settings.SmtpServerAddress);
            scheduler.Context.Put("port", EmailSchedulerSettings.Settings.SmtpServerPort);
            scheduler.Context.Put("password", EmailSchedulerSettings.Settings.SmtpServerPassword);
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<EmailJob>().WithIdentity("emailJob").Build();

            //string cronExpression = "0 0/1 * 1/1 * ? *"; //Every 1 minute
            string cronExpression = String.Format("0 {0} {1} 1/{2} * ? *", DateTime.Now.Minute, DateTime.Now.Hour, EmailSchedulerSettings.Settings.JobInterval);
            ITrigger trigger = TriggerBuilder.Create()
                                .WithIdentity("emailTrigger")
                                .WithCronSchedule(cronExpression)
                                .ForJob("emailJob")
                                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }

    public class EmailJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            var schedulerContext = context.Scheduler.Context;
            var invoiceServiceInstance = (InvoiceService)schedulerContext.Get("InvoiceService");

            #region Create Html File
            List<Invoice> outstandingInvocies = invoiceServiceInstance.GetOutstandingInvocies(DateTime.Now.Date.AddDays(-10), DateTime.Now);
            string htmlReportTemplate = @"<!DOCTYPE html>
                                          <html>
                                          <header>
                                          <title>Outstanding Invocies Report</title>
                                          </header>
                                          <body>
                                          <table border=""1"">
                                          <tr>
                                              <th>Client Name</th>
                                              <th>Invoice #</th> 
                                              <th>Total Amount</th>
                                              <th>Tax</th>
                                              <th>Net Amount</th>
                                              <th>Description</th>
                                          </tr>
                                            {TableRows}
                                          </table></body>
                                          </html>";

            string invoiceRow = outstandingInvocies.Aggregate(string.Empty, (current, invoice) => current + string.Format("<tr><td>{0}</td><td># {1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>", invoice.Client.Name, invoice.InvoiceId, invoice.Total, invoice.Tax, invoice.Net, invoice.Description.Substring(0, Math.Min(invoice.Description.Length, 200))));
            var htmlResult = htmlReportTemplate.Replace("{TableRows}", invoiceRow);
            System.IO.File.WriteAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/OutstandingInvociesReport.html"), htmlResult);
            #endregion

            var fromAddressString = (string)schedulerContext.Get("fromAddress");
            var toAddressString = (string)schedulerContext.Get("toAddress");
            var host = (string)schedulerContext.Get("host");
            var port = (int)schedulerContext.Get("port");
            var password = (string)schedulerContext.Get("password");


            var fromAddress = new MailAddress(fromAddressString, "SampleAppDemo");
            var toAddress = new MailAddress(toAddressString, "AdminUser");
            string fromPassword = password;
            const string subject = "Outstanding Invocies Report";
            string body = htmlResult;

            var smtp = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }

    public class EmailSchedulerSettings : ConfigurationSection
    {
        private static EmailSchedulerSettings settings = ConfigurationManager.GetSection("EmailSchedulerSettings") as EmailSchedulerSettings;

        public static EmailSchedulerSettings Settings
        {
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("fromAddress", IsRequired = true)]
        public string FromAddress
        {
            get { return (string)this["fromAddress"]; }
            set { this["fromAddress"] = value; }
        }

        [ConfigurationProperty("toAddress", IsRequired = true)]
        public string ToAddress
        {
            get { return (string)this["toAddress"]; }
            set { this["toAddress"] = value; }
        }


        [ConfigurationProperty("smtpServerAddress", IsRequired = true)]
        public string SmtpServerAddress
        {
            get { return (string)this["smtpServerAddress"]; }
            set { this["smtpServerAddress"] = value; }
        }

        [ConfigurationProperty("smtpServerPort", IsRequired = true)]
        public int SmtpServerPort
        {
            get { return (int)this["smtpServerPort"]; }
            set { this["smtpServerPort"] = value; }
        }

        [ConfigurationProperty("smtpServerPassword", IsRequired = true)]
        public string SmtpServerPassword
        {
            get { return (string)this["smtpServerPassword"]; }
            set { this["smtpServerPassword"] = value; }
        }

        [ConfigurationProperty("jobInterval", IsRequired = true)]
        public int JobInterval
        {
            get { return (int)this["jobInterval"]; }
            set { this["jobInterval"] = value; }
        }
    }
}