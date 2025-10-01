using Aspire.Hosting;
using Microsoft.Extensions.Logging;

namespace Corporate.Cashflow.Integrations.Texts
{
    [CollectionDefinition("CashFlowApp")]
    public class CashflowTestContainerFixture : ICollectionFixture<TestContainerFixture>
    {
    }

    public class TestContainerFixture : IAsyncLifetime
    {
        private IDistributedApplicationTestingBuilder _builder;
        private DistributedApplication? _app;
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(1800);
        public async Task InitializeAsync()
        {
            var cancellationToken = new CancellationTokenSource(DefaultTimeout).Token;
            _builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Corporate_CashFlow_AppHost>(cancellationToken);
            _builder.Services.AddLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Debug);
                logging.AddFilter(_builder.Environment.ApplicationName, LogLevel.Debug);
                logging.AddFilter("Aspire.", LogLevel.Debug);
            });
            _builder.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });

            _app = await _builder.BuildAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
            await _app.StartAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);


        }

        public HttpClient ClientIdentityServerApi() => _app.CreateHttpClient("cashflow-identityserver-api");
        public HttpClient ClientCashFlowApi() => _app.CreateHttpClient("cash-flow-api");

        public async Task DisposeAsync()
        {
            await _app.DisposeAsync();
        }
    }
}
