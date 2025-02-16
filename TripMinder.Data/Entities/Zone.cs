using System.ComponentModel.DataAnnotations;

namespace TripMinder.Data.Entities
{
    public class Zone
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }


}
