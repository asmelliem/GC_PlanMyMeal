
using System;
using System.Collections.Generic;

#nullable disable

namespace GC_PlanMyMeal.Models
{
    public partial class SavedRecipe
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? RecipeId { get; set; }
        public int? CustomeRecipeId { get; set; }

        public virtual CustomRecipe CustomeRecipe { get; set; }
    }
}
