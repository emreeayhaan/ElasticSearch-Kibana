using ElasticSearch.Core.Configuration;
using Nest;

namespace ElasticSearch.Logging.ElasticSearch
{
    public class ElasticClientProvider
    {
        public ElasticClientProvider(Microsoft.Extensions.Options.IOptions<ElasticConnectionSettings> elasticConfig)
        {
            ElasticSearchHost = elasticConfig.Value.ElasticSearchHost;
            ElasticClient = CreateClient();
        }

        private ElasticClient CreateClient()
        {
            var connectionSettings = new ConnectionSettings(new Uri(ElasticSearchHost))
                .DisablePing()
                .DisableDirectStreaming(true)
                .SniffOnStartup(false)
                .SniffOnConnectionFault(false);

            return new ElasticClient(connectionSettings);
        }

        public ElasticClient CreateClientWithIndex(string defaultIndex)
        {
            var connectionSettings = new ConnectionSettings(new Uri(ElasticSearchHost))
                .DisablePing()
                .SniffOnStartup(false)
                .SniffOnConnectionFault(false)
                .DefaultIndex(defaultIndex);

            return new ElasticClient(connectionSettings);
        }

        public ElasticClient ElasticClient { get; }

        public string ElasticSearchHost { get; set; }
    }
}