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
using SknC.Web.Core.Entities;
using SknC.Web.Models.ViewModels;

namespace SknC.Web.Controllers
{
    [Authorize] // Enforce login to access profile
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;

        public ProfileController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // GET: /Profile
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("User not found");

            var model = new UserProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                SkinType = user.SkinType
            };

            return View(model);
        }

        // POST: /Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("User not found");

            // Update allowed fields
            user.FullName = model.FullName;
            user.SkinType = model.SkinType;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Flash message for success
                TempData["SuccessMessage"] = "Profile updated successfully! 🎉";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}