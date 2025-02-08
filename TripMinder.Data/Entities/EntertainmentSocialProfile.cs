using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class EntertainmentSocialProfile
    {
        [Key]
        public int Id { get; set; }
        public string PlatformName { get; set; }  // Facebook, Twitter, etc.

        [Url(ErrorMessage = "Invalid URL format")]
        public string ProfileLink { get; set; }

        [ForeignKey("Entertainment")]
        public int EntertainmentId { get; set; }
        public Entertainment Entertainment { get; set; }
    }


}
