/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 25/11/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using SknC.Web.Core.Enums;

namespace SknC.Web.Core.Entities
{
    public class User : IdentityUser 
    {        
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        // Profile Data
        public SkinType SkinType { get; set; }
        
        // Navigation Properties
        public ICollection<InventoryProduct> Inventory { get; set; } = new List<InventoryProduct>();
        public ICollection<Routine> Routines { get; set; } = new List<Routine>();
    }
}