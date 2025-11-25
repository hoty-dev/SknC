/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 25/11/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using SknC.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SknC.Web.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace SknC.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // 1. Configure DbContext with SQLite
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        // 2. CONFIGURE IDENTITY (Security)
        // Link Identity with our User entity and DbContext
        builder.Services.AddDefaultIdentity<User>(options => 
        {
            // Relaxed password requirements for development
            options.SignIn.RequireConfirmedAccount = false; 
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddEntityFrameworkStores<AppDbContext>();

        var app = builder.Build();

        // --- SEEDER START (Updated for Identity) ---
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                // We request the UserManager to securely create the test user
                var userManager = services.GetRequiredService<UserManager<User>>();
                
                // Async call to initialize DB
                await DbInitializer.Initialize(context, userManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
        // --- SEEDER END ---

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        // 3. SECURITY MIDDLEWARES (Order matters)
        app.UseAuthentication(); // Identity: Who are you?
        app.UseAuthorization();  // Identity: What can you do?

        app.MapStaticAssets();
        
        // Default MVC Route
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();
        
        // Required for Identity UI (Login/Register pages are Razor Pages)
        app.MapRazorPages(); 

        await app.RunAsync();
    }
}