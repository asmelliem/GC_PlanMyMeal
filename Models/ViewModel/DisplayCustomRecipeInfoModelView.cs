using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Models.ViewModel
{
    public class DisplayCustomRecipeInfoModelView
    {
        public string RecipeName { get; set; }
        public List<string> Ingredients { get; set; }
        public string Directions { get; set; }
        public string Notes { get; set; }
    }
}
