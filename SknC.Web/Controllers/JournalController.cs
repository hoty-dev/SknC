/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 23/11/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SknC.Web.Core.Entities;
using SknC.Web.Infrastructure.Data;
using SknC.Web.Models.ViewModels;

namespace SknC.Web.Controllers
{
    public class JournalController : Controller
    {
        private readonly AppDbContext _context;

        public JournalController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Journal
        public async Task<IActionResult> Index()
        {
            int userId = 1; // Hardcoded user

            var entries = await _context.JournalEntries
                .Where(j => j.UserId == userId)
                .OrderByDescending(j => j.Date)
                .ToListAsync();

            return View(entries);
        }

        // GET: /Journal/Create
        public IActionResult Create()
        {
            var model = new JournalEntryViewModel
            {
                Date = DateTime.Today
            };
            return View(model);
        }

        // POST: /Journal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JournalEntryViewModel model)
        {
            if (ModelState.IsValid)
            {
                int userId = 1;

                // Check if entry already exists for this date? (Optional logic)
                
                var entry = new JournalEntry
                {
                    UserId = userId,
                    Date = model.Date,
                    OverallRating = model.OverallRating,
                    SleepHours = model.SleepHours,
                    StressLevel = model.StressLevel,
                    Notes = model.Notes
                };

                _context.JournalEntries.Add(entry);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}