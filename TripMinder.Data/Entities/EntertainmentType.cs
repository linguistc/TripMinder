using System.ComponentModel.DataAnnotations;

namespace TripMinder.Data.Entities
{
    
    /// <summary>
    /// Cenima, Theatre, Concerts ..etc
    /// </summary>
    public class EntertainmentType // الوصف التجاري
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
