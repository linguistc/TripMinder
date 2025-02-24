namespace TripMinder.Core.Bases
{
    public class RespondHandler
    {
        public RespondHandler()
        {

        }
        public Respond<T> Deleted<T>()
        {
            return new Respond<T>()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = "Deleted Successfully"
            };
        }
        public Respond<T> Success<T>(T entity, object Meta = null)
        {
            return new Respond<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = "Added Successfully",
                Meta = Meta
            };
        }
        public Respond<T> Unauthorized<T>()
        {
            return new Respond<T>()
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                Succeeded = true,
                Message = "UnAuthorized"
            };
        }
        public Respond<T> BadRequest<T>(string Message = null)
        {
            return new Respond<T>()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = Message == null ? "Bad Request" : Message
            };
        }

        public Respond<T> NotFound<T>(string message = null)
        {
            return new Respond<T>()
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message == null ? "Not Found" : message
            };
        }

        public Respond<T> Created<T>(T entity, object Meta = null)
        {
            return new Respond<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = "Created",
                Meta = Meta
            };
        }
    }
}
