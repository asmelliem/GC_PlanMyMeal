using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.RecipeService.Models
{
    //Strictly used for API
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ReadyInMinutes { get; set; }
        public int Servings { get; set; }
        public string SourceUrl { get; set; }
        public string Image { get; set; }
        public string Summary { get; set; }
        public string Instructions { get; set; }
        public List<ExtendedIngredients> ExtendedIngredients { get; set; } 
    }

    public class ReciepeApiResults
    {
        public List<Recipe> Results { get; set; }
    }
}
