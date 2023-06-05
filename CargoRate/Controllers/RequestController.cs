using CargoRate.Data;
using CargoRate.Models;
using Microsoft.AspNetCore.Mvc;

namespace CargoRate.Controllers
{
    public class RequestController : Controller
    {
        private readonly AppDbContext db;
        public RequestController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(UserCargoDataModel model)
        {
            try
            {
                var averagePrice = db.Cargos
                .Where(x => x.DeparturePoint == model.DeparturePoint && x.ArrivalPoint == model.ArrivalPoint && x.TrailerType == model.TrailerType)
                .Average(p => p.Price);
                model.Price = averagePrice;
            }
            catch (Exception ex)
            {
                RedirectToAction("Index", "Home");
            }
            
            return RedirectToAction("Result",model);
        }
        public IActionResult Result(UserCargoDataModel result)
        {
            return View(result);
        }
    }
}
