using System.ComponentModel.DataAnnotations;

namespace TripMinder.Data.Entities
{
   
    public class RestaurantClass // A,B and C
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
    }

}
