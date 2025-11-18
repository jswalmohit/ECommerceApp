namespace ECommerceApp.EComm.Commons.Exceptions
{
    public class AppException : Exception
    {
        public int StatusCode { get; }
        public Dictionary<string, string[]>? ValidationErrors { get; }

        public AppException(string message, int statusCode = 500) 
            : base(message)
        {
            StatusCode = statusCode;
        }

        public AppException(string message, Dictionary<string, string[]> validationErrors, int statusCode = 400)
            : base(message)
        {
            StatusCode = statusCode;
            ValidationErrors = validationErrors;
        }

        public AppException(string message, Exception innerException, int statusCode = 500)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}

