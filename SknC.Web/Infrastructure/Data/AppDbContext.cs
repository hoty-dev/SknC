/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 25/11/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SknC.Web.Core.Entities;

namespace SknC.Web.Infrastructure.Data
{
    // CRITICAL: Inherit from IdentityDbContext<User> (our custom class), NOT generic IdentityUser
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ProductReference> ProductReferences { get; set; }
        public DbSet<InventoryProduct> InventoryProducts { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<RoutineStep> RoutineSteps { get; set; }
        public DbSet<RoutineExecution> RoutineExecutions { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        
        // --- NEW TABLES (Ingredients Ticket #14) ---
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<ProductIngredient> ProductIngredients { get; set; }
        // -------------------------------------------

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // CRITICAL: Must call base.OnModelCreating to configure Identity tables correctly
            base.OnModelCreating(modelBuilder);

            // Configure SQLite decimal support
            modelBuilder.Entity<InventoryProduct>()
                .Property(p => p.PurchasePrice)
                .HasColumnType("TEXT");

            // Ensure Email is unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // --- MANY-TO-MANY CONFIGURATION ---
            // Define composite primary key for the join table
            modelBuilder.Entity<ProductIngredient>()
                .HasKey(pi => new { pi.ProductReferenceId, pi.IngredientId });

            // Relationship 1: Product has many Ingredients
            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.ProductReference)
                .WithMany(p => p.ProductIngredients)
                .HasForeignKey(pi => pi.ProductReferenceId);

            // Relationship 2: Ingredient is in many Products
            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Ingredient)
                .WithMany(i => i.ProductIngredients)
                .HasForeignKey(pi => pi.IngredientId);
        }
    }
}