using System;
using System.Collections.Generic;
using GC_PlanMyMeal.RecipeService.Models;

namespace GC_PlanMyMeal.Models
{
    public class RecipeConfirmationInfoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Summary { get; set; }
        public string Instructions { get; set; }
        public List<ExtendedIngredients> ExtendedIngredients { get; set; }
        public bool UserSavedRecipe { get; set; }
    }
}
