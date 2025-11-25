/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 25/11/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using Microsoft.AspNetCore.Identity; // Importante
using SknC.Web.Core.Enums;

namespace SknC.Web.Core.Entities
{
    // Inherits from IdentityUser to include ASP.NET Identity functionality
    public class User : IdentityUser
    {
        // Personalized properties that Identity does not include by default
        public string FullName { get; set; } = string.Empty;

        public SkinType SkinType { get; set; }
        
        // Navigation Properties
        public ICollection<InventoryProduct> Inventory { get; set; } = new List<InventoryProduct>();
        public ICollection<Routine> Routines { get; set; } = new List<Routine>();
    }
}