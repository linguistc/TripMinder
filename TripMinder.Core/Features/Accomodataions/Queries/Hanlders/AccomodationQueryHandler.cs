using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Accomodataions.Queries.Models;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Features.Accomodataions.Queries.Hanlders
{
    public class AccomodationQueryHandler : RespondHandler
                                         , IRequestHandler<GetSingleAccomodationQuery, Respond<GetSingleAccomodationResponse>>
                                         , IRequestHandler<GetAccomodationsListQuery, Respond<List<GetAccomodationsListResponse>>>
    {
        #region Fields
        private readonly IAccomodationService service;
        private readonly IMapper mapper;
        #endregion


        #region Constructors
        public AccomodationQueryHandler(IAccomodationService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }
        #endregion


        #region Functions
        public async Task<Respond<GetSingleAccomodationResponse>> Handle(GetSingleAccomodationQuery request, CancellationToken cancellationToken)
        {
            var accomodation = await this.service.GetAccomodationByIdAsync(request.Id);

            if (accomodation == null)
                return NotFound<GetSingleAccomodationResponse>();

            var result = this.mapper.Map<GetSingleAccomodationResponse>(accomodation);

            return Success(result);
        }

        Task<Respond<List<GetAccomodationsListResponse>>> IRequestHandler<GetAccomodationsListQuery, Respond<List<GetAccomodationsListResponse>>>.Handle(GetAccomodationsListQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
