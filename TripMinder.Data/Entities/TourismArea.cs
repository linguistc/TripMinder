using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TripMinder.Data.Entities
{
    public class TourismArea
    {
        public TourismArea()
        {
            this.BusinessSocialProfiles = new HashSet<TourismSocialProfile>();
            this.Images = new HashSet<TourismImage>();
        }


        // Shared Properties
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Description")]
        public int DescriptionId { get; set; }
        public virtual TourismDescription Description { get; set; }

        public Location Location { get; set; }

        [ForeignKey("Location")]
        public int LocationId { get; set; }

        [ForeignKey("Class")]
        public int ClassId { get; set; }

        public virtual GeneralClass Class { get; set; }


        [ForeignKey("Zone")]
        public int ZoneId { get; set; }

        public virtual Zone Zone { get; set; }

        public double AveragePricePerAdult { get; set; }

        [ForeignKey("PlaceCategory")]
        public int CategoryID { get; set; }
        public virtual PlaceCategory PlaceCategory { get; set; }

        public bool HasKidsArea { get; set; }

        public virtual ICollection<TourismSocialProfile> BusinessSocialProfiles { get; set; }

        public virtual ICollection<TourismImage> Images { get; set; }



    }

}
