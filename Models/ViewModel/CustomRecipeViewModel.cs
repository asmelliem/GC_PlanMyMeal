using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Models.ViewModel
{
    public class CustomRecipeViewModel
    {
        public int? Id { get; set; }
        public string UserId { get; set; }
        public string RecipeName { get; set; }
        public string Ingredients { get; set; }
        public string Directions { get; set; }
        public string Notes { get; set; }
    }
}
