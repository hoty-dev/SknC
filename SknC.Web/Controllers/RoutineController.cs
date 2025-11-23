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
using Microsoft.AspNetCore.Mvc.Rendering;
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
            int userId = 1; // TODO: Replace with logged-in user

            var routines = await _context.Routines
                .Where(r => r.UserId == userId)
                .Include(r => r.Steps)
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
                int userId = 1;

                var newRoutine = new Routine
                {
                    UserId = userId,
                    Name = model.Name,
                    Type = model.Type
                };

                _context.Routines.Add(newRoutine);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = newRoutine.Id });
            }
            return View(model);
        }
        // GET: /Routine/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            int userId = 1;

            var routine = await _context.Routines
                .Include(r => r.Steps)
                    .ThenInclude(s => s.InventoryProduct)
                        .ThenInclude(ip => ip.ProductReference)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (routine == null) return NotFound();

            var viewModel = new RoutineDetailViewModel
            {
                RoutineId = routine.Id,
                RoutineName = routine.Name,
                Steps = routine.Steps.OrderBy(s => s.OrderIndex).ToList(),
                NewStepOrder = routine.Steps.Count + 1,
                
                InventoryList = _context.InventoryProducts
                    .Where(i => i.UserId == userId && i.Status != Core.Enums.ProductStatus.Discarded)
                    .Include(i => i.ProductReference)
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.ProductReference.Brand} - {p.ProductReference.CommercialName}"
                    })
                    .ToList()
            };

            return View(viewModel);
        }
        // POST: /Routine/AddStep
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStep(RoutineDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newStep = new RoutineStep
                {
                    RoutineId = model.RoutineId,
                    InventoryProductId = model.NewStepInventoryId,
                    OrderIndex = model.NewStepOrder,
                    SpecialInstructions = model.NewStepInstructions
                };

                _context.RoutineSteps.Add(newStep);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = model.RoutineId });
            }

            // Reload data if validation fails
             var routine = await _context.Routines
                .Include(r => r.Steps)
                    .ThenInclude(s => s.InventoryProduct)
                        .ThenInclude(ip => ip.ProductReference)
                .FirstOrDefaultAsync(m => m.Id == model.RoutineId);
            
            model.RoutineName = routine?.Name ?? "Error";
            model.Steps = routine?.Steps.OrderBy(s => s.OrderIndex).ToList() ?? new List<RoutineStep>();
             
             int userId = 1;
             model.InventoryList = _context.InventoryProducts
                .Where(i => i.UserId == userId)
                .Include(i => i.ProductReference)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.ProductReference.Brand} - {p.ProductReference.CommercialName}"
                })
                .ToList();

            return View("Details", model);
        }
    }
}