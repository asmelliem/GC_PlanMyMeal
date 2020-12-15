using GC_PlanMyMeal.Areas.Identity.Data;
using GC_PlanMyMeal.Models;
using GC_PlanMyMeal.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Repository
{
    public class RepositoryClient: IRepositoryClient
    {
        private readonly GC_PlanMyMealIdentityDbContext _context;

        public RepositoryClient(GC_PlanMyMealIdentityDbContext context)
        {
            _context = context;
        }

        //Saves recipes to the database
        public async Task<bool> SaveRecipe(int? recipeId, string userId)
        {
            var savedRecipe = new SavedRecipe()
            {
                UserId = userId,
                RecipeId = recipeId
            };
            try
            {
                _context.SavedRecipes.Add(savedRecipe);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }         
            return true;
        }
        //Verify if particular recipe is saved to the database
        public async Task<bool> FindSavedRecipe(int recipeId, string userId)
        {
            var result = await _context.SavedRecipes.FirstOrDefaultAsync(r => r.RecipeId == recipeId && r.UserId == userId);
            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //Retrieve a list of saved recipes from the database
        public async Task<List<SavedRecipe>> RetrieveRecipeList(string userId)
        {
            var result = await _context.SavedRecipes.Where(r => r.UserId == userId).ToListAsync();
            return result;
        }
        //Retreieve a particular custom recipe from the database
        public async Task<CustomRecipe> RetrieveCustomRecipe(string userId, int? customRecipeId)
        {
            var result = await _context.CustomRecipes.FirstOrDefaultAsync(r => r.UserId == userId && r.Id == customRecipeId);
            return result;
        }
        //Retreieve a list of recipes from the databse
        public async Task<List<CustomRecipe>> RetrieveCustomRecipeList(string userId)
        {
            var result = await _context.CustomRecipes.Where(r => r.UserId == userId).ToListAsync();
            return result;
        }
        //Deletes a recipe from the saved recipe table and returns true/false on success/failure
        public async Task<bool> DeleteRecipe(string userId, int? recipeId, int? customId)
        {
            try
            {
                if(recipeId != null)
                {
                    var recipe = await _context.SavedRecipes.FirstOrDefaultAsync(r => r.RecipeId == recipeId && r.UserId == userId);
                    _context.SavedRecipes.Remove(recipe);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    var recipe = await _context.CustomRecipes.FirstOrDefaultAsync(r => r.Id == customId && r.UserId == userId);
                    var isRemovedFromCustomRecipeTable = await DeleteParticularRecipeFromMealPlans(customId, userId);
                    if(isRemovedFromCustomRecipeTable)
                    {
                        _context.CustomRecipes.Remove(recipe);
                        _context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }                
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //Add custom recipe to the customRecipes table and returns true/false on success/failure
        public async Task<bool> AddCustomRecipe(CustomRecipe customRecipe)
        {
            try
            {
                await _context.CustomRecipes.AddAsync(customRecipe);
                _context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        //Update custom recipe for customRecipes table and returns true/false on success/failure
        public async Task<bool> UpdateRecipe(CustomRecipe customRecipe)
        {
            try
            {
                var recipe = await _context.CustomRecipes.SingleOrDefaultAsync(r => r.Id == customRecipe.Id);
                recipe.RecipeName = customRecipe.RecipeName;
                recipe.Ingredients = customRecipe.Ingredients;
                recipe.Directions = customRecipe.Directions;
                recipe.Notes = customRecipe.Notes;
                _context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        //Add meal to the RecipeCalendar table. Returns true/false on success/failure
        public async Task<bool> SaveMealPlan(RecipeCalendar recipe)
        {
            try
            {
                await _context.RecipeCalendars.AddAsync(recipe);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        //Returns a list of meals on the recipeCalendar for a particular user for meal planning
        public async Task<List<RecipeCalendar>> GetMealPlan(string userId)
        {
            var result = await _context.RecipeCalendars.Where(r => r.UserId == userId && r.CookDate >= DateTime.Today && r.CookDate <= DateTime.Today.AddDays(6)).ToListAsync();
            return result;
            
        }
        //Used to verify if user already has an existing meal for the time and meal type
        public async Task<bool> VerifyMealPlanStatus(RecipeCalendar recipe)
        {
            var result = await _context.RecipeCalendars.FirstOrDefaultAsync(r => r.UserId == recipe.UserId && r.CookDate == recipe.CookDate && r.MealTime == recipe.MealTime);
            if(result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }
        //Deletes custom recipe from RecipeCalendar. Returns true/false on success/failure
        public async Task<bool> DeleteCustomRecipeFromMealPlan(int customRecipeId, string userId, int numDaysFromToday)
        {
            try
            {
                var day = DateTime.Today.AddDays(numDaysFromToday);
                var recipe = await _context.RecipeCalendars.FirstOrDefaultAsync(r => r.UserId == userId && r.CustomRecipeId == customRecipeId && r.CookDate == day);
                _context.RecipeCalendars.Remove(recipe);
                _context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        //Deletes api recipe from RecipeCalendar. Returns true/false on success/failure
        public async Task<bool> DeleteAPIRecipeFromMealPlan(int recipeId, string userId, int numDaysFromToday)
        {
            var day = DateTime.Today.AddDays(numDaysFromToday);
            try
            {
                var recipe = await _context.RecipeCalendars.FirstOrDefaultAsync(r => r.UserId == userId && r.RecipeId == recipeId && r.CookDate == day);
                _context.RecipeCalendars.Remove(recipe);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        public async Task<bool> DeleteParticularRecipeFromMealPlans(int? customRecipeId, string userId)
        {
            try
            {
                var recipes = await _context.RecipeCalendars.Where(r => r.UserId == userId && r.CustomRecipeId == customRecipeId).ToListAsync();
                foreach (var recipe in recipes)
                {
                    _context.RecipeCalendars.Remove(recipe);
                    _context.SaveChanges();
                }
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            
            
        }
    }
}
