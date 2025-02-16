using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class BookmarkEntertainment
    {
        public int Id { get; set; }

        [ForeignKey("UserBookMark")]
        public int BookmarkId { get; set; }
        public UserBookMark UserBookMark { get; set; }

        public Entertainment Entertainment { get; set; }
        [ForeignKey("Entertainment")]
        public int EntertainmentId { get; set; }
    }
   
}
