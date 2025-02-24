using MediatR;
using AutoMapper;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Restaurants.Queries.Models;
using TripMinder.Core.Features.Restaurants.Queries.Responses;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Features.Restaurants.Queries.Handlers
{
    public class RestaurantQueryHandler : RespondHandler
                                        , IRequestHandler<GetAllRestaurantsQuery, Respond<List<GetAllRestaurantsResponse>>>
                                        , IRequestHandler<GetRestaurantByIdQuery, Respond<GetSingleRestaurantResponse>>
    {

        #region Fields
        private readonly IRestaurantService restaurantService;
        private readonly IMapper mapper;

        #endregion


        #region Constructors
        public RestaurantQueryHandler(IRestaurantService service, IMapper mapper)
        {
            this.mapper = mapper;
            this.restaurantService = service;
        }

        #endregion


        #region Functions
        public async Task<Respond<List<GetAllRestaurantsResponse>>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            var restaurantList = await this.restaurantService.GetAllRestaurantsAsync();

            var restaurantMapper = this.mapper.Map<List<GetAllRestaurantsResponse>>(restaurantList);

            return Success(restaurantMapper);
        }

        public async Task<Respond<GetSingleRestaurantResponse>> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
        {
            var restaurant = await this.restaurantService.GetRestaurantByIdAsync(request.Id);

            if (restaurant == null)
                return NotFound<GetSingleRestaurantResponse>("Object Not Found");

            var result = this.mapper.Map<GetSingleRestaurantResponse>(restaurant);
        
            return Success(result);
        }

        #endregion
    }
}
