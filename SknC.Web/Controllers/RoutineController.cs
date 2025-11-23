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
    public class RoutineController : Controller
    {
        private readonly AppDbContext _context;

        public RoutineController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Routine
        public async Task<IActionResult> Index()
        {
            // TODO: Replace with logged-in user ID
            int userId = 1;

            var routines = await _context.Routines
                .Where(r => r.UserId == userId)
                .OrderBy(r => r.Type)
                .ToListAsync();

            return View(routines);
        }

        // GET: /Routine/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Routine/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoutineViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Replace with logged-in user ID
                int userId = 1;

                var newRoutine = new Routine
                {
                    UserId = userId,
                    Name = model.Name,
                    Type = model.Type
                };

                _context.Routines.Add(newRoutine);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}