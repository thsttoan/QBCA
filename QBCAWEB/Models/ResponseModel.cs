namespace QBCAWEB.Models
{
    public class ResponseModel<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int StatusCode { get; set; }

        public static ResponseModel<T> SuccessResult(T data, string message = "Success")
        {
            return new ResponseModel<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = 200
            };
        }

        public static ResponseModel<T> ErrorResult(string message, int statusCode = 400)
        {
            return new ResponseModel<T>
            {
                Success = false,
                Message = message,
                Data = default(T),
                StatusCode = statusCode
            };
        }
    }
}