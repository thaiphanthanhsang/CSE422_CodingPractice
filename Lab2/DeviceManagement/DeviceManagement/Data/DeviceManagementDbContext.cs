using DeviceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Data;

public class DeviceManagementDbContext : DbContext
{
    public DeviceManagementDbContext(DbContextOptions<DeviceManagementDbContext> options) : base(options) { }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<DeviceCategory> DeviceCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>()
             .HasOne(d => d.DeviceCategory)
             .WithMany(dc => dc.Devices)
             .HasForeignKey(d => d.DeviceCategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Device>()
            .HasOne(d => d.Employee)
            .WithMany(u => u.Devices)
            .HasForeignKey(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<DeviceCategory>().HasData(
    new DeviceCategory { Id = 1, Name = "Computer", Description = "Desktops and laptops" },
    new DeviceCategory { Id = 2, Name = "Printer", Description = "Printers and scanners" },
    new DeviceCategory { Id = 3, Name = "Phone", Description = "Mobile phones and tablets" }
);

    }
}


