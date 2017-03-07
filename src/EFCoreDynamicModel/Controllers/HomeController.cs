using Microsoft.AspNetCore.Mvc;

namespace EFCoreDynamicModel.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //var s=  DynamicModelDbContext.GetType().GetTypeInfo().GetMethod("Set").MakeGenericMethod(type).Invoke(context, null) as IQueryable;
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}