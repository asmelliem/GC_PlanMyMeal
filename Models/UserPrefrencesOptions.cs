using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Models
{
    public class UserPrefrencesOptions
    {
        public IReadOnlyList<string> Diet { get; set; }
        public IReadOnlyList<string> Intolerances { get; set; }
    }
}
