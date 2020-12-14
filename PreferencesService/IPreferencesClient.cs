using System;
using System.Threading.Tasks;
using GC_PlanMyMeal.Models;
using Microsoft.AspNetCore.Mvc;

namespace GC_PlanMyMeal.PreferencesService
{
    public interface IPreferencesClient
    {
        Task<UserPreference> SaveUserPreferences(int id, string userId, string diet, Intolerances intolerances, int? maxCalorie, int? maxCarb, int? maxProtein, int? minProtein);
        Task<bool> SavedUserPreferences(string userId);
        Task<UserPreference> RetrieveUserPreferences(string userId);
    }
}
