using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Models
{
    public class UserPreferencesViewModel
    {
        public string UserID { get; set; }
        public string Diet { get; set; }
        public Intolerances Intolerances { get; set; }
        public int? MaxCalorie { get; set; }
        public int? MaxCarb { get; set; }
        public int? MaxProtein { get; set; }
        public int? MinProtein { get; set; }
    }
}
