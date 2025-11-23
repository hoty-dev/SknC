/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 20/11/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using SknC.Web.Core.Entities;
using SknC.Web.Core.Enums;

namespace SknC.Web.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // 1. Ensure the database is created
            context.Database.EnsureCreated();

            // 2. Check if we already have data
            if (context.ProductReferences.Any())
            {
                return; // DB has been seeded
            }

            // 3. Create seed data (Product References)
            var products = new ProductReference[]
            {
                new ProductReference
                {
                    Brand = "CeraVe",
                    CommercialName = "Foaming Facial Cleanser",
                    Category = ProductCategory.Cleanser,
                    Barcode = "3606000537484"
                },
                new ProductReference
                {
                    Brand = "La Roche-Posay",
                    CommercialName = "Anthelios Melt-in Milk Sunscreen SPF 60",
                    Category = ProductCategory.Sunscreen,
                    Barcode = "3337875583626"
                },
                new ProductReference
                {
                    Brand = "The Ordinary",
                    CommercialName = "Niacinamide 10% + Zinc 1%",
                    Category = ProductCategory.Serum,
                    Barcode = "769915190311"
                },
                new ProductReference
                {
                    Brand = "Neutrogena",
                    CommercialName = "Hydro Boost Water Gel",
                    Category = ProductCategory.Moisturizer,
                    Barcode = "070501110478"
                }
            };

            // 4. Add to context and save
            context.ProductReferences.AddRange(products);

            if (!context.Users.Any())
            {
                var user = new User
                {
                    FullName = "Test User",
                    Email = "test@sknc.app",
                    SkinType = SkinType.Combination
                };
                context.Users.Add(user);
            }

            context.SaveChanges();
        }
    }
}