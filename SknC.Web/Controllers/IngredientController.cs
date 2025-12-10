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
using Microsoft.EntityFrameworkCore;
using SknC.Web.Core.Entities;
using SknC.Web.Infrastructure.Data;

namespace SknC.Web.Controllers
{
    [Authorize]
    public class IngredientController : Controller
    {
        private readonly AppDbContext _context;

        public IngredientController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Ingredient
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ingredients.OrderBy(i => i.InciName).ToListAsync());
        }

        // GET: /Ingredient/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Ingredient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                _context.Ingredients.Add(ingredient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }
    }
}