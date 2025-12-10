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
using Microsoft.AspNetCore.Mvc.Rendering;
using SknC.Web.Core.Enums;

namespace SknC.Web.Models.ViewModels
{
    public class CreateProductViewModel
    {
        [Required]
        [Display(Name = "Brand")]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Product Name")]
        public string CommercialName { get; set; } = string.Empty;

        public ProductCategory Category { get; set; }

        public string? Barcode { get; set; }

        // --- MULTI-SELECT INGREDIENTS ---
        [Display(Name = "Select Active Ingredients")]
        public List<int> SelectedIngredientIds { get; set; } = new();

        public IEnumerable<SelectListItem>? AvailableIngredients { get; set; }
    }
}