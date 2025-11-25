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
        builder.Services.AddRazorPages(); // 1. IMPORTANTE: Necesario para las pantallas de Identity

        // Configure DbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Configure Identity
        builder.Services.AddDefaultIdentity<User>(options => 
        {
            options.SignIn.RequireConfirmedAccount = false; 
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddDefaultUI() // 2. IMPORTANTE: Asegura que las rutas de login por defecto funcionen
        .AddEntityFrameworkStores<AppDbContext>();

        var app = builder.Build();

        // --- SEEDER ---
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                await DbInitializer.Initialize(context, userManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        // Configure Pipeline
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication(); // 3. ¿Quién sos?
        app.UseAuthorization();  // 4. ¿Qué podés hacer?

        app.MapStaticAssets();
        
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();
        
        app.MapRazorPages(); // 5. Mapear las rutas de login

        await app.RunAsync();
    }
}