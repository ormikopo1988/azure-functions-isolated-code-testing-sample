using DotNet.Testcontainers.Builders;
using System.Threading.Tasks;
using Testcontainers.CosmosDb;
using Xunit;

namespace FunctionApp.IsolatedDemo.Api.Tests.Integration
{
    public class NotesFunctionFixture : IAsyncLifetime
    {
        private readonly CosmosDbContainer _cosmosDbContainer = new CosmosDbBuilder()
            .WithImage("mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator:latest")
            .WithName("Cosmos_DB")
            .WithPortBinding(8081, 8081)
            .WithPortBinding(10251, 10251)
            .WithPortBinding(10252, 10252)
            .WithPortBinding(10253, 10253)
            .WithPortBinding(10254, 10254)
            .WithEnvironment("AZURE_COSMOS_EMULATOR_PARTITION_COUNT", "1")
            .WithEnvironment("AZURE_COSMOS_EMULATOR_IP_ADDRESS_OVERRIDE", "127.0.0.1")
            .WithEnvironment("AZURE_COSMOS_EMULATOR_ENABLE_DATA_PERSISTENCE", "false")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(10251))
            .Build();
        private readonly NotificationApiServer _notificationApiServer = new();

        public string GetCosmosDbConnectionString() => _cosmosDbContainer.GetConnectionString();
        public string GetNotificationApiServerUrl() => _notificationApiServer.Url;

        public async Task DisposeAsync()
        {
            await _cosmosDbContainer.DisposeAsync();
            _notificationApiServer.Dispose();
        }

        public async Task InitializeAsync()
        {
            _notificationApiServer.Start();
            _notificationApiServer.SetupRequestDetails();
            await _cosmosDbContainer.StartAsync();
        }
    }
}
