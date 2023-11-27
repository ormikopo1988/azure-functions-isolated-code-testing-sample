using System.Text.Json.Serialization;

namespace FunctionApp.IsolatedDemo.Api.Contracts.Requests
{
    public class CreateNoteRequest
    {
        [JsonPropertyName("title")]
        public string Title { get; init; } = default!;

        [JsonPropertyName("body")]
        public string Body { get; init; } = default!;
    }
}
