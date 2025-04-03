using System.ComponentModel.DataAnnotations;

namespace TripMinder.Data.Entities
{
    
    /// <summary>
    /// Museums, Histories, Beaches ..etc
    /// </summary>
    public class TourismType // الوصف التجاري
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
    }

}
