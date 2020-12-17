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

        //get a list of the user's saved recipes and pass that to the view to display
        public async Task<IActionResult> SavedRecipeList()
        {     
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

        //Deleting a saved recipe from the database and returning back to the saved recipe view
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

        //Directs user to the Create Custom Recipe page/form
        public IActionResult CreateRecipePage()
        {
            return View(new CustomRecipeViewModel());
        }

        [HttpPost]
        //Adds custom recipe to the database, then redirects to the saved recipe view
        public async Task<IActionResult> CreateRecipe(string recipeName, string ingredients, string directions, string notes, int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(recipeName))
            {
                TempData["Error"] = "You cannot have a blank name for recipe. Please try again.";
                return RedirectToAction("Error", "Home");
            }
            var customRecipe = new CustomRecipe()
            {
                UserId = userId,
                RecipeName = recipeName,
                Ingredients = string.IsNullOrEmpty(ingredients) ? string.Empty : ingredients,
                Directions = string.IsNullOrEmpty(directions) ? string.Empty : directions,
                Notes = string.IsNullOrEmpty(notes) ? string.Empty : notes
            };
            var isRecipeSaved = false;
            if (id == null)
            {                
                isRecipeSaved = await _repositoryClient.AddCustomRecipe(customRecipe);
                
            }
            else
            {
                customRecipe.Id = id.Value;
                isRecipeSaved = await _repositoryClient.UpdateRecipe(customRecipe);
            }

            if (isRecipeSaved)
            {
                return RedirectToAction("SavedRecipeList", "Recipe");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }

        }
        
        //Retrieve's custom recipe info to display on the edit form
        public async Task<IActionResult> EditCustomRecipe(SavedRecipeListViewModel recipe)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customRecipeInfo = await _repositoryClient.RetrieveCustomRecipe(userId, recipe.CustomeRecipeId);
            var customRecipe = new CustomRecipeViewModel()
            {
                UserId = customRecipeInfo.UserId,
                RecipeName = customRecipeInfo.RecipeName,
                Ingredients = customRecipeInfo.Ingredients,
                Directions = string.IsNullOrEmpty(customRecipeInfo.Directions) ?string.Empty : customRecipeInfo.Directions,
                Notes = string.IsNullOrEmpty(customRecipeInfo.Notes) ? string.Empty : customRecipeInfo.Notes,
                Id = customRecipeInfo.Id
            };
            return View("CreateRecipePage", customRecipe);
        }

        //Display custom recipe info for the user
        public async Task<IActionResult> DisplayCustomRecipeInfo(int customRecipeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customRecipeInfo = await _repositoryClient.RetrieveCustomRecipe(userId, customRecipeId);
            var ingredientsList = customRecipeInfo.Ingredients.Split(',').ToList();
            var customRecipe = new DisplayCustomRecipeInfoModelView()
            {
                RecipeName = customRecipeInfo.RecipeName,
                Ingredients = ingredientsList,
                Directions = customRecipeInfo.Directions,
                Notes = customRecipeInfo.Notes
            };
            return View(customRecipe);
        }
    }
}
