using GC_PlanMyMeal.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Models.ViewModel
{
    public class MealPlanningFormViewModel
    {
        public int? RecipeId { get; set; }
        public int? CustomRecipeId { get; set; }
        public DateTime CookDate { get; set; }
        public MealTimeType MealTime { get; set; }
        public List<MealTimeSelectOptions> MealTimeSelectOptions { get; set; } 
    }
}
