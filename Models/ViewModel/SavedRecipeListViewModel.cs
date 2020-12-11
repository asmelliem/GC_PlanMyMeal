using GC_PlanMyMeal.RecipeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Models.ViewModel
{
    public class SavedRecipeListViewModel
    {
        public int? RecipeId { get; set; }
        public int? CustomeRecipeId { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string RecipeURL { get; set; }
    }
}
