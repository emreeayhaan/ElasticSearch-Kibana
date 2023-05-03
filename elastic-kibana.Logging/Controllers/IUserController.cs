using Microsoft.AspNetCore.Mvc;

namespace elastic_kibana.Logging.Controllers
{
    public interface IUserController
    {
        public int GetUserById();

        public int GetById(int userId);

        public void GetUserLog(int? userId, DateTime? beginDate, DateTime? endDate, string controller, string action, int? page, int? rowCount);
    }
}
