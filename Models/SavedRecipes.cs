using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Models
{
    public class SavedRecipes
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RecipeId { get; set; }
        public CustomRecipes CustomeRecipeId { get; set; }
    }
}
