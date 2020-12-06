using GC_PlanMyMeal.Configuration;
using GC_PlanMyMeal.RecipeService.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.RecipeService
{
    public class RecipeClient : ISearchRecipe
    {
        private readonly HttpClient _httpClient;
        private readonly SpoonacularConfiguration _config;

        public RecipeClient(HttpClient httpClient, SpoonacularConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<Recipe> SearchForRecipeById (int id)
        {            
            var response = await _httpClient.GetAsync($"recipes/{id}/information?apiKey={_config.ApiKey}");
            var recipe = JsonConvert.DeserializeObject<Recipe>(await response.Content.ReadAsStringAsync());
            return recipe;
        }

        //&intolerances=egg&diet=vegetarian&maxCarbs=400&maxProtein=15&minProtein=1
        public async Task<List<Recipe>> SearchForRecipeByQuery (string diet, string intolerance, int? maxCalorie, int? maxCarb, int? maxProtein, int? minProtein)
        {
            StringBuilder query = new StringBuilder();
            if(diet != null){ query.Append($"&diet={diet}"); }
            if (intolerance != null) { query.Append($"&intolerances={intolerance}"); }
            if (maxCalorie.HasValue) { query.Append($"&maxCalorie={maxCalorie}"); }
            if (maxCarb.HasValue) { query.Append($"&maxCarb={maxCarb}"); }
            if (maxProtein.HasValue) { query.Append($"&maxProtein={maxProtein}"); }
            if (minProtein.HasValue) { query.Append($"&minProtein={minProtein}"); }
            var response = await _httpClient.GetAsync($"/recipes/complexSearch?apiKey={_config.ApiKey}{query}");
            var recipe = JsonConvert.DeserializeObject<ReciepeApiResults>(await response.Content.ReadAsStringAsync());
            return recipe.Results;
        }
    }
}
