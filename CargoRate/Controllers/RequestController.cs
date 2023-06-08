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
            var startCoordinates = await geocodingService.Code(model.DeparturePoint.ToString());
            var finishCoordinates = await geocodingService.Code(model.ArrivalPoint.ToString());

            var radius = 100000;
            var earthRadius = 6371;

            var latDistance = (radius / 1000.0) / earthRadius;
            var lngDistance = latDistance / Math.Cos(startCoordinates.Latitude * Math.PI / 180);

            var minLat = startCoordinates.Latitude - (latDistance * 180 / Math.PI);
            var maxLat = startCoordinates.Latitude + (latDistance * 180 / Math.PI);
            var minLon = startCoordinates.Longitude - (lngDistance * 180 / Math.PI);
            var maxLon = startCoordinates.Longitude + (lngDistance * 180 / Math.PI);

            var cargos = db.Cargos.ToList();

            var trips = new List<Cargo>();

            foreach(var trip in cargos)
            {
                var loc = await geocodingService.Code(trip.DeparturePoint.ToString());

                if(loc.Latitude>=minLat && loc.Latitude<=maxLat && loc.Longitude>=minLon && loc.Longitude <= maxLon)
                {
                    trips.Add(trip);
                }
            }

            return View("Result", trips);
        }

    }
}
