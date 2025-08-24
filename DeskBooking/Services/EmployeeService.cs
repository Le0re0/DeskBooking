using DeskBooking.Data;
using DeskBooking.Models;
using System.Collections.Generic;
using System.Linq;

namespace DeskBooking.Services;

public class EmployeeService
{
    private readonly AppDbContext _context;

    public EmployeeService(AppDbContext context)
    {
        _context = context;
    }

    // CREATE
    public void AddEmployee(Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();
    }

    // READ (all)
    public List<Employee> GetAllEmployees()
    {
        return _context.Employees.ToList();
    }

    // READ (by id)
    public Employee? GetEmployeeById(int id)
    {
        return _context.Employees.FirstOrDefault(e => e.Id == id);
    }

    // UPDATE
    public void UpdateEmployee(Employee employee)
    {
        _context.Employees.Update(employee);
        _context.SaveChanges();
    }

    // DELETE
    public void DeleteEmployee(int id)
    {
        var employee = _context.Employees.FirstOrDefault(e => e.Id == id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }
    }
}