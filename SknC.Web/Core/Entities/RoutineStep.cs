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

namespace SknC.Web.Core.Entities
{
    public class RoutineStep
    {
        [Key]
        public int Id { get; set; }

        public int RoutineId { get; set; }
        public Routine? Routine { get; set; }

        public int OrderIndex { get; set; } // 1, 2, 3...

        public int InventoryProductId { get; set; }
        public InventoryProduct? InventoryProduct { get; set; }

        [StringLength(200)]
        public string? SpecialInstructions { get; set; } // e.g., "Apply on damp skin"
    }
}