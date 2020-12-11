using GC_PlanMyMeal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Repository
{
    public interface IRepositoryClient
    {
        Task<bool> SaveRecipe(int? recipeId, string userId);
        Task<bool> FindSavedRecipe(int recipeId, string userId);
        Task<List<SavedRecipe>> RetrieveRecipeList(string userId);
        Task<bool> DeleteRecipe(string userId, int? recipeId, int? customId);
    }
}
