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
using System.ComponentModel.DataAnnotations.Schema;
using SknC.Web.Core.Enums;

namespace SknC.Web.Core.Entities
{
    /// <summary>
    /// Represents a physical product instance owned by a user.
    /// Includes expiration tracking, status, and personal usage data.
    /// </summary>
    public class InventoryProduct
    {
        [Key]
        public int Id { get; set; }

        // Relationships
        [Required]
        public string UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public int ProductReferenceId { get; set; }
        public ProductReference? ProductReference { get; set; }

        // Inventory Details
        public ProductStatus Status { get; set; } = ProductStatus.Wishlist;
        
        public DateTime? DateOpened { get; set; }
        public DateTime? DateFinished { get; set; }
        
        /// <summary>
        /// Period After Opening in months (image of the open jar symbol).
        /// </summary>
        public int? PaoMonths { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PurchasePrice { get; set; }

        [Range(1, 5)]
        public int? PersonalRating { get; set; }

        [StringLength(500)]
        public string? PersonalNotes { get; set; }

        // Logic Methods
        public bool IsExpired()
        {
            if (!DateOpened.HasValue || !PaoMonths.HasValue) return false;
            return DateTime.Now > DateOpened.Value.AddMonths(PaoMonths.Value);
        }
    }
}