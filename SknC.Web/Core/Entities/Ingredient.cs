/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 10/12/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using System.ComponentModel.DataAnnotations;
using SknC.Web.Core.Enums;

namespace SknC.Web.Core.Entities
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string InciName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? CommonName { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public IngredientFunction Function { get; set; }

        // --- NEW PROPERTY (Ticket #32) ---
        // If set, this ingredient triggers a warning for this specific skin type.
        // Nullable because many ingredients are safe for everyone.
        public SkinType? NotRecommendedFor { get; set; }

        public ICollection<ProductIngredient> ProductIngredients { get; set; } = new List<ProductIngredient>();
    }
}