namespace Domain
{
    public class Result<T>
    {
        public Result()
        {

        }

        public Result(T data, string message = "Success")
        {
            Data = data;
            Success = true;
            Message = message;
        }

        public Result(string message, bool success = false, List<string>? errors = null)
        {
            Data = default;
            Success = success;
            Message = message;
            Errors = errors;
        }

        public bool Success { get; set; }

        public string Message { get; set; }

        public T? Data { get; set; }
        public List<string> Errors { get; set; } = default!;

        public static implicit operator Result<T>(T data) => new(data);
    }
}
