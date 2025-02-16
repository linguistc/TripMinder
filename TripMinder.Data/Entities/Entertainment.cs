using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TripMinder.Data.Entities
{
    public class Entertainment
    {
        public Entertainment()
        {
            this.BusinessSocialProfiles = new HashSet<EntertainmentSocialProfile>();
            this.Images = new HashSet<EntertainmentImage>();
        }



        // Shared Properties
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Description")]
        public int DescriptionId { get; set; }
        public virtual EntertainmentDescription Description { get; set; }

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

        public virtual ICollection<EntertainmentSocialProfile> BusinessSocialProfiles { get; set; }

        public virtual ICollection<EntertainmentImage> Images { get; set; }
    }

}
