
namespace Backend_Teamwork.src.Utils
{
    public class CustomException : Exception
    {
        public int StatusCode { get; set; }

        public CustomException(int statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public static CustomException NotFound(string message)
        {
            return new CustomException(404, message);
        }

        public static CustomException BadRequest(string message)
        {
            return new CustomException(400, message);
        }

        public static CustomException UnAuthorized(string message)
        {
            return new CustomException(401, message);
        }

        public static CustomException Forbidden(string message)
        {
            return new CustomException(403, message);
        }

        public static CustomException InternalServer(string message)
        {
            return new CustomException(500, message);
        }
    }
}
