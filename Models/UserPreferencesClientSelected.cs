using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Models
{
    public class UserPreferencesClientSelected
    {
        public string UserId { get; set; }
        public string Diet { get; set; }
        public List<string> Intolerances { get; set; }
        public int? MaxCalorie { get; set; }
        public int? MaxCarb { get; set; }
        public int? MaxProtein { get; set; }
        public int? MinProtein { get; set; }
    }
}
