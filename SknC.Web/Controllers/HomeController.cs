/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 23/11/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SknC.Web.Infrastructure.Data;
using SknC.Web.Models;
using SknC.Web.Models.ViewModels;

namespace SknC.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        // Inject Database Context
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int userId = 1; // Hardcoded user for MVP
            var today = DateTime.Today;

            // 1. Fetch Inventory Stats
            var products = await _context.InventoryProducts
                .Where(i => i.UserId == userId)
                .Include(i => i.ProductReference)
                .ToListAsync();

            // 2. Fetch Routine Stats
            var totalRoutines = await _context.Routines
                .CountAsync(r => r.UserId == userId);

            var executionsToday = await _context.RoutineExecutions
                .Where(e => e.Routine != null && e.Routine.UserId == userId && e.DateExecuted >= today)
                .CountAsync();

            // 3. Build ViewModel
            var model = new DashboardViewModel
            {
                TotalProducts = products.Count,
                ActiveProducts = products.Count(p => p.Status == Core.Enums.ProductStatus.InUse),
                TotalRoutines = totalRoutines,
                CompletedToday = executionsToday,
                // Simple logic: expiring in next 30 days based on PAO
                ExpiringSoon = products
                    .Where(p => p.Status == Core.Enums.ProductStatus.InUse && p.IsExpired())
                    .Take(5)
                    .ToList()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}