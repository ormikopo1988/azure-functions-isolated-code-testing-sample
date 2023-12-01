using FunctionApp.IsolatedDemo.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using FluentAssertions;
using NSubstitute.ReturnsExtensions;
using NSubstitute.ExceptionExtensions;
using FunctionApp.IsolatedDemo.Api.Options;
using FunctionApp.IsolatedDemo.Api.Dtos;
using FunctionApp.IsolatedDemo.Api.Contracts.Requests;
using FunctionApp.IsolatedDemo.Api.Contracts.Responses;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;
using System.Threading;
using Microsoft.Extensions.Logging.Abstractions;
using Bogus;

namespace FunctionApp.IsolatedDemo.Api.Tests.Unit
{
    public class NotesFunctionTests
    {
        private readonly NotesFunction _sut;

        private readonly string _newNoteId = Guid.NewGuid().ToString();
        private readonly DateTime _newNoteLastUpdatedOn = DateTime.UtcNow;
        private readonly INoteService _noteService = Substitute.For<INoteService>();
        private readonly ILogger<NotesFunction> _logger = NullLogger<NotesFunction>.Instance;
        private readonly Faker<CreateNoteRequest> _noteGenerator =
            new Faker<CreateNoteRequest>()
                .RuleFor(x => x.Title, faker => faker.Lorem.Random.Words())
                .RuleFor(x => x.Body, faker => faker.Lorem.Random.Words());

        public NotesFunctionTests()
        {
            _sut = new NotesFunction(_noteService, _logger);
        }

        [Fact]
        public async Task Post_ShouldReturnOkObjectResultWithCreatedNoteDetails_WhenCalledWithValidNoteDetails()
        {
            // Arrange
            var createNoteRequest = _noteGenerator.Generate();

            var expectedResult = new CreateNoteResponse
            {
                Id = _newNoteId,
                LastUpdatedOn = _newNoteLastUpdatedOn,
                Title = createNoteRequest.Title,
                Body = createNoteRequest.Body
            };

            _noteService
                .CreateNoteAsync(Arg.Any<CreateNoteOptions>())
                .Returns(
                    new NoteDto
                    {
                        Id = _newNoteId,
                        Body = createNoteRequest.Body,
                        Title = createNoteRequest.Title,
                        LastUpdatedOn = _newNoteLastUpdatedOn
                    });

            // Act
            var response = await _sut.Post(
                new CreateNoteRequest
                {
                    Title = createNoteRequest.Title,
                    Body = createNoteRequest.Body
                });

            // Assert
            var result = response as CreatedResult;
            result!.StatusCode.Should().Be(StatusCodes.Status201Created);
            result!.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequestObjectResultWithRespectiveErrorMessage_WhenCalledWithInvalidNoteDetails()
        {
            // Arrange
            _noteService.CreateNoteAsync(Arg.Any<CreateNoteOptions>(), Arg.Any<CancellationToken>()).ReturnsNull();

            // Act
            var response = await _sut.Post(_noteGenerator.Generate());

            // Assert
            var result = response as BadRequestObjectResult;
            result!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result!.Value.Should().BeEquivalentTo("This HTTP triggered NotesFunction executed successfully, but you passed in a bad request model for the note creation process.");
        }

        [Fact]
        public async Task Post_ShouldLogTheExceptionAndReturnInternalServerError_WhenNoteServiceThrowsAnException()
        {
            // Arrange
            _noteService.CreateNoteAsync(Arg.Any<CreateNoteOptions>(), Arg.Any<CancellationToken>()).ThrowsAsync<Exception>();

            // Act
            var response = await _sut.Post(_noteGenerator.Generate());

            // Assert
            var result = response as BadRequestObjectResult;
            result!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}