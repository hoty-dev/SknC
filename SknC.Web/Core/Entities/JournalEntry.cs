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

namespace SknC.Web.Core.Entities
{
    public class JournalEntry
    {
        [Key]
        public int Id { get; set; }

        // Relationship: Who wrote this entry?
        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        // When? (Date only, time doesn't matter much for daily summary)
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

        // Note: Photo path will be added in Ticket #11
    }
}