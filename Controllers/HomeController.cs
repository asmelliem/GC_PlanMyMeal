using GC_PlanMyMeal.Models;
using GC_PlanMyMeal.PreferencesService;
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
        private readonly IPreferencesClient _preferencesClient;

        public HomeController(ISearchRecipe recipeClient, IRepositoryClient repositoryClient, IPreferencesClient preferencesClient)
        {
            _recipeClient = recipeClient;
            _repositoryClient = repositoryClient;
            _preferencesClient = preferencesClient;
        }

        //Return home page
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var hasUserPreferences = await _preferencesClient.SavedUserPreferences(userId);            
            var intolerances = new Intolerances();

            if (User.Identity.IsAuthenticated && hasUserPreferences)
            {
                var userPreferences = await _preferencesClient.RetrieveUserPreferences(userId);
                var intolerenceList = userPreferences.Intolerances.Split(',').ToList();
                if (intolerenceList.Contains("egg")) { intolerances.egg = true; }
                if (intolerenceList.Contains("dairy")) { intolerances.dairy = true; }
                if (intolerenceList.Contains("gluton")) { intolerances.gluton = true; }
                if (intolerenceList.Contains("grain")) { intolerances.grain = true; }
                if (intolerenceList.Contains("peanut")) { intolerances.peanut = true; }
                if (intolerenceList.Contains("sesame")) { intolerances.sesame = true; }
                if (intolerenceList.Contains("seafood")) { intolerances.seafood = true; }
                if (intolerenceList.Contains("shellfish")) { intolerances.shellfish = true; }
                if (intolerenceList.Contains("soy")) { intolerances.sulfite = true; }
                if (intolerenceList.Contains("sulfite")) { intolerances.sulfite = true; }
                if (intolerenceList.Contains("treeNut")) { intolerances.treeNut = true; }
                if (intolerenceList.Contains("wheat")) { intolerances.wheat = true; }
                var recipeList = await _recipeClient.SearchForRecipeByQuery(userPreferences.Diet, intolerances, userPreferences.MaxCalorie, userPreferences.MaxCarb, userPreferences.MaxProtein, userPreferences.MinProtein);
                Random random = new Random();
                var randomRecipeNumber = random.Next(recipeList.Count);
                var randomRecipe = recipeList.ElementAt(randomRecipeNumber);
                ViewBag.RecipeName = randomRecipe.Title;
                ViewBag.ImageURL = randomRecipe.Image;
                ViewBag.Id = randomRecipe.Id;
                ViewBag.Summary = randomRecipe.Summary;
                return View();                
            }
            else
            {
                var recipeList = await _recipeClient.SearchForAllRecipes();
                Random random = new Random();
                var randomRecipeNumber = random.Next(recipeList.Count);
                var randomRecipe = recipeList.ElementAt(randomRecipeNumber);
                ViewBag.RecipeName = randomRecipe.Title;
                ViewBag.ImageURL = randomRecipe.Image;
                ViewBag.Id = randomRecipe.Id;
                ViewBag.Summary = randomRecipe.Summary;
                return View();
            }
        }
        
        //This is called when users put in their search criteria via the home page. This calls out to the recipeClient, which makes the API
        //calls to return back the list of recipes
        public async Task<IActionResult> SearchRecipe(string diet, Intolerances intolerances, int? maxCalorie, int? maxCarb, int? maxProtein, int? minProtein)
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

        //Directs to ConfirmSaveRecipe when users click the Save Recipe button. This shows the recipe info
        public async Task<IActionResult> ConfirmSaveRecipe(int id)
        {
            //Regex to remove the html tags from the summary and directions 
            var htmlRegEx = "<[^>]*>";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var recipe = await _recipeClient.SearchForRecipeById(id);
            var isSavedRecipe = await _repositoryClient.FindSavedRecipe(id, userId);
            var recipeResult = new RecipeConfirmationInfoViewModel()
            {
                Title = recipe.Title,
                Image = recipe.Image,
                Id = recipe.Id,
                Summary = Regex.Replace(recipe.Summary ?? "No Summary Available", htmlRegEx, string.Empty),
                Instructions = Regex.Replace(recipe.Instructions ?? "No Instructions Available", htmlRegEx, string.Empty),
                ExtendedIngredients = recipe.ExtendedIngredients,
                UserSavedRecipe = isSavedRecipe
            };
            return View(recipeResult);
        }

        //Calls the respository client to save the recipe to the database
        public async Task<IActionResult> SaveRecipe(RecipeConfirmationInfoViewModel recipeInfo)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var recipeIsSaved = await _repositoryClient.SaveRecipe(recipeInfo.Id, userId);
                if (recipeIsSaved)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return RedirectToAction("Index", "Home");
        }       

        //Returns generic error page. Used for our error handling
        public IActionResult Error()
        {
            return View();
        }
    }
}
