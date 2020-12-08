using GC_PlanMyMeal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Repository
{
    public interface IRepositoryClient
    {
        Task<bool> SaveRecipe(int? recipeId, string userId);
    }
}
