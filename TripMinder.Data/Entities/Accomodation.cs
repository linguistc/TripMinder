using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TripMinder.Data.Entities
{

    // Helper Classes
    
    public class Location
    {
        public int Id {  get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Address { get; set; }
    }

    public class Accomodation
    {

        public Accomodation() 
        {
            this.BusinessSocialProfiles = new HashSet<AccomodationSocialProfile>();
            this.Images = new HashSet<AccomodationImage>();
        }


        // Shared Properties
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Description")]
        public int DescriptionId { get; set; }
        public virtual AccomodationDescription Description { get; set; }

        public Location Location { get; set; }

        [ForeignKey("Location")]
        public int LocationId { get; set; }

        [ForeignKey("Class")]
        public int ClassId { get; set; }

        public virtual AccomodationClass Class { get; set; }


        [ForeignKey("Zone")]
        public int ZoneId { get; set; }

        public virtual Zone Zone { get; set; }

        public double AveragePricePerAdult { get; set; }

        [ForeignKey("PlaceCategory")]
        public int CategoryID { get; set; }
        public virtual PlaceCategory PlaceCategory { get; set; }

        public bool HasKidsArea { get; set; }

        public virtual ICollection<AccomodationSocialProfile> BusinessSocialProfiles { get; set; }

        public virtual ICollection<AccomodationImage> Images { get; set; }


        public int NumOfBeds { get; set; } = 1;

        public int NumOfMembers { get; set; } = 1;   

    }


    // abstract classes
    #region
    public abstract class Description
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
    }

    public abstract class Class
    {
        
    }

    public abstract class Category
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
    }

    #endregion
}
