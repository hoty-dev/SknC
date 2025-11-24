/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 24/11/2025
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
        private readonly IWebHostEnvironment _webHostEnvironment; // Necessary to get the server folder path

        public JournalController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /Journal
        public async Task<IActionResult> Index()
        {
            // TODO: Replace with logged-in user
            int userId = 1;

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
                string? uniqueFileName = null;

                // --- FILE UPLOAD LOGIC ---
                if (model.Photo != null)
                {
                    // 1. Define upload folder path (wwwroot/uploads/journal)
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "journal");
                    
                    // 2. Create directory if it doesn't exist
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    // 3. Generate unique filename to prevent overwriting
                    // Format: GUID + Original Filename (e.g. "a1b2c3d4_myselfie.jpg")
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // 4. Save file to disk
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Photo.CopyToAsync(fileStream);
                    }
                }
                // -------------------------

                var entry = new JournalEntry
                {
                    UserId = userId,
                    Date = model.Date,
                    OverallRating = model.OverallRating,
                    SleepHours = model.SleepHours,
                    StressLevel = model.StressLevel,
                    Notes = model.Notes,
                    // Store the relative URL path in the database, NOT the physical path
                    PhotoPath = uniqueFileName != null ? "/uploads/journal/" + uniqueFileName : null
                };

                _context.JournalEntries.Add(entry);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}