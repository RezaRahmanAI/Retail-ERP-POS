using Microsoft.EntityFrameworkCore;
using RetailERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetailERP.Infrastructure.Data;

public class ErpDbContext : DbContext
{
    public ErpDbContext(DbContextOptions<ErpDbContext> option): base(option)
    {
        
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Stock> Stocks { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.Property(x => x.Name).IsRequired().HasMaxLength(100);
        });

        // Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
            entity.Property(x => x.SKU).IsRequired().HasMaxLength(50);
            entity.Property(x => x.CostPrice).HasColumnType("decimal(18,2)");
            entity.Property(x => x.SalePrice).HasColumnType("decimal(18,2)");
            entity.Property(x => x.VatPercent).HasColumnType("decimal(5,2)");

            entity.HasOne(x => x.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(x => x.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Branch
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.ToTable("Branches");
            entity.Property(x => x.Name).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Code).HasMaxLength(20);
        });

        // Stock
        modelBuilder.Entity<Stock>(entity =>
        {
            entity.ToTable("Stocks");

            entity.HasIndex(x => new { x.ProductId, x.BranchId }).IsUnique();

            entity.Property(x => x.Quantity)
                  .HasColumnType("decimal(18,2)");

            entity.HasOne(x => x.Product)
                  .WithMany(p => p.Stocks)
                  .HasForeignKey(x => x.ProductId);

            entity.HasOne(x => x.Branch)
                  .WithMany(b => b.Stocks)
                  .HasForeignKey(x => x.BranchId);
        });
    }

}
