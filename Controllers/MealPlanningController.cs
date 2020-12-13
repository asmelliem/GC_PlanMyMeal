using GC_PlanMyMeal.Models;
using GC_PlanMyMeal.Models.Enums;
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
    public class MealPlanningController : Controller
    {
        private readonly ISearchRecipe _recipeClient;
        private readonly IRepositoryClient _repositoryClient;

        public MealPlanningController(ISearchRecipe recipeClient, IRepositoryClient repositoryClient)
        {
            _recipeClient = recipeClient;
            _repositoryClient = repositoryClient;
        }
        public ActionResult MealCalendar()
        {
            return View();
        }

        public IActionResult MealPlanningForm(SavedRecipeListViewModel recipe)
        {
            var mealTimeTypes = new List<MealTimeSelectOptions>();

            foreach (MealTimeType mealTimeType in Enum.GetValues(typeof(MealTimeType)))
            {
                mealTimeTypes.Add(new MealTimeSelectOptions()
                {
                    MealTimeType = mealTimeType,
                    MealTimeTypeValue = mealTimeType.ToString()
                });
            }

            var recipeInfo = new MealPlanningFormViewModel()
            {
                RecipeId = recipe.RecipeId,
                CustomRecipeId = recipe.CustomeRecipeId,
                MealTimeSelectOptions = mealTimeTypes
            };

            return View(recipeInfo);
        }

        [HttpPost]
        public async Task<IActionResult> SaveMealPlan(int? CustomRecipeId, int? RecipeId, DateTime CookDate, MealTimeType MealTime)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var recipe = new RecipeCalendar()
            {
                UserId = userId,
                RecipeId = RecipeId,
                CustomRecipeId = CustomRecipeId,
                CookDate = CookDate,
                MealTime = MealTime.ToString()
            };

            var isMealPlanSaved = await _repositoryClient.SaveMealPlan(recipe);
            if(isMealPlanSaved)
            {
                return RedirectToAction("MealCalendar", "MealPlanning");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
