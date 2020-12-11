using System;
using System.Text;

namespace GC_PlanMyMeal.Models
{
    public class Intolerances
    {
        public bool egg { get; set; }
        public bool dairy { get; set; }
        public bool gluton { get; set; }
        public bool grain { get; set; }
        public bool peanut { get; set; }
        public bool sesame { get; set; }
        public bool seafood { get; set; }
        public bool shellfish { get; set; }
        public bool soy { get; set; }
        public bool sulfite { get; set; }
        public bool treeNut { get; set; }
        public bool wheat { get; set; }


        public override string ToString()
        {
            StringBuilder intolerancesString = new StringBuilder();

            if(egg == true) { intolerancesString.Append($"egg,"); }
            if(dairy == true) { intolerancesString.Append($"dairy,"); }
            if(gluton == true) { intolerancesString.Append("gluton"); }
            if(grain == true) { intolerancesString.Append($"grain,"); }
            if(peanut == true) { intolerancesString.Append($"peanut,"); }
            if(sesame == true) { intolerancesString.Append($"sesame,"); }
            if (seafood == true) { intolerancesString.Append($"seafood,"); }
            if (shellfish == true) { intolerancesString.Append($"shellfish,"); }
            if(soy == true) { intolerancesString.Append($"soy,"); }
            if(sulfite == true) { intolerancesString.Append($"sulfite,"); }
            if(treeNut == true) { intolerancesString.Append($"treeNut,"); }
            if(wheat == true) { intolerancesString.Append($"wheat,"); }

            return intolerancesString.ToString();
        }
    }
}
