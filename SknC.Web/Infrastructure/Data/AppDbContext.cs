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
    // Inherit from IdentityDbContext to support Authentication
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // 'Users' DbSet is inherited from IdentityDbContext, so we don't need to declare it manually.
        
        public DbSet<ProductReference> ProductReferences { get; set; }
        public DbSet<InventoryProduct> InventoryProducts { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<RoutineStep> RoutineSteps { get; set; }
        public DbSet<RoutineExecution> RoutineExecutions { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Must call base.OnModelCreating to configure Identity tables
            base.OnModelCreating(modelBuilder);

            // Configure SQLite decimal support
            modelBuilder.Entity<InventoryProduct>()
                .Property(p => p.PurchasePrice)
                .HasColumnType("TEXT");

            // Ensure Email is unique (redundant with Identity but good practice)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}