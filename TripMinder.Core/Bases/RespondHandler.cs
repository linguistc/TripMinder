using Microsoft.Extensions.Localization;
using TripMinder.Core.Resources;

namespace TripMinder.Core.Bases
{
    public class RespondHandler
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;


        public RespondHandler()
        {
            
        }
        
        public RespondHandler(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
        }
        public Respond<T> Deleted<T>(string message = null)
        {
            return new Respond<T>()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message == null ? stringLocalizer[SharedResourcesKeys.Deleted] : message

            };
        }

        public Respond<T> Success<T>(T entity, object Meta = null)
        {
            return new Respond<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = stringLocalizer[SharedResourcesKeys.Success],
                Meta = Meta
            };
        }
        public Respond<T> Unauthorized<T>(string message = null)
        {
            return new Respond<T>()
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                Succeeded = true,
                Message = message == null ? stringLocalizer[SharedResourcesKeys.UnAuthorized] : message
            };
        }
        public Respond<T> BadRequest<T>(string message = null)
        {
            return new Respond<T>()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = message == null ? stringLocalizer[SharedResourcesKeys.BadRequest] : message
            };
        }

        public Respond<T> UnprocessableEntity<T>(string message = null)
        {
            return new Respond<T>()
            {
                StatusCode = System.Net.HttpStatusCode.UnprocessableEntity,
                Succeeded = false,
                Message = message == null ? stringLocalizer[SharedResourcesKeys.UnprocessableEntity] : message
            };
        }

        public Respond<T> NotFound<T>(string message = null)
        {
            return new Respond<T>()
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message == null ? stringLocalizer[SharedResourcesKeys.NotFound] : message
            };
        }

        public Respond<T> Created<T>(T entity, object Meta = null)
        {
            return new Respond<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = stringLocalizer[SharedResourcesKeys.Created],
                Meta = Meta
            };
        }
    }
}
