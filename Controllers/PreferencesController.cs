using GC_PlanMyMeal.Areas.Identity.Data;
using GC_PlanMyMeal.DatabaseModels;
using GC_PlanMyMeal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Controllers
{

    public class PreferencesController : Controller
    {
        private readonly GC_PlanMyMealIdentityDbContext _context;

        public PreferencesController(GC_PlanMyMealIdentityDbContext context)
        {
            _context = context;
        }
        public IActionResult UserPreferences()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SavePreferences(string diet, Intolerances intolerances, int? maxCalorie, int? maxCarb, int? maxProtein, int? minProtein)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var savedUserPreferences = new UserPreference()
            {
                UserId = userId,
                Diet = diet,
                Intolerances = intolerances.ToString(),
                MaxCalorie = maxCalorie,
                MaxCarb = maxCarb,
                MaxProtein = maxProtein,
                MinProtein = minProtein,
            };

            
            _context.UserPreferences.Add(savedUserPreferences);
            _context.SaveChangesAsync();
            return View(savedUserPreferences);
        }
    }
}
