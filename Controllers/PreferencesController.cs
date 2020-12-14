using GC_PlanMyMeal.Areas.Identity.Data;
using GC_PlanMyMeal.DatabaseModels;
using GC_PlanMyMeal.Models;
using GC_PlanMyMeal.PreferencesService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IPreferencesClient _preferencesClient;

        public PreferencesController(GC_PlanMyMealIdentityDbContext context, IPreferencesClient preferencesClient)
        {
            _context = context;
            _preferencesClient = preferencesClient;
        }
        public async Task<IActionResult> UserPreferences()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var savedUserPreferences = await _preferencesClient.SavedUserPreferences(userId);

            if (savedUserPreferences == false)
            {
                return View();
            }
            else
            {
                var retrieveUserPreferences = await _preferencesClient.RetrieveUserPreferences(userId);
               
                return View("SavePreferences", retrieveUserPreferences);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePreferences(UserPreferencesViewModel userPreferencesViewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var savedUserPreferences = await _preferencesClient.SaveUserPreferences(userPreferencesViewModel.Id, userId, userPreferencesViewModel.Diet, userPreferencesViewModel.Intolerances, userPreferencesViewModel.MaxCalorie, userPreferencesViewModel.MaxCarb, userPreferencesViewModel.MaxProtein, userPreferencesViewModel.MinProtein);
            var savedUserPreferencesModel = new UserPreference()
            {
                UserId = savedUserPreferences.UserId,
                Diet = savedUserPreferences.Diet,
                Intolerances = savedUserPreferences.Intolerances,
                MaxCalorie = savedUserPreferences.MaxCalorie,
                MaxCarb = savedUserPreferences.MaxCarb,
                MaxProtein = savedUserPreferences.MaxProtein,
                MinProtein = savedUserPreferences.MinProtein,
            };
            return View(savedUserPreferencesModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditPreferences()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var retrieveUserPreferences = await _preferencesClient.RetrieveUserPreferences(userId);
            List<string> intolerenceList = retrieveUserPreferences.Intolerances.Split(',').ToList();
            var intolerances = new Intolerances();
            if (intolerenceList.Contains("egg")) { intolerances.egg = true; }
            if (intolerenceList.Contains("dairy")) { intolerances.dairy = true; }
            if (intolerenceList.Contains("gluton")) { intolerances.gluton = true; }
            if (intolerenceList.Contains("grain")) { intolerances.grain = true; }
            if (intolerenceList.Contains("peanut")) { intolerances.peanut = true; }
            if (intolerenceList.Contains("sesame")) { intolerances.sesame = true; }
            if (intolerenceList.Contains("seafood")) { intolerances.seafood = true; }
            if (intolerenceList.Contains("shellfish")) { intolerances.shellfish = true; }
            if (intolerenceList.Contains("soy")) { intolerances.sulfite = true; }
            if (intolerenceList.Contains("sulfite")) { intolerances.sulfite = true; }
            if (intolerenceList.Contains("treeNut")) { intolerances.treeNut = true; }
            if (intolerenceList.Contains("wheat")) { intolerances.wheat = true; }
            var retrieveUserPreferencesModel = new UserPreferencesViewModel()
            {
                Id = retrieveUserPreferences.Id,
                UserID = userId,
                Diet = retrieveUserPreferences.Diet,
                Intolerances = intolerances,
                MaxCalorie = retrieveUserPreferences.MaxCalorie,
                MaxCarb = retrieveUserPreferences.MaxCarb,
                MaxProtein = retrieveUserPreferences.MaxProtein,
                MinProtein = retrieveUserPreferences.MinProtein,
            };

            return View(retrieveUserPreferencesModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditPreferencesSaved(UserPreferencesViewModel userPreferenceViewModel)
        {
            //check for previously saved prefernce
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var previousPreferences = await _preferencesClient.RetrieveUserPreferences(userId);
            if (previousPreferences == null)
            {
                //why is the user editing a preference? should we just save instead? idk for u to decide
            }

           
                previousPreferences.Diet = userPreferenceViewModel?.Diet;
            previousPreferences.Intolerances = !string.IsNullOrEmpty(userPreferenceViewModel.Intolerances.ToString()) ? userPreferenceViewModel.Intolerances.ToString() : "";
   
                previousPreferences.MaxCalorie = userPreferenceViewModel?.MaxCalorie;
      
                previousPreferences.MaxCarb = userPreferenceViewModel?.MaxCarb;
            
                previousPreferences.MaxProtein = userPreferenceViewModel?.MaxProtein;
            
                previousPreferences.MinProtein = userPreferenceViewModel?.MinProtein;

            _context.Update(previousPreferences);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(UserPreferences));
        }
    }
}
