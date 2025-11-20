/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 20/11/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using System.ComponentModel.DataAnnotations;
using SknC.Web.Core.Enums;

namespace SknC.Web.Core.Entities
{
    /// <summary>
    /// Represents the immutable reference data of a skincare product (Global Catalog).
    /// Does not contain specific user data like expiration date or purchase price.
    /// </summary>
    public class ProductReference
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string CommercialName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Barcode { get; set; }

        public ProductCategory Category { get; set; }

        // We will link Ingredients later
        // public List<Ingredient> Ingredients { get; set; } = new();
    }
}
