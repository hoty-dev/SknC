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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SknC.Web.Core.Entities;
using SknC.Web.Infrastructure.Data;
using SknC.Web.Models.ViewModels;

namespace SknC.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Product (Catalog)
        public async Task<IActionResult> Index()
        {
            var products = await _context.ProductReferences
                .Include(p => p.ProductIngredients)
                .ThenInclude(pi => pi.Ingredient)
                .OrderBy(p => p.Brand)
                .ToListAsync();
            return View(products);
        }

        // GET: /Product/Create
        public IActionResult Create()
        {
            var model = new CreateProductViewModel
            {
                AvailableIngredients = _context.Ingredients
                    .OrderBy(i => i.InciName)
                    .Select(i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = $"{i.InciName} ({i.CommonName ?? i.Function.ToString()})"
                    })
                    .ToList()
            };
            return View(model);
        }

        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Create the Product Reference
                var product = new ProductReference
                {
                    Brand = model.Brand,
                    CommercialName = model.CommercialName,
                    Category = model.Category,
                    Barcode = model.Barcode
                };

                _context.ProductReferences.Add(product);
                await _context.SaveChangesAsync(); // Save to get the ID

                // 2. Create the Links (ProductIngredients)
                if (model.SelectedIngredientIds.Any())
                {
                    foreach (var ingredientId in model.SelectedIngredientIds)
                    {
                        _context.ProductIngredients.Add(new ProductIngredient
                        {
                            ProductReferenceId = product.Id,
                            IngredientId = ingredientId
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            // Reload list if failed
            model.AvailableIngredients = _context.Ingredients
                .Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.InciName })
                .ToList();
            return View(model);
        }
    }
}