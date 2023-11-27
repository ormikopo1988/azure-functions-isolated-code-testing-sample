using System.Text.Json.Serialization;

namespace FunctionApp.IsolatedDemo.Api.Models
{
    public class NoteCreatedRequest
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = default!;

        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;
    }
}
