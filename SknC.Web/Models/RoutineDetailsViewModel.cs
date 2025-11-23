/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 23/11/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SknC.Web.Core.Entities;

namespace SknC.Web.Models.ViewModels
{
    public class RoutineDetailViewModel
    {
        // --- Display Data (Read Only) ---
        public int RoutineId { get; set; }
        public string RoutineName { get; set; } = string.Empty;
        public List<RoutineStep> Steps { get; set; } = new();

        // --- Add New Step Form Data ---
        [Display(Name = "Select Product")]
        [Required]
        public int NewStepInventoryId { get; set; }

        [Display(Name = "Step Order")]
        [Required]
        [Range(1, 20)]
        public int NewStepOrder { get; set; } = 1;

        [Display(Name = "Instructions (Optional)")]
        public string? NewStepInstructions { get; set; }

        // Dropdown list for the View
        public IEnumerable<SelectListItem>? InventoryList { get; set; }
    }
}