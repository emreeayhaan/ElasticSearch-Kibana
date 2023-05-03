using elastic_kibana.Logging.Interface;
using ElasticSearch.Core.Models;
using ElasticSearch.Logging.ElasticSearch;
using Nest;

namespace elastic_kibana.Logging.Service
{
    public class ElasticSearchService<T> : IElasticSearchService<T> where T : class
    {
        ElasticClientProvider _provider;
        ElasticClient _client;
        public ElasticSearchService(ElasticClientProvider provider)
        {
            _provider = provider;
            _client = _provider.ElasticClient;
        }
        public void CheckExistsAndInsertLog(T logModel, string indexName)
        {

            if (!_client.Indices.Exists(indexName).Exists)
            {
                var newIndexName = indexName + System.DateTime.Now.Ticks;

                var indexSettings = new IndexSettings();
                indexSettings.NumberOfReplicas = 1;
                indexSettings.NumberOfShards = 3;

                var response = _client.Indices.Create(newIndexName, index =>
                   index.Map<T>(m => m.AutoMap()
                          )
                  .InitializeUsing(new IndexState() { Settings = indexSettings })
                  .Aliases(a => a.Alias(indexName)));

            }
            IndexResponse responseIndex = _client.Index<T>(logModel, idx => idx.Index(indexName));
            int a = 0;
        }

        public IReadOnlyCollection<LoginLogModel> SearchLoginLog(string userID, DateTime? BeginDate, DateTime? EndDate, string controller = "", string action = "", int? page = 0, int? rowCount = 10, string? indexName = "login_log")
        {
            BeginDate = BeginDate == null ? DateTime.Parse("01/01/2020") : BeginDate;
            EndDate = EndDate == null ? DateTime.Now : EndDate;
            var response = _client.Search<LoginLogModel>(s => s
            .From(page)
            .Size(rowCount)
            .Sort(ss => ss.Descending(p => p.PostDate))
            .Query(q => q
                .Bool(b => b
                    .Must(
                        q => q.Term(t => t.UserID, userID.ToLower().Trim()),
                        q => q.Term(t => t.Controller, controller.ToLower().Trim()),
                        q => q.Term(t => t.Action, action.ToLower().Trim()),
                         q => q.DateRange(r => r
                        .Field(f => f.PostDate)
                        .GreaterThanOrEquals(DateMath.Anchored(((DateTime)BeginDate).AddDays(-1)))
                        .LessThanOrEquals(DateMath.Anchored(((DateTime)EndDate).AddDays(1)))
                        ))
                     )
                  )
            .Index(indexName)
            );
            return response.Documents;
        }
    }
}
