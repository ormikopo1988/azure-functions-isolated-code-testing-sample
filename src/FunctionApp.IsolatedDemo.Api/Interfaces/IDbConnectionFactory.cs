using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionApp.IsolatedDemo.Api.Interfaces
{
    public interface IDbConnectionFactory
    {
        public Task<IDbConnection> CreateConnectionAsync(CancellationToken ct = default);
    }
}
