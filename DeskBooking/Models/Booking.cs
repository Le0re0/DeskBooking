using System;
using System.ComponentModel.DataAnnotations;


namespace DeskBooking.Models;

public class Booking
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public int DeskId { get; set; }
    public Desk? Desk { get; set; }

    public DateTime BookedFrom { get; set; }
    public DateTime BookedUntil { get; set; }
}
