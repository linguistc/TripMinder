using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class User
    {
        public User()
        {
            this.SocialMediaProfiles = new HashSet<UserSocialProfile>();
            this.Images = new HashSet<UserImage>();
            this.History = new HashSet<UserHistory>();
            this.Bookmarks = new HashSet<UserBookMark>();
            this.Ratings = new HashSet<UserRating>();
            //this.Preferences = new HashSet<UserPreference>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set; }  // Unique Username

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public ICollection<UserSocialProfile> SocialMediaProfiles { get; set; }
        public ICollection<UserImage> Images { get; set; }
        public ICollection<UserHistory> History { get; set; }
        public ICollection<UserBookMark> Bookmarks { get; set; }
        public ICollection<UserRating> Ratings { get; set; }

        //public ICollection<UserPreference> Preferences { get; set; }
    }  


}
