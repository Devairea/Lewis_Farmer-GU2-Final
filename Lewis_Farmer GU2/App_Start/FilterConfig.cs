using System.Web;
using System.Web.Mvc;

namespace Lewis_Farmer_GU2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
