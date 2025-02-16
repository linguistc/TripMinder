using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class AccomodationSocialProfile
    {
        [Key]
        public int Id { get; set; }
        public string PlatformName { get; set; }  // Facebook, Twitter, etc.

        [Url(ErrorMessage = "Invalid URL format")]
        public string ProfileLink { get; set; }

        [ForeignKey("Accomodation")]
        public int AccomodationId { get; set; }
        public Accomodation Accomodation { get; set; }
    }
}
