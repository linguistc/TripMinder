using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class BookMarkTourism
    {
        public int Id { get; set; }

        [ForeignKey("UserBookMark")]
        public int BookmarkId { get; set; }
        public UserBookMark UserBookMark { get; set; }

        public TourismArea TourismArea { get; set; }
        [ForeignKey("TourismArea")]
        public int TourismId { get; set; }
    }
   
}
