using GC_PlanMyMeal.Models;
using GC_PlanMyMeal.RecipeService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISearchRecipe _recipeClient;
        public HomeController(ISearchRecipe recipeClient)
        {
            _recipeClient = recipeClient;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> SearchRecipe(string diet, string intolerances, int maxCalorie, int maxCarb, int maxProtein, int minProtein)
        {
            var recipe = await _recipeClient.SearchForRecipeByQuery(diet, intolerances, maxCalorie, maxCarb, maxProtein, minProtein);
            var recipeResult = new RecipeSearchResult()
            {
                Id = recipe.Id,
                Title = recipe.Title,
                ReadyInMinutes = recipe.ReadyInMinutes,
                Servings = recipe.Servings,
                SourceUrl = recipe.SourceUrl
            };
            return View(recipeResult);
        }
    }
}
