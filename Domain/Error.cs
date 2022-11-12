namespace Domain
{
    public class Error
    {
        public Error()
        {
        }

        public Error(string error)
        {
            Message = error;
        }

        public Error(IEnumerable<string> errors, string message)
        {
            Message = message;
            Errors = errors.ToList();
        }

        public bool Success { get; set; } = false;
        public string Message { get; set; }
        public List<string> Errors { get; set; } = default!;
    }
}