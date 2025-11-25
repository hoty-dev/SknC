/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 22/11/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/


using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SknC.Web.Core.Enums;

namespace SknC.Web.Models.ViewModels
{
    public class AddInventoryItemViewModel
    {
        [Display(Name = "Select Product")]
        [Required(ErrorMessage = "You must select a product from the catalog")]
        public int ProductReferenceId { get; set; }

        // List for dropwdown in view
        public IEnumerable<SelectListItem>? ProductList { get; set; }

        [Display(Name = "Current Status")]
        public ProductStatus Status { get; set; } = ProductStatus.InUse;

        [Display(Name = "Price Paid")]
        [DataType(DataType.Currency)]
        public decimal? PurchasePrice { get; set; }

        [Display(Name = "Personal Notes")]
        public string? PersonalNotes { get; set; }
    }
}