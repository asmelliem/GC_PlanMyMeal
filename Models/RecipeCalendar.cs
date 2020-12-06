using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Models
{
    public class RecipeCalendar
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RecipeId { get; set; }
        public CustomRecipes CustomeRecipeId { get; set; }
        public DateTime CookDate { get; set; }
        public string MealTime { get; set; }
        public bool HasBeenCooked { get; set; }




    }
}
