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

namespace SknC.Web.Core.Entities
{
    public class RoutineExecution
    {
        [Key]
        public int Id { get; set; }

        // Relationship: Which routine was executed?
        [Required]
        public int RoutineId { get; set; }
        public Routine? Routine { get; set; }

        // When did it happen?
        [Required]
        public DateTime DateExecuted { get; set; } = DateTime.Now;

        // Was it fully completed? (For future use, default true for now)
        public bool IsCompleted { get; set; } = true;

        // Optional notes for this specific day (e.g. "Skipped sunscreen because it was raining")
        [StringLength(500)]
        public string? DailyNotes { get; set; }
    }
}