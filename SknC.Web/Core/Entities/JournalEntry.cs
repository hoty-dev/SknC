/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 24/11/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using System.ComponentModel.DataAnnotations;
using SknC.Web.Core.Enums;

namespace SknC.Web.Core.Entities
{
    public class JournalEntry
    {
        [Key]
        public int Id { get; set; }

        // Relationship: Who wrote this entry?
        [Required]
        public string UserId { get; set; }
        public User? User { get; set; }

        // When?
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        // How is the skin? (1-10)
        [Range(1, 10)]
        public int OverallRating { get; set; }

        // External Factors
        [Range(0, 24)]
        public int SleepHours { get; set; }

        public StressLevel StressLevel { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        // Optional photo attachment
        [StringLength(255)]
        public string? PhotoPath { get; set; } // Stores relative path: "/uploads/journal/xyz.jpg"
    }
}