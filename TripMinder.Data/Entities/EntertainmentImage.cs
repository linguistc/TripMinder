using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class EntertainmentImage
    {
        [Key]
        public int Id { get; set; }

        public string Source { get; set; }

        [ForeignKey("Entertainment")]
        public int EntertainmentId { get; set; }
        public Entertainment Entertainment { get; set; }


        // placed in the navigation property
        //public int PlaceCategoryId { get; set; }

        //public PlaceCategory PlaceCategory { get; set; }

    }

   
}
