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
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Profile Data
        public SkinType SkinType { get; set; }
        
        // Navigation Properties (Relationships)
        public ICollection<InventoryProduct> Inventory { get; set; } = new List<InventoryProduct>();
        public ICollection<Routine> Routines { get; set; } = new List<Routine>();
    }
}