using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class RestaurantSocialProfile
    {
        [Key]
        public int Id { get; set; }
        public string PlatformName { get; set; }  // Facebook, Twitter, etc.

        [Url(ErrorMessage = "Invalid URL format")]
        public string ProfileLink { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }


}
