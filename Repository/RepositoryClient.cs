﻿using GC_PlanMyMeal.Areas.Identity.Data;
using GC_PlanMyMeal.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Repository
{
    public class RepositoryClient: IRepositoryClient
    {
        private readonly GC_PlanMyMealIdentityDbContext _context;

        public RepositoryClient(GC_PlanMyMealIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveRecipe(int? recipeId, string userId)
        {
            var savedRecipe = new SavedRecipe()
            {
                UserId = userId,
                RecipeId = recipeId
            };
            
            _context.SavedRecipes.Add(savedRecipe);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
