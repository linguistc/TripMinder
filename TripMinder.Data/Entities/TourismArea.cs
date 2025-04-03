using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TripMinder.Data.Entities
{
    public class TourismArea
    {
        // Shared Properties
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("TourismType")]
        public int TourismTypeId { get; set; }
        public virtual TourismType TourismType { get; set; }

        
        [ForeignKey("Class")]
        public int ClassId { get; set; }

        public virtual TourismAreaClass Class { get; set; }


        [ForeignKey("Zone")]
        public int ZoneId { get; set; }

        public virtual Zone Zone { get; set; }

        public double Rating { get; set; }
        
        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        
        public Governorate Governorate { get; set; }

        public string? Description { get; set; }
        
        public string Address { get; set; }
        
        public string? MapLink { get; set; }
        
        public double AveragePricePerAdult { get; set; }

        [ForeignKey("PlaceType")]
        public int PlaceTypeId { get; set; }
        public virtual PlaceType PlaceType { get; set; } // TourismArea

        public bool HasKidsArea { get; set; }
        
        public string? ContactLink { get; set; } 
        
        public string? ImageSource { get; set; }



    }
}
