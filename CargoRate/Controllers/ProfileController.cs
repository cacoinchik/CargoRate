using CargoRate.Data;
using CargoRate.Models;
using CargoRate.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CargoRate.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext db;
        private UserManager<User> userManager;
        public ProfileController(AppDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ProfileModels models = new ProfileModels();
                models.User=await db.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                models.Subscription=await db.Subscriptions.FirstOrDefaultAsync(s=>s.UserName==models.User.UserName && s.Status=="Активен");
                return View(models);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            EditUserViewModel model = new EditUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                CompanyName= user.CompanyName
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Name = model.Name;
                    user.LastName = model.LastName;
                    user.CompanyName = model.CompanyName;

                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Profile");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Subscribe()
        {
            return View(await db.Rates.ToListAsync());
        }


        [HttpPost]
        public async Task<IActionResult> Subscribe(string selectedRate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var sub = new Subscription
                    {
                        UserName = User.Identity.Name,
                        Status = "Активен",
                        StartDate = DateTime.Now,
                        RateId=db.Rates.FirstOrDefault(n=>n.Name==selectedRate).Id
                    };
                    sub.EndDate = sub.StartDate.AddMonths(db.Rates.FirstOrDefault(d => d.Id == sub.RateId).Duration);

                    await db.Subscriptions.AddAsync(sub);
                    await db.SaveChangesAsync();

                    return RedirectToAction("Info", "Profile");
                }catch (Exception ex)
                {

                }
            }
            return View();
        }

        public IActionResult Info()
        {
            return View();
        }

    }
}
