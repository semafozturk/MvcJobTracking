using System.Web;
using System.Web.Mvc;

namespace DXTreelist0904 {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}