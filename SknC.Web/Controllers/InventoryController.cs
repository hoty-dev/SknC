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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SknC.Web.Core.Entities;
using SknC.Web.Infrastructure.Data;
using SknC.Web.Models.ViewModels;

namespace SknC.Web.Controllers
{
    [Authorize] // Protege el acceso al inventario
    public class InventoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public InventoryController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        public IActionResult Create()
        {
            var viewModel = new AddInventoryItemViewModel
            {
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
    }
}