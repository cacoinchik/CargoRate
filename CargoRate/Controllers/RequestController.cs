using CargoRate.Data;
using CargoRate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CargoRate.Service;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace CargoRate.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Index(UserCargoDataModel model)
        {
            GeocodingService geocodingService = new GeocodingService();

            var startCoordinates = await geocodingService.Code(model.DeparturePoint);
            var finishCoordinates = await geocodingService.Code(model.ArrivalPoint);

            var startParameters = new GeoCoordinatesParameter();
            var finishParameters = new GeoCoordinatesParameter();
            startParameters = geocodingService.GetGeoCoordinatesParameter(startCoordinates);
            finishParameters = geocodingService.GetGeoCoordinatesParameter(finishCoordinates);

            var cargos = db.Cargos.ToList();

            var startRadiusCargos = new List<Cargo>();
            var finishRadiusCargos = new List<Cargo>();

            foreach (var trip in cargos)
            {
                var locStart = await geocodingService.Code(trip.DeparturePoint);
                var locFinish = await geocodingService.Code(trip.ArrivalPoint);

                if (locStart.Latitude >= startParameters.minLat && locStart.Latitude <= startParameters.maxLat && locStart.Longitude >= startParameters.minLon && locStart.Longitude <= startParameters.maxLon)
                {
                    startRadiusCargos.Add(trip);
                }

                if (locFinish.Latitude >= finishParameters.minLat && locFinish.Latitude <= finishParameters.maxLat && locFinish.Longitude >= finishParameters.minLon && locFinish.Longitude <= finishParameters.maxLon)
                {
                    finishRadiusCargos.Add(trip);
                }
            }

            var relevantCargos = new List<Cargo>();

            foreach (var start in startRadiusCargos)
            {
                foreach (var finish in finishRadiusCargos)
                {
                    Cargo thisCargo = db.Cargos.FirstOrDefault(x => x.DeparturePoint == start.DeparturePoint && x.ArrivalPoint == finish.ArrivalPoint && x.TrailerType==model.TrailerType);
                    if (thisCargo != null)
                    {
                        relevantCargos.Add(thisCargo);
                    }
                }
            }

            var averageCargos = relevantCargos.Any() ? relevantCargos.Average(p => p.Price) : 0;

            model.Price = averageCargos;

            return View("Result", model);
        }

    }
}
