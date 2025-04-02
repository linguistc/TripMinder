using System.ComponentModel.DataAnnotations;

namespace TripMinder.Data.Entities;

public class Governorate
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}