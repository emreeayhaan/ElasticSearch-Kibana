using elastic_kibana.Logging.Infrastructure;
using ElasticSearch.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace elastic_kibana.Logging.Controllers
{
    [ServiceFilter(typeof(LoginFilter))]
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserController _userService;

        public UserController(IUserController userService)
        {
            _userService = userService;
        }

        [LogAttribute]
        [HttpGet(nameof(GetUserById))]
        public ServiceResponse<UserModel> GetUserById(int userId)
        {
            var response = new ServiceResponse<UserModel>(HttpContext);
            response.Entity = _userService.GetById(userId);
            response.IsSuccessful = true;
            return response;
        }

        [HttpGet("GetUserLog")]
        public ServiceResponse<LoginLogModel> GetUserLog(int? userId, DateTime? BeginDate, DateTime? EndDate, string controller = "", string action = "", int? page = 0, int? rowCount = 10)
        {
            var response = new ServiceResponse<LoginLogModel>(HttpContext);
            response.List = _userService.GetUserLog(userId, BeginDate, EndDate, controller, action, page, rowCount);
            response.IsSuccessful = true;
            return response;
        }
    }
}