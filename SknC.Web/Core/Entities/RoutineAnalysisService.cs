/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 10/12/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using SknC.Web.Core.Entities;
using SknC.Web.Core.Enums;

namespace SknC.Web.Services
{
    public interface IRoutineAnalysisService
    {
        List<string> AnalyzeRoutine(Routine routine);
    }

    public class RoutineAnalysisService : IRoutineAnalysisService
    {
        // Reglas "Hardcoded" para el MVP. 
        // En el futuro, esto podría venir de base de datos.
        private readonly List<(IngredientFunction A, IngredientFunction B, string Message)> _conflictRules = new()
        {
            (IngredientFunction.Retinoid, IngredientFunction.Exfoliant, "⚠️ Caution: Mixing Retinol and Exfoliants (AHA/BHA) can cause severe irritation."),
            (IngredientFunction.Retinoid, IngredientFunction.Retinoid, "⚠️ Warning: Multiple Retinoids detected. Use only one to avoid burns."),
            (IngredientFunction.Exfoliant, IngredientFunction.Exfoliant, "⚠️ Caution: Multiple Exfoliants detected. Risk of over-exfoliation."),
            (IngredientFunction.Retinoid, IngredientFunction.Antioxidant, "ℹ️ Tip: Some users prefer using Vitamin C in AM and Retinol in PM to maximize stability.")
        };

        public List<string> AnalyzeRoutine(Routine routine)
        {
            var warnings = new List<string>();
            
            // 1. Aplanar todos los ingredientes de la rutina en una lista
            // Routine -> Steps -> InventoryProduct -> ProductReference -> Ingredients
            var allIngredients = routine.Steps
                .Where(s => s.InventoryProduct?.ProductReference != null)
                .SelectMany(s => s.InventoryProduct!.ProductReference!.ProductIngredients)
                .Select(pi => pi.Ingredient)
                .Where(i => i != null)
                .ToList();

            if (!allIngredients.Any()) return warnings;

            // 2. Chequear cada regla contra los ingredientes presentes
            foreach (var rule in _conflictRules)
            {
                bool hasA = allIngredients.Any(i => i!.Function == rule.A);
                bool hasB = allIngredients.Any(i => i!.Function == rule.B);

                // Caso especial: Si la regla es A vs A (ej: doble retinol), necesitamos contar que haya > 1
                if (rule.A == rule.B)
                {
                    if (allIngredients.Count(i => i!.Function == rule.A) > 1)
                    {
                        warnings.Add(rule.Message);
                    }
                }
                else if (hasA && hasB)
                {
                    warnings.Add(rule.Message);
                }
            }

            return warnings.Distinct().ToList();
        }
    }
}