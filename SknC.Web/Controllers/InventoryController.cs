/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 11/12/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SknC.Web.Core.Entities;
using SknC.Web.Infrastructure.Data;
using SknC.Web.Models.ViewModels;
using SknC.Web.Services; // Namespace for the analysis service

namespace SknC.Web.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IRoutineAnalysisService _analysisService; // Inject Analysis Service

        public InventoryController(
            AppDbContext context, 
            UserManager<User> userManager,
            IRoutineAnalysisService analysisService)
        {
            _context = context;
            _userManager = userManager;
            _analysisService = analysisService;
        }

        // GET: /Inventory
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            
            if (userId == null) return Challenge();

            var inventory = await _context.InventoryProducts
                .Include(i => i.ProductReference)
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.DateOpened)
                .ToListAsync();

            return View(inventory);
        }

        // GET: /Inventory/Create
        // UPDATE: Async to support fetching user and product data for analysis
        public async Task<IActionResult> Create(int? productReferenceId)
        {
            var viewModel = new AddInventoryItemViewModel
            {
                ProductReferenceId = productReferenceId ?? 0,
                ProductList = _context.ProductReferences
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.Brand} - {p.CommercialName}"
                    })
                    .ToList()
            };

            // NEW LOGIC: Pre-emptive Skin Type Warning (Ticket #32)
            // If a product is selected from catalog, check if it's safe for the user
            if (productReferenceId.HasValue && productReferenceId.Value > 0)
            {
                var user = await _userManager.GetUserAsync(User);
                var userSkinType = user?.SkinType ?? Core.Enums.SkinType.Normal;

                var product = await _context.ProductReferences
                    .Include(p => p.ProductIngredients)
                    .ThenInclude(pi => pi.Ingredient)
                    .FirstOrDefaultAsync(p => p.Id == productReferenceId.Value);

                if (product != null)
                {
                    // Run the specific product analysis
                    var warnings = _analysisService.AnalyzeProduct(product, userSkinType);
                    
                    if (warnings.Any())
                    {
                        // Pass warnings to the view to show the red alert
                        ViewBag.SkinWarnings = warnings;
                    }
                }
            }

            return View(viewModel);
        }

        // POST: /Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddInventoryItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null) return Challenge();

                var newItem = new InventoryProduct
                {
                    UserId = userId,
                    ProductReferenceId = model.ProductReferenceId,
                    Status = model.Status,
                    PurchasePrice = model.PurchasePrice,
                    PersonalNotes = model.PersonalNotes,
                    DateOpened = model.Status == Core.Enums.ProductStatus.InUse ? DateTime.Now : null
                };

                _context.InventoryProducts.Add(newItem);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            model.ProductList = _context.ProductReferences
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.Brand} - {p.CommercialName}"
                })
                .ToList();

            return View(model);
        }

        // GET: /Inventory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var inventoryItem = await _context.InventoryProducts
                .Include(i => i.ProductReference)
                    .ThenInclude(pr => pr.ProductIngredients)
                        .ThenInclude(pi => pi.Ingredient)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (inventoryItem == null) return NotFound();

            return View(inventoryItem);
        }

        // GET: /Inventory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var inventoryItem = await _context.InventoryProducts
                .Include(i => i.ProductReference)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (inventoryItem == null) return NotFound();

            return View(inventoryItem);
        }

        // POST: /Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var inventoryItem = await _context.InventoryProducts
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (inventoryItem != null)
            {
                _context.InventoryProducts.Remove(inventoryItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}