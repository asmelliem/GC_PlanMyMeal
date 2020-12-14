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

        //Calls the repositoryClient to get a list of meals by the user in the RecipeCalendar table for the next 6 days
        //groups these meals by MealTime (breakfast,lunch,dinner,snack)
        //Loops through the different gorupings in the meal group and maps out the meal to the correct day (mealNameOne, mealNameTwo)
        public async Task<ActionResult> MealCalendar()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var meals = await _repositoryClient.GetMealPlan(userId);
            var mealPlan = new List<MealCalendarDataRowViewModel>();
            var mealGroups = meals.GroupBy(m => m.MealTime);

            foreach (var group in mealGroups.OrderBy(m => (MealTimeType)Enum.Parse(typeof(MealTimeType), m.Key) ))
            {
                var mealType = new MealCalendarDataRowViewModel();
                mealType.MealTimeType = (MealTimeType)Enum.Parse(typeof(MealTimeType), group.Key);
                var mealsByDay = group.ToList();
                mealType.MealNameOne = await MappingHelpers.MealMapping(mealsByDay.FirstOrDefault(m => m.CookDate.Date == DateTime.Today), _recipeClient, _repositoryClient);
                mealType.MealNameTwo = await MappingHelpers.MealMapping(mealsByDay.FirstOrDefault(m => m.CookDate.Date == DateTime.Today.AddDays(1)), _recipeClient, _repositoryClient);
                mealType.MealNameThree = await MappingHelpers.MealMapping(mealsByDay.FirstOrDefault(m => m.CookDate.Date == DateTime.Today.AddDays(2)), _recipeClient, _repositoryClient);
                mealType.MealNameFour = await MappingHelpers.MealMapping(mealsByDay.FirstOrDefault(m => m.CookDate.Date == DateTime.Today.AddDays(3)), _recipeClient, _repositoryClient);
                mealType.MealNameFive = await MappingHelpers.MealMapping(mealsByDay.FirstOrDefault(m => m.CookDate.Date == DateTime.Today.AddDays(4)), _recipeClient, _repositoryClient);
                mealType.MealNameSix = await MappingHelpers.MealMapping(mealsByDay.FirstOrDefault(m => m.CookDate.Date == DateTime.Today.AddDays(5)), _recipeClient, _repositoryClient);
                mealType.MealNameSeven = await MappingHelpers.MealMapping(mealsByDay.FirstOrDefault(m => m.CookDate.Date == DateTime.Today.AddDays(6)), _recipeClient, _repositoryClient);
                mealPlan.Add(mealType);
            }
            return View(mealPlan);
        }


        //Displays the list of meal time options for the form
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

        public IActionResult MealCalendarError()
        {
            return View();

        }
        
        //Checks to make sure the user doesn't already have a meal in the same date/meal time. If not, then saves the meal
        //to the database. If another already exists, returns error message page
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
            var isExistingMealPlan = await _repositoryClient.VerifyMealPlanStatus(recipe);
            if(isExistingMealPlan)
            {
                return RedirectToAction("MealCalendarError", "MealPlanning");
            }
            else
            {
                var isMealPlanSaved = await _repositoryClient.SaveMealPlan(recipe);
                if (isMealPlanSaved)
                {
                    return RedirectToAction("MealCalendar", "MealPlanning");
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }            
        }


        //Returns recipe URL for the api recipes
        public async Task<IActionResult> DisplayRecipeInfo(int recipeId)
        {            
            var recipe = await _recipeClient.SearchForRecipeById(recipeId);
            return Redirect(recipe.SourceUrl);
        }

        //Deletes custom recipes from RecipeCalendar database
        public async Task<IActionResult> DeleteCustomRecipe(int customRecipeId, int numDaysFromToday)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var recipeIsDelete = await _repositoryClient.DeleteCustomRecipeFromMealPlan(customRecipeId, userId, numDaysFromToday);
            if (recipeIsDelete)
            {
                return RedirectToAction("MealDeleteCalendar", "MealPlanning");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        //Deletes api recipes from RecipeCalendar database
        public async Task<IActionResult> DeleteAPIRecipe(int recipeId, int numDaysFromToday)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var recipeIsDelete = await _repositoryClient.DeleteAPIRecipeFromMealPlan(recipeId, userId, numDaysFromToday);
            if (recipeIsDelete)
            {
                return RedirectToAction("MealDeleteCalendar", "MealPlanning");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        //Calls the repositoryClient to get a list of meals by the user in the RecipeCalendar table for the next 6 days
        //groups these meals by MealTime (breakfast,lunch,dinner,snack)
        //Loops through the different gorupings in the meal group and maps out the meal to the correct day (mealNameOne, mealNameTwo)
        public async Task<IActionResult> MealDeleteCalendar()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var meals = await _repositoryClient.GetMealPlan(userId);
            var mealPlan = new List<MealCalendarDataRowViewModel>();
            var mealGroups = meals.GroupBy(m => m.MealTime);

            foreach (var group in mealGroups.OrderBy(m => (MealTimeType)Enum.Parse(typeof(MealTimeType), m.Key)))
            {
                var mealType = new MealCalendarDataRowViewModel();
                mealType.MealTimeType = (MealTimeType)Enum.Parse(typeof(MealTimeType), group.Key);
                var mealsByDay = group.ToList();
                mealType.MealNameOne = await MappingHelpers.MealMapping(mealsByDay.FirstOrDefault(m => m.CookDate.Date == DateTime.Today), _recipeClient, _repositoryClient);
                mealType.MealNameTwo = await MappingHelpers.MealMapping(mealsByDay.FirstOrDefault(m => m.CookDate.Date == DateTime.Today.AddDays(1)), _recipeClient, _repositoryClient);
                mealType.MealNameThree = await MappingHelpers.MealMapping(mealsByDay.FirstOrDefault(m => m.CookDate.Date == DateTime.Today.AddDays(2)), _recipeClient, _repositoryClient);
                mealType.MealNameFour = await MappingHelpers.MealMapping(mealsByDay.FirstOrDefault(m => m.CookDate.Date == DateTime.Today.AddDays(3)), _recipeClient, _repositoryClient);
                mealType.MealNameFive = await MappingHelpers.MealMapping(mealsByDay.FirstOrDefault(m => m.CookDate.Date == DateTime.Today.AddDays(4)), _recipeClient, _repositoryClient);
                mealType.MealNameSix = await MappingHelpers.MealMapping(mealsByDay.FirstOrDefault(m => m.CookDate.Date == DateTime.Today.AddDays(5)), _recipeClient, _repositoryClient);
                mealType.MealNameSeven = await MappingHelpers.MealMapping(mealsByDay.FirstOrDefault(m => m.CookDate.Date == DateTime.Today.AddDays(6)), _recipeClient, _repositoryClient);
                mealPlan.Add(mealType);
            }
            return View(mealPlan);
        }
    }
}
