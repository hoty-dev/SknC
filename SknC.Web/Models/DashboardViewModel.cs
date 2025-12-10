/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 10/12/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using SknC.Web.Core.Entities;

namespace SknC.Web.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }
        public string UserSkinType { get; set; } = "Not Set";
        
        // Routine Stats
        public int TotalRoutines { get; set; }
        public int CompletedToday { get; set; }
        
        // Alerts
        public List<InventoryProduct> ExpiringSoon { get; set; } = new();

        // Arrays for Chart.js (Dates as strings, Ratings as integers)
        public string[] ChartLabels { get; set; } = Array.Empty<string>();
        public int[] ChartValues { get; set; } = Array.Empty<int>();
    }
}