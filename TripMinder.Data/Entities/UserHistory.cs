using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class UserHistory
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        public string Action { get; set; } // مثل "بحث عن مطعم", "حجز فندق"
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }


}
