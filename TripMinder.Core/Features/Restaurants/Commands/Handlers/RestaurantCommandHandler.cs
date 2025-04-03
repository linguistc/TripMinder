using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Restaurants.Commands.Models;
using TripMinder.Core.Resources;
using TripMinder.Data.Entities;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Features.Restaurants.Commands.Handlers
{
    public class RestaurantCommandHandler : RespondHandler
                                          , IRequestHandler<CreateRestaurantCommand, Respond<string>>
                                          , IRequestHandler<UpdateRestaurantCommand, Respond<string>>
                                          , IRequestHandler<DeleteRestaurantCommand, Respond<string>>
    {
        #region Fields
        private readonly IMapper mapper;
        private readonly IRestaurantService restaurantService;
        private readonly IStringLocalizer<SharedResources> stringlocalizer;
        #endregion

        #region Constructors
        public RestaurantCommandHandler(IMapper mapper, IRestaurantService restaurantService, IStringLocalizer<SharedResources> stringlocalizer) : base(stringlocalizer)
        {
            this.mapper = mapper;
            this.restaurantService = restaurantService;
            this.stringlocalizer = stringlocalizer;
        }
        #endregion

        #region Methods


        public async Task<Respond<string>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurantMapper = this.mapper.Map<Restaurant>(request);

            var result = await this.restaurantService.CreateAsync(restaurantMapper);

            if (result == "Created") return Created("");
            else return BadRequest<string>();



        }

        public async Task<Respond<string>> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = await this.restaurantService.GetRestaurantByIdAsync(request.Id);
            if (restaurant == null) return NotFound<string>();

            var restaurantMapper = this.mapper.Map(request, restaurant);
            var result = await this.restaurantService.UpdateAsync(restaurantMapper);
            
            if(result == "Updated") return Success($"{this.stringlocalizer[SharedResourcesKeys.Updated]} {restaurantMapper.Id}");
            
            else return BadRequest<string>();
        }

        public async Task<Respond<string>> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = await this.restaurantService.GetRestaurantByIdAsync(request.Id);
            if (restaurant == null) return NotFound<string>();
            
            var result = await this.restaurantService.DeleteAsync(restaurant);
            if (result == "Deleted") return Deleted<string>($"{this.stringlocalizer[SharedResourcesKeys.Deleted]} {restaurant.Id}");
            else return BadRequest<string>();
        }

        #endregion
    }
}
