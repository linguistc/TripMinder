using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class BookMarkTrip
    {
        public int Id { get; set; }

        [ForeignKey("UserBookMark")]
        public int BookmarkId { get; set; }
        public UserBookMark UserBookMark { get; set; }

        public TripSuggestion TripSuggestion { get; set; }
        [ForeignKey("TripSuggestion")]
        public int TripSuggestionId { get; set; }

    }
   
}
