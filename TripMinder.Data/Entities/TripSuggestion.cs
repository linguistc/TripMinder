using System.ComponentModel.DataAnnotations;

namespace TripMinder.Data.Entities
{
    public class TripSuggestion
    {
        public TripSuggestion() 
        {
            this.Accomodations = new HashSet<Accomodation>();
            this.Restaurants = new HashSet<Restaurant>();
            this.Entertainments = new HashSet<Entertainment>();
            this.TourismAreas = new HashSet<TourismArea>();        
        }


        [Key]
        public int Id { get; set; }
        public double Budget { get; set; }

        public ICollection<Accomodation>? Accomodations { get; set; }
        public ICollection<Restaurant>? Restaurants { get; set; }
        public ICollection<Entertainment>? Entertainments { get; set; }
        public ICollection<TourismArea>? TourismAreas { get; set; }
    }


}
