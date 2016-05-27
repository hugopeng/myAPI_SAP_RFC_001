using System.Web;
using System.Web.Mvc;

namespace myAPI_SAP_RFC_001
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
