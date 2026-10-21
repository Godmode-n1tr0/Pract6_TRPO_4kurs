using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pract6.Models;

public partial class WebApiDatabaseContext : DbContext
{
    public WebApiDatabaseContext()
    {
    }

    public WebApiDatabaseContext(DbContextOptions<WebApiDatabaseContext> options): base(options)
    {
    }

    public virtual DbSet<Medicine> Medicines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=sql.ects;Database=WebApiDatabase;User Id=student_00;Password=student_00;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Medicine__3214EC07F4783701");

            entity.Property(e => e.Manufacturer).HasMaxLength(120);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
