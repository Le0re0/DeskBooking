using System;
using System.Linq;
using DeskBooking.Data;
using DeskBooking.Models;

namespace DeskBooking
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            // Seed Employees
            if (!db.Employees.Any())
            {
                db.Employees.AddRange(
                    new Employee { FirstName = "Alice", LastName = "Smith", Email = "alice@example.com" },
                    new Employee { FirstName = "Bob", LastName = "Jones", Email = "bob@example.com" },
                    new Employee { FirstName = "Charlie", LastName = "Brown", Email = "charlie@example.com" },
                    new Employee { FirstName = "Diana", LastName = "Prince", Email = "diana@example.com" },
                    new Employee { FirstName = "Eve", LastName = "Adams", Email = "eve@example.com" }
                );
            }

            // Seed Desks
            if (!db.Desks.Any())
            {
                db.Desks.AddRange(
                    new Desk { DeskNumber = "D-101", IsActive = true },
                    new Desk { DeskNumber = "D-102", IsActive = true },
                    new Desk { DeskNumber = "D-103", IsActive = true },
                    new Desk { DeskNumber = "D-104", IsActive = false },
                    new Desk { DeskNumber = "D-105", IsActive = true }
                );
            }

            db.SaveChanges();

            // Seed Bookings
            if (!db.Bookings.Any())
            {
                var employees = db.Employees.Take(5).ToList();
                var desks = db.Desks.Where(d => d.IsActive).Take(5).ToList();
                int count = Math.Min(employees.Count, desks.Count);
                for (int i = 0; i < count; i++)
                {
                    db.Bookings.Add(new Booking
                    {
                        EmployeeId = employees[i].Id,
                        DeskId = desks[i].Id,
                        BookedFrom = DateTime.Today.AddDays(i),
                        BookedUntil = DateTime.Today.AddDays(i + 1)
                    });
                }
                db.SaveChanges();
            }
        }
    }
}
