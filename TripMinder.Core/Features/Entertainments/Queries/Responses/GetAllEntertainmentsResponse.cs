using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripMinder.Core.Features.Entertainments.Queries.Responses
{
    public class GetAllEntertainmentsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string Class { get; set; }

        public string Zone { get; set; }

        public double AveragePricePerAdult { get; set; }

        public bool HasKidsArea { get; set; }

    }
}
