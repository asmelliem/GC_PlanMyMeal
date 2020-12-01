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
        private readonly IConfiguration _configuration;
        public RecipeClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<Recipe> SearchForRecipeById (int id)
        {
            var apiKey = _configuration.GetValue<string>("apiKey");
            var response = await _httpClient.GetAsync($"recipes/{id}/information?apiKey={apiKey}");
            var recipe = JsonConvert.DeserializeObject<Recipe>(await response.Content.ReadAsStringAsync());
            return recipe;
        }
    }
}
