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
    public class InventoryController : Controller
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Inventory
        public async Task<IActionResult> Index()
        {
            // TODO: Replace with current logged-in user ID (Authentication logic pending)
            int userId = 1;

            // Retrieve user's inventory including product details
            var inventory = await _context.InventoryProducts
                .Include(i => i.ProductReference)
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.DateOpened)
                .ToListAsync();

            return View(inventory);
        }

        // GET: /Inventory/Create
        public IActionResult Create()
        {
            var viewModel = new AddInventoryItemViewModel
            {
                // Populate the dropdown list with products from the global catalog
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
                // TODO: Replace with current logged-in user ID
                int userId = 1;

                // Map ViewModel to Entity
                var newItem = new InventoryProduct
                {
                    UserId = userId,
                    ProductReferenceId = model.ProductReferenceId,
                    Status = model.Status,
                    PurchasePrice = model.PurchasePrice,
                    PersonalNotes = model.PersonalNotes,
                    // If status is "In Use", automatically set the open date to today
                    DateOpened = model.Status == Core.Enums.ProductStatus.InUse ? DateTime.Now : null
                };

                _context.InventoryProducts.Add(newItem);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If validation fails, reload the dropdown list so the form is not empty
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