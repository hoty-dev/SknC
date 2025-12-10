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

namespace SknC.Web.Models.ViewModels
{
    public class UserProfileViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty; // Read-only in UI mostly

        [Required]
        [Display(Name = "Skin Type")]
        public SkinType SkinType { get; set; }
    }
}