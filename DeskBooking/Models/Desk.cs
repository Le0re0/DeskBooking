using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DeskBooking.Models;

public class Desk
{
    [Key]
    public int Id { get; set; }
    public string DeskNumber { get; set; } = null!;
    public bool IsActive { get; set; } = true; // Default to available

}

public class BookedDesk
{
    public int Id { get; set; }

    [ForeignKey("Desk")]
    public int DeskId { get; set; }

    [ForeignKey("Booking")]
    public int BookingId { get; set; }

    public DateTime BookedFrom { get; set; }
    public DateTime BookedUntil { get; set; }


    public required Employee Employee { get; set; }
}