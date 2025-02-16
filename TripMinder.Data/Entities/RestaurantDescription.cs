using System.ComponentModel.DataAnnotations;

namespace TripMinder.Data.Entities
{
    
    public class RestaurantDescription // الوصف التجاري
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
    }


}
