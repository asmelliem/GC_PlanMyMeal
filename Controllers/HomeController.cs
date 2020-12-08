using GC_PlanMyMeal.Models;
using GC_PlanMyMeal.RecipeService;
using GC_PlanMyMeal.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISearchRecipe _recipeClient;
        private readonly IRepositoryClient _repositoryClient;
        
        public HomeController(ISearchRecipe recipeClient, IRepositoryClient repositoryClient)
        {
            _recipeClient = recipeClient;
            _repositoryClient = repositoryClient;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> SearchRecipe(string diet, string intolerances, int? maxCalorie, int? maxCarb, int? maxProtein, int? minProtein)
        {
            var recipes = await _recipeClient.SearchForRecipeByQuery(diet, intolerances, maxCalorie, maxCarb, maxProtein, minProtein);
            var recipeSearchResults = new List<RecipeSearchResult>();
            foreach (var recipe in recipes)
            {
                var recipeResult = new RecipeSearchResult()
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Image = recipe.Image,
                };
                recipeSearchResults.Add(recipeResult);
            }
          
            return View(recipeSearchResults);
        }

        public async Task<IActionResult> ConfirmSaveRecipe(RecipeSearchResult recipeSearchResult)
        {
            var htmlRegEx = "<[^>]*>";
            var recipe = await _recipeClient.SearchForRecipeById(recipeSearchResult.Id);
            var recipeResult = new RecipeConfirmationInfoViewModel()
            {
                Title = recipe.Title,
                Image = recipe.Image,
                Id = recipe.Id,
                Summary = Regex.Replace(recipe.Summary, htmlRegEx, string.Empty),
                Instructions = Regex.Replace(recipe.Instructions, htmlRegEx, string.Empty),
                ExtendedIngredients = recipe.ExtendedIngredients
            };
            return View(recipeResult);
        }

        public async Task<IActionResult> SaveRecipe(RecipeConfirmationInfoViewModel recipeInfo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var recipeIsSaved = await _repositoryClient.SaveRecipe(recipeInfo.Id, userId);
            if(recipeIsSaved)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
            
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
