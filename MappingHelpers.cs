using GC_PlanMyMeal.Models;
using GC_PlanMyMeal.Models.ViewModel;
using GC_PlanMyMeal.RecipeService;
using GC_PlanMyMeal.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal
{
    public static class MappingHelpers
    {
        //Ueed to map recipes we retrieve of type RecipeCalendar to convert to type RecipeInfoViewModel
        public static async Task<RecipeInfoViewModel> MealMapping(RecipeCalendar recipe, ISearchRecipe recipeClient, IRepositoryClient repositoryClient)
        {
            if(recipe == null)
            {
                return new RecipeInfoViewModel();
            }

            string name;

            if(recipe.CustomRecipeId != null)
            {
                var recipeInfo = await repositoryClient.RetrieveCustomRecipe(recipe.UserId, recipe.CustomRecipeId);
                name = recipeInfo.RecipeName;
            }
            else
            {
                var recipeInfo = await recipeClient.SearchForRecipeById(recipe.RecipeId);
                name = recipeInfo.Title;
            }

            return new RecipeInfoViewModel()
            {
                RecipeId = recipe.RecipeId,
                CustomRecipeId = recipe.CustomRecipeId,
                RecipeName = name
            };
        }
    }
}
