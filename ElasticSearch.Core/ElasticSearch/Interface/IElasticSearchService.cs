using ElasticSearch.Core.Models;

namespace elastic_kibana.Logging.Interface
{
    public interface IElasticSearchService<T> where T : class
    {
        public void CheckExistsAndInsertLog(T logMode, string indexName);
        public IReadOnlyCollection<LoginLogModel> SearchLoginLog(string userID, DateTime? BeginDate, DateTime? EndDate, string controller = "", string action = "", int? page = 0, int? rowCount = 10, string? indexName = "login_log");

        //public IReadOnlyCollection<ErrorLogModel> SearchErrorLog(int? userID, int? errorCode, DateTime? BeginDate, DateTime? EndDate, string controller = "", string action = "", string method = "", string services = "", int page = 0, int rowCount = 10, string indexName = "error_log");
        //public IReadOnlyCollection<AuditLogModel> SearchAuditLog(int? userID, DateTime? BeginDate, DateTime? EndDate, string className = "", string operation = "Update", int page = 0, int rowCount = 10, string indexName = "audit_log");
    }
}
