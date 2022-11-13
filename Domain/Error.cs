using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Error
    {
        public Error()
        {
        }

        public Error(IEnumerable<string> errors, string message)
        {
            Message = message;
            Errors = errors.ToList();
        }

        [JsonPropertyName("success")]
        public bool Success { get; set; } = false;

        [JsonPropertyName("message")]
        public string Message { get; set; }
        
        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; } = default!;

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}