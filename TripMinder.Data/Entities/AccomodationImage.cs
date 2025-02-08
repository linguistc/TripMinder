using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class AccomodationImage
    {
        [Key]
        public int Id { get; set; }

        public string Source { get; set; }

        [ForeignKey("Accomodation")]
        public int AccomodationId { get; set; }
        public Accomodation Accomodation { get; set; }

        // placed in the navigation property
        //public int PlaceCategoryId { get; set; }

        //public PlaceCategory PlaceCategory { get; set; }

    }
}
