using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SampleApplication.Service.Services
{
    public interface ICurrencyService
    {
        decimal Convert(decimal amount, string currencyCodeFrom, string currencyCodeTo);
    }
    public class CurrencyService : ICurrencyService
    {
        public decimal Convert(decimal amount, string currencyCodeFrom, string currencyCodeTo)
        {
            string pattern = "http://www.xe.com/currencyconverter/convert/?Amount={0}&From={1}&To={2}";
            WebClient w = new WebClient();
            string s = w.DownloadString(string.Format(pattern, amount, currencyCodeFrom, currencyCodeTo));
            HtmlDocument doc = new HtmlDocument();
            byte[] byteArray = Encoding.ASCII.GetBytes(s);
            MemoryStream stream = new MemoryStream(byteArray);
            doc.Load(stream);
            var itemList = doc.DocumentNode.SelectNodes("//td[span[@class='uccResCde']]").Select(p => p.InnerText.Substring(0, p.InnerText.IndexOf("&nbsp;")))
                  .ToList();

            return decimal.Parse(itemList[1]);
        }
    }
}
