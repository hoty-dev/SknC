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
        builder.Services.AddRazorPages();

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
        .AddDefaultUI()
        .AddEntityFrameworkStores<AppDbContext>();

        builder.Services.AddScoped<SknC.Web.Services.IRoutineAnalysisService, SknC.Web.Services.RoutineAnalysisService>();

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

        app.UseAuthentication(); // Who are you?
        app.UseAuthorization();  // What can you do?

        app.MapStaticAssets();
        
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();
        
        app.MapRazorPages();

        await app.RunAsync();
    }
}