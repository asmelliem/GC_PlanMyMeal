using GC_PlanMyMeal.Models.ViewModel;
using GC_PlanMyMeal.RecipeService;
using GC_PlanMyMeal.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ISearchRecipe _recipeClient;
        private readonly IRepositoryClient _repositoryClient;

        public RecipeController(ISearchRecipe recipeClient, IRepositoryClient repositoryClient)
        {
            _recipeClient = recipeClient;
            _repositoryClient = repositoryClient;
        }

        public async Task<IActionResult> SavedRecipeList()
        {
            //get a list of the user's saved recipes and pass that to the view to display
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var savedRecipeList = await _repositoryClient.RetrieveRecipeList(userId);
            var recipeList = new List<SavedRecipeListViewModel>();
            foreach(var recipe in savedRecipeList)
            {
                if (recipe.RecipeId != null)
                {
                    var recipeInfo = await _recipeClient.SearchForRecipeById(recipe.RecipeId);
                    var recipeResult = new SavedRecipeListViewModel()
                    {
                        RecipeId = recipe.RecipeId,
                        Name = recipeInfo.Title,
                        ImageURL = recipeInfo.Image,
                        RecipeURL = recipeInfo.SourceUrl
                    };
                    recipeList.Add(recipeResult);
                }
            }
            return View(recipeList);
        }

        public async Task<IActionResult> DeleteSavedRecipe(SavedRecipeListViewModel recipeInfo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isRecipeDeleted = await _repositoryClient.DeleteRecipe(userId, recipeInfo.RecipeId, null);
            if(isRecipeDeleted)
            {
                return RedirectToAction("SavedRecipeList", "Recipe");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
            
        }
    }
}
