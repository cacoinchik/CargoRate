using Microsoft.AspNetCore.Mvc;

namespace CargoRate.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
