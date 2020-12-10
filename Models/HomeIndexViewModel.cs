using System;
using System.Collections.Generic;

namespace GC_PlanMyMeal.Models
{
    public class HomeIndexViewModel
    {
        public string Diet { get; set; }
        public string Intolerances{ get; set; }
        public int MaxCalorie { get; set; }
        public int MaxCarb { get; set; }
        public int MaxProtein { get; set; }
        public int MinProtein { get; set; }
    }
}
