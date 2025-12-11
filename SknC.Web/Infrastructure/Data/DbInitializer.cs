/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 10/12/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SknC.Web.Core.Entities;
using SknC.Web.Core.Enums;

namespace SknC.Web.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(AppDbContext context, UserManager<User> userManager)
        {
            context.Database.Migrate();

            // 1. INGREDIENTS
            if (!context.Ingredients.Any())
            {
                var ingredients = new Ingredient[]
                {
                    new Ingredient { InciName = "Retinol", CommonName = "Vitamin A", Function = IngredientFunction.Retinoid, Description = "Anti-aging powerhouse." },
                    // UPDATED: Glycolic Acid flagged for Sensitive Skin (Ticket #32)
                    new Ingredient { InciName = "Glycolic Acid", CommonName = "AHA", Function = IngredientFunction.Exfoliant, Description = "Strong exfoliant.", NotRecommendedFor = SkinType.Sensitive },
                    new Ingredient { InciName = "Ascorbic Acid", CommonName = "Vitamin C", Function = IngredientFunction.Antioxidant, Description = "Brightening agent." },
                    new Ingredient { InciName = "Niacinamide", CommonName = "Vitamin B3", Function = IngredientFunction.Active, Description = "Barrier repair." }
                };
                context.Ingredients.AddRange(ingredients);
                await context.SaveChangesAsync();
            }

            // 2. PRODUCTS
            if (!context.ProductReferences.Any())
            {
                var retinolSerum = new ProductReference { Brand = "The Ordinary", CommercialName = "Retinol 1% in Squalane", Category = ProductCategory.Serum, Barcode = "111" };
                var glycolicToner = new ProductReference { Brand = "The Ordinary", CommercialName = "Glycolic Acid 7% Solution", Category = ProductCategory.Toner, Barcode = "222" };
                var vitCSerum = new ProductReference { Brand = "La Roche-Posay", CommercialName = "Vitamin C10 Serum", Category = ProductCategory.Serum, Barcode = "333" };
                var moisturizer = new ProductReference { Brand = "CeraVe", CommercialName = "Moisturizing Cream", Category = ProductCategory.Moisturizer, Barcode = "444" };

                context.ProductReferences.AddRange(retinolSerum, glycolicToner, vitCSerum, moisturizer);
                await context.SaveChangesAsync();

                // 3. LINK INGREDIENTS TO PRODUCTS
                var retinol = await context.Ingredients.FirstAsync(i => i.InciName == "Retinol");
                var glycolic = await context.Ingredients.FirstAsync(i => i.InciName == "Glycolic Acid");
                
                context.ProductIngredients.Add(new ProductIngredient { ProductReferenceId = retinolSerum.Id, IngredientId = retinol.Id, Concentration = "1%" });
                context.ProductIngredients.Add(new ProductIngredient { ProductReferenceId = glycolicToner.Id, IngredientId = glycolic.Id, Concentration = "7%" });
                
                await context.SaveChangesAsync();
            }

            // 4. TEST USER (Configured with Sensitive Skin for testing)
            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    UserName = "test@sknc.app",
                    Email = "test@sknc.app",
                    FullName = "Test User",
                    SkinType = SkinType.Sensitive, // Set to Sensitive to trigger the warning
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}