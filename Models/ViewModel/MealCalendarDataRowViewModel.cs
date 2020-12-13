using GC_PlanMyMeal.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Models.ViewModel
{
    public class MealCalendarDataRowViewModel
    {
        public MealTimeType MealTimeType { get; set; }
        public RecipeInfoViewModel MealNameOne { get; set; }
        public RecipeInfoViewModel MealNameTwo { get; set; }
        public RecipeInfoViewModel MealNameThree { get; set; }
        public RecipeInfoViewModel MealNameFour { get; set; }
        public RecipeInfoViewModel MealNameFive { get; set; }
        public RecipeInfoViewModel MealNameSix { get; set; }
        public RecipeInfoViewModel MealNameSeven { get; set; }
    }

    public class RecipeInfoViewModel
    {
        public int? CustomRecipeId { get; set; }
        public int? RecipeId { get; set; }
        public string RecipeName { get; set; }
    }
}
