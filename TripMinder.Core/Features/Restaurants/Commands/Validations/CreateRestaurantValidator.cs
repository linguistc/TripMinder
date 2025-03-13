using FluentValidation;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Features.Restaurants.Commands.Models;
using TripMinder.Core.Resources;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Features.Restaurants.Commands.Validations
{
    public class SignInValidator : AbstractValidator<CreateRestaurantCommand>
    {

        #region Fields
        private readonly IRestaurantService restaurantService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly IZoneService zoneService;
        private readonly IFoodCategoryService foodCategoryService;
        private readonly IRestaurantClassService restaurantClassService;
        private readonly IPlaceTypeService placeTypeService;
        #endregion

        #region Constructor
        public SignInValidator(IRestaurantService restaurantService
            , IStringLocalizer<SharedResources> stringLocalizer, IZoneService zoneService
            , IFoodCategoryService foodCategoryService, IRestaurantClassService restaurantClassService
            , IPlaceTypeService placeTypeService)
        {
            this.restaurantService = restaurantService; 
            this.stringLocalizer = stringLocalizer;
            this.zoneService = zoneService;
            this.foodCategoryService = foodCategoryService;
            this.restaurantClassService = restaurantClassService;
            this.placeTypeService = placeTypeService;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Functions
        public void ApplyValidationsRules()
        {
        }

        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}
