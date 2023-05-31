using CargoRate.Data;
using CargoRate.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.Xml.Linq;

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

        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    XDocument xDocument = XDocument.Load(file.OpenReadStream());
                    List<Cargo> cargoList = xDocument.Root.Elements("грузоперевозка").
                        Select(x => new Cargo
                        {
                            DeparturePoint = x.Element("отправитель").Value,
                            ArrivalPoint = x.Element("получатель").Value,
                            Price = Convert.ToDecimal(x.Element("цена").Value),
                            TrailerType = x.Element("транспорт").Element("прицеп").Value
                        }).ToList();

                    await db.Cargos.AddRangeAsync(cargoList);
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
