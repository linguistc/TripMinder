using System.ComponentModel.DataAnnotations;

namespace TripMinder.Data.Entities
{
   
    public class GeneralClass // A,B and C
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
