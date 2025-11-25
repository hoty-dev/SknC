/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 25/11/2025
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
            // Apply pending migrations automatically
            context.Database.Migrate();

            // 1. Seed Products (if not exists)
            if (!context.ProductReferences.Any())
            {
                var products = new ProductReference[]
                {
                    new ProductReference { Brand = "CeraVe", CommercialName = "Foaming Facial Cleanser", Category = ProductCategory.Cleanser, Barcode = "3606000537484" },
                    new ProductReference { Brand = "La Roche-Posay", CommercialName = "Anthelios Melt-in Milk Sunscreen SPF 60", Category = ProductCategory.Sunscreen, Barcode = "3337875583626" },
                    new ProductReference { Brand = "The Ordinary", CommercialName = "Niacinamide 10% + Zinc 1%", Category = ProductCategory.Serum, Barcode = "769915190311" },
                    new ProductReference { Brand = "Neutrogena", CommercialName = "Hydro Boost Water Gel", Category = ProductCategory.Moisturizer, Barcode = "070501110478" }
                };
                context.ProductReferences.AddRange(products);
                await context.SaveChangesAsync();
            }

            // 2. Seed Default User (using Identity)
            // Check if there are any users using the UserManager from Identity
            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    UserName = "test@sknc.app", // Identity requires explicit UserName
                    Email = "test@sknc.app",
                    FullName = "Test User",
                    SkinType = SkinType.Combination,
                    EmailConfirmed = true
                };

                // Create the user with a secure default password
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}