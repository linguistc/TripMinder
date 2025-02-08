using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    //public abstract class Place
    //{
    //    public Place() 
    //    {
    //        this.BusinessSocialProfiles = new HashSet<BusinessSocialProfile>();
    //        this.Images = new HashSet<PlaceImage>();
    //    }

    //    // Shared Properties
    //    [Key]
    //    public int Id { get; set; }
    //    public string Name { get; set; }

    //    [ForeignKey("Description")]
    //    public int DescriptionId { get; set; }
    //    public virtual Description Description { get; set; }

    //    public Location Location { get; set; }


    //    [Url(ErrorMessage = "Invalid URL format")]
    //    public string SocialMediaLink { get; set; }

    //    [ForeignKey("Class")]
    //    public int ClassId { get; set; }

    //    public virtual Class Class { get; set; }


    //    [ForeignKey("Zone")]
    //    public int ZoneId { get; set; }

    //    public virtual Zone Zone { get; set; }

    //    public double AveragePricePerAdult { get; set; }

    //    [ForeignKey("PlaceCategory")]
    //    public int CategoryID { get; set; }
    //    public virtual PlaceCategory PlaceCategory { get; set; }

    //    public bool HasKidsArea { get; set; }

    //    public virtual ICollection<BusinessSocialProfile> BusinessSocialProfiles { get; set; }

    //    public virtual ICollection<PlaceImage> Images { get; set; }


    //}
}
