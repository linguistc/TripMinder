using System.ComponentModel.DataAnnotations;

namespace TripMinder.Data.Entities
{
   
    public class FoodCategory // Food Types
    {

        public FoodCategory()
        {
            this.RestaurantFoodCategories = new HashSet<RestaurantFoodCategory>();
        }

        [Key]
        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<RestaurantFoodCategory> RestaurantFoodCategories { get; set; }
        
    }


}
