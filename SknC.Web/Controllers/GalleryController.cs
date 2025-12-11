/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 10/12/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SknC.Web.Core.Entities;
using SknC.Web.Infrastructure.Data;
using SknC.Web.Models.ViewModels;

namespace SknC.Web.Controllers
{
    [Authorize]
    public class GalleryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public GalleryController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Gallery
        // Shows all photos to select from
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            // Fetch only entries that have a photo
            var photos = await _context.JournalEntries
                .Where(j => j.UserId == userId && j.PhotoPath != null)
                .OrderByDescending(j => j.Date)
                .ToListAsync();

            return View(photos);
        }

        // POST: /Gallery/Compare
        // Receives a list of selected IDs from the form
        [HttpPost]
        public async Task<IActionResult> Compare(List<int> selectedIds)
        {
            if (selectedIds == null || selectedIds.Count != 2)
            {
                TempData["Error"] = "Please select exactly two photos to compare.";
                return RedirectToAction(nameof(Index));
            }

            var userId = _userManager.GetUserId(User);
            
            // Fetch the two selected entries
            var entries = await _context.JournalEntries
                .Where(j => selectedIds.Contains(j.Id) && j.UserId == userId)
                .OrderBy(j => j.Date) // Order by date ASC (Oldest first)
                .ToListAsync();

            if (entries.Count != 2)
            {
                return NotFound();
            }

            var viewModel = new PhotoComparisonViewModel
            {
                BeforeEntry = entries[0], // Oldest date
                AfterEntry = entries[1]   // Newest date
            };

            return View(viewModel);
        }
    }
}