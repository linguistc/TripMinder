using System.ComponentModel.DataAnnotations;

namespace TripMinder.Data.Entities
{
    // Main Navigation Properties
    
    public class AccomodationClass // A,B,C and D
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }


}
