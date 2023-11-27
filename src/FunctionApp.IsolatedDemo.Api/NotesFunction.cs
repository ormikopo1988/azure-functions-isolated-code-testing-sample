using System.Threading.Tasks;
using System.Threading;
using Microsoft.Azure.Functions.Worker;
using FunctionApp.IsolatedDemo.Api.Contracts.Responses;
using FunctionApp.IsolatedDemo.Api.Contracts.Requests;
using FunctionApp.IsolatedDemo.Api.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using FunctionApp.IsolatedDemo.Api.Options;
using System;

namespace FunctionApp.IsolatedDemo.Api
{
    public class NotesFunction
    {
        private readonly INoteService _noteService;
        private readonly ILogger<NotesFunction> _logger;

        public NotesFunction(INoteService noteService, ILogger<NotesFunction> logger)
        {
            _noteService = noteService;
            _logger = logger;
        }

        [Function("NotesFunction")]
        public async Task<IActionResult> Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "notes")]
            [Microsoft.Azure.Functions.Worker.Http.FromBody] CreateNoteRequest createNoteRequest,
            CancellationToken ct = default)
        {
            _logger.LogInformation("C# HTTP trigger NotesFunction processed a request.");

            try
            {
                var newNoteDto = await _noteService.CreateNoteAsync(new CreateNoteOptions
                {
                    Body = createNoteRequest.Body,
                    Title = createNoteRequest.Title
                }, ct);

                if (newNoteDto is null)
                {
                    return new BadRequestObjectResult("This HTTP triggered NotesFunction executed successfully, but you passed in a bad request model for the note creation process.");
                }

                return new CreatedResult("/notes/" + newNoteDto.Id, new CreateNoteResponse
                {
                    Id = newNoteDto.Id,
                    Title = newNoteDto.Title,
                    Body = newNoteDto.Body,
                    LastUpdatedOn = newNoteDto.LastUpdatedOn
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in {ClassName} -> {MethodName} method.", nameof(NotesFunction), nameof(Post));

                return new BadRequestObjectResult("Internal server error.");
            }
        }
    }
}
