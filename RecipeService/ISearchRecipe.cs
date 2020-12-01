using GC_PlanMyMeal.RecipeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.RecipeService
{
    public interface ISearchRecipe
    {
        Task<Recipe> SearchForRecipeById (int id);
    }
}
