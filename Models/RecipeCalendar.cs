using System;
using System.Collections.Generic;

#nullable disable

namespace GC_PlanMyMeal.Models
{
    public partial class RecipeCalendar
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? RecipeId { get; set; }
        public int? CustomRecipeId { get; set; }
        public DateTime CookDate { get; set; }
        public string MealTime { get; set; }
        public bool HasBeenCooked { get; set; }
        public virtual CustomRecipe CustomRecipe { get; set; }
    }
}
