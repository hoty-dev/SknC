/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 25/11/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SknC.Web.Core.Entities;
using SknC.Web.Infrastructure.Data;
using SknC.Web.Models;
using SknC.Web.Models.ViewModels;

namespace SknC.Web.Controllers
{
    [Authorize] // Protects Dashboard
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager; // Inject UserManager

        public HomeController(ILogger<HomeController> logger, AppDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Get the real user (String)
            var userId = _userManager.GetUserId(User);
            
            // If for some reason it is null (expired session), redirect to login
            if (userId == null) return RedirectToPage("/Account/Login", new { area = "Identity" });

            var today = DateTime.Today;

            // 1. Inventory Stats
            var products = await _context.InventoryProducts
                .Where(i => i.UserId == userId) 
                .Include(i => i.ProductReference)
                .ToListAsync();

            // 2. Routine Stats
            var totalRoutines = await _context.Routines
                .CountAsync(r => r.UserId == userId); 

            var executionsToday = await _context.RoutineExecutions
                .Where(e => e.Routine != null && e.Routine.UserId == userId && e.DateExecuted >= today)
                .CountAsync();

            // 3. CHART DATA LOGIC
            var journalEntries = await _context.JournalEntries
                .Where(j => j.UserId == userId)
                .OrderBy(j => j.Date)
                .Take(14) 
                .ToListAsync();

            var labels = journalEntries.Select(j => j.Date.ToString("dd/MM")).ToArray();
            var values = journalEntries.Select(j => j.OverallRating).ToArray();

            // 4. Build ViewModel
            var model = new DashboardViewModel
            {
                TotalProducts = products.Count,
                ActiveProducts = products.Count(p => p.Status == Core.Enums.ProductStatus.InUse),
                TotalRoutines = totalRoutines,
                CompletedToday = executionsToday,
                ExpiringSoon = products
                    .Where(p => p.Status == Core.Enums.ProductStatus.InUse && p.IsExpired())
                    .Take(5)
                    .ToList(),
                ChartLabels = labels,
                ChartValues = values
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