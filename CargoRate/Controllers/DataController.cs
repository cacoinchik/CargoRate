using CargoRate.Data;
using CargoRate.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace CargoRate.Controllers
{
    public class DataController : Controller
    {
        private readonly AppDbContext db;

        public DataController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    var doc = new XmlDocument();
                    doc.Load(file.OpenReadStream());
                    var cargoDoc = doc.SelectNodes("//грузоперевозка");
                    foreach (XmlNode cargo in cargoDoc)
                    {
                        var newCargo = new Cargo
                        {
                            DeparturePoint = cargo.SelectSingleNode("отправитель").InnerText,
                            ArrivalPoint = cargo.SelectSingleNode("получатель").InnerText,
                            Price=3142m,
                            TrailerType="default"
                        };
                        db.Cargos.Add(newCargo);
                    }

                    await db.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            return View();
        }
    }
}
