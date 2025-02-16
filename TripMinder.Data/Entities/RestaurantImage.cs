using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class RestaurantImage
    {
        [Key]
        public int Id { get; set; }

        public string Source { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        // placed in the navigation property

        //public int PlaceCategoryId { get; set; }

        //public PlaceCategory PlaceCategory { get; set; }

    }

   
}
