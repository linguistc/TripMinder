using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class BookMarkRestaurant
    {
        public int Id { get; set; }

        [ForeignKey("UserBookMark")]
        public int BookmarkId { get; set; }
        public UserBookMark UserBookMark { get; set; }

        public Restaurant Restaurant { get; set; }
        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }
    }
   
}
