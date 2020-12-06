using System;
using System.Collections.Generic;

#nullable disable

namespace GC_PlanMyMeal.Models
{
    public partial class CustomRecipe
    {
        public CustomRecipe()
        {
            RecipeCalendars = new HashSet<RecipeCalendar>();
            SavedRecipes = new HashSet<SavedRecipe>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string RecipeName { get; set; }
        public string Ingredients { get; set; }
        public string Directions { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<RecipeCalendar> RecipeCalendars { get; set; }
        public virtual ICollection<SavedRecipe> SavedRecipes { get; set; }
    }
}
