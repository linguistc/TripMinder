using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class EntertainmentSuggestion
    {
        [Key]
        public int Id { get; set; }
        public TripSuggestion TripSuggestion { get; set; }
        [ForeignKey("TripSuggestion")]
        public int SuggestionId { get; set; }

        [ForeignKey("Entertainment")]
        public int EntertainmentId { get; set; }
        public Entertainment Entertainment { get; set; }

    }


}
