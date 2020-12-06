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
        public IActionResult UserPreferences()
        {
            var userPreferences = new UserPreferences();
            userPreferences.ClientSelectedPreferences = new UserPreferencesClientSelected();
            userPreferences.PreferencesOptions = new UserPrefrencesOptions()
            {
                Diet = AllowedUserPreferenceOptions.Diet,
                Intolerances = AllowedUserPreferenceOptions.Intolerances
            };
            return View(userPreferences);
        }

        [HttpPost]
        public IActionResult SavePrefrences()
        {
            var request = this.Request;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            request.Form.TryGetValue("ClientSelectedPreferences.Diet", out var diet);
            request.Form.TryGetValue("ClientSelectedPreferences.MaxCalorie", out var maxCalorie);
            request.Form.TryGetValue("ClientSelectedPreferences.MaxCarb", out var maxCarb);
            request.Form.TryGetValue("ClientSelectedPreferences.MinProtein", out var minProtein);
            request.Form.TryGetValue("ClientSelectedPreferences.MaxProtein", out var maxProtein);

            var userIntolerances = new StringBuilder();
            foreach(var intolerance in AllowedUserPreferenceOptions.Intolerances)
            {
                if(request.Form.TryGetValue(intolerance, out var intoleranceValue))
                {
                    if(intoleranceValue[0] == "true")
                    {
                        userIntolerances.Append($"{intolerance},");
                    }                        
                }
            }

            var savedUserPreferences = new DBUserPreferences()
            {
                UserId = userId,
                Diet = diet.FirstOrDefault(),
                Intolerances = userIntolerances.ToString(),
                MaxCalorie = int.TryParse(maxCalorie.FirstOrDefault(), out var maxCalories) ? maxCalories : null,
                MaxCarb = int.TryParse(maxCarb.FirstOrDefault(), out var maxCarbs) ? maxCarbs : null,
                MaxProtein = int.TryParse(maxProtein.FirstOrDefault(), out var maxProteins) ? maxProteins : null,
                MinProtein = int.TryParse(minProtein.FirstOrDefault(), out var minProteins) ? minProteins : null
            };
            return View();
        }
    }
}
