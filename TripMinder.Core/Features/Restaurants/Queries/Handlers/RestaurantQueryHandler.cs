﻿using MediatR;
using AutoMapper;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Bases;
using TripMinder.Core.Behaviors;
using TripMinder.Core.Features.Restaurants.Queries.Models;
using TripMinder.Core.Features.Restaurants.Queries.Responses;
using TripMinder.Core.Resources;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Features.Restaurants.Queries.Handlers
{
    public class RestaurantQueryHandler : RespondHandler
                                        , IRequestHandler<GetRestaurantsListQuery, Respond<List<GetRestaurantsListResponse>>>
                                        , IRequestHandler<GetRestaurantsListByZoneIdQuery, Respond<List<GetRestaurantsListResponse>>>
                                        , IRequestHandler<GetRestaurantsListByGovernorateIdQuery, Respond<List<GetRestaurantsListResponse>>>
                                        , IRequestHandler<GetRestaurantByIdQuery, Respond<GetRestaurantByIdResponse>>
    {

        #region Fields
        private readonly IRestaurantService restaurantService;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        #endregion


        #region Constructors
        public RestaurantQueryHandler(IRestaurantService service, IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.restaurantService = service;
            this.stringLocalizer = stringLocalizer;
        }

        #endregion


        #region Functions
        public async Task<Respond<List<GetRestaurantsListResponse>>> Handle(GetRestaurantsListQuery request, CancellationToken cancellationToken)
        {
            var restaurantList = await this.restaurantService.GetRestaurantsListAsync();

            var restaurantMapper = this.mapper.Map<List<GetRestaurantsListResponse>>(restaurantList);

            var result = Success(restaurantMapper);
            
            result.Meta = new {Count = restaurantMapper.Count};

            return result;
        }

        public async Task<Respond<GetRestaurantByIdResponse>> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
        {
            var restaurant = await this.restaurantService.GetRestaurantByIdWithIncludeAsync(request.Id);

            if (restaurant == null)
                return NotFound<GetRestaurantByIdResponse>(this.stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = this.mapper.Map<GetRestaurantByIdResponse>(restaurant);
        
            return Success(result);
        }

        public async Task<Respond<List<GetRestaurantsListResponse>>> Handle(GetRestaurantsListByZoneIdQuery request, CancellationToken cancellationToken)
        {
            var restaurantsList = await this.restaurantService.GetRestaurantsListByZoneIdAsync(request.ZoneId, cancellationToken);

            restaurantsList.ForEach(a => a.Score = CalculateScoreBehavior.CalculateScore(a.Class.Type, request.Priority, a.AveragePricePerAdult));

            var restaurantMapper = this.mapper.Map<List<GetRestaurantsListResponse>>(restaurantsList);

            var result = Success(restaurantMapper);

            result.Meta = new { Count = restaurantMapper.Count };

            return result;
        }

        public async Task<Respond<List<GetRestaurantsListResponse>>> Handle(GetRestaurantsListByGovernorateIdQuery request, CancellationToken cancellationToken)
        {
            var restaurantsList = await this.restaurantService.GetRestaurantsListByGovernorateIdAsync(request.GovernorateId, cancellationToken);

            restaurantsList.ForEach(a => a.Score = CalculateScoreBehavior.CalculateScore(a.Class.Type, request.Priority, a.AveragePricePerAdult));

            var restaurantMapper = this.mapper.Map<List<GetRestaurantsListResponse>>(restaurantsList);

            var result = Success(restaurantMapper);

            result.Meta = new { Count = restaurantMapper.Count };

            return result;
        }
        #endregion

    }
}
