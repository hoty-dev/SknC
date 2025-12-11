/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 11/12/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using SknC.Web.Core.Entities;
using SknC.Web.Core.Enums;

namespace SknC.Web.Services
{
    public interface IRoutineAnalysisService
    {
        List<string> AnalyzeRoutine(Routine routine, SkinType userSkinType);
        // NEW: Analyze a single product for skin type compatibility
        List<string> AnalyzeProduct(ProductReference product, SkinType userSkinType);
    }

    public class RoutineAnalysisService : IRoutineAnalysisService
    {
        private readonly List<(IngredientFunction A, IngredientFunction B, string Message)> _conflictRules = new()
        {
            (IngredientFunction.Retinoid, IngredientFunction.Exfoliant, "⚠️ Caution: Mixing Retinol and Exfoliants (AHA/BHA) can cause severe irritation."),
            (IngredientFunction.Retinoid, IngredientFunction.Retinoid, "⚠️ Warning: Multiple Retinoids detected. Use only one to avoid burns."),
            (IngredientFunction.Exfoliant, IngredientFunction.Exfoliant, "⚠️ Caution: Multiple Exfoliants detected. Risk of over-exfoliation."),
            (IngredientFunction.Retinoid, IngredientFunction.Antioxidant, "ℹ️ Tip: Some users prefer using Vitamin C in AM and Retinol in PM to maximize stability.")
        };

        public List<string> AnalyzeRoutine(Routine routine, SkinType userSkinType)
        {
            var warnings = new List<string>();
            
            var allIngredients = routine.Steps
                .Where(s => s.InventoryProduct?.ProductReference != null)
                .SelectMany(s => s.InventoryProduct!.ProductReference!.ProductIngredients)
                .Select(pi => pi.Ingredient)
                .Where(i => i != null)
                .ToList();

            if (!allIngredients.Any()) return warnings;

            // 1. Chemical Conflicts
            foreach (var rule in _conflictRules)
            {
                bool hasA = allIngredients.Any(i => i!.Function == rule.A);
                bool hasB = allIngredients.Any(i => i!.Function == rule.B);

                if (rule.A == rule.B)
                {
                    if (allIngredients.Count(i => i!.Function == rule.A) > 1) warnings.Add(rule.Message);
                }
                else if (hasA && hasB)
                {
                    warnings.Add(rule.Message);
                }
            }

            // 2. Skin Type Compatibility
            foreach (var ingredient in allIngredients)
            {
                if (ingredient!.NotRecommendedFor.HasValue && ingredient.NotRecommendedFor.Value == userSkinType)
                {
                    warnings.Add($"🛑 Skin Type Alert: '{ingredient.InciName}' is not recommended for {userSkinType} skin.");
                }
            }

            return warnings.Distinct().ToList();
        }

        // NEW METHOD: Single Product Analysis
        public List<string> AnalyzeProduct(ProductReference product, SkinType userSkinType)
        {
            var warnings = new List<string>();

            if (product.ProductIngredients == null || !product.ProductIngredients.Any()) 
                return warnings;

            foreach (var pi in product.ProductIngredients)
            {
                var ingredient = pi.Ingredient;
                if (ingredient != null && ingredient.NotRecommendedFor.HasValue && ingredient.NotRecommendedFor.Value == userSkinType)
                {
                    warnings.Add($"🛑 Skin Type Alert: '{ingredient.InciName}' is not recommended for {userSkinType} skin.");
                }
            }

            return warnings;
        }
    }
}