using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripMinder.Data.Entities
{
    public class RestaurantSuggestion
    {
        [Key]
        public int Id { get; set; }
        public TripSuggestion TripSuggestion { get; set; }
        [ForeignKey("TripSuggestion")]
        public int SuggestionId { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

    }


}
