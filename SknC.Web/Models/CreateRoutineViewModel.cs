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
using SknC.Web.Core.Enums;

namespace SknC.Web.Models.ViewModels
{
    public class CreateRoutineViewModel
    {
        [Required(ErrorMessage = "Please give your routine a name")]
        [Display(Name = "Routine Name")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Routine Type")]
        public RoutineType Type { get; set; }
    }
}