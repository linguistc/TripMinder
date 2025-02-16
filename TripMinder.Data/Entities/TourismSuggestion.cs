using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class TourismSuggestion
    {
        [Key]
        public int Id { get; set; }
        public TripSuggestion TripSuggestion { get; set; }
        [ForeignKey("TripSuggestion")]
        public int SuggestionId { get; set; }

        [ForeignKey("TourismArea")]
        public int TourismAreaId { get; set; }
        public TourismArea TourismArea { get; set; }

    }


}
