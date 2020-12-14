using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GC_PlanMyMeal.Areas.Identity.Data;
using GC_PlanMyMeal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GC_PlanMyMeal.PreferencesService
{
    public class PreferencesClient : IPreferencesClient
    {
        private readonly GC_PlanMyMealIdentityDbContext _context;

        public PreferencesClient(GC_PlanMyMealIdentityDbContext context)
        {
            _context = context;
        }

       public async Task<bool> SavedUserPreferences(string userId)
        {
            var result = await _context.UserPreferences.FirstOrDefaultAsync(r => r.UserId == userId);
            if(result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<UserPreference> RetrieveUserPreferences(string userId)
        {
            var result = await _context.UserPreferences.FirstOrDefaultAsync(r => r.UserId == userId);
            return result;
        }

        public async Task<UserPreference> SaveUserPreferences(int id, string userId, string diet, Intolerances intolerances, int? maxCalorie, int? maxCarb, int? maxProtein, int? minProtein)
        {
            var savedUserPreferences = new UserPreference()
            {
                Id = id,
                UserId = userId,
                Diet = diet,
                Intolerances = intolerances.ToString(),
                MaxCalorie = maxCalorie,
                MaxCarb = maxCarb,
                MaxProtein = maxProtein,
                MinProtein = minProtein,
            };
            try
            {
                _context.UserPreferences.Add(savedUserPreferences);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return savedUserPreferences;
        }
    }
}
