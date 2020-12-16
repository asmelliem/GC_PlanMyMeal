using GC_PlanMyMeal.Configuration;
using GC_PlanMyMeal.Models;
using GC_PlanMyMeal.RecipeService.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.RecipeService
{
    //Cache is used to store recipe Id so as to save api consumption when making the calls to search by recipe Id
    public class RecipeClientWithCache : RecipeClient
    {
        private readonly ConcurrentDictionary<int, Recipe> _recipeCache;

        public RecipeClientWithCache(HttpClient httpClient, SpoonacularConfiguration config, ConcurrentDictionary<int, Recipe> recipeCache) : base(httpClient, config)
        {
            _recipeCache = recipeCache;
        }

        public override async Task<Recipe> SearchForRecipeById(int? id)
        {
            if (id == null)
            {
                return await base.SearchForRecipeById(id);
            }

            if(_recipeCache.TryGetValue(id.Value, out var recipe))
            {
                return recipe;
            }

            recipe = await base.SearchForRecipeById(id);
            _recipeCache.TryAdd(id.Value, recipe);
            return recipe;
        }        
    }  
}
