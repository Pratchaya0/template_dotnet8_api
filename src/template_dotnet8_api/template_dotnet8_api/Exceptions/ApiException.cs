namespace template_dotnet8_api.Exceptions
{
    public enum ResponseType
    {
        BadRequest = 400,
        Conflict = 409,
        NoContent = 204,
        NotFound = 404,
        Unauthorized = 401,
        UnsupportMediaType = 415
    }
    public class ApiException : Exception
    {
        public ResponseType ResponseType { get; private set; }

        public ApiException(string message) : base(message)
        {

        }
    }
}
