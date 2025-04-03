using System.ComponentModel.DataAnnotations;

namespace TripMinder.Data.Entities
{


    /// <summary>
    /// Appartement, Hotel, Villa ..etc
    /// </summary>
    public class AccomodationType  
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
    }   
    

}
