using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;

namespace TripMinder.Core.Features.Accomodataions.Queries.Models
{
    public class GetSingleAccomodationQuery : IRequest<Respond<GetSingleAccomodationResponse>>
    {
        public int Id { get; set; }

        public GetSingleAccomodationQuery(int id) 
        {
            this.Id = id;
        }
    }
}
