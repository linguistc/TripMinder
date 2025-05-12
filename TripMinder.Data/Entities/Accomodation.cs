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
    public class Accomodation
    {
        

        // Shared Properties
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        
        [ForeignKey("AccomodationType")]
        public int AccomodationTypeId { get; set; }
        public virtual AccomodationType AccomodationType { get; set; }

        [ForeignKey("Class")]
        public int ClassId { get; set; }

        public virtual AccomodationClass Class { get; set; }


        [ForeignKey("Zone")]
        public int ZoneId { get; set; }

        public virtual Zone Zone { get; set; }

        public double Rating { get; set; }
        
        // [ForeignKey("Governorate")]
        // public int GovernorateId { get; set; }
        //
        // public Governorate Governorate { get; set; }

        public string? Description { get; set; }
        
        public string Address { get; set; }
        
        public string? MapLink { get; set; }
        
        public double AveragePricePerAdult { get; set; }

        [ForeignKey("PlaceType")]
        public int PlaceTypeId { get; set; }
        public virtual PlaceType PlaceType { get; set; } // Accomodation

        public bool HasKidsArea { get; set; }
        
        public string? ContactLink { get; set; } 
        
        public byte[]? ImgData { get; set; }
        
        public int NumOfBeds { get; set; } = 1;
        
        public string? BedStatus { get; set; }
        public int NumOfMembers { get; set; } = 1;
        
        public float Score { get; set; }
    }
    
}
