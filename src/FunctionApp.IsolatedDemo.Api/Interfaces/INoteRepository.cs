using FunctionApp.IsolatedDemo.Api.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionApp.IsolatedDemo.Api.Interfaces
{
    public interface INoteRepository
    {
        Task<Note> CreateAsync(Note note, CancellationToken ct = default);
    }
}
