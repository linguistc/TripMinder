using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class UserSocialProfile
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        public string PlatformName { get; set; }  // Facebook, Twitter, etc.

        [Url(ErrorMessage = "Invalid URL format")]
        public string ProfileLink { get; set; }

    }

}
