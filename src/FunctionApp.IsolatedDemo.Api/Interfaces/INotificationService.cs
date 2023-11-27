using FunctionApp.IsolatedDemo.Api.Options;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionApp.IsolatedDemo.Api.Interfaces
{
    public interface INotificationService
    {
        Task SendNoteCreatedEventAsync(CreateNoteOptions createNoteOptions, CancellationToken ct = default);
    }
}
