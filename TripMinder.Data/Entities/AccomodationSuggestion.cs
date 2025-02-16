using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class AccomodationSuggestion
    {
        [Key]
        public int Id { get; set; }
        public TripSuggestion TripSuggestion { get; set; }
        [ForeignKey("TripSuggestion")]
        public int SuggestionId { get; set; }

        [ForeignKey("Accomodation")]
        public int AccomodationId { get; set; }
        public Accomodation Accomodation { get; set; }

    }


}
