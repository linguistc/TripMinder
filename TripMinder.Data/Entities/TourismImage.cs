using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class TourismImage
    {
        [Key]
        public int Id { get; set; }
        public string Source { get; set; }

        [ForeignKey("TourismArea")]
        public int TourismAreaId { get; set; }
        public TourismArea TourismArea { get; set; }

        // placed in the navigation property
        //public int PlaceCategoryId { get; set; }

        //public PlaceCategory PlaceCategory { get; set; }

    }


}
