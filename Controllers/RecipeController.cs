using GC_PlanMyMeal.Models;
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
            var savedCustomRecipeList = await _repositoryClient.RetrieveCustomRecipeList(userId);
            var recipeList = new List<SavedRecipeListViewModel>();
            foreach(var recipe in savedRecipeList)
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
            foreach(var recipe in savedCustomRecipeList)
            {
                var recipeResult = new SavedRecipeListViewModel()
                {
                    CustomeRecipeId = recipe.Id,
                    Name = recipe.RecipeName,
                    ImageURL = null,
                    RecipeURL = null
                };
                recipeList.Add(recipeResult);
            }
            return View(recipeList);
        }

        public async Task<IActionResult> DeleteSavedRecipe(SavedRecipeListViewModel recipeInfo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isRecipeDeleted = false;
            if (recipeInfo.CustomeRecipeId == null)
            {
                isRecipeDeleted = await _repositoryClient.DeleteRecipe(userId, recipeInfo.RecipeId, null);
            }
            else
            {
                isRecipeDeleted = await _repositoryClient.DeleteRecipe(userId, null, recipeInfo.CustomeRecipeId);
            }
            
            if(isRecipeDeleted)
            {
                return RedirectToAction("SavedRecipeList", "Recipe");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }            
        }

        public IActionResult CreateRecipePage()
        {
            return View();
        }

        public async Task<IActionResult> CreateRecipe(string recipeName, string ingredients, string directions, string notes)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customRecipe = new CustomRecipe()
            {
                UserId = userId,
                RecipeName = recipeName,
                Ingredients = ingredients,
                Directions = directions,
                Notes = notes
            };
            var isRecipeSaved = await _repositoryClient.AddCustomRecipe(customRecipe);
            if(isRecipeSaved)
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
