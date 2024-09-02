namespace SmartWeather.Api.Contract
{
    public class ApiResponse<T> where T : class
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse(Status status, string message, T? data)
        {
            Message = message;
            Status = status;
            Data = data;
        }

        public static ApiResponse<T> Success(T? data)
        {
            return new ApiResponse<T>(Status.OK, BaseResponses.OK, data);
        }

        public static ApiResponse<T> Failure(string customMessage = BaseResponses.INTERNAL_ERROR)
        {
            return new ApiResponse<T>(Status.INTERNAL_ERROR, customMessage, null);
        }
    }
}
