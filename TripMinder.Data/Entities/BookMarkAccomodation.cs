using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class BookMarkAccomodation
    {
        public int Id { get; set; }

        [ForeignKey("UserBookMark")]
        public int BookmarkId { get; set; }
        public UserBookMark UserBookMark { get; set; }

        public Accomodation Accomodation { get; set; }
        [ForeignKey("Accomodation")]
        public int AccomodationId { get; set; }
    }
   
}
