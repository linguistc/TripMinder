using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class TourismSocialProfile
    {
        [Key]
        public int Id { get; set; }
        public string PlatformName { get; set; }  // Facebook, Twitter, etc.

        [Url(ErrorMessage = "Invalid URL format")]
        public string ProfileLink { get; set; }

        [ForeignKey("TourismArea")]
        public int TourismAreaId { get; set; }
        public TourismArea TourismArea { get; set; }
    }


}
