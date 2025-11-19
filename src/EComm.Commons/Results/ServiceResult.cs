namespace ECommerceApp.EComm.Commons.Results
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public int? ErrorCode { get; set; }
        public Dictionary<string, string[]>? ValidationErrors { get; set; }

        public static ServiceResult<T> Success(T data)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data
            };
        }

        public static ServiceResult<T> Failure(string errorMessage, int? errorCode = null)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }

        public static ServiceResult<T> ValidationFailure(Dictionary<string, string[]> validationErrors)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                ValidationErrors = validationErrors,
                ErrorMessage = "Validation failed"
            };
        }
    }

    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public int? ErrorCode { get; set; }
        public Dictionary<string, string[]>? ValidationErrors { get; set; }

        public static ServiceResult Success()
        {
            return new ServiceResult
            {
                IsSuccess = true
            };
        }

        public static ServiceResult Failure(string errorMessage, int? errorCode = null)
        {
            return new ServiceResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }

        public static ServiceResult ValidationFailure(Dictionary<string, string[]> validationErrors)
        {
            return new ServiceResult
            {
                IsSuccess = false,
                ValidationErrors = validationErrors,
                ErrorMessage = "Validation failed"
            };
        }
    }
}

