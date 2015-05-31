using System;
using System.Globalization;
using System.Linq;

namespace SampleApplication.Web.Common
{

    public class Helpers
    {
        public static string GetCurrencySymbol(string ISOCurrencySymbol)
        {
            var symbol = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(culture =>
                {
                    try
                    {
                        return new RegionInfo(culture.LCID);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(ri => ri != null && ri.ISOCurrencySymbol == ISOCurrencySymbol)
                .Select(ri => ri.CurrencySymbol)
                .FirstOrDefault();
            return symbol ?? String.Empty;
        }
    }
}