using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class UserBookMark
    {
        public UserBookMark() 
        {
            this.BookMarkTrips = new HashSet<BookMarkTrip>();
            this.BookMarkAccomodations = new HashSet<BookMarkAccomodation>();
            this.BookMarkRestaurants = new HashSet<BookMarkRestaurant>();
            this.BookmarkEntertainments = new HashSet<BookmarkEntertainment>();
            this.BookMarkTourisms = new HashSet<BookMarkTourism>();
        }

        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        //[ForeignKey("TripSuggestion")]
        //public int TripSuggestionId { get; set; }
        //public TripSuggestion TripSuggestion { get; set; }

        public ICollection<BookMarkTrip> BookMarkTrips { get; set; }
        public ICollection<BookMarkAccomodation> BookMarkAccomodations { get; set; }
        public ICollection<BookMarkRestaurant> BookMarkRestaurants { get; set; }
        public ICollection<BookmarkEntertainment> BookmarkEntertainments { get; set; }
        public ICollection<BookMarkTourism> BookMarkTourisms { get; set; }


        //[ForeignKey("Place")]
        //public int PlaceId { get; set; }
        //public Place Place { get; set; }

        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    }
   
}
