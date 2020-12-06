using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GC_PlanMyMeal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GC_PlanMyMeal.Areas.Identity.Data
{
    public class GC_PlanMyMealIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public GC_PlanMyMealIdentityDbContext(DbContextOptions<GC_PlanMyMealIdentityDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<CustomRecipe> CustomRecipes { get; set; }
        public virtual DbSet<RecipeCalendar> RecipeCalendars { get; set; }
        public virtual DbSet<SavedRecipe> SavedRecipes { get; set; }
        public virtual DbSet<UserPreference> UserPreferences { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CustomRecipe>(entity =>
            {
                entity.Property(e => e.Directions)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Ingredients)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Notes)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.RecipeName)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            builder.Entity<RecipeCalendar>(entity =>
            {
                entity.ToTable("RecipeCalendar");

                entity.Property(e => e.CookDate).HasColumnType("datetime");

                entity.Property(e => e.MealTime)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.CustomRecipe)
                    .WithMany(p => p.RecipeCalendars)
                    .HasForeignKey(d => d.CustomRecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RecipeCal__Custo__5EBF139D");
            });

            builder.Entity<SavedRecipe>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.CustomeRecipe)
                    .WithMany(p => p.SavedRecipes)
                    .HasForeignKey(d => d.CustomeRecipeId)
                    .HasConstraintName("FK__SavedReci__Custo__6FE99F9F");
            });

            builder.Entity<UserPreference>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Diet)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Intolerances)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
