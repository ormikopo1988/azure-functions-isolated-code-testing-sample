using FunctionApp.IsolatedDemo.Api.Dtos;
using FunctionApp.IsolatedDemo.Api.Options;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionApp.IsolatedDemo.Api.Interfaces
{
    public interface INoteService
    {
        Task<NoteDto> CreateNoteAsync(CreateNoteOptions createNoteOptions, CancellationToken ct = default);
    }
}
