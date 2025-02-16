using System.ComponentModel.DataAnnotations;

namespace TripMinder.Data.Entities
{
    

    public class PlaceCategory // مطعم إقامة ترفيه سياحة
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
    }

}
