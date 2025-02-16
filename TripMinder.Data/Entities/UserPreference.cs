using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class UserPreference
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        public string Category { get; set; } // مثل "مطاعم", "فنادق"
        public string Value { get; set; } // مثل "إيطالي", "5 نجوم"
    }

}
