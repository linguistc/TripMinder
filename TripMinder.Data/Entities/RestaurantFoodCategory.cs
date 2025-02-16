using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    
    public class RestaurantFoodCategory
    {
        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        [ForeignKey("FoodCategory")]
        public int FoodCategoryId { get; set; }

        public virtual Restaurant Restaurant { get; set; }
        public virtual FoodCategory FoodCategory { get; set; }
        
    }


}
