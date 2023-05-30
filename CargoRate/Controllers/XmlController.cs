using CargoRate.Data;
using CargoRate.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace CargoRate.Controllers
{
    public class XmlController : Controller
    {
        private readonly AppDbContext db;

        public XmlController(AppDbContext db)
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
                            OrderNum = int.Parse(cargo.SelectSingleNode("номер_заказа").InnerText),
                            Start = cargo.SelectSingleNode("отправитель").InnerText,
                            End = cargo.SelectSingleNode("получатель").InnerText
                        };
                        db.Cargos.Add(newCargo);
                    }

                    await db.SaveChangesAsync();

                    return RedirectToAction("Index","Home");
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
