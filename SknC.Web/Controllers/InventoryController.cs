/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 25/11/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SknC.Web.Core.Entities;
using SknC.Web.Infrastructure.Data;
using SknC.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization; // Required to protect the controller
using Microsoft.AspNetCore.Identity; // Required to get current user

namespace SknC.Web.Controllers
{
    [Authorize] // Critical: Only logged-in users can access Inventory
    public class InventoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager; // We inject the User Manager

        public InventoryController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Inventory
        public async Task<IActionResult> Index()
        {
            // Get the current logged-in User ID (It's a string/GUID now)
            var userId = _userManager.GetUserId(User);

            if (userId == null) return NotFound("User not found");

            // Fetch user's inventory including the product details (Reference)
            var inventory = await _context.InventoryProducts
                .Include(i => i.ProductReference)
                .Where(i => i.UserId == userId) // EF Core handles string comparison automatically
                .OrderByDescending(i => i.DateOpened)
                .ToListAsync();

            return View(inventory);
        }

        // GET: /Inventory/Create
        public IActionResult Create()
        {
            var viewModel = new AddInventoryItemViewModel
            {
                // Load the dropdown list from the global catalog
                ProductList = _context.ProductReferences
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.Brand} - {p.CommercialName}"
                    })
                    .ToList()
            };

            return View(viewModel);
        }

        // POST: /Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddInventoryItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Get current User ID
                var userId = _userManager.GetUserId(User);
                
                if (userId == null) return Challenge(); // Force login if session is lost

                // Map ViewModel to Entity
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

            // If validation fails, reload the dropdown
            model.ProductList = _context.ProductReferences
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.Brand} - {p.CommercialName}"
                })
                .ToList();

            return View(model);
        }
    }
}