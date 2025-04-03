using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class Zone
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        
        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        
        public Governorate Governorate { get; set; }
    }


}
