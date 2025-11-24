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

namespace SknC.Web.Models.ViewModels
{
    public class JournalEntryViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required]
        [Range(1, 10)]
        [Display(Name = "Skin Feeling (1-10)")]
        public int OverallRating { get; set; } = 5;

        [Required]
        [Range(0, 24)]
        [Display(Name = "Hours of Sleep")]
        public int SleepHours { get; set; } = 7;

        [Required]
        [Display(Name = "Stress Level")]
        public StressLevel StressLevel { get; set; }

        [StringLength(1000)]
        [Display(Name = "Daily Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Progress Photo (Optional)")]
        public IFormFile? Photo { get; set; }
    }
}