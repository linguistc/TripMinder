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
        //private readonly IDescriptionService accomodationService;
        #endregion

        #region Constructor
        public SignInValidator(IRestaurantService restaurantService
            , IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.restaurantService = restaurantService;
            //this.stringLocalizer = stringLocalizer;
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
