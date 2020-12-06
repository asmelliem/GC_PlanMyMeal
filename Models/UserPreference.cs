using System;
using System.Collections.Generic;

#nullable disable

namespace GC_PlanMyMeal.Models
{
    public partial class UserPreference
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Diet { get; set; }
        public string Intolerances { get; set; }
        public int? MaxCalorie { get; set; }
        public int? MaxCarb { get; set; }
        public int? MinProtein { get; set; }
        public int? MaxProtein { get; set; }
    }
}
