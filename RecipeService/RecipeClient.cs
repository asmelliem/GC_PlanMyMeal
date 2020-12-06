using GC_PlanMyMeal.Configuration;
using GC_PlanMyMeal.RecipeService.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    }
}
