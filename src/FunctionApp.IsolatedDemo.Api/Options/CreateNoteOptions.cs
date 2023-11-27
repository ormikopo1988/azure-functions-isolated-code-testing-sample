namespace FunctionApp.IsolatedDemo.Api.Options
{
    public class CreateNoteOptions
    {
        public string Title { get; init; } = default!;

        public string Body { get; init; } = default!;
    }
}
