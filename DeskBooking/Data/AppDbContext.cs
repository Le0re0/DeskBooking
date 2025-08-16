using System;
using System.IO;
using DeskBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace DeskBooking.Data;

public class AppDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Desk> Desks { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    public DbSet<BookedDesk> BookedDesks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dataFolder = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Data");
        System.IO.Directory.CreateDirectory(dataFolder);
        var dbPath = System.IO.Path.Combine(dataFolder, "DeskBooking.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
}