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
    public class PhotoComparisonViewModel
    {
        public JournalEntry BeforeEntry { get; set; } = new();
        public JournalEntry AfterEntry { get; set; } = new();
    }
}