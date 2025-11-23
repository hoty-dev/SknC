/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 23/11/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using Microsoft.EntityFrameworkCore;
using SknC.Web.Core.Entities;

namespace SknC.Web.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ProductReference> ProductReferences { get; set; }
        public DbSet<InventoryProduct> InventoryProducts { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<RoutineStep> RoutineSteps { get; set; }
        public DbSet<RoutineExecution> RoutineExecutions { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure SQLite decimal support
            modelBuilder.Entity<InventoryProduct>()
                .Property(p => p.PurchasePrice)
                .HasColumnType("TEXT");

            // Ensure Email is unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}