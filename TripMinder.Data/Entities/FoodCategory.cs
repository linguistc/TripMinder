using System.ComponentModel.DataAnnotations;

namespace TripMinder.Data.Entities
{
    /// <summary>
    /// SeaFood, FastFood, Italian, Vegan,..etc
    /// </summary>
    public class FoodCategory
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
    }


}
